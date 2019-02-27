using System;
using AOTSerializer.Common;

namespace AOTSerializer.MessagePack.Formatters
{
    public sealed class Int16Formatter : FormatterBase<Int16>
    {
        public static readonly Int16Formatter Instance = new Int16Formatter();

        private Int16Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Int16 value, IResolver resolver)
        {
            return MessagePackBinary.WriteInt16(ref bytes, offset, value);
        }

        public override Int16 Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadInt16(bytes, offset, out readSize);
        }
    }

    public sealed class NullableInt16Formatter : FormatterBase<Int16?>
    {
        public static readonly NullableInt16Formatter Instance = new NullableInt16Formatter();

        private NullableInt16Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Int16? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteInt16(ref bytes, offset, value.Value);
            }
        }

        public override Int16? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadInt16(bytes, offset, out readSize);
            }
        }
    }

    public sealed class Int16ArrayFormatter : FormatterBase<Int16[]>
    {
        public static readonly Int16ArrayFormatter Instance = new Int16ArrayFormatter();

        private Int16ArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Int16[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteInt16(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Int16[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Int16[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt16(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class Int32Formatter : FormatterBase<Int32>
    {
        public static readonly Int32Formatter Instance = new Int32Formatter();

        private Int32Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Int32 value, IResolver resolver)
        {
            return MessagePackBinary.WriteInt32(ref bytes, offset, value);
        }

        public override Int32 Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadInt32(bytes, offset, out readSize);
        }
    }

    public sealed class NullableInt32Formatter : FormatterBase<Int32?>
    {
        public static readonly NullableInt32Formatter Instance = new NullableInt32Formatter();

        private NullableInt32Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Int32? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteInt32(ref bytes, offset, value.Value);
            }
        }

        public override Int32? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadInt32(bytes, offset, out readSize);
            }
        }
    }

    public sealed class Int32ArrayFormatter : FormatterBase<Int32[]>
    {
        public static readonly Int32ArrayFormatter Instance = new Int32ArrayFormatter();

        private Int32ArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Int32[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteInt32(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Int32[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Int32[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class Int64Formatter : FormatterBase<Int64>
    {
        public static readonly Int64Formatter Instance = new Int64Formatter();

        private Int64Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Int64 value, IResolver resolver)
        {
            return MessagePackBinary.WriteInt64(ref bytes, offset, value);
        }

        public override Int64 Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadInt64(bytes, offset, out readSize);
        }
    }

    public sealed class NullableInt64Formatter : FormatterBase<Int64?>
    {
        public static readonly NullableInt64Formatter Instance = new NullableInt64Formatter();

        private NullableInt64Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Int64? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteInt64(ref bytes, offset, value.Value);
            }
        }

        public override Int64? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadInt64(bytes, offset, out readSize);
            }
        }
    }

    public sealed class Int64ArrayFormatter : FormatterBase<Int64[]>
    {
        public static readonly Int64ArrayFormatter Instance = new Int64ArrayFormatter();

        private Int64ArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Int64[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteInt64(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Int64[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Int64[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class UInt16Formatter : FormatterBase<UInt16>
    {
        public static readonly UInt16Formatter Instance = new UInt16Formatter();

        private UInt16Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, UInt16 value, IResolver resolver)
        {
            return MessagePackBinary.WriteUInt16(ref bytes, offset, value);
        }

        public override UInt16 Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
        }
    }

    public sealed class NullableUInt16Formatter : FormatterBase<UInt16?>
    {
        public static readonly NullableUInt16Formatter Instance = new NullableUInt16Formatter();

        private NullableUInt16Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, UInt16? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteUInt16(ref bytes, offset, value.Value);
            }
        }

        public override UInt16? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
            }
        }
    }

    public sealed class UInt16ArrayFormatter : FormatterBase<UInt16[]>
    {
        public static readonly UInt16ArrayFormatter Instance = new UInt16ArrayFormatter();

        private UInt16ArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, UInt16[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override UInt16[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new UInt16[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class UInt32Formatter : FormatterBase<UInt32>
    {
        public static readonly UInt32Formatter Instance = new UInt32Formatter();

        private UInt32Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, UInt32 value, IResolver resolver)
        {
            return MessagePackBinary.WriteUInt32(ref bytes, offset, value);
        }

        public override UInt32 Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
        }
    }

    public sealed class NullableUInt32Formatter : FormatterBase<UInt32?>
    {
        public static readonly NullableUInt32Formatter Instance = new NullableUInt32Formatter();

        private NullableUInt32Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, UInt32? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteUInt32(ref bytes, offset, value.Value);
            }
        }

        public override UInt32? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
            }
        }
    }

    public sealed class UInt32ArrayFormatter : FormatterBase<UInt32[]>
    {
        public static readonly UInt32ArrayFormatter Instance = new UInt32ArrayFormatter();

        private UInt32ArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, UInt32[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override UInt32[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new UInt32[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class UInt64Formatter : FormatterBase<UInt64>
    {
        public static readonly UInt64Formatter Instance = new UInt64Formatter();

        private UInt64Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, UInt64 value, IResolver resolver)
        {
            return MessagePackBinary.WriteUInt64(ref bytes, offset, value);
        }

        public override UInt64 Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
        }
    }

    public sealed class NullableUInt64Formatter : FormatterBase<UInt64?>
    {
        public static readonly NullableUInt64Formatter Instance = new NullableUInt64Formatter();

        private NullableUInt64Formatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, UInt64? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteUInt64(ref bytes, offset, value.Value);
            }
        }

        public override UInt64? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
            }
        }
    }

    public sealed class UInt64ArrayFormatter : FormatterBase<UInt64[]>
    {
        public static readonly UInt64ArrayFormatter Instance = new UInt64ArrayFormatter();

        private UInt64ArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, UInt64[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override UInt64[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new UInt64[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class SingleFormatter : FormatterBase<Single>
    {
        public static readonly SingleFormatter Instance = new SingleFormatter();

        private SingleFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Single value, IResolver resolver)
        {
            return MessagePackBinary.WriteSingle(ref bytes, offset, value);
        }

        public override Single Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadSingle(bytes, offset, out readSize);
        }
    }

    public sealed class NullableSingleFormatter : FormatterBase<Single?>
    {
        public static readonly NullableSingleFormatter Instance = new NullableSingleFormatter();

        private NullableSingleFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Single? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteSingle(ref bytes, offset, value.Value);
            }
        }

        public override Single? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadSingle(bytes, offset, out readSize);
            }
        }
    }

    public sealed class SingleArrayFormatter : FormatterBase<Single[]>
    {
        public static readonly SingleArrayFormatter Instance = new SingleArrayFormatter();

        private SingleArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Single[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteSingle(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Single[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Single[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class DoubleFormatter : FormatterBase<Double>
    {
        public static readonly DoubleFormatter Instance = new DoubleFormatter();

        private DoubleFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Double value, IResolver resolver)
        {
            return MessagePackBinary.WriteDouble(ref bytes, offset, value);
        }

        public override Double Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadDouble(bytes, offset, out readSize);
        }
    }

    public sealed class NullableDoubleFormatter : FormatterBase<Double?>
    {
        public static readonly NullableDoubleFormatter Instance = new NullableDoubleFormatter();

        private NullableDoubleFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Double? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteDouble(ref bytes, offset, value.Value);
            }
        }

        public override Double? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadDouble(bytes, offset, out readSize);
            }
        }
    }

    public sealed class DoubleArrayFormatter : FormatterBase<Double[]>
    {
        public static readonly DoubleArrayFormatter Instance = new DoubleArrayFormatter();

        private DoubleArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Double[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteDouble(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Double[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Double[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class BooleanFormatter : FormatterBase<Boolean>
    {
        public static readonly BooleanFormatter Instance = new BooleanFormatter();

        private BooleanFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Boolean value, IResolver resolver)
        {
            return MessagePackBinary.WriteBoolean(ref bytes, offset, value);
        }

        public override Boolean Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
        }
    }

    public sealed class NullableBooleanFormatter : FormatterBase<Boolean?>
    {
        public static readonly NullableBooleanFormatter Instance = new NullableBooleanFormatter();

        private NullableBooleanFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Boolean? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteBoolean(ref bytes, offset, value.Value);
            }
        }

        public override Boolean? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
            }
        }
    }

    public sealed class BooleanArrayFormatter : FormatterBase<Boolean[]>
    {
        public static readonly BooleanArrayFormatter Instance = new BooleanArrayFormatter();

        private BooleanArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Boolean[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Boolean[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Boolean[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class ByteFormatter : FormatterBase<Byte>
    {
        public static readonly ByteFormatter Instance = new ByteFormatter();

        private ByteFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Byte value, IResolver resolver)
        {
            return MessagePackBinary.WriteByte(ref bytes, offset, value);
        }

        public override Byte Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadByte(bytes, offset, out readSize);
        }
    }

    public sealed class NullableByteFormatter : FormatterBase<Byte?>
    {
        public static readonly NullableByteFormatter Instance = new NullableByteFormatter();

        private NullableByteFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Byte? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteByte(ref bytes, offset, value.Value);
            }
        }

        public override Byte? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadByte(bytes, offset, out readSize);
            }
        }
    }


    public sealed class SByteFormatter : FormatterBase<SByte>
    {
        public static readonly SByteFormatter Instance = new SByteFormatter();

        private SByteFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, SByte value, IResolver resolver)
        {
            return MessagePackBinary.WriteSByte(ref bytes, offset, value);
        }

        public override SByte Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadSByte(bytes, offset, out readSize);
        }
    }

    public sealed class NullableSByteFormatter : FormatterBase<SByte?>
    {
        public static readonly NullableSByteFormatter Instance = new NullableSByteFormatter();

        private NullableSByteFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, SByte? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteSByte(ref bytes, offset, value.Value);
            }
        }

        public override SByte? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadSByte(bytes, offset, out readSize);
            }
        }
    }

    public sealed class SByteArrayFormatter : FormatterBase<SByte[]>
    {
        public static readonly SByteArrayFormatter Instance = new SByteArrayFormatter();

        private SByteArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, SByte[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteSByte(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override SByte[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new SByte[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadSByte(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class CharFormatter : FormatterBase<Char>
    {
        public static readonly CharFormatter Instance = new CharFormatter();

        private CharFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Char value, IResolver resolver)
        {
            return MessagePackBinary.WriteChar(ref bytes, offset, value);
        }

        public override Char Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadChar(bytes, offset, out readSize);
        }
    }

    public sealed class NullableCharFormatter : FormatterBase<Char?>
    {
        public static readonly NullableCharFormatter Instance = new NullableCharFormatter();

        private NullableCharFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, Char? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteChar(ref bytes, offset, value.Value);
            }
        }

        public override Char? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadChar(bytes, offset, out readSize);
            }
        }
    }

    public sealed class CharArrayFormatter : FormatterBase<Char[]>
    {
        public static readonly CharArrayFormatter Instance = new CharArrayFormatter();

        private CharArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Char[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteChar(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override Char[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new Char[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadChar(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class DateTimeFormatter : FormatterBase<DateTime>
    {
        public static readonly DateTimeFormatter Instance = new DateTimeFormatter();

        private DateTimeFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, DateTime value, IResolver resolver)
        {
            return MessagePackBinary.WriteDateTime(ref bytes, offset, value);
        }

        public override DateTime Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
        }
    }

    public sealed class NullableDateTimeFormatter : FormatterBase<DateTime?>
    {
        public static readonly NullableDateTimeFormatter Instance = new NullableDateTimeFormatter();

        private NullableDateTimeFormatter()
        {
        }

        public override int Serialize(ref byte[] bytes, int offset, DateTime? value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteDateTime(ref bytes, offset, value.Value);
            }
        }

        public override DateTime? Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
            }
        }
    }

    public sealed class DateTimeArrayFormatter : FormatterBase<DateTime[]>
    {
        public static readonly DateTimeArrayFormatter Instance = new DateTimeArrayFormatter();

        private DateTimeArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, DateTime[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteDateTime(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override DateTime[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new DateTime[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

}