using System;
using System.Linq;

namespace AOTSerializer.Generator
{
    public interface IResolverRegisterInfo
    {
        string FullName { get; }

        string FormatterName { get; }
    }

    public class ObjectSerializationInfo : IResolverRegisterInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Namespace { get; set; }
        public bool IsClass { get; set; }
        public bool IsStruct { get { return !IsClass; } }
        public MemberSerializationInfo[] ConstructorParameters { get; set; }
        public MemberSerializationInfo[] Members { get; set; }

        public string FormatterName => (Namespace == null ? Name : Namespace + "." + Name) + "Formatter";

        public int WriteCount
        {
            get
            {
                return Members.Count(x => x.IsReadable);
            }
        }

        public string GetConstructorString()
        {
            var args = string.Join(", ", ConstructorParameters.Select(x => "__" + x.Name + "__"));
            return $"{FullName}({args})";
        }
    }

    public abstract class MemberSerializationInfo
    {
        public bool IsProperty { get; set; }
        public bool IsField { get; set; }
        public bool IsWritable { get; set; }
        public bool IsReadable { get; set; }
        public bool IsExtensionData { get; set; }
        public string StringKey { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string ShortTypeName { get; set; }

        public abstract string GetSerializeMethodString();

        public abstract string GetDeserializeMethodString();
    }

    public class EnumSerializationInfo : IResolverRegisterInfo
    {
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string UnderlyingType { get; set; }

        public string FormatterName => (Namespace == null ? Name : Namespace + "." + Name) + "Formatter";
    }

    public class GenericSerializationInfo : IResolverRegisterInfo, IEquatable<GenericSerializationInfo>
    {
        public string FullName { get; set; }

        public string FormatterName { get; set; }

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