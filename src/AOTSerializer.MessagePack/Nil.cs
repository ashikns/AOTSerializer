using AOTSerializer.Common;
using System;

namespace AOTSerializer.MessagePack
{
    public struct Nil : IEquatable<Nil>
    {
        public static readonly Nil Default = new Nil();

        public override bool Equals(object obj)
        {
            return obj is Nil;
        }

        public bool Equals(Nil other)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "()";
        }
    }
}

namespace AOTSerializer.MessagePack.Formatters
{
    public class NilFormatter : FormatterBase<Nil>
    {
        public static readonly IFormatter<Nil> Instance = new NilFormatter();

        private NilFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Nil value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteNil(ref bytes, offset);
        }

        public override Nil Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadNil(bytes, offset, out var readSize);
            offset += readSize;
            return result;
        }
    }

    // NullableNil is same as Nil.
    public class NullableNilFormatter : FormatterBase<Nil?>
    {
        public static readonly IFormatter<Nil?> Instance = new NullableNilFormatter();

        private NullableNilFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Nil? value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteNil(ref bytes, offset);
        }

        public override Nil? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadNil(bytes, offset, out var readSize);
            offset += readSize;
            return result;
        }
    }
}