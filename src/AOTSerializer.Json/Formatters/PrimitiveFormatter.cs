using AOTSerializer.Common;
using AOTSerializer.Json.Internal;
using System;
using System.Collections.Generic;

namespace AOTSerializer.Json.Formatters
{
    public sealed class Int16Formatter : FormatterBase<Int16>, IObjectPropertyNameFormatter<Int16>
    {
        public static readonly Int16Formatter Default = new Int16Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int16 value, IResolver resolver)
        {
            JsonUtility.WriteInt16(ref bytes, ref offset, value);
        }

        public override Int16 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadInt16(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Int16 value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt16(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Int16 DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadInt16(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableInt16Formatter : FormatterBase<Int16?>, IObjectPropertyNameFormatter<Int16?>
    {
        public static readonly NullableInt16Formatter Default = new NullableInt16Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int16? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteInt16(ref bytes, ref offset, value.Value);
            }
        }

        public override Int16? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadInt16(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Int16? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt16(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Int16? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadInt16(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class Int16ArrayFormatter : FormatterBase<Int16[]>
    {
        public static readonly Int16ArrayFormatter Default = new Int16ArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int16[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteInt16(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteInt16(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Int16[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<Int16>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadInt16(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadInt16(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class Int32Formatter : FormatterBase<Int32>, IObjectPropertyNameFormatter<Int32>
    {
        public static readonly Int32Formatter Default = new Int32Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int32 value, IResolver resolver)
        {
            JsonUtility.WriteInt32(ref bytes, ref offset, value);
        }

        public override Int32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadInt32(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Int32 value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt32(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Int32 DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadInt32(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableInt32Formatter : FormatterBase<Int32?>, IObjectPropertyNameFormatter<Int32?>
    {
        public static readonly NullableInt32Formatter Default = new NullableInt32Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int32? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteInt32(ref bytes, ref offset, value.Value);
            }
        }

        public override Int32? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadInt32(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Int32? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Int32? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadInt32(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class Int32ArrayFormatter : FormatterBase<Int32[]>
    {
        public static readonly Int32ArrayFormatter Default = new Int32ArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int32[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteInt32(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteInt32(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Int32[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<Int32>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadInt32(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadInt32(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class Int64Formatter : FormatterBase<Int64>, IObjectPropertyNameFormatter<Int64>
    {
        public static readonly Int64Formatter Default = new Int64Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int64 value, IResolver resolver)
        {
            JsonUtility.WriteInt64(ref bytes, ref offset, value);
        }

        public override Int64 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadInt64(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Int64 value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt64(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Int64 DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadInt64(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableInt64Formatter : FormatterBase<Int64?>, IObjectPropertyNameFormatter<Int64?>
    {
        public static readonly NullableInt64Formatter Default = new NullableInt64Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int64? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteInt64(ref bytes, ref offset, value.Value);
            }
        }

        public override Int64? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadInt64(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Int64? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt64(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Int64? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadInt64(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class Int64ArrayFormatter : FormatterBase<Int64[]>
    {
        public static readonly Int64ArrayFormatter Default = new Int64ArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Int64[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteInt64(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteInt64(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Int64[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<Int64>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadInt64(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadInt64(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class UInt16Formatter : FormatterBase<UInt16>, IObjectPropertyNameFormatter<UInt16>
    {
        public static readonly UInt16Formatter Default = new UInt16Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt16 value, IResolver resolver)
        {
            JsonUtility.WriteUInt16(ref bytes, ref offset, value);
        }

        public override UInt16 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadUInt16(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, UInt16 value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteUInt16(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public UInt16 DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadUInt16(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableUInt16Formatter : FormatterBase<UInt16?>, IObjectPropertyNameFormatter<UInt16?>
    {
        public static readonly NullableUInt16Formatter Default = new NullableUInt16Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt16? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteUInt16(ref bytes, ref offset, value.Value);
            }
        }

        public override UInt16? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadUInt16(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, UInt16? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteUInt16(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public UInt16? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadUInt16(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class UInt16ArrayFormatter : FormatterBase<UInt16[]>
    {
        public static readonly UInt16ArrayFormatter Default = new UInt16ArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt16[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteUInt16(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteUInt16(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override UInt16[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<UInt16>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadUInt16(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadUInt16(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class UInt32Formatter : FormatterBase<UInt32>, IObjectPropertyNameFormatter<UInt32>
    {
        public static readonly UInt32Formatter Default = new UInt32Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt32 value, IResolver resolver)
        {
            JsonUtility.WriteUInt32(ref bytes, ref offset, value);
        }

        public override UInt32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadUInt32(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, UInt32 value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteUInt32(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public UInt32 DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadUInt32(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableUInt32Formatter : FormatterBase<UInt32?>, IObjectPropertyNameFormatter<UInt32?>
    {
        public static readonly NullableUInt32Formatter Default = new NullableUInt32Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt32? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteUInt32(ref bytes, ref offset, value.Value);
            }
        }

        public override UInt32? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadUInt32(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, UInt32? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteUInt32(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public UInt32? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadUInt32(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class UInt32ArrayFormatter : FormatterBase<UInt32[]>
    {
        public static readonly UInt32ArrayFormatter Default = new UInt32ArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt32[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteUInt32(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteUInt32(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override UInt32[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<UInt32>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadUInt32(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadUInt32(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class UInt64Formatter : FormatterBase<UInt64>, IObjectPropertyNameFormatter<UInt64>
    {
        public static readonly UInt64Formatter Default = new UInt64Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt64 value, IResolver resolver)
        {
            JsonUtility.WriteUInt64(ref bytes, ref offset, value);
        }

        public override UInt64 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadUInt64(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, UInt64 value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteUInt64(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public UInt64 DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadUInt64(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableUInt64Formatter : FormatterBase<UInt64?>, IObjectPropertyNameFormatter<UInt64?>
    {
        public static readonly NullableUInt64Formatter Default = new NullableUInt64Formatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt64? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteUInt64(ref bytes, ref offset, value.Value);
            }
        }

        public override UInt64? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadUInt64(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, UInt64? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteUInt64(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public UInt64? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadUInt64(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class UInt64ArrayFormatter : FormatterBase<UInt64[]>
    {
        public static readonly UInt64ArrayFormatter Default = new UInt64ArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, UInt64[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteUInt64(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteUInt64(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override UInt64[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<UInt64>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadUInt64(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadUInt64(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class SingleFormatter : FormatterBase<Single>, IObjectPropertyNameFormatter<Single>
    {
        public static readonly SingleFormatter Default = new SingleFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Single value, IResolver resolver)
        {
            JsonUtility.WriteSingle(ref bytes, ref offset, value);
        }

        public override Single Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadSingle(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Single value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteSingle(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Single DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadSingle(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableSingleFormatter : FormatterBase<Single?>, IObjectPropertyNameFormatter<Single?>
    {
        public static readonly NullableSingleFormatter Default = new NullableSingleFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Single? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteSingle(ref bytes, ref offset, value.Value);
            }
        }

        public override Single? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadSingle(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Single? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Single? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadSingle(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class SingleArrayFormatter : FormatterBase<Single[]>
    {
        public static readonly SingleArrayFormatter Default = new SingleArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Single[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteSingle(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteSingle(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Single[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<Single>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadSingle(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadSingle(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class DoubleFormatter : FormatterBase<Double>, IObjectPropertyNameFormatter<Double>
    {
        public static readonly DoubleFormatter Default = new DoubleFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Double value, IResolver resolver)
        {
            JsonUtility.WriteDouble(ref bytes, ref offset, value);
        }

        public override Double Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadDouble(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Double value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteDouble(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Double DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadDouble(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableDoubleFormatter : FormatterBase<Double?>, IObjectPropertyNameFormatter<Double?>
    {
        public static readonly NullableDoubleFormatter Default = new NullableDoubleFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Double? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteDouble(ref bytes, ref offset, value.Value);
            }
        }

        public override Double? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadDouble(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Double? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteDouble(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Double? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadDouble(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class DoubleArrayFormatter : FormatterBase<Double[]>
    {
        public static readonly DoubleArrayFormatter Default = new DoubleArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Double[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteDouble(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteDouble(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Double[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<Double>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadDouble(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadDouble(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class BooleanFormatter : FormatterBase<Boolean>, IObjectPropertyNameFormatter<Boolean>
    {
        public static readonly BooleanFormatter Default = new BooleanFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Boolean value, IResolver resolver)
        {
            JsonUtility.WriteBoolean(ref bytes, ref offset, value);
        }

        public override Boolean Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadBoolean(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Boolean value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteBoolean(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Boolean DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadBoolean(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableBooleanFormatter : FormatterBase<Boolean?>, IObjectPropertyNameFormatter<Boolean?>
    {
        public static readonly NullableBooleanFormatter Default = new NullableBooleanFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Boolean? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteBoolean(ref bytes, ref offset, value.Value);
            }
        }

        public override Boolean? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadBoolean(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Boolean? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteBoolean(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Boolean? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadBoolean(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class BooleanArrayFormatter : FormatterBase<Boolean[]>
    {
        public static readonly BooleanArrayFormatter Default = new BooleanArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Boolean[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteBoolean(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteBoolean(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Boolean[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<Boolean>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadBoolean(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadBoolean(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class ByteFormatter : FormatterBase<Byte>, IObjectPropertyNameFormatter<Byte>
    {
        public static readonly ByteFormatter Default = new ByteFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Byte value, IResolver resolver)
        {
            JsonUtility.WriteByte(ref bytes, ref offset, value);
        }

        public override Byte Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadByte(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Byte value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteByte(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Byte DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadByte(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableByteFormatter : FormatterBase<Byte?>, IObjectPropertyNameFormatter<Byte?>
    {
        public static readonly NullableByteFormatter Default = new NullableByteFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Byte? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteByte(ref bytes, ref offset, value.Value);
            }
        }

        public override Byte? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadByte(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Byte? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteByte(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public Byte? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadByte(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }


    public sealed class SByteFormatter : FormatterBase<SByte>, IObjectPropertyNameFormatter<SByte>
    {
        public static readonly SByteFormatter Default = new SByteFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, SByte value, IResolver resolver)
        {
            JsonUtility.WriteSByte(ref bytes, ref offset, value);
        }

        public override SByte Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadSByte(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, SByte value, IResolver resolver)
        {
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteSByte(ref bytes, ref offset, value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public SByte DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadSByte(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class NullableSByteFormatter : FormatterBase<SByte?>, IObjectPropertyNameFormatter<SByte?>
    {
        public static readonly NullableSByteFormatter Default = new NullableSByteFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, SByte? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteSByte(ref bytes, ref offset, value.Value);
            }
        }

        public override SByte? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return JsonUtility.ReadSByte(bytes, ref offset);
            }
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, SByte? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteSByte(ref bytes, ref offset, value.Value);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public SByte? DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var key = JsonUtility.ReadStringSegmentRaw(bytes, ref offset);
            var value = NumberConverter.ReadSByte(key.Array, key.Offset, out var readCount);
            offset += readCount;
            return value;
        }
    }

    public sealed class SByteArrayFormatter : FormatterBase<SByte[]>
    {
        public static readonly SByteArrayFormatter Default = new SByteArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, SByte[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteSByte(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteSByte(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override SByte[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<SByte>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadSByte(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadSByte(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

}