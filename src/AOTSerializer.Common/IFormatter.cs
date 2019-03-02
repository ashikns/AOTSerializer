using System;

namespace AOTSerializer.Common
{
    public interface IFormatter
    {
        void Serialize(ref byte[] bytes, ref int offset, object value, IResolver resolver);
        object Deserialize(byte[] bytes, ref int offset, Type type, IResolver resolver);
    }

    public interface IFormatter<T> : IFormatter
    {
        T Deserialize(byte[] bytes, ref int offset, IResolver resolver);
    }

    public abstract class FormatterBase<T> : IFormatter<T>
    {
        public void Serialize(ref byte[] bytes, ref int offset, object value, IResolver resolver)
        {
            if (!(value is T))
            {
                throw new Exception($"{nameof(value)} should be of type {typeof(T)}");
            }
            Serialize(ref bytes, ref offset, (T)value, resolver);
        }

        public object Deserialize(byte[] bytes, ref int offset, Type type, IResolver resolver)
        {
            if (type != typeof(T))
            {
                throw new Exception($"{nameof(type)} should be of type {typeof(T)}");
            }
            return Deserialize(bytes, ref offset, resolver);
        }

        public abstract void Serialize(ref byte[] bytes, ref int offset, T value, IResolver resolver);
        public abstract T Deserialize(byte[] bytes, ref int offset, IResolver resolver);
    }
}
