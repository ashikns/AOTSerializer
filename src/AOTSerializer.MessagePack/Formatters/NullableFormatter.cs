using AOTSerializer.Common;

namespace AOTSerializer.MessagePack.Formatters
{
    public sealed class NullableFormatter<T> : FormatterBase<T?>
        where T : struct
    {
        public override int Serialize(ref byte[] bytes, int offset, T? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return resolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Value, resolver);
            }
        }

        public override T? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return resolver.GetFormatter<T>().Deserialize(bytes, offset, out readSize, resolver);
            }
        }
    }

    public sealed class StaticNullableFormatter<T> : FormatterBase<T?>
        where T : struct
    {
        readonly IFormatter<T> underlyingFormatter;

        public StaticNullableFormatter(IFormatter<T> underlyingFormatter)
        {
            this.underlyingFormatter = underlyingFormatter;
        }

        public override int Serialize(ref byte[] bytes, int offset, T? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return underlyingFormatter.Serialize(ref bytes, offset, value.Value, resolver);
            }
        }

        public override T? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return underlyingFormatter.Deserialize(bytes, offset, out readSize, resolver);
            }
        }
    }
}