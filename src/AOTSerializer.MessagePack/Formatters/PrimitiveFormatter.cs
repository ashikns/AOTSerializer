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

        public override void Serialize(ref byte[] bytes, ref int offset, Int16 value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteInt16(ref bytes, offset, value);
        }

        public override Int16 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadInt16(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteInt16(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadInt16(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteInt16(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Int16[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt16(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value);
        }

        public override Int32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadInt32(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadInt32(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteInt32(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Int32[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value);
        }

        public override Int64 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadInt64(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadInt64(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteInt64(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Int64[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadInt64(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value);
        }

        public override UInt16 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadUInt16(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadUInt16(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteUInt16(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new UInt16[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt16(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value);
        }

        public override UInt32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadUInt32(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadUInt32(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteUInt32(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new UInt32[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value);
        }

        public override UInt64 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadUInt64(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadUInt64(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteUInt64(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new UInt64[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteSingle(ref bytes, offset, value);
        }

        public override Single Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadSingle(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteSingle(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadSingle(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteSingle(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Single[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadSingle(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value);
        }

        public override Double Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadDouble(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadDouble(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteDouble(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Double[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value);
        }

        public override Boolean Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadBoolean(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadBoolean(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Boolean[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value);
        }

        public override Byte Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadByte(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadByte(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
            offset += MessagePackBinary.WriteSByte(ref bytes, offset, value);
        }

        public override SByte Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadSByte(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteSByte(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadSByte(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteSByte(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new SByte[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadSByte(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteChar(ref bytes, offset, value);
        }

        public override Char Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadChar(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteChar(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadChar(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteChar(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new Char[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadChar(bytes, offset, out readSize);
                    offset += readSize;
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
            offset += MessagePackBinary.WriteDateTime(ref bytes, offset, value);
        }

        public override DateTime Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadDateTime(bytes, offset, out var readSize);
			offset += readSize;
			return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteDateTime(ref bytes, offset, value.Value);
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
                var result = MessagePackBinary.ReadDateTime(bytes, offset, out var readSize);
				offset += readSize;
				return result;
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
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    offset += MessagePackBinary.WriteDateTime(ref bytes, offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
                offset += readSize;
                var array = new DateTime[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
                    offset += readSize;
                }
                return array;
            }
        }
    }

}