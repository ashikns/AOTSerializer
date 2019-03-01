using AOTSerializer.Common;
using System;

namespace AOTSerializer.MessagePack.Formatters
{
    public sealed class Int16Formatter : FormatterBase<Int16>
    {
        public static readonly Int16Formatter Instance = new Int16Formatter();

        private Int16Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int16 value, IResolver resolver)
        {
            MessagePackBinary.WriteInt16(ref bytes, ref offset, value);
        }

        public override Int16 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadInt16(bytes, ref offset);
        }
    }

    public sealed class NullableInt16Formatter : FormatterBase<Int16?>
    {
        public static readonly NullableInt16Formatter Instance = new NullableInt16Formatter();

        private NullableInt16Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int16? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteInt16(ref bytes, ref offset, value.Value);
            }
        }

        public override Int16? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadInt16(bytes, ref offset);
            }
        }
    }

    public sealed class Int16ArrayFormatter : FormatterBase<Int16[]>
    {
        public static readonly Int16ArrayFormatter Instance = new Int16ArrayFormatter();

        private Int16ArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int16[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteInt16(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Int16[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Int16[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt16(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Int32 value, IResolver resolver)
        {
            MessagePackBinary.WriteInt32(ref bytes, ref offset, value);
        }

        public override Int32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadInt32(bytes, ref offset);
        }
    }

    public sealed class NullableInt32Formatter : FormatterBase<Int32?>
    {
        public static readonly NullableInt32Formatter Instance = new NullableInt32Formatter();

        private NullableInt32Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int32? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteInt32(ref bytes, ref offset, value.Value);
            }
        }

        public override Int32? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadInt32(bytes, ref offset);
            }
        }
    }

    public sealed class Int32ArrayFormatter : FormatterBase<Int32[]>
    {
        public static readonly Int32ArrayFormatter Instance = new Int32ArrayFormatter();

        private Int32ArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int32[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteInt32(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Int32[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Int32[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt32(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Int64 value, IResolver resolver)
        {
            MessagePackBinary.WriteInt64(ref bytes, ref offset, value);
        }

        public override Int64 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadInt64(bytes, ref offset);
        }
    }

    public sealed class NullableInt64Formatter : FormatterBase<Int64?>
    {
        public static readonly NullableInt64Formatter Instance = new NullableInt64Formatter();

        private NullableInt64Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int64? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteInt64(ref bytes, ref offset, value.Value);
            }
        }

        public override Int64? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadInt64(bytes, ref offset);
            }
        }
    }

    public sealed class Int64ArrayFormatter : FormatterBase<Int64[]>
    {
        public static readonly Int64ArrayFormatter Instance = new Int64ArrayFormatter();

        private Int64ArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Int64[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteInt64(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Int64[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Int64[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt64(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, UInt16 value, IResolver resolver)
        {
            MessagePackBinary.WriteUInt16(ref bytes, ref offset, value);
        }

        public override UInt16 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadUInt16(bytes, ref offset);
        }
    }

    public sealed class NullableUInt16Formatter : FormatterBase<UInt16?>
    {
        public static readonly NullableUInt16Formatter Instance = new NullableUInt16Formatter();

        private NullableUInt16Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, UInt16? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteUInt16(ref bytes, ref offset, value.Value);
            }
        }

        public override UInt16? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadUInt16(bytes, ref offset);
            }
        }
    }

    public sealed class UInt16ArrayFormatter : FormatterBase<UInt16[]>
    {
        public static readonly UInt16ArrayFormatter Instance = new UInt16ArrayFormatter();

        private UInt16ArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, UInt16[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteUInt16(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override UInt16[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new UInt16[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt16(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, UInt32 value, IResolver resolver)
        {
            MessagePackBinary.WriteUInt32(ref bytes, ref offset, value);
        }

        public override UInt32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadUInt32(bytes, ref offset);
        }
    }

    public sealed class NullableUInt32Formatter : FormatterBase<UInt32?>
    {
        public static readonly NullableUInt32Formatter Instance = new NullableUInt32Formatter();

        private NullableUInt32Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, UInt32? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteUInt32(ref bytes, ref offset, value.Value);
            }
        }

        public override UInt32? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadUInt32(bytes, ref offset);
            }
        }
    }

    public sealed class UInt32ArrayFormatter : FormatterBase<UInt32[]>
    {
        public static readonly UInt32ArrayFormatter Instance = new UInt32ArrayFormatter();

        private UInt32ArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, UInt32[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteUInt32(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override UInt32[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new UInt32[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt32(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, UInt64 value, IResolver resolver)
        {
            MessagePackBinary.WriteUInt64(ref bytes, ref offset, value);
        }

        public override UInt64 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadUInt64(bytes, ref offset);
        }
    }

    public sealed class NullableUInt64Formatter : FormatterBase<UInt64?>
    {
        public static readonly NullableUInt64Formatter Instance = new NullableUInt64Formatter();

        private NullableUInt64Formatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, UInt64? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteUInt64(ref bytes, ref offset, value.Value);
            }
        }

        public override UInt64? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadUInt64(bytes, ref offset);
            }
        }
    }

    public sealed class UInt64ArrayFormatter : FormatterBase<UInt64[]>
    {
        public static readonly UInt64ArrayFormatter Instance = new UInt64ArrayFormatter();

        private UInt64ArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, UInt64[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteUInt64(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override UInt64[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new UInt64[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt64(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Single value, IResolver resolver)
        {
            MessagePackBinary.WriteSingle(ref bytes, ref offset, value);
        }

        public override Single Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadSingle(bytes, ref offset);
        }
    }

    public sealed class NullableSingleFormatter : FormatterBase<Single?>
    {
        public static readonly NullableSingleFormatter Instance = new NullableSingleFormatter();

        private NullableSingleFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Single? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteSingle(ref bytes, ref offset, value.Value);
            }
        }

        public override Single? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadSingle(bytes, ref offset);
            }
        }
    }

    public sealed class SingleArrayFormatter : FormatterBase<Single[]>
    {
        public static readonly SingleArrayFormatter Instance = new SingleArrayFormatter();

        private SingleArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Single[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteSingle(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Single[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Single[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadSingle(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Double value, IResolver resolver)
        {
            MessagePackBinary.WriteDouble(ref bytes, ref offset, value);
        }

        public override Double Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadDouble(bytes, ref offset);
        }
    }

    public sealed class NullableDoubleFormatter : FormatterBase<Double?>
    {
        public static readonly NullableDoubleFormatter Instance = new NullableDoubleFormatter();

        private NullableDoubleFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Double? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteDouble(ref bytes, ref offset, value.Value);
            }
        }

        public override Double? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadDouble(bytes, ref offset);
            }
        }
    }

    public sealed class DoubleArrayFormatter : FormatterBase<Double[]>
    {
        public static readonly DoubleArrayFormatter Instance = new DoubleArrayFormatter();

        private DoubleArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Double[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteDouble(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Double[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Double[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadDouble(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Boolean value, IResolver resolver)
        {
            MessagePackBinary.WriteBoolean(ref bytes, ref offset, value);
        }

        public override Boolean Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadBoolean(bytes, ref offset);
        }
    }

    public sealed class NullableBooleanFormatter : FormatterBase<Boolean?>
    {
        public static readonly NullableBooleanFormatter Instance = new NullableBooleanFormatter();

        private NullableBooleanFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Boolean? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteBoolean(ref bytes, ref offset, value.Value);
            }
        }

        public override Boolean? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadBoolean(bytes, ref offset);
            }
        }
    }

    public sealed class BooleanArrayFormatter : FormatterBase<Boolean[]>
    {
        public static readonly BooleanArrayFormatter Instance = new BooleanArrayFormatter();

        private BooleanArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Boolean[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteBoolean(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Boolean[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Boolean[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadBoolean(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Byte value, IResolver resolver)
        {
            MessagePackBinary.WriteByte(ref bytes, ref offset, value);
        }

        public override Byte Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadByte(bytes, ref offset);
        }
    }

    public sealed class NullableByteFormatter : FormatterBase<Byte?>
    {
        public static readonly NullableByteFormatter Instance = new NullableByteFormatter();

        private NullableByteFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Byte? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteByte(ref bytes, ref offset, value.Value);
            }
        }

        public override Byte? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadByte(bytes, ref offset);
            }
        }
    }


    public sealed class SByteFormatter : FormatterBase<SByte>
    {
        public static readonly SByteFormatter Instance = new SByteFormatter();

        private SByteFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, SByte value, IResolver resolver)
        {
            MessagePackBinary.WriteSByte(ref bytes, ref offset, value);
        }

        public override SByte Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadSByte(bytes, ref offset);
        }
    }

    public sealed class NullableSByteFormatter : FormatterBase<SByte?>
    {
        public static readonly NullableSByteFormatter Instance = new NullableSByteFormatter();

        private NullableSByteFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, SByte? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteSByte(ref bytes, ref offset, value.Value);
            }
        }

        public override SByte? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadSByte(bytes, ref offset);
            }
        }
    }

    public sealed class SByteArrayFormatter : FormatterBase<SByte[]>
    {
        public static readonly SByteArrayFormatter Instance = new SByteArrayFormatter();

        private SByteArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, SByte[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteSByte(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override SByte[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new SByte[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadSByte(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, Char value, IResolver resolver)
        {
            MessagePackBinary.WriteChar(ref bytes, ref offset, value);
        }

        public override Char Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadChar(bytes, ref offset);
        }
    }

    public sealed class NullableCharFormatter : FormatterBase<Char?>
    {
        public static readonly NullableCharFormatter Instance = new NullableCharFormatter();

        private NullableCharFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, Char? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteChar(ref bytes, ref offset, value.Value);
            }
        }

        public override Char? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadChar(bytes, ref offset);
            }
        }
    }

    public sealed class CharArrayFormatter : FormatterBase<Char[]>
    {
        public static readonly CharArrayFormatter Instance = new CharArrayFormatter();

        private CharArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Char[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteChar(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override Char[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new Char[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadChar(bytes, ref offset);
                }
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

        public override void Serialize(ref byte[] bytes, ref int offset, DateTime value, IResolver resolver)
        {
            MessagePackBinary.WriteDateTime(ref bytes, ref offset, value);
        }

        public override DateTime Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadDateTime(bytes, ref offset);
        }
    }

    public sealed class NullableDateTimeFormatter : FormatterBase<DateTime?>
    {
        public static readonly NullableDateTimeFormatter Instance = new NullableDateTimeFormatter();

        private NullableDateTimeFormatter()
        {
        }

        public override void Serialize(ref byte[] bytes, ref int offset, DateTime? value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteDateTime(ref bytes, ref offset, value.Value);
            }
        }

        public override DateTime? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                return MessagePackBinary.ReadDateTime(bytes, ref offset);
            }
        }
    }

    public sealed class DateTimeArrayFormatter : FormatterBase<DateTime[]>
    {
        public static readonly DateTimeArrayFormatter Instance = new DateTimeArrayFormatter();

        private DateTimeArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, DateTime[] value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteDateTime(ref bytes, ref offset, value[i]);
                }
            }
        }

        public override DateTime[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new DateTime[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadDateTime(bytes, ref offset);
                }
                return array;
            }
        }
    }

}