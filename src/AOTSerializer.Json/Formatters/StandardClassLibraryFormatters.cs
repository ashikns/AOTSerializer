using AOTSerializer.Common;
using AOTSerializer.Internal;
using AOTSerializer.Json.Formatters.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOTSerializer.Json.Formatters
{
    // MEMO:should write/read base64 directly like corefxlab/System.Binary.Base64
    // https://github.com/dotnet/corefxlab/tree/master/src/System.Binary.Base64/System/Binary
    public sealed class ByteArrayFormatter : FormatterBase<byte[]>
    {
        public static readonly IFormatter<byte[]> Default = new ByteArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, byte[] value, IResolver resolver)
        {
            if (value == null) { JsonUtility.WriteNull(ref bytes, ref offset); return; }
            JsonUtility.WriteString(ref bytes, ref offset, Convert.ToBase64String(value, Base64FormattingOptions.None));
        }

        public override byte[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }
            return Convert.FromBase64String(JsonUtility.ReadString(bytes, ref offset));
        }
    }

    public sealed class ByteArraySegmentFormatter : FormatterBase<ArraySegment<byte>>
    {
        public static readonly IFormatter<ArraySegment<byte>> Default = new ByteArraySegmentFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, ArraySegment<byte> value, IResolver resolver)
        {
            if (value.Array == null) { JsonUtility.WriteNull(ref bytes, ref offset); return; }
            JsonUtility.WriteString(ref bytes, ref offset, Convert.ToBase64String(value.Array, value.Offset, value.Count, Base64FormattingOptions.None));
        }

        public override ArraySegment<byte> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return default; }

            var str = JsonUtility.ReadString(bytes, ref offset);
            return new ArraySegment<byte>(Convert.FromBase64String(str));
        }
    }

    public sealed class NullableStringFormatter : FormatterBase<string>, IObjectPropertyNameFormatter<string>
    {
        public static readonly IFormatter<string> Default = new NullableStringFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, string value, IResolver resolver)
        {
            JsonUtility.WriteString(ref bytes, ref offset, value);
        }

        public override string Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadString(bytes, ref offset);
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, string value, IResolver resolver)
        {
            JsonUtility.WriteString(ref bytes, ref offset, value);
        }

        public string DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadString(bytes, ref offset);
        }
    }

    public sealed class NullableStringArrayFormatter : FormatterBase<string[]>
    {
        public static readonly NullableStringArrayFormatter Default = new NullableStringArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, string[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                JsonUtility.WriteString(ref bytes, ref offset, value[0]);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteString(ref bytes, ref offset, value[i]);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override string[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<string>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(JsonUtility.ReadString(bytes, ref offset));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(JsonUtility.ReadString(bytes, ref offset));
            }

            return result.ToArray();
        }
    }

    public sealed class CharFormatter : FormatterBase<char>
    {
        public static readonly CharFormatter Default = new CharFormatter();

        // MEMO:can be improvement write directly
        public override void Serialize(ref byte[] bytes, ref int offset, char value, IResolver resolver)
        {
            JsonUtility.WriteString(ref bytes, ref offset, value.ToString(CultureInfo.InvariantCulture));
        }

        public override char Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return JsonUtility.ReadString(bytes, ref offset)[0];
        }
    }

    public sealed class NullableCharFormatter : FormatterBase<Char?>
    {
        public static readonly NullableCharFormatter Default = new NullableCharFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Char? value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                CharFormatter.Default.Serialize(ref bytes, ref offset, value.Value, resolver);
            }
        }

        public override Char? Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return CharFormatter.Default.Deserialize(bytes, ref offset, resolver);
            }
        }
    }

    public sealed class CharArrayFormatter : FormatterBase<char[]>
    {
        public static readonly CharArrayFormatter Default = new CharArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, char[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            if (value.Length != 0)
            {
                CharFormatter.Default.Serialize(ref bytes, ref offset, value[0], resolver);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                CharFormatter.Default.Serialize(ref bytes, ref offset, value[i], resolver);
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override char[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<char>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return result.ToArray();
            }
            else
            {
                result.Add(CharFormatter.Default.Deserialize(bytes, ref offset, resolver));
            }

            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
                result.Add(CharFormatter.Default.Deserialize(bytes, ref offset, resolver));
            }

            return result.ToArray();
        }
    }

    public sealed class GuidFormatter : FormatterBase<Guid>, IObjectPropertyNameFormatter<Guid>
    {
        public static readonly IFormatter<Guid> Default = new GuidFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Guid value, IResolver resolver)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 38);

            JsonUtility.WriteRawUnsafe(ref bytes, ref offset, (byte)'\"');

            new GuidBits(ref value).Write(bytes, ref offset); // len = 36

            JsonUtility.WriteRawUnsafe(ref bytes, ref offset, (byte)'\"');
        }

        public override Guid Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var segment = JsonUtility.ReadStringSegment(bytes, ref offset);
            return new GuidBits(segment).Value;
        }

        public void SerializeToPropertyName(ref byte[] bytes, ref int offset, Guid value, IResolver resolver)
        {
            Serialize(ref bytes, ref offset, value, resolver);
        }

        public Guid DeserializeFromPropertyName(byte[] bytes, ref int offset, IResolver resolver)
        {
            return Deserialize(bytes, ref offset, resolver);
        }
    }

    public sealed class DecimalFormatter : FormatterBase<decimal>
    {
        public static readonly IFormatter<decimal> Default = new DecimalFormatter();
        private readonly bool serializeAsString;

        public DecimalFormatter()
            : this(false)
        {

        }

        public DecimalFormatter(bool serializeAsString)
        {
            this.serializeAsString = serializeAsString;
        }

        public override void Serialize(ref byte[] bytes, ref int offset, decimal value, IResolver resolver)
        {
            if (serializeAsString)
            {
                JsonUtility.WriteString(ref bytes, ref offset, value.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                // write as number format.
                JsonUtility.WriteRaw(ref bytes, ref offset, StringEncoding.UTF8.GetBytes(value.ToString(CultureInfo.InvariantCulture)));
            }
        }

        public override decimal Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var token = JsonUtility.GetCurrentJsonToken(bytes, ref offset);
            if (token == JsonToken.Number)
            {
                var number = JsonUtility.ReadNumberSegment(bytes, ref offset);
                return decimal.Parse(StringEncoding.UTF8.GetString(number.Array, number.Offset, number.Count), NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            else if (token == JsonToken.String)
            {
                return decimal.Parse(JsonUtility.ReadString(bytes, ref offset), NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            else
            {
                throw new InvalidOperationException("Invalid Json Token for DecimalFormatter:" + token);
            }
        }
    }

    public sealed class UriFormatter : FormatterBase<Uri>
    {
        public static readonly IFormatter<Uri> Default = new UriFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Uri value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteString(ref bytes, ref offset, value.ToString());
            }
        }

        public override Uri Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return new Uri(JsonUtility.ReadString(bytes, ref offset), UriKind.RelativeOrAbsolute);
            }
        }
    }

    public sealed class VersionFormatter : FormatterBase<Version>
    {
        public static readonly IFormatter<Version> Default = new VersionFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Version value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteString(ref bytes, ref offset, value.ToString());
            }
        }

        public override Version Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }
            else
            {
                return new Version(JsonUtility.ReadString(bytes, ref offset));
            }
        }
    }

    public sealed class KeyValuePairFormatter<TKey, TValue> : FormatterBase<KeyValuePair<TKey, TValue>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, KeyValuePair<TKey, TValue> value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, StandardClassLibraryFormatterHelper.keyValuePairName[0]);
            resolver.GetFormatterWithVerify<TKey>().Serialize(ref bytes, ref offset, value.Key, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, StandardClassLibraryFormatterHelper.keyValuePairName[1]);
            resolver.GetFormatterWithVerify<TValue>().Serialize(ref bytes, ref offset, value.Value, resolver);

            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override KeyValuePair<TKey, TValue> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("Data is Nil, KeyValuePair can not be null.");
            }

            TKey resultKey = default;
            TValue resultValue = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                StandardClassLibraryFormatterHelper.keyValuePairAutomata.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        resultKey = resolver.GetFormatterWithVerify<TKey>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        resultValue = resolver.GetFormatterWithVerify<TValue>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new KeyValuePair<TKey, TValue>(resultKey, resultValue);
        }
    }

    public sealed class StringBuilderFormatter : FormatterBase<StringBuilder>
    {
        public static readonly IFormatter<StringBuilder> Default = new StringBuilderFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, StringBuilder value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteString(ref bytes, ref offset, value.ToString());
            }
        }

        public override StringBuilder Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }
            return new StringBuilder(JsonUtility.ReadString(bytes, ref offset));
        }
    }

    // BitArray can be represents other format...
    public sealed class BitArrayFormatter : FormatterBase<BitArray>
    {
        public static readonly IFormatter<BitArray> Default = new BitArrayFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, BitArray value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            JsonUtility.WriteBoolean(ref bytes, ref offset, value[0]);
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteBoolean(ref bytes, ref offset, value[i]);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override BitArray Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var result = new List<bool>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);

            if (JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                return new BitArray(result.ToArray());
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

            return new BitArray(result.ToArray());
        }
    }

    public sealed class TypeFormatter : FormatterBase<Type>
    {
        public static readonly TypeFormatter Default = new TypeFormatter();

        public TypeFormatter()
        {

        }

        public override void Serialize(ref byte[] bytes, ref int offset, Type value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                JsonUtility.WriteString(ref bytes, ref offset, value.FullName);
            }
        }

        public override Type Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }
            return Type.GetType(JsonUtility.ReadString(bytes, ref offset), true);
        }
    }

    public sealed class BigIntegerFormatter : FormatterBase<BigInteger>
    {
        public static readonly IFormatter<BigInteger> Default = new BigIntegerFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, BigInteger value, IResolver resolver)
        {
            // JSON.NET writes Integer format, not compatible.
            JsonUtility.WriteString(ref bytes, ref offset, value.ToString(CultureInfo.InvariantCulture));
        }

        public override BigInteger Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return BigInteger.Parse(JsonUtility.ReadString(bytes, ref offset), CultureInfo.InvariantCulture);
        }
    }

    // Convert to [Real, Imaginary]
    public sealed class ComplexFormatter : FormatterBase<Complex>
    {
        public static readonly IFormatter<Complex> Default = new ComplexFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, Complex value, IResolver resolver)
        {
            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            JsonUtility.WriteDouble(ref bytes, ref offset, value.Real);
            JsonUtility.WriteValueSeparator(ref bytes, ref offset);
            JsonUtility.WriteDouble(ref bytes, ref offset, value.Imaginary);
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override Complex Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            var real = JsonUtility.ReadDouble(bytes, ref offset);
            JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            var imaginary = JsonUtility.ReadDouble(bytes, ref offset);
            JsonUtility.ReadIsEndArrayWithVerify(bytes, ref offset);

            return new Complex(real, imaginary);
        }
    }

    public sealed class LazyFormatter<T> : FormatterBase<Lazy<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Lazy<T> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                resolver.GetFormatterWithVerify<T>().Serialize(ref bytes, ref offset, value.Value, resolver);
            }
        }

        public override Lazy<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            // deserialize immediately(no delay, because capture byte[] causes memory leak)
            var v = resolver.GetFormatterWithVerify<T>().Deserialize(bytes, ref offset, resolver);
            return new Lazy<T>(() => v);
        }
    }

    public sealed class TaskUnitFormatter : FormatterBase<Task>
    {
        public static readonly IFormatter<Task> Default = new TaskUnitFormatter();
        private static readonly Task CompletedTask = Task.FromResult<object>(null);

        public override void Serialize(ref byte[] bytes, ref int offset, Task value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                value.Wait(); // wait!
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
        }

        public override Task Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (!JsonUtility.ReadIsNull(bytes, ref offset)) { throw new InvalidOperationException("Invalid input"); }
            return CompletedTask;
        }
    }

    public sealed class TaskValueFormatter<T> : FormatterBase<Task<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Task<T> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
            }
            else
            {
                // value.Result -> wait...!
                resolver.GetFormatterWithVerify<T>().Serialize(ref bytes, ref offset, value.Result, resolver);
            }
        }

        public override Task<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var v = resolver.GetFormatterWithVerify<T>().Deserialize(bytes, ref offset, resolver);
            return Task.FromResult(v);
        }
    }

    public sealed class ValueTaskFormatter<T> : FormatterBase<ValueTask<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, ValueTask<T> value, IResolver resolver)
        {
            // value.Result -> wait...!
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
            if (value == null) { JsonUtility.WriteNull(ref bytes, ref offset); return; }

            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var memoryStream = new MemoryStream();
            value.CopyTo(memoryStream);
            var buffer = memoryStream.ToArray();

            JsonUtility.WriteString(ref bytes, ref offset, Convert.ToBase64String(buffer, Base64FormattingOptions.None));
        }

        public override Stream Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            return new MemoryStream(Convert.FromBase64String(JsonUtility.ReadString(bytes, ref offset)));
        }
    }
}

namespace AOTSerializer.Json.Formatters.Internal
{
    internal static class StandardClassLibraryFormatterHelper
    {
        internal static readonly byte[][] keyValuePairName;
        internal static readonly AutomataDictionary keyValuePairAutomata;

        static StandardClassLibraryFormatterHelper()
        {
            keyValuePairName = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Key").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Value").ToArray(),
            };
            keyValuePairAutomata = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Key")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Value")), 1 },
            };
        }
    }
}