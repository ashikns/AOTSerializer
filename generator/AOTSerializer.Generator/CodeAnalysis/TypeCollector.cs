using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOTSerializer.Generator
{
    internal class ReferenceSymbols
    {
        public readonly INamedTypeSymbol DataContractAttribute;
        public readonly INamedTypeSymbol DataMemberAttribute;
        public readonly INamedTypeSymbol IgnoreDataMemberAttribute;

        public readonly string JsonObject;
        public readonly string JsonObjectSerializationModeKey;
        public readonly string JsonObjectSerializationModeValueOptIn;
        public readonly string JsonProperty;
        public readonly string JsonPropertyName;
        public readonly string JsonExtensionData;
        public readonly string JsonExtensionReadable;
        public readonly string JsonExtensionWritable;

        public readonly Dictionary<INamedTypeSymbol, IEnumerable<INamedTypeSymbol>> GenericTypeSpecialCases;

        public ReferenceSymbols(Compilation compilation)
        {
            DataContractAttribute = compilation.GetTypeByMetadataName(typeof(System.Runtime.Serialization.DataContractAttribute).FullName);
            if (DataContractAttribute == null)
            {
                throw new InvalidOperationException("failed to get metadata");
            }
            DataMemberAttribute = compilation.GetTypeByMetadataName(typeof(System.Runtime.Serialization.DataMemberAttribute).FullName);
            if (DataMemberAttribute == null)
            {
                throw new InvalidOperationException("failed to get metadata");
            }
            IgnoreDataMemberAttribute = compilation.GetTypeByMetadataName(typeof(System.Runtime.Serialization.IgnoreDataMemberAttribute).FullName);
            if (IgnoreDataMemberAttribute == null)
            {
                throw new InvalidOperationException("failed to get metadata");
            }

            JsonObject = typeof(Newtonsoft.Json.JsonObjectAttribute).FullName;
            JsonObjectSerializationModeKey = nameof(Newtonsoft.Json.MemberSerialization);
            JsonObjectSerializationModeValueOptIn = nameof(Newtonsoft.Json.MemberSerialization.OptIn);
            JsonProperty = typeof(Newtonsoft.Json.JsonPropertyAttribute).FullName;
            JsonPropertyName = nameof(Newtonsoft.Json.JsonPropertyAttribute.PropertyName);
            JsonExtensionData = typeof(Newtonsoft.Json.JsonExtensionDataAttribute).FullName;
            JsonExtensionReadable = nameof(Newtonsoft.Json.JsonExtensionDataAttribute.ReadData);
            JsonExtensionWritable = nameof(Newtonsoft.Json.JsonExtensionDataAttribute.WriteData);

            // generic special case types
            GenericTypeSpecialCases = new Dictionary<INamedTypeSymbol, IEnumerable<INamedTypeSymbol>>();

            if (compilation.GetTypeByMetadataName(typeof(ILookup<,>).FullName) is var lookupType && lookupType != null)
            {
                var groupingType = compilation.GetTypeByMetadataName(typeof(IGrouping<,>).FullName);
                var enumerableType = compilation.GetTypeByMetadataName(typeof(IEnumerable<>).FullName);

                if (groupingType == null || enumerableType == null)
                {
                    throw new Exception($"Can't find necessary helping types for {typeof(ILookup<,>)}");
                }

                GenericTypeSpecialCases.Add(lookupType, new[] { groupingType, enumerableType });
            }
        }
    }

    public abstract class TypeCollector
    {
        private static readonly SymbolDisplayFormat BinaryWriteFormat = new SymbolDisplayFormat(
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.ExpandNullable,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly);
        private static readonly SymbolDisplayFormat ShortTypeNameFormat = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes);

        private INamedTypeSymbol[] TargetTypes { get; }
        private ReferenceSymbols TypeReferences { get; }

        // visitor workspace:
        private HashSet<ITypeSymbol> AlreadyCollected { get; set; }
        private List<ObjectSerializationInfo> CollectedObjectInfo { get; set; }
        private List<EnumSerializationInfo> CollectedEnumInfo { get; set; }
        private List<GenericSerializationInfo> CollectedGenericInfo { get; set; }

        // --- 

        protected TypeCollector(Compilation compilation)
        {
            var compilationErrors = compilation.GetDiagnostics().Where(x => x.Severity == DiagnosticSeverity.Error).ToArray();
            if (compilationErrors.Length != 0)
            {
                throw new InvalidOperationException($"detect compilation error:{string.Join("\n", compilationErrors.Select(x => x.ToString()))}");
            }

            TypeReferences = new ReferenceSymbols(compilation);

            TargetTypes = compilation.GetNamedTypeSymbols()
                .Where(x =>
                {
                    return x.DeclaredAccessibility == Accessibility.Public;
                })
                .Where(x => (x.TypeKind == TypeKind.Interface) || (x.TypeKind == TypeKind.Class) || (x.TypeKind == TypeKind.Struct))
                .ToArray();
        }

        // EntryPoint
        public (ObjectSerializationInfo[] objectInfo, EnumSerializationInfo[] enumInfo, GenericSerializationInfo[] genericInfo) Collect()
        {
            ResetWorkspace();

            foreach (var item in TargetTypes)
            {
                CollectCore(item);
            }

            return (CollectedObjectInfo.ToArray(), CollectedEnumInfo.ToArray(), CollectedGenericInfo.Distinct().ToArray());
        }

        private void ResetWorkspace()
        {
            AlreadyCollected = new HashSet<ITypeSymbol>();
            CollectedObjectInfo = new List<ObjectSerializationInfo>();
            CollectedEnumInfo = new List<EnumSerializationInfo>();
            CollectedGenericInfo = new List<GenericSerializationInfo>();
        }

        // Gate of recursive collect
        private void CollectCore(ITypeSymbol typeSymbol)
        {
            if (!AlreadyCollected.Add(typeSymbol))
            {
                return;
            }

            if (IsEmbeddedType(typeSymbol))
            {
                return;
            }

            if (typeSymbol.TypeKind == TypeKind.Array)
            {
                CollectArray(typeSymbol as IArrayTypeSymbol);
                return;
            }

            if (!IsAllowAccessibility(typeSymbol))
            {
                return;
            }

            var type = typeSymbol as INamedTypeSymbol;

            if (typeSymbol.TypeKind == TypeKind.Enum)
            {
                CollectEnum(type);
                return;
            }

            if (type.IsGenericType)
            {
                CollectGeneric(type);
                return;
            }

            if (type.Locations[0].IsInMetadata)
            {
                return;
            }

            CollectObject(type);
            return;
        }

        private void CollectEnum(INamedTypeSymbol type)
        {
            var info = CreateEnumInfo();
            info.Name = type.Name;
            info.Namespace = type.ContainingNamespace.IsGlobalNamespace ? null : type.ContainingNamespace.ToDisplayString();
            info.FullName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            info.UnderlyingType = type.EnumUnderlyingType.ToDisplayString(BinaryWriteFormat);

            CollectedEnumInfo.Add(info);
        }

        private void CollectArray(IArrayTypeSymbol array)
        {
            var elemType = array.ElementType;
            CollectCore(elemType);

            var info = new GenericSerializationInfo
            {
                FullName = array.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            };

            info.SetFormatterName(GetArrayFormatterName(array));

            CollectedGenericInfo.Add(info);

            return;
        }

        private void CollectGeneric(INamedTypeSymbol type)
        {
            // nullable
            if (type.IsNullable())
            {
                CollectCore(type.TypeArguments[0]);

                if (!IsEmbeddedType(type.TypeArguments[0]))
                {
                    var nullableInfo = new GenericSerializationInfo
                    {
                        FullName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
                    };
                    nullableInfo.SetFormatterName(GetNullableFormatterName(type.TypeArguments[0]));

                    CollectedGenericInfo.Add(nullableInfo);
                }
                return;
            }

            // collection
            foreach (var item in type.TypeArguments)
            {
                CollectCore(item);
            }

            var genericType = type.ConstructUnboundGenericType();
            var formatter = GetGenericFormatterName(genericType, type.TypeArguments);

            var info = new GenericSerializationInfo
            {
                FullName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
            };
            info.SetFormatterName(formatter);

            CollectedGenericInfo.Add(info);

            if (TypeReferences.GenericTypeSpecialCases.ContainsKey(genericType))
            {
                foreach (var helperType in TypeReferences.GenericTypeSpecialCases[genericType])
                {
                    CollectCore(helperType.Construct(type.TypeArguments.ToArray()));
                }
            }
        }

        private void CollectObject(INamedTypeSymbol type)
        {
            var isClass = !type.IsValueType;

            var dataContractAttribute = type.GetAttributes().FirstOrDefault(a => a.AttributeClass == TypeReferences.DataContractAttribute);
            var jsonObjectAttribute = type.GetAttributes().FindAttributeShortName(TypeReferences.JsonObject);
            var isMarkedOptIn = dataContractAttribute != null ||
                jsonObjectAttribute?.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonObjectSerializationModeKey) == TypeReferences.JsonObjectSerializationModeValueOptIn;

            var jsonExtensionAttributeEncountered = false;

            var stringMembers = new Dictionary<string, MemberSerializationInfo>();

            foreach (var item in type.GetAllMembers().OfType<IPropertySymbol>().Where(x => !x.IsOverride))
            {
                if (item.GetAttributes().Any(x => x.AttributeClass == TypeReferences.IgnoreDataMemberAttribute)) { continue; }
                if (item.IsIndexer) { continue; }

                var dataMemberAttrib = item.GetAttributes().FirstOrDefault(a => a.AttributeClass == TypeReferences.DataMemberAttribute);
                var jsonPropertyAttrib = item.GetAttributes().FindAttributeShortName(TypeReferences.JsonProperty);
                var jsonExtensionAttrib = item.GetAttributes().FindAttributeShortName(TypeReferences.JsonExtensionData);

                if (isMarkedOptIn && dataMemberAttrib == null && jsonPropertyAttrib == null && jsonExtensionAttrib == null) continue;

                if (jsonExtensionAttrib != null)
                {
                    if (jsonExtensionAttributeEncountered)
                        throw new Exception("Json extension attribute should only appear once");

                    if (!item.Type.Name.Contains("Dictionary") && !item.Type.AllInterfaces.Any(i => i.Name.Contains("Dictionary")))
                        throw new Exception("Json extension attribute should be adorned on a Dictionary type");

                    jsonExtensionAttributeEncountered = true;
                }

                var name = item.Name;
                if (dataMemberAttrib != null) { name = dataMemberAttrib.GetSingleNamedArgumentValueFromSyntaxTree("Name"); }
                else if (jsonPropertyAttrib != null) { name = jsonPropertyAttrib.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonPropertyName); }

                bool isReadable, isWritable;
                if (jsonExtensionAttrib == null)
                {
                    isReadable = (item.GetMethod != null) && item.GetMethod.DeclaredAccessibility == Accessibility.Public && !item.IsStatic;
                    isWritable = (item.SetMethod != null) && item.SetMethod.DeclaredAccessibility == Accessibility.Public && !item.IsStatic;
                }
                else
                {
                    // JsonExtension writable means the property itself is readable
                    isReadable = (item.GetMethod != null) && bool.Parse(jsonExtensionAttrib.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonExtensionWritable) ?? "true");
                    isWritable = (item.SetMethod != null) && bool.Parse(jsonExtensionAttrib.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonExtensionReadable) ?? "true");
                }

                var member = CreateMemberInfo();
                member.IsReadable = isReadable;
                member.IsWritable = isWritable;
                member.StringKey = item.Name;
                member.IsProperty = true;
                member.IsField = false;
                member.IsExtensionData = jsonExtensionAttrib != null;
                member.Name = item.Name;
                member.Type = item.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                member.ShortTypeName = item.Type.ToDisplayString(BinaryWriteFormat);

                if (!member.IsReadable && !member.IsWritable) { continue; }
                stringMembers.Add(member.StringKey, member);

                CollectCore(item.Type); // recursive collect
            }
            foreach (var item in type.GetAllMembers().OfType<IFieldSymbol>())
            {
                if (item.GetAttributes().Any(x => x.AttributeClass == TypeReferences.IgnoreDataMemberAttribute)) { continue; }
                if (item.IsImplicitlyDeclared) continue;

                var dataMemberAttrib = item.GetAttributes().FirstOrDefault(a => a.AttributeClass == TypeReferences.DataMemberAttribute);
                var jsonPropertyAttrib = item.GetAttributes().FindAttributeShortName(TypeReferences.JsonProperty);
                var jsonExtensionAttrib = item.GetAttributes().FindAttributeShortName(TypeReferences.JsonExtensionData);

                if (isMarkedOptIn && dataMemberAttrib == null && jsonPropertyAttrib == null && jsonExtensionAttrib == null) continue;

                if (jsonExtensionAttrib != null)
                {
                    if (jsonExtensionAttributeEncountered)
                        throw new Exception("Json extension attribute should only appear once");

                    if (!item.Type.Name.Contains("Dictionary") && !item.Type.AllInterfaces.Any(i => i.Name.Contains("Dictionary")))
                        throw new Exception("Json extension attribute should be adorned on a Dictionary type");

                    jsonExtensionAttributeEncountered = true;
                }

                var name = item.Name;
                if (dataMemberAttrib != null) { name = dataMemberAttrib.GetSingleNamedArgumentValueFromSyntaxTree("Name"); }
                else if (jsonPropertyAttrib != null) { name = jsonPropertyAttrib.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonPropertyName); }

                bool isReadable, isWritable;
                if (jsonExtensionAttrib == null)
                {
                    isReadable = item.DeclaredAccessibility == Accessibility.Public && !item.IsStatic;
                    isWritable = item.DeclaredAccessibility == Accessibility.Public && !item.IsReadOnly && !item.IsStatic;
                }
                else
                {
                    // JsonExtension writable means the property itself is readable
                    isReadable = bool.Parse(jsonExtensionAttrib.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonExtensionWritable) ?? "true");
                    isWritable = bool.Parse(jsonExtensionAttrib.GetSingleNamedArgumentValueFromSyntaxTree(TypeReferences.JsonExtensionReadable) ?? "true");
                }

                var member = CreateMemberInfo();
                member.IsReadable = isReadable;
                member.IsWritable = isWritable;
                member.StringKey = item.Name;
                member.IsProperty = false;
                member.IsField = true;
                member.IsExtensionData = jsonExtensionAttrib != null;
                member.Name = item.Name;
                member.Type = item.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                member.ShortTypeName = item.Type.ToDisplayString(BinaryWriteFormat);

                if (!member.IsReadable && !member.IsWritable) { continue; }
                stringMembers.Add(member.StringKey, member);
                CollectCore(item.Type); // recursive collect
            }

            // GetConstructor
            var ctorEnumerator =
                    type.Constructors.Where(x => x.DeclaredAccessibility == Accessibility.Public && !x.IsImplicitlyDeclared).OrderBy(x => x.Parameters.Length)
                    .Concat(type.Constructors.Where(x => x.DeclaredAccessibility == Accessibility.Public).OrderBy(x => x.Parameters.Length).Take(1))
                    .GetEnumerator();

            IMethodSymbol ctor = null;
            if (ctorEnumerator.MoveNext())
            {
                ctor = ctorEnumerator.Current;
            }

            // struct allows null ctor
            if (ctor == null && isClass) { throw new MessagePackGeneratorResolveFailedException("can't find public constructor. type:" + type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)); }

            var constructorParameters = new List<MemberSerializationInfo>();
            if (ctor != null)
            {
                var constructorLookupDictionary = stringMembers.ToLookup(x => x.Key, x => x, StringComparer.OrdinalIgnoreCase);
                do
                {
                    constructorParameters.Clear();
                    var ctorParamIndex = 0;
                    foreach (var item in ctor.Parameters)
                    {
                        MemberSerializationInfo paramMember;
                        var hasKey = constructorLookupDictionary[item.Name];
                        var len = hasKey.Count();
                        if (len == 1)
                        {
                            paramMember = hasKey.First().Value;
                            if (item.Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat) == paramMember.Type && paramMember.IsReadable)
                            {
                                constructorParameters.Add(paramMember);
                            }
                            else
                            {
                                ctor = null;
                                continue;
                            }
                        }
                        else
                        {
                            ctor = null;
                            continue;
                        }
                        ctorParamIndex++;
                    }
                } while (TryGetNextConstructor(ctorEnumerator, ref ctor));

                if (ctor == null)
                {
                    throw new MessagePackGeneratorResolveFailedException("can't find matched constructor. type:" + type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
                }
            }

            var info = CreateObjectInfo();
            info.IsClass = isClass;
            info.ConstructorParameters = constructorParameters.ToArray();
            info.Members = stringMembers.Values.ToArray();
            info.Name = type.ToDisplayString(ShortTypeNameFormat).Replace(".", "_");
            info.FullName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            info.Namespace = type.ContainingNamespace.IsGlobalNamespace ? null : type.ContainingNamespace.ToDisplayString();

            CollectedObjectInfo.Add(info);
        }

        private static bool TryGetNextConstructor(IEnumerator<IMethodSymbol> ctorEnumerator, ref IMethodSymbol ctor)
        {
            if (ctorEnumerator == null || ctor != null)
            {
                return false;
            }

            if (ctorEnumerator.MoveNext())
            {
                ctor = ctorEnumerator.Current;
                return true;
            }
            else
            {
                ctor = null;
                return false;
            }
        }

        private bool IsAllowAccessibility(ITypeSymbol symbol)
        {
            do
            {
                if (symbol.DeclaredAccessibility != Accessibility.Public)
                {
                    return false;
                }
                symbol = symbol.ContainingType;
            } while (symbol != null);

            return true;
        }

        protected abstract ObjectSerializationInfo CreateObjectInfo();
        protected abstract MemberSerializationInfo CreateMemberInfo();
        protected abstract EnumSerializationInfo CreateEnumInfo();
        protected abstract bool IsEmbeddedType(ITypeSymbol type);
        protected abstract string GetNullableFormatterName(ITypeSymbol type);
        protected abstract string GetArrayFormatterName(IArrayTypeSymbol arrayType);
        protected abstract string GetGenericFormatterName(ITypeSymbol genericType, IEnumerable<ITypeSymbol> typeArgs);
    }

    public class MessagePackGeneratorResolveFailedException : Exception
    {
        public MessagePackGeneratorResolveFailedException(string message)
            : base(message)
        {

        }
    }
}