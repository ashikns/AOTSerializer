using AOTSerializer.Common;

namespace AOTSerializer.MessagePack.Formatters
{
    public sealed class NullableFormatter<T> : FormatterBase<T?>
        where T : struct
    {
        public override void Serialize(ref byte[] bytes, ref int offset, T? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                resolver.GetFormatter<T>().Serialize(ref bytes, ref offset, value.Value, resolver);
            }
        }

        public override T? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return resolver.GetFormatter<T>().Deserialize(bytes, ref offset, resolver);
            }
        }
    }

    public sealed class StaticNullableFormatter<T> : FormatterBase<T?>
        where T : struct
    {
        private readonly IFormatter<T> underlyingFormatter;

        public StaticNullableFormatter(IFormatter<T> underlyingFormatter)
        {
            this.underlyingFormatter = underlyingFormatter;
        }

        public override void Serialize(ref byte[] bytes, ref int offset, T? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                underlyingFormatter.Serialize(ref bytes, ref offset, value.Value, resolver);
            }
        }

        public override T? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return underlyingFormatter.Deserialize(bytes, ref offset, resolver);
            }
        }
    }
}