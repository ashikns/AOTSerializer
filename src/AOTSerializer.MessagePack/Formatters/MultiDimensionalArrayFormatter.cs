using AOTSerializer.Common;
using System;

namespace AOTSerializer.MessagePack.Formatters
{
    // multi dimensional array serialize to [i, j, [seq]]

    public sealed class TwoDimensionalArrayFormatter<T> : FormatterBase<T[,]>
    {
        private const int ArrayLength = 3;

        public override void Serialize(ref byte[] bytes, ref int offset, T[,] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
                return;
            }

            var i = value.GetLength(0);
            var j = value.GetLength(1);

            var formatter = resolver.GetFormatterWithVerify<T>();

            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, ArrayLength);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, i);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, j);

            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
            foreach (var item in value)
            {
                formatter.Serialize(ref bytes, ref offset, item, resolver);
            }
        }

        public override T[,] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
                return null;
            }

            var formatter = resolver.GetFormatterWithVerify<T>();

            var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            if (len != ArrayLength) throw new InvalidOperationException("Invalid T[,] format");

            var iLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var jLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var maxLen = MessagePackBinary.ReadArrayHeader(bytes, ref offset);

            var array = new T[iLength, jLength];

            var i = 0;
            var j = -1;
            for (int loop = 0; loop < maxLen; loop++)
            {
                if (j < jLength - 1)
                {
                    j++;
                }
                else
                {
                    j = 0;
                    i++;
                }

                array[i, j] = formatter.Deserialize(bytes, ref offset, resolver);
            }

            return array;
        }
    }

    public sealed class ThreeDimensionalArrayFormatter<T> : FormatterBase<T[,,]>
    {
        private const int ArrayLength = 4;

        public override void Serialize(ref byte[] bytes, ref int offset, T[,,] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
                return;
            }

            var i = value.GetLength(0);
            var j = value.GetLength(1);
            var k = value.GetLength(2);

            var formatter = resolver.GetFormatterWithVerify<T>();

            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, ArrayLength);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, i);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, j);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, k);

            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
            foreach (var item in value)
            {
                formatter.Serialize(ref bytes, ref offset, item, resolver);
            }
        }

        public override T[,,] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
                return null;
            }

            var formatter = resolver.GetFormatterWithVerify<T>();

            var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            if (len != ArrayLength) throw new InvalidOperationException("Invalid T[,,] format");

            var iLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var jLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var kLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var maxLen = MessagePackBinary.ReadArrayHeader(bytes, ref offset);

            var array = new T[iLength, jLength, kLength];

            var i = 0;
            var j = 0;
            var k = -1;
            for (int loop = 0; loop < maxLen; loop++)
            {
                if (k < kLength - 1)
                {
                    k++;
                }
                else if (j < jLength - 1)
                {
                    k = 0;
                    j++;
                }
                else
                {
                    k = 0;
                    j = 0;
                    i++;
                }

                array[i, j, k] = formatter.Deserialize(bytes, ref offset, resolver);
            }

            return array;
        }
    }

    public sealed class FourDimensionalArrayFormatter<T> : FormatterBase<T[,,,]>
    {
        private const int ArrayLength = 5;

        public override void Serialize(ref byte[] bytes, ref int offset, T[,,,] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
                return;
            }

            var i = value.GetLength(0);
            var j = value.GetLength(1);
            var k = value.GetLength(2);
            var l = value.GetLength(3);

            var formatter = resolver.GetFormatterWithVerify<T>();

            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, ArrayLength);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, i);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, j);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, k);
            MessagePackBinary.WriteInt32(ref bytes, ref offset, l);

            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
            foreach (var item in value)
            {
                formatter.Serialize(ref bytes, ref offset, item, resolver);
            }
        }

        public override T[,,,] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset++;
                return null;
            }

            var formatter = resolver.GetFormatterWithVerify<T>();

            var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            if (len != ArrayLength) throw new InvalidOperationException("Invalid T[,,,] format");

            var iLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var jLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var kLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var lLength = MessagePackBinary.ReadInt32(bytes, ref offset);
            var maxLen = MessagePackBinary.ReadArrayHeader(bytes, ref offset);

            var array = new T[iLength, jLength, kLength, lLength];

            var i = 0;
            var j = 0;
            var k = 0;
            var l = -1;
            for (int loop = 0; loop < maxLen; loop++)
            {
                if (l < lLength - 1)
                {
                    l++;
                }
                else if (k < kLength - 1)
                {
                    l = 0;
                    k++;
                }
                else if (j < jLength - 1)
                {
                    l = 0;
                    k = 0;
                    j++;
                }
                else
                {
                    l = 0;
                    k = 0;
                    j = 0;
                    i++;
                }

                array[i, j, k, l] = formatter.Deserialize(bytes, ref offset, resolver);
            }

            return array;
        }
    }
}