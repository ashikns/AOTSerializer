using AOTSerializer.Common;

namespace AOTSerializer.MessagePack.Formatters
{
    public class ObjectFallbackFormatter : FormatterBase<object>
    {
        public static readonly ObjectFallbackFormatter Default = new ObjectFallbackFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, object value, IResolver resolver)
        {
            MessagePackBinary.WriteNil(ref bytes, ref offset);
        }

        public override object Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            MessagePackBinary.ReadNil(bytes, ref offset);
            return null;
        }
    }
}
