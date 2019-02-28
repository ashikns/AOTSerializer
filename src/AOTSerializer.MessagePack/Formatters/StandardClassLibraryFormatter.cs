using AOTSerializer.Common;
using AOTSerializer.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOTSerializer.MessagePack.Formatters
{
    // NET40 -> BigInteger, Complex, Tuple

    // byte[] is special. represents bin type.
    public sealed class ByteArrayFormatter : FormatterBase<byte[]>
    {
        public static readonly ByteArrayFormatter Instance = new ByteArrayFormatter();

        private ByteArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, byte[] value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteBytes(ref bytes, offset, value);
        }

        public override byte[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadBytes(bytes, offset, out var readSize);
            offset += readSize;
            return result;
        }
    }

    public sealed class NullableStringFormatter : FormatterBase<String>
    {
        public static readonly NullableStringFormatter Instance = new NullableStringFormatter();

        private NullableStringFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, String value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteString(ref bytes, offset, value);
        }

        public override String Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadString(bytes, offset, out var readSize);
            offset += readSize;
            return result;
        }
    }

    public sealed class NullableStringArrayFormatter : FormatterBase<String[]>
    {
        public static readonly NullableStringArrayFormatter Instance = new NullableStringArrayFormatter();

        private NullableStringArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, String[] value, IResolver resolver)
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
                    offset += MessagePackBinary.WriteString(ref bytes, offset, value[i]);
                }
            }
        }

        public override String[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
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
                var array = new String[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                }
                return array;
            }
        }
    }

    public sealed class DecimalFormatter : FormatterBase<Decimal>
    {
        public static readonly DecimalFormatter Instance = new DecimalFormatter();

        private DecimalFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, decimal value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.ToString(CultureInfo.InvariantCulture));
        }

        public override decimal Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = decimal.Parse(MessagePackBinary.ReadString(bytes, offset, out var readSize), CultureInfo.InvariantCulture);
            offset += readSize;
            return result;
        }
    }

    public sealed class TimeSpanFormatter : FormatterBase<TimeSpan>
    {
        public static readonly IFormatter<TimeSpan> Instance = new TimeSpanFormatter();

        private TimeSpanFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, TimeSpan value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteInt64(ref bytes, offset, value.Ticks);
        }

        public override TimeSpan Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadInt64(bytes, offset, out var readSize);
            offset += readSize;
            return new TimeSpan(result);
        }
    }

    public sealed class DateTimeOffsetFormatter : FormatterBase<DateTimeOffset>
    {
        public static readonly IFormatter<DateTimeOffset> Instance = new DateTimeOffsetFormatter();

        private DateTimeOffsetFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, DateTimeOffset value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
            offset += MessagePackBinary.WriteDateTime(ref bytes, offset, new DateTime(value.Ticks, DateTimeKind.Utc)); // current ticks as is
            offset += MessagePackBinary.WriteInt16(ref bytes, offset, (short)value.Offset.TotalMinutes); // offset is normalized in minutes
        }

        public override DateTimeOffset Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
            offset += readSize;

            if (count != 2) throw new InvalidOperationException("Invalid DateTimeOffset format.");

            var utc = MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
            offset += readSize;

            var dtOffsetMinutes = MessagePackBinary.ReadInt16(bytes, offset, out readSize);
            offset += readSize;

            return new DateTimeOffset(utc.Ticks, TimeSpan.FromMinutes(dtOffsetMinutes));
        }
    }

    public sealed class GuidFormatter : FormatterBase<Guid>
    {
        public static readonly IFormatter<Guid> Instance = new GuidFormatter();

        private GuidFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Guid value, IResolver resolver)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 38);

            bytes[offset] = MessagePackCode.Str8;
            bytes[offset + 1] = unchecked((byte)36);
            new GuidBits(ref value).Write(bytes, offset + 2);
            offset += 38;
        }

        public override Guid Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var segment = MessagePackBinary.ReadStringSegment(bytes, offset, out var readSize);
            offset += readSize;
            return new GuidBits(segment).Value;
        }
    }

    public sealed class UriFormatter : FormatterBase<Uri>
    {
        public static readonly IFormatter<Uri> Instance = new UriFormatter();

        private UriFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Uri value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteString(ref bytes, offset, value.ToString());
            }
        }

        public override Uri Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var result = MessagePackBinary.ReadString(bytes, offset, out var readSize);
                offset += readSize;
                return new Uri(result, UriKind.RelativeOrAbsolute);
            }
        }
    }

    public sealed class VersionFormatter : FormatterBase<Version>
    {
        public static readonly IFormatter<Version> Instance = new VersionFormatter();

        private VersionFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Version value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteString(ref bytes, offset, value.ToString());
            }
        }

        public override Version Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var result = MessagePackBinary.ReadString(bytes, offset, out var readSize);
                offset += readSize;
                return new Version(result);
            }
        }
    }

    public sealed class KeyValuePairFormatter<TKey, TValue> : FormatterBase<KeyValuePair<TKey, TValue>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, KeyValuePair<TKey, TValue> value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
            resolver.GetFormatter<TKey>().Serialize(ref bytes, ref offset, value.Key, resolver);
            resolver.GetFormatter<TValue>().Serialize(ref bytes, ref offset, value.Value, resolver);
        }

        public override KeyValuePair<TKey, TValue> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
            offset += readSize;

            if (count != 2) throw new InvalidOperationException("Invalid KeyValuePair format.");

            var key = resolver.GetFormatter<TKey>().Deserialize(bytes, ref offset, resolver);
            var value = resolver.GetFormatter<TValue>().Deserialize(bytes, ref offset, resolver);

            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }

    public sealed class StringBuilderFormatter : FormatterBase<StringBuilder>
    {
        public static readonly IFormatter<StringBuilder> Instance = new StringBuilderFormatter();

        private StringBuilderFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, StringBuilder value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteString(ref bytes, offset, value.ToString());
            }
        }

        public override StringBuilder Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var result = MessagePackBinary.ReadString(bytes, offset, out var readSize);
                offset += readSize;
                return new StringBuilder(result);
            }
        }
    }

    public sealed class BitArrayFormatter : FormatterBase<BitArray>
    {
        public static readonly IFormatter<BitArray> Instance = new BitArrayFormatter();

        private BitArrayFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, BitArray value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var len = value.Length;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, len);
                for (int i = 0; i < len; i++)
                {
                    offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Get(i));
                }
            }
        }

        public override BitArray Deserialize(byte[] bytes, ref int offset, IResolver resolver)
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

                var array = new BitArray(len);
                for (int i = 0; i < len; i++)
                {
                    array[i] = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                    offset += readSize;
                }

                return array;
            }
        }
    }

    public sealed class TypeFormatter : FormatterBase<Type>
    {
        public static readonly TypeFormatter Default = new TypeFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Type value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                offset += MessagePackBinary.WriteString(ref bytes, offset, value.FullName);
            }
        }

        public override Type Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var result = MessagePackBinary.ReadString(bytes, offset, out var readSize);
                offset += readSize;
                return Type.GetType(result, true);
            }
        }
    }

    public sealed class BigIntegerFormatter : FormatterBase<System.Numerics.BigInteger>
    {
        public static readonly IFormatter<System.Numerics.BigInteger> Instance = new BigIntegerFormatter();

        private BigIntegerFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, System.Numerics.BigInteger value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteBytes(ref bytes, offset, value.ToByteArray());
        }

        public override System.Numerics.BigInteger Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var result = MessagePackBinary.ReadBytes(bytes, offset, out var readSize);
            offset += readSize;
            return new System.Numerics.BigInteger(result);
        }
    }

    public sealed class ComplexFormatter : FormatterBase<System.Numerics.Complex>
    {
        public static readonly IFormatter<System.Numerics.Complex> Instance = new ComplexFormatter();

        private ComplexFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, System.Numerics.Complex value, IResolver resolver)
        {
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Real);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Imaginary);
        }

        public override System.Numerics.Complex Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out var readSize);
            offset += readSize;

            if (count != 2) throw new InvalidOperationException("Invalid Complex format.");

            var real = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
            offset += readSize;

            var imaginary = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
            offset += readSize;

            return new System.Numerics.Complex(real, imaginary);
        }
    }

    public sealed class LazyFormatter<T> : FormatterBase<Lazy<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Lazy<T> value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                resolver.GetFormatter<T>().Serialize(ref bytes, ref offset, value.Value, resolver);
            }
        }

        public override Lazy<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                // deserialize immediately(no delay, because capture byte[] causes memory leak)
                var v = resolver.GetFormatter<T>().Deserialize(bytes, ref offset, resolver);
                return new Lazy<T>(() => v);
            }
        }
    }

    public sealed class TaskUnitFormatter : FormatterBase<Task>
    {
        public static readonly IFormatter<Task> Instance = new TaskUnitFormatter();
        private static readonly Task CompletedTask = Task.FromResult<object>(null);

        private TaskUnitFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Task value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                value.Wait(); // wait...!
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
        }

        public override Task Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (!MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("Invalid input");
            }
            else
            {
                offset += 1;
                return CompletedTask;
            }
        }
    }

    public sealed class TaskValueFormatter<T> : FormatterBase<Task<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Task<T> value, IResolver resolver)
        {
            if (value == null)
            {
                offset += MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                // value.Result -> wait...!
                resolver.GetFormatter<T>().Serialize(ref bytes, ref offset, value.Result, resolver);
            }
        }

        public override Task<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var v = resolver.GetFormatter<T>().Deserialize(bytes, ref offset, resolver);
                return Task.FromResult(v);
            }
        }
    }

    public sealed class ValueTaskFormatter<T> : FormatterBase<ValueTask<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, ValueTask<T> value, IResolver resolver)
        {
            resolver.GetFormatter<T>().Serialize(ref bytes, ref offset, value.Result, resolver);
        }

        public override ValueTask<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var v = resolver.GetFormatter<T>().Deserialize(bytes, ref offset, resolver);
            return new ValueTask<T>(v);
        }
    }
}