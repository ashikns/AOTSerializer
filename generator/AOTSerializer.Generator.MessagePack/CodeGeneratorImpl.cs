using AOTSerializer.Common;
using AOTSerializer.MessagePack;
using AOTSerializer.MessagePack.Formatters;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOTSerializer.Generator.MessagePack
{
    public class CodeGeneratorImpl : CodeGenerator
    {
        private HashSet<ITypeSymbol> ConcreteFormatters { get; }
        private Dictionary<ITypeSymbol, string> GenericFormatters { get; }
        private Dictionary<ITypeSymbol, (string WriteFunc, string ReadFunc)> PrimitiveFuncs { get; }

        protected override string FormatterNamespacePrefix => "AOTSerializer.MessagePack.Formatters.";

        public CodeGeneratorImpl(Compilation compilation, Type[] additionalTypesToInclude = null)
            : base(compilation, additionalTypesToInclude)
        {
            ConcreteFormatters = new HashSet<ITypeSymbol>();
            GenericFormatters = new Dictionary<ITypeSymbol, string>();
            PrimitiveFuncs = new Dictionary<ITypeSymbol, (string WriteFunc, string ReadFunc)>();

            foreach (var item in FormatterMap.ConcreteFormatterMap)
            {
                if (GetTypeSymbolForType(item.Key, compilation) is var type && type != null)
                {
                    ConcreteFormatters.Add(type);
                }
            }

            foreach (var item in FormatterMap.GenericFormatterMap)
            {
                if (GetTypeSymbolForType(item.Key, compilation) is INamedTypeSymbol type && type != null)
                {
                    var formatterName = item.Value.FullName;
                    GenericFormatters.Add(type.ConstructUnboundGenericType(), formatterName.Substring(0, formatterName.IndexOf('`')));
                }
            }

            foreach (var func in FormatterMap.PrimitiveFuncs)
            {
                if (GetTypeSymbolForType(func.Key, compilation) is var primitiveType && primitiveType != null)
                {
                    PrimitiveFuncs.Add(primitiveType, func.Value);
                }
            }
        }

        protected override bool HasBuiltinFormatter(ITypeSymbol type)
        {
            return ConcreteFormatters.Contains(type);
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

        protected override string GetGenericFormatterName(INamedTypeSymbol genericType)
        {
            var formatterName = GenericFormatters[genericType.ConstructUnboundGenericType()];
            return $"{formatterName}<{string.Join(", ", genericType.TypeArguments.Select(t => t.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)))}>";
        }

        protected override MemberSerializationInfo MakeMemberSerializationInfo(ISymbol symbol, ITypeSymbol type)
        {
            var info = new MemberSerializationInfo(symbol, type);

            info.SerializeMethodString = PrimitiveFuncs.ContainsKey(type)
            ? $"{nameof(MessagePackBinary)}.{PrimitiveFuncs[type].WriteFunc}(ref bytes, ref offset, value.{info.Name})"
            : $"resolver.{nameof(FormatterResolverExtensions.GetFormatterWithVerify)}<{type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>().Serialize(ref bytes, ref offset, value.{info.Name}, resolver)";

            info.DeserializeMethodString = PrimitiveFuncs.ContainsKey(type)
            ? $"{nameof(MessagePackBinary)}.{PrimitiveFuncs[type].ReadFunc}(bytes, ref offset)"
            : $"resolver.{nameof(FormatterResolverExtensions.GetFormatterWithVerify)}<{type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)}>().Deserialize(bytes, ref offset, resolver)";

            return info;
        }

        protected override string Generate(
            IEnumerable<ObjectSerializationInfo> objectSerializationInfos,
            IEnumerable<GenericSerializationInfo> genericSerializationInfos,
            IEnumerable<EnumSerializationInfo> enumSerializationInfos)
        {
            throw new NotImplementedException();
        }
    }
}
