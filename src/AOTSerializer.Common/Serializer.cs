using AOTSerializer.Common;
using AOTSerializer.Internal;
using System;

namespace AOTSerializer
{
    public static partial class Serializer
    {
        public static IResolver DefaultResolver { get; set; }

        public static byte[] Serialize(object value)
        {
            return Serialize(value, DefaultResolver);
        }

        public static byte[] Serialize(object value, IResolver resolver)
        {
            byte[] buf = null;
            int offset = 0;
            Serialize(ref buf, ref offset, value, resolver);
            BinaryUtil.FastResize(ref buf, offset);
            return buf;
        }

        public static void Serialize(ref byte[] buffer, ref int offset, object value)
        {
            Serialize(ref buffer, ref offset, value, DefaultResolver);
        }

        public static void Serialize(ref byte[] buffer, ref int offset, object value, IResolver resolver)
        {
            resolver.GetFormatter(value.GetType()).SerializeNonGeneric(ref buffer, ref offset, value, resolver);
        }


        public static object Deserialize(byte[] data, Type type)
        {
            return Deserialize(data, type, DefaultResolver);
        }

        public static object Deserialize(byte[] data, Type type, IResolver resolver)
        {
            int offset = 0;
            return Deserialize(data, ref offset, type, resolver);
        }

        public static object Deserialize(byte[] data, ref int offset, Type type)
        {
            return Deserialize(data, ref offset, type, DefaultResolver);
        }

        public static object Deserialize(byte[] data, ref int offset, Type type, IResolver resolver)
        {
            return resolver.GetFormatter(type).DeserializeNonGeneric(data, ref offset, type, resolver);
        }

        public static T Deserialize<T>(byte[] data)
        {
            return Deserialize<T>(data, DefaultResolver);
        }

        public static T Deserialize<T>(byte[] data, IResolver resolver)
        {
            int offset = 0;
            return Deserialize<T>(data, ref offset, resolver);
        }

        public static T Deserialize<T>(byte[] data, ref int offset)
        {
            return Deserialize<T>(data, ref offset, DefaultResolver);
        }

        public static T Deserialize<T>(byte[] data, ref int offset, IResolver resolver)
        {
            return resolver.GetFormatter<T>().Deserialize(data, ref offset, resolver);
        }
    }
}
