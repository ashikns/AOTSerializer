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

        public override int Serialize(ref byte[] bytes, int offset, byte[] value, IResolver resolver)
        {
            return MessagePackBinary.WriteBytes(ref bytes, offset, value);
        }

        public override byte[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadBytes(bytes, offset, out readSize);
        }
    }

    public sealed class NullableStringFormatter : FormatterBase<String>
    {
        public static readonly NullableStringFormatter Instance = new NullableStringFormatter();

        private NullableStringFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, String value, IResolver resolver)
        {
            return MessagePackBinary.WriteString(ref bytes, offset, value);
        }

        public override String Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return MessagePackBinary.ReadString(bytes, offset, out readSize);
        }
    }

    public sealed class NullableStringArrayFormatter : FormatterBase<String[]>
    {
        public static readonly NullableStringArrayFormatter Instance = new NullableStringArrayFormatter();

        private NullableStringArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, String[] value, IResolver resolver)
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
                    offset += MessagePackBinary.WriteString(ref bytes, offset, value[i]);
                }

                return offset - startOffset;
            }
        }

        public override String[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                var array = new String[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                }
                readSize = offset - startOffset;
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

        public override int Serialize(ref byte[] bytes, int offset, decimal value, IResolver resolver)
        {
            return MessagePackBinary.WriteString(ref bytes, offset, value.ToString(CultureInfo.InvariantCulture));
        }

        public override decimal Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return decimal.Parse(MessagePackBinary.ReadString(bytes, offset, out readSize), CultureInfo.InvariantCulture);
        }
    }

    public sealed class TimeSpanFormatter : FormatterBase<TimeSpan>
    {
        public static readonly IFormatter<TimeSpan> Instance = new TimeSpanFormatter();

        private TimeSpanFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, TimeSpan value, IResolver resolver)
        {
            return MessagePackBinary.WriteInt64(ref bytes, offset, value.Ticks);
        }

        public override TimeSpan Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return new TimeSpan(MessagePackBinary.ReadInt64(bytes, offset, out readSize));
        }
    }

    public sealed class DateTimeOffsetFormatter : FormatterBase<DateTimeOffset>
    {
        public static readonly IFormatter<DateTimeOffset> Instance = new DateTimeOffsetFormatter();

        private DateTimeOffsetFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, DateTimeOffset value, IResolver resolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
            offset += MessagePackBinary.WriteDateTime(ref bytes, offset, new DateTime(value.Ticks, DateTimeKind.Utc)); // current ticks as is
            offset += MessagePackBinary.WriteInt16(ref bytes, offset, (short)value.Offset.TotalMinutes); // offset is normalized in minutes
            return offset - startOffset;
        }

        public override DateTimeOffset Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            var startOffset = offset;
            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            if (count != 2) throw new InvalidOperationException("Invalid DateTimeOffset format.");

            var utc = MessagePackBinary.ReadDateTime(bytes, offset, out readSize);
            offset += readSize;

            var dtOffsetMinutes = MessagePackBinary.ReadInt16(bytes, offset, out readSize);
            offset += readSize;

            readSize = offset - startOffset;

            return new DateTimeOffset(utc.Ticks, TimeSpan.FromMinutes(dtOffsetMinutes));
        }
    }

    public sealed class GuidFormatter : FormatterBase<Guid>
    {
        public static readonly IFormatter<Guid> Instance = new GuidFormatter();

        private GuidFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Guid value, IResolver resolver)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 38);

            bytes[offset] = MessagePackCode.Str8;
            bytes[offset + 1] = unchecked((byte)36);
            new GuidBits(ref value).Write(bytes, offset + 2);
            return 38;
        }

        public override Guid Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            var segment = MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
            return new GuidBits(segment).Value;
        }
    }

    public sealed class UriFormatter : FormatterBase<Uri>
    {
        public static readonly IFormatter<Uri> Instance = new UriFormatter();

        private UriFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Uri value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteString(ref bytes, offset, value.ToString());
            }
        }

        public override Uri Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return new Uri(MessagePackBinary.ReadString(bytes, offset, out readSize), UriKind.RelativeOrAbsolute);
            }
        }
    }

    public sealed class VersionFormatter : FormatterBase<Version>
    {
        public static readonly IFormatter<Version> Instance = new VersionFormatter();

        private VersionFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, Version value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteString(ref bytes, offset, value.ToString());
            }
        }

        public override Version Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return new Version(MessagePackBinary.ReadString(bytes, offset, out readSize));
            }
        }
    }

    public sealed class KeyValuePairFormatter<TKey, TValue> : FormatterBase<KeyValuePair<TKey, TValue>>
    {
        public override int Serialize(ref byte[] bytes, int offset, KeyValuePair<TKey, TValue> value, IResolver resolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
            offset += resolver.GetFormatter<TKey>().Serialize(ref bytes, offset, value.Key, resolver);
            offset += resolver.GetFormatter<TValue>().Serialize(ref bytes, offset, value.Value, resolver);
            return offset - startOffset;
        }

        public override KeyValuePair<TKey, TValue> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            var startOffset = offset;
            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            if (count != 2) throw new InvalidOperationException("Invalid KeyValuePair format.");

            var key = resolver.GetFormatter<TKey>().Deserialize(bytes, offset, out readSize, resolver);
            offset += readSize;

            var value = resolver.GetFormatter<TValue>().Deserialize(bytes, offset, out readSize, resolver);
            offset += readSize;

            readSize = offset - startOffset;
            return new KeyValuePair<TKey, TValue>(key, value);
        }
    }

    public sealed class StringBuilderFormatter : FormatterBase<StringBuilder>
    {
        public static readonly IFormatter<StringBuilder> Instance = new StringBuilderFormatter();

        private StringBuilderFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, StringBuilder value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteString(ref bytes, offset, value.ToString());
            }
        }

        public override StringBuilder Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return new StringBuilder(MessagePackBinary.ReadString(bytes, offset, out readSize));
            }
        }
    }

    public sealed class BitArrayFormatter : FormatterBase<BitArray>
    {
        public static readonly IFormatter<BitArray> Instance = new BitArrayFormatter();

        private BitArrayFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, BitArray value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                var len = value.Length;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, len);
                for (int i = 0; i < len; i++)
                {
                    offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.Get(i));
                }

                return offset - startOffset;
            }
        }

        public override BitArray Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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

                var array = new BitArray(len);
                for (int i = 0; i < len; i++)
                {
                    array[i] = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                    offset += readSize;
                }

                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class TypeFormatter : FormatterBase<Type>
    {
        public static readonly TypeFormatter Default = new TypeFormatter();

        public override int Serialize(ref byte[] bytes, int offset, Type value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteString(ref bytes, offset, value.FullName);
            }
        }

        public override Type Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                return Type.GetType(MessagePackBinary.ReadString(bytes, offset, out readSize), true);
            }
        }
    }

    public sealed class BigIntegerFormatter : FormatterBase<System.Numerics.BigInteger>
    {
        public static readonly IFormatter<System.Numerics.BigInteger> Instance = new BigIntegerFormatter();

        private BigIntegerFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, System.Numerics.BigInteger value, IResolver resolver)
        {
            return MessagePackBinary.WriteBytes(ref bytes, offset, value.ToByteArray());
        }

        public override System.Numerics.BigInteger Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            return new System.Numerics.BigInteger(MessagePackBinary.ReadBytes(bytes, offset, out readSize));
        }
    }

    public sealed class ComplexFormatter : FormatterBase<System.Numerics.Complex>
    {
        public static readonly IFormatter<System.Numerics.Complex> Instance = new ComplexFormatter();

        private ComplexFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, System.Numerics.Complex value, IResolver resolver)
        {
            var startOffset = offset;
            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Real);
            offset += MessagePackBinary.WriteDouble(ref bytes, offset, value.Imaginary);
            return offset - startOffset;
        }

        public override System.Numerics.Complex Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            var startOffset = offset;
            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            if (count != 2) throw new InvalidOperationException("Invalid Complex format.");

            var real = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
            offset += readSize;

            var imaginary = MessagePackBinary.ReadDouble(bytes, offset, out readSize);
            offset += readSize;

            readSize = offset - startOffset;
            return new System.Numerics.Complex(real, imaginary);
        }
    }

    public sealed class LazyFormatter<T> : FormatterBase<Lazy<T>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Lazy<T> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return resolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Value, resolver);
            }
        }

        public override Lazy<T> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                // deserialize immediately(no delay, because capture byte[] causes memory leak)
                var v = resolver.GetFormatter<T>().Deserialize(bytes, offset, out readSize, resolver);
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

        public override int Serialize(ref byte[] bytes, int offset, Task value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                value.Wait(); // wait...!
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
        }

        public override Task Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (!MessagePackBinary.IsNil(bytes, offset))
            {
                throw new InvalidOperationException("Invalid input");
            }
            else
            {
                readSize = 1;
                return CompletedTask;
            }
        }
    }

    public sealed class TaskValueFormatter<T> : FormatterBase<Task<T>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Task<T> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                // value.Result -> wait...!
                return resolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Result, resolver);
            }
        }

        public override Task<T> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var v = resolver.GetFormatter<T>().Deserialize(bytes, offset, out readSize, resolver);
                return Task.FromResult(v);
            }
        }
    }

    public sealed class ValueTaskFormatter<T> : FormatterBase<ValueTask<T>>
    {
        public override int Serialize(ref byte[] bytes, int offset, ValueTask<T> value, IResolver resolver)
        {
            return resolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Result, resolver);
        }

        public override ValueTask<T> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            var v = resolver.GetFormatter<T>().Deserialize(bytes, offset, out readSize, resolver);
            return new ValueTask<T>(v);
        }
    }
}