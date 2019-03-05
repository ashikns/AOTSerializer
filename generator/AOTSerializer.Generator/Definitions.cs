using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace AOTSerializer.Generator
{
    public interface IResolverRegisterInfo
    {
        ITypeSymbol Type { get; }
        string FullName { get; }
        string FormatterName { get; }
    }

    internal static class DisplayFormat
    {
        public static readonly SymbolDisplayFormat BinaryWriteFormat = new SymbolDisplayFormat(
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
                miscellaneousOptions: SymbolDisplayMiscellaneousOptions.ExpandNullable,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly);
        public static readonly SymbolDisplayFormat ShortTypeNameFormat = new SymbolDisplayFormat(
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypes);
    }

    public class MemberSerializationInfo
    {
        public ITypeSymbol Type { get; }
        public string Name { get; }
        public bool IsProperty { get; }
        public bool IsField { get; }
        public string TypeArguments { get; }

        public string StringKey => Name;
        public string ShortTypeName => Type.ToDisplayString(DisplayFormat.BinaryWriteFormat);
        public string FullTypeName => Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        public bool IsWritable { get; set; }
        public bool IsReadable { get; set; }
        public bool IsExtensionData { get; set; }

        public string SerializeMethodString { get; set; }
        public string DeserializeMethodString { get; set; }

        public MemberSerializationInfo(ISymbol symbol, ITypeSymbol type)
        {
            Name = symbol.Name;
            Type = type;
            IsProperty = true;
            IsField = false;

            if (type is INamedTypeSymbol namedType && namedType.IsGenericType)
            {
                TypeArguments = string.Join(", ", namedType.TypeArguments.Select(x => x.ToDisplayString()));
            }
        }
    }

    public class ObjectSerializationInfo : IResolverRegisterInfo
    {
        public ITypeSymbol Type { get; }

        public string FormatterName => (Namespace == null ? Name : Namespace + "." + Name) + "Formatter";
        public string Name => Type.ToDisplayString(DisplayFormat.ShortTypeNameFormat).Replace(".", "_");
        public string FullName => Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
        public string Namespace => NamespacePrefix + (Type.ContainingNamespace.IsGlobalNamespace ? null : Type.ContainingNamespace.ToDisplayString());
        public bool IsClass => !Type.IsValueType;
        public bool IsStruct => Type.IsValueType;
        public int WriteCount => Members.Count(x => x.IsReadable);

        public string NamespacePrefix { get; set; }
        public bool HasConstructor { get; set; }
        public MemberSerializationInfo[] ConstructorParameters { get; set; }
        public MemberSerializationInfo[] Members { get; set; }


        public ObjectSerializationInfo(ITypeSymbol type)
        {
            Type = type;
        }

        public string GetConstructorString()
        {
            var args = string.Join(", ", ConstructorParameters.Select(x => "__" + x.Name + "__"));
            return $"{FullName}({args})";
        }
    }

    public class EnumSerializationInfo : IResolverRegisterInfo
    {
        public ITypeSymbol Type { get; }
        public string UnderlyingType { get; }

        public string FormatterName => (Namespace == null ? Name : Namespace + "." + Name) + "Formatter";
        public string Namespace => NamespacePrefix + (Type.ContainingNamespace.IsGlobalNamespace ? null : Type.ContainingNamespace.ToDisplayString());
        public string Name => Type.Name;
        public string FullName => Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        public string NamespacePrefix { get; set; }

        public EnumSerializationInfo(INamedTypeSymbol type)
        {
            Type = type;
            UnderlyingType = type.EnumUnderlyingType.ToDisplayString(DisplayFormat.BinaryWriteFormat);
        }
    }

    public class GenericSerializationInfo : IResolverRegisterInfo, IEquatable<GenericSerializationInfo>
    {
        public ITypeSymbol Type { get; }
        public string FormatterName { get; }

        public string FullName => Type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

        public GenericSerializationInfo(ITypeSymbol type, string formatterName)
        {
            Type = type;
            FormatterName = formatterName;
        }

        public bool Equals(GenericSerializationInfo other)
        {
            return FullName.Equals(other.FullName);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}