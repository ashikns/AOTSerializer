using AOTSerializer.Common;

namespace AOTSerializer.Json
{
    internal interface IObjectPropertyNameFormatter<T>
    {
        void SerializeToPropertyName(ref byte[] bytes, ref int offset, T value, IResolver resolver);
        T DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver);
    }
}
