using AOTSerializer.Common;
using AOTSerializer.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
            MessagePackBinary.WriteBytes(ref bytes, ref offset, value);
        }

        public override byte[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadBytes(bytes, ref offset);
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
            MessagePackBinary.WriteString(ref bytes, ref offset, value);
        }

        public override String Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return MessagePackBinary.ReadString(bytes, ref offset);
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, value.Length);
                for (int i = 0; i < value.Length; i++)
                {
                    MessagePackBinary.WriteString(ref bytes, ref offset, value[i]);
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                var array = new String[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadString(bytes, ref offset);
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
            MessagePackBinary.WriteString(ref bytes, ref offset, value.ToString(CultureInfo.InvariantCulture));
        }

        public override decimal Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return decimal.Parse(MessagePackBinary.ReadString(bytes, ref offset), CultureInfo.InvariantCulture);
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
            MessagePackBinary.WriteInt64(ref bytes, ref offset, value.Ticks);
        }

        public override TimeSpan Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return new TimeSpan(MessagePackBinary.ReadInt64(bytes, ref offset));
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
            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 2);
            MessagePackBinary.WriteDateTime(ref bytes, ref offset, new DateTime(value.Ticks, DateTimeKind.Utc)); // current ticks as is
            MessagePackBinary.WriteInt16(ref bytes, ref offset, (short)value.Offset.TotalMinutes); // offset is normalized in minutes
        }

        public override DateTimeOffset Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            if (count != 2) throw new InvalidOperationException("Invalid DateTimeOffset format.");

            var utc = MessagePackBinary.ReadDateTime(bytes, ref offset);
            var dtOffsetMinutes = MessagePackBinary.ReadInt16(bytes, ref offset);
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

            bytes[offset++] = MessagePackCode.Str8;
            bytes[offset++] = unchecked((byte)36);
            new GuidBits(ref value).Write(bytes, ref offset);
        }

        public override Guid Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return new GuidBits(MessagePackBinary.ReadStringSegment(bytes, ref offset)).Value;
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteString(ref bytes, ref offset, value.ToString());
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
                return new Uri(MessagePackBinary.ReadString(bytes, ref offset), UriKind.RelativeOrAbsolute);
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteString(ref bytes, ref offset, value.ToString());
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
                return new Version(MessagePackBinary.ReadString(bytes, ref offset));
            }
        }
    }

    public sealed class KeyValuePairFormatter<TKey, TValue> : FormatterBase<KeyValuePair<TKey, TValue>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, KeyValuePair<TKey, TValue> value, IResolver resolver)
        {
            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 2);
            resolver.GetFormatterWithVerify<TKey>().Serialize(ref bytes, ref offset, value.Key, resolver);
            resolver.GetFormatterWithVerify<TValue>().Serialize(ref bytes, ref offset, value.Value, resolver);
        }

        public override KeyValuePair<TKey, TValue> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            if (count != 2) throw new InvalidOperationException("Invalid KeyValuePair format.");

            var key = resolver.GetFormatterWithVerify<TKey>().Deserialize(bytes, ref offset, resolver);
            var value = resolver.GetFormatterWithVerify<TValue>().Deserialize(bytes, ref offset, resolver);
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteString(ref bytes, ref offset, value.ToString());
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
                return new StringBuilder(MessagePackBinary.ReadString(bytes, ref offset));
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                var len = value.Length;
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, len);
                for (int i = 0; i < len; i++)
                {
                    MessagePackBinary.WriteBoolean(ref bytes, ref offset, value.Get(i));
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
                var len = MessagePackBinary.ReadArrayHeader(bytes, ref offset);

                var array = new BitArray(len);
                for (int i = 0; i < len; i++)
                {
                    array[i] = MessagePackBinary.ReadBoolean(bytes, ref offset);
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteString(ref bytes, ref offset, value.FullName);
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
                return Type.GetType(MessagePackBinary.ReadString(bytes, ref offset), true);
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
            MessagePackBinary.WriteBytes(ref bytes, ref offset, value.ToByteArray());
        }

        public override System.Numerics.BigInteger Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return new System.Numerics.BigInteger(MessagePackBinary.ReadBytes(bytes, ref offset));
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
            MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 2);
            MessagePackBinary.WriteDouble(ref bytes, ref offset, value.Real);
            MessagePackBinary.WriteDouble(ref bytes, ref offset, value.Imaginary);
        }

        public override System.Numerics.Complex Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            if (count != 2) throw new InvalidOperationException("Invalid Complex format.");

            var real = MessagePackBinary.ReadDouble(bytes, ref offset);
            var imaginary = MessagePackBinary.ReadDouble(bytes, ref offset);
            return new System.Numerics.Complex(real, imaginary);
        }
    }

    public sealed class LazyFormatter<T> : FormatterBase<Lazy<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Lazy<T> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                resolver.GetFormatterWithVerify<T>().Serialize(ref bytes, ref offset, value.Value, resolver);
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
                var v = resolver.GetFormatterWithVerify<T>().Deserialize(bytes, ref offset, resolver);
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                value.Wait(); // wait...!
                MessagePackBinary.WriteNil(ref bytes, ref offset);
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
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                // value.Result -> wait...!
                resolver.GetFormatterWithVerify<T>().Serialize(ref bytes, ref offset, value.Result, resolver);
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
                var v = resolver.GetFormatterWithVerify<T>().Deserialize(bytes, ref offset, resolver);
                return Task.FromResult(v);
            }
        }
    }

    public sealed class ValueTaskFormatter<T> : FormatterBase<ValueTask<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, ValueTask<T> value, IResolver resolver)
        {
            resolver.GetFormatterWithVerify<T>().Serialize(ref bytes, ref offset, value.Result, resolver);
        }

        public override ValueTask<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var v = resolver.GetFormatterWithVerify<T>().Deserialize(bytes, ref offset, resolver);
            return new ValueTask<T>(v);
        }
    }

    public sealed class StreamFormatter : FormatterBase<Stream>
    {
        public static readonly IFormatter<Stream> Default = new StreamFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Stream value, IResolver resolver)
        {
            var memoryStream = new MemoryStream();
            value.CopyTo(memoryStream);
            var buffer = memoryStream.ToArray();

            MessagePackBinary.WriteBytes(ref bytes, ref offset, buffer);
        }

        public override Stream Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return new MemoryStream(MessagePackBinary.ReadBytes(bytes, ref offset));
        }
    }
}