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

            JsonObject = nameof(Newtonsoft.Json.JsonObjectAttribute).Substring(0, nameof(Newtonsoft.Json.JsonObjectAttribute).IndexOf("Attribute"));
            JsonObjectSerializationModeKey = nameof(Newtonsoft.Json.MemberSerialization);
            JsonObjectSerializationModeValueOptIn = nameof(Newtonsoft.Json.MemberSerialization.OptIn);
            JsonProperty = nameof(Newtonsoft.Json.JsonPropertyAttribute).Substring(0, nameof(Newtonsoft.Json.JsonPropertyAttribute).IndexOf("Attribute")); ;
            JsonPropertyName = nameof(Newtonsoft.Json.JsonPropertyAttribute.PropertyName);
            JsonExtensionData = nameof(Newtonsoft.Json.JsonExtensionDataAttribute).Substring(0, nameof(Newtonsoft.Json.JsonExtensionDataAttribute).IndexOf("Attribute")); ;
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

    public abstract class CodeGenerator
    {
        private List<ITypeSymbol> TargetTypes { get; }
        private ReferenceSymbols TypeReferences { get; }

        // visitor workspace:
        private HashSet<ITypeSymbol> AlreadyCollected { get; set; }
        private List<ObjectSerializationInfo> CollectedObjectInfo { get; set; }
        private List<EnumSerializationInfo> CollectedEnumInfo { get; set; }
        private List<GenericSerializationInfo> CollectedGenericInfo { get; set; }

        // --- 

        protected CodeGenerator(Compilation compilation, IEnumerable<ITypeSymbol> additionalTypesToInclude)
        {
            TypeReferences = new ReferenceSymbols(compilation);

            var compilationTypes = compilation.GetNamedTypeSymbols()
                .Where(x =>
                {
                    return x.DeclaredAccessibility == Accessibility.Public;
                })
                .Where(x => (x.TypeKind == TypeKind.Interface) || (x.TypeKind == TypeKind.Class) || (x.TypeKind == TypeKind.Struct))
                .ToList();

            TargetTypes = compilationTypes.Concat(additionalTypesToInclude ?? new ITypeSymbol[0]).ToList();
        }

        public string Generate()
        {
            Collect();
            return Generate(CollectedObjectInfo, CollectedGenericInfo.Distinct(), CollectedEnumInfo);
        }

        private void Collect()
        {
            ResetWorkspace();

            foreach (var item in TargetTypes)
            {
                CollectCore(item);
            }
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

            if (HasBuiltinFormatter(typeSymbol))
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

            //if (type.Locations[0].IsInMetadata)
            //{
            //    return;
            //}

            CollectObject(type);
            return;
        }

        private void CollectEnum(INamedTypeSymbol type)
        {
            CollectedEnumInfo.Add(new EnumSerializationInfo(type) { NamespacePrefix = FormatterNamespacePrefix });
        }

        private void CollectArray(IArrayTypeSymbol array)
        {
            CollectCore(array.ElementType);
            CollectedGenericInfo.Add(new GenericSerializationInfo(array, GetArrayFormatterName(array)));
            return;
        }

        private void CollectGeneric(INamedTypeSymbol type)
        {
            foreach (var item in type.TypeArguments)
            {
                CollectCore(item);
            }

            // nullable
            if (type.IsNullable())
            {
                CollectedGenericInfo.Add(new GenericSerializationInfo(type, GetNullableFormatterName(type)));
                return;
            }

            // collection
            if (HasGenericFormatter(type, out var genericFormatterName))
            {
                CollectedGenericInfo.Add(new GenericSerializationInfo(type, genericFormatterName));
            }
            else
            {
                return;
            }

            var genericType = type.ConstructUnboundGenericType();

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
                if (item.IsIndexer) { continue; }
                if (item.GetAttributes().Any(x => x.AttributeClass == TypeReferences.IgnoreDataMemberAttribute)) { continue; }
                if (item.Type == type) { continue; }

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

                if (!isWritable && !type.Constructors.Any(c => c.Parameters.Any(p => string.Compare(p.Name, item.Name, true) == 0)))
                {
                    // No set method for property and no constructor takes an argument with this name
                    continue;
                }

                var member = MakeMemberSerializationInfo(item, item.Type);
                member.IsReadable = isReadable;
                member.IsWritable = isWritable;
                member.IsExtensionData = jsonExtensionAttrib != null;

                if (!member.IsReadable && !member.IsWritable) { continue; }
                stringMembers.Add(member.StringKey, member);

                CollectCore(item.Type); // recursive collect
            }
            foreach (var item in type.GetAllMembers().OfType<IFieldSymbol>())
            {
                if (item.IsImplicitlyDeclared) continue;
                if (item.GetAttributes().Any(x => x.AttributeClass == TypeReferences.IgnoreDataMemberAttribute)) { continue; }
                if (item.Type == type) { continue; }

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

                var member = MakeMemberSerializationInfo(item, item.Type);
                member.IsReadable = isReadable;
                member.IsWritable = isWritable;
                member.IsExtensionData = jsonExtensionAttrib != null;

                if (!member.IsReadable && !member.IsWritable) { continue; }
                stringMembers.Add(member.StringKey, member);
                CollectCore(item.Type); // recursive collect
            }

            if (stringMembers.Count == 0) { return; }

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
                            if (item.Type == paramMember.Type && paramMember.IsReadable)
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

            var info = new ObjectSerializationInfo(type)
            {
                ConstructorParameters = constructorParameters.ToArray(),
                Members = stringMembers.Values.ToArray(),
                HasConstructor = ctor != null,
                NamespacePrefix = FormatterNamespacePrefix,
            };

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

        protected abstract bool HasBuiltinFormatter(ITypeSymbol type);
        protected abstract bool HasGenericFormatter(INamedTypeSymbol genericType, out string genericFormatterName);
        protected abstract string GetArrayFormatterName(IArrayTypeSymbol arrayType);
        protected abstract string GetNullableFormatterName(INamedTypeSymbol nullableType);
        protected abstract MemberSerializationInfo MakeMemberSerializationInfo(ISymbol symbol, ITypeSymbol type);

        protected abstract string Generate(
            IEnumerable<ObjectSerializationInfo> objectSerializationInfos,
            IEnumerable<GenericSerializationInfo> genericSerializationInfos,
            IEnumerable<EnumSerializationInfo> enumSerializationInfos);

        protected abstract string FormatterNamespacePrefix { get; }
    }

    public class MessagePackGeneratorResolveFailedException : Exception
    {
        public MessagePackGeneratorResolveFailedException(string message)
            : base(message)
        {

        }
    }
}