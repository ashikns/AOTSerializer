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

    public abstract class CodeGenerator
    {
        private INamedTypeSymbol[] TargetTypes { get; }
        private ReferenceSymbols TypeReferences { get; }

        // visitor workspace:
        private HashSet<ITypeSymbol> AlreadyCollected { get; set; }
        private List<ObjectSerializationInfo> CollectedObjectInfo { get; set; }
        private List<EnumSerializationInfo> CollectedEnumInfo { get; set; }
        private List<GenericSerializationInfo> CollectedGenericInfo { get; set; }

        // --- 

        protected CodeGenerator(Compilation compilation)
        {
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

            if (type.Locations[0].IsInMetadata)
            {
                return;
            }

            CollectObject(type);
            return;
        }

        private void CollectEnum(INamedTypeSymbol type)
        {
            CollectedEnumInfo.Add(new EnumSerializationInfo(type));
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
            CollectedGenericInfo.Add(new GenericSerializationInfo(type, GetGenericFormatterName(type)));

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

            if (stringMembers.Count == 0) { return; }

            var info = new ObjectSerializationInfo(type)
            {
                ConstructorParameters = constructorParameters.ToArray(),
                Members = stringMembers.Values.ToArray(),
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

        protected static ITypeSymbol GetTypeSymbolForType(Type type, Compilation compilation)
        {
            if (type.IsArray && compilation.GetTypeByMetadataName(type.GetElementType().FullName) is var arrayElementType && arrayElementType != null)
            {
                var allSymbols = compilation.GetSymbolsWithName((_) => true);
                foreach (var symbol in allSymbols)
                {
                    switch (symbol.Kind)
                    {
                        case SymbolKind.ArrayType:
                            var currentArrayType = (IArrayTypeSymbol)symbol;
                            if (currentArrayType.ElementType == arrayElementType
                                && currentArrayType.Rank == type.GetArrayRank())
                            {
                                return currentArrayType;
                            }
                            break;
                        case SymbolKind.Field:
                            var fieldType = ((IFieldSymbol)symbol).Type;
                            if (fieldType.Kind == SymbolKind.ArrayType
                                && ((IArrayTypeSymbol)fieldType).ElementType == arrayElementType
                                && ((IArrayTypeSymbol)fieldType).Rank == type.GetArrayRank())
                            {
                                return fieldType;
                            }
                            break;
                        case SymbolKind.Property:
                            var propertyType = ((IPropertySymbol)symbol).Type;
                            if (propertyType.Kind == SymbolKind.ArrayType
                                && ((IArrayTypeSymbol)propertyType).ElementType == arrayElementType
                                && ((IArrayTypeSymbol)propertyType).Rank == type.GetArrayRank())
                            {
                                return propertyType;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            if (!type.IsConstructedGenericType)
            {
                return compilation.GetTypeByMetadataName(type.FullName);
            }

            // get all typeInfo's for the Type arguments 
            var typeArgumentsTypeInfos = type.GenericTypeArguments.Select(a => GetTypeSymbolForType(a, compilation));

            var openType = type.GetGenericTypeDefinition();
            var typeSymbol = compilation.GetTypeByMetadataName(openType.FullName);
            return typeSymbol.Construct(typeArgumentsTypeInfos.ToArray());
        }

        protected abstract bool HasBuiltinFormatter(ITypeSymbol type);
        protected abstract MemberSerializationInfo MakeMemberSerializationInfo(ISymbol symbol, ITypeSymbol type);
        protected abstract string GetArrayFormatterName(IArrayTypeSymbol arrayType);
        protected abstract string GetNullableFormatterName(INamedTypeSymbol nullableType);
        protected abstract string GetGenericFormatterName(INamedTypeSymbol genericType);
    }

    public class MessagePackGeneratorResolveFailedException : Exception
    {
        public MessagePackGeneratorResolveFailedException(string message)
            : base(message)
        {

        }
    }
}