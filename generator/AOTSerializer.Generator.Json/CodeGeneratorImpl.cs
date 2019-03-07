using AOTSerializer.Common;
using AOTSerializer.Generator.Json.Templates;
using AOTSerializer.Json;
using AOTSerializer.Json.Formatters;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOTSerializer.Generator.Json
{
    public class CodeGeneratorImpl : CodeGenerator
    {
        private HashSet<ITypeSymbol> ConcreteFormatters { get; }
        private Dictionary<ITypeSymbol, string> GenericFormatters { get; }
        private Dictionary<ITypeSymbol, (string WriteFunc, string ReadFunc)> PrimitiveFuncs { get; }

        protected override string FormatterNamespacePrefix => "AOTSerializer.Json.Formatters.";

        public CodeGeneratorImpl(Compilation compilation, IEnumerable<ITypeSymbol> additionalTypesToInclude = null)
            : base(compilation, additionalTypesToInclude)
        {
            ConcreteFormatters = new HashSet<ITypeSymbol>();
            GenericFormatters = new Dictionary<ITypeSymbol, string>();
            PrimitiveFuncs = new Dictionary<ITypeSymbol, (string WriteFunc, string ReadFunc)>();

            foreach (var item in FormatterMap.ConcreteFormatterMap)
            {
                if (RoslynExtensions.GetTypeSymbolForType(item.Key, compilation) is var type && type != null)
                {
                    ConcreteFormatters.Add(type);
                }
            }

            foreach (var item in FormatterMap.GenericFormatterMap)
            {
                if (RoslynExtensions.GetTypeSymbolForType(item.Key, compilation) is INamedTypeSymbol type && type != null)
                {
                    var formatterName = item.Value.FullName;
                    GenericFormatters.Add(type.ConstructUnboundGenericType(), formatterName.Substring(0, formatterName.IndexOf('`')));
                }
            }

            foreach (var func in FormatterMap.PrimitiveFuncs)
            {
                if (RoslynExtensions.GetTypeSymbolForType(func.Key, compilation) is var primitiveType && primitiveType != null)
                {
                    PrimitiveFuncs.Add(primitiveType, func.Value);
                }
            }
        }

        protected override bool HasBuiltinFormatter(ITypeSymbol type)
        {
            return ConcreteFormatters.Contains(type);
        }

        protected override bool HasGenericFormatter(INamedTypeSymbol genericType, out string genericFormatterName)
        {
            if (GenericFormatters.TryGetValue(genericType.ConstructUnboundGenericType(), out var formatterName))
            {
                genericFormatterName = $"{formatterName}<{string.Join(", ", genericType.TypeArguments.Select(t => t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))}>";
                return true;
            }
            else
            {
                genericFormatterName = string.Empty;
                return false;
            }
        }

        protected override string GetArrayFormatterName(IArrayTypeSymbol arrayType)
        {
            Type arrayFormatterType;

            if (arrayType.IsSZArray)
            {
                arrayFormatterType = typeof(ArrayFormatter<>);
            }
            else
            {
                switch (arrayType.Rank)
                {
                    case 1:
                        arrayFormatterType = typeof(ArrayFormatter<>);
                        break;
                    case 2:
                        arrayFormatterType = typeof(ArrayFormatter<>);
                        break;
                    case 3:
                        arrayFormatterType = typeof(ArrayFormatter<>);
                        break;
                    case 4:
                        arrayFormatterType = typeof(ArrayFormatter<>);
                        break;
                    default: throw new InvalidOperationException("does not supports array dimention, " + arrayType.Name);
                }
            }

            var formatterName = arrayFormatterType.FullName.Substring(0, arrayFormatterType.FullName.IndexOf('`'));
            return $"{formatterName}<{arrayType.ElementType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>";
        }

        protected override string GetNullableFormatterName(INamedTypeSymbol nullableType)
        {
            var nullableFormatterType = typeof(NullableFormatter<>);
            var formatterName = nullableFormatterType.FullName.Substring(0, nullableFormatterType.FullName.IndexOf('`'));
            return $"{formatterName}<{nullableType.TypeArguments[0].ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>";
        }

        protected override string GetSerializeMethodString(ISymbol symbol, ITypeSymbol type)
        {
            return PrimitiveFuncs.ContainsKey(type)
            ? $"{nameof(JsonUtility)}.{PrimitiveFuncs[type].WriteFunc}(ref bytes, ref offset, value.{symbol.Name})"
            : $"resolver.{nameof(FormatterResolverExtensions.GetFormatterWithVerify)}<{type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>().Serialize(ref bytes, ref offset, value.{symbol.Name}, resolver)";
        }

        protected override string GetDeserializeMethodString(ISymbol symbol, ITypeSymbol type)
        {
            return PrimitiveFuncs.ContainsKey(type)
            ? $"{nameof(JsonUtility)}.{PrimitiveFuncs[type].ReadFunc}(bytes, ref offset)"
            : $"resolver.{nameof(FormatterResolverExtensions.GetFormatterWithVerify)}<{type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>().Deserialize(bytes, ref offset, resolver)";
        }

        protected override string Generate(
            string resolverName,
            IEnumerable<ObjectSerializationInfo> objectSerializationInfos,
            IEnumerable<GenericSerializationInfo> genericSerializationInfos,
            IEnumerable<EnumSerializationInfo> enumSerializationInfos)
        {
            var objectFormatterTemplates = objectSerializationInfos
                .GroupBy(x => x.Namespace)
                .Select(x => new FormatterTemplate()
                {
                    Namespace = x.Key,
                    ObjectSerializationInfos = x.ToArray(),
                })
                .ToArray();

            var enumFormatterTemplates = enumSerializationInfos
                .GroupBy(x => x.Namespace)
                .Select(x => new EnumTemplate()
                {
                    Namespace = x.Key,
                    EnumSerializationInfos = x.ToArray()
                })
                .ToArray();

            var resolverTemplate = new ResolverTemplate()
            {
                Namespace = "AOTSerializer.Json.Resolvers",
                ResolverName = resolverName,
                RegisterInfos = genericSerializationInfos.Cast<IResolverRegisterInfo>().Concat(enumSerializationInfos).Concat(objectSerializationInfos).ToArray()
            };

            var sb = new StringBuilder();
            sb.AppendLine(resolverTemplate.TransformText());
            sb.AppendLine();
            foreach (var item in enumFormatterTemplates)
            {
                var text = item.TransformText();
                sb.AppendLine(text);
            }
            sb.AppendLine();
            sb.AppendLine();
            foreach (var item in objectFormatterTemplates)
            {
                var text = item.TransformText();
                sb.AppendLine(text);
            }

            return sb.ToString();
        }
    }
}
