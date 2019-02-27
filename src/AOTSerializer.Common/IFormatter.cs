using System;

namespace AOTSerializer.Common
{
    public interface IFormatter
    {
        int SerializeNonGeneric(ref byte[] bytes, int offset, object value, IResolver resolver);
        object DeserializeNonGeneric(byte[] bytes, int offset, Type type, out int readSize, IResolver resolver);
    }

    public interface IFormatter<T> : IFormatter
    {
        int Serialize(ref byte[] bytes, int offset, T value, IResolver resolver);
        T Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver);
    }

    public abstract class FormatterBase<T> : IFormatter<T>
    {
        public int SerializeNonGeneric(ref byte[] bytes, int offset, object value, IResolver resolver)
        {
            if (!(value is T))
            {
                throw new Exception($"{nameof(value)} should be of type {typeof(T)}");
            }
            return Serialize(ref bytes, offset, (T)value, resolver);
        }

        public object DeserializeNonGeneric(byte[] bytes, int offset, Type type, out int readSize, IResolver resolver)
        {
            if (type != typeof(T))
            {
                throw new Exception($"{nameof(type)} should be of type {typeof(T)}");
            }
            return Deserialize(bytes, offset, out readSize, resolver);
        }

        public abstract int Serialize(ref byte[] bytes, int offset, T value, IResolver resolver);
        public abstract T Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver);
    }
}
