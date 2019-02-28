using AOTSerializer.Common;
using AOTSerializer.Json.Internal;
using System;
using System.Globalization;

namespace AOTSerializer.Json.Formatters
{
    public sealed class DateTimeFormatter : FormatterBase<DateTime>
    {
        public static readonly IFormatter<DateTime> Default = new DateTimeFormatter();
        public static readonly IFormatter<DateTime> ISO8601 = new DateTimeFormatter("o");

        private readonly string formatString;

        public DateTimeFormatter()
        {
            this.formatString = null;
        }

        public DateTimeFormatter(string formatString)
        {
            this.formatString = formatString;
        }

        public override void Serialize(ref byte[] bytes, ref int offset, DateTime value, IResolver resolver)
        {
            JsonUtility.WriteString(ref bytes, ref offset, value.ToString(formatString));
        }

        public override DateTime Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var str = JsonUtility.ReadString(bytes, ref offset);
            return formatString == null
                ? DateTime.Parse(str, CultureInfo.InvariantCulture)
                : DateTime.ParseExact(str, formatString, CultureInfo.InvariantCulture);
        }
    }

    public sealed class UnixTimestampDateTimeFormatter : FormatterBase<DateTime>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, DateTime value, IResolver resolver)
        {
            var ticks = ((DateTimeOffset)value.ToUniversalTime()).ToUnixTimeSeconds();
            JsonUtility.WriteQuotation(ref bytes, ref offset);
            JsonUtility.WriteInt64(ref bytes, ref offset, ticks);
            JsonUtility.WriteQuotation(ref bytes, ref offset);
        }

        public override DateTime Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var str = JsonUtility.ReadStringSegment(bytes, ref offset);
            var ticks = NumberConverter.ReadInt64(str.Array, str.Offset, out var readCount);
            offset += readCount;
            return DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
        }
    }

    public sealed class DateTimeOffsetFormatter : FormatterBase<DateTimeOffset>
    {
        public static readonly IFormatter<DateTimeOffset> Default = new DateTimeOffsetFormatter();
        public static readonly IFormatter<DateTimeOffset> ISO8601 = new DateTimeOffsetFormatter("o");

        private readonly string formatString;

        public DateTimeOffsetFormatter()
        {
            this.formatString = null;
        }

        public DateTimeOffsetFormatter(string formatString)
        {
            this.formatString = formatString;
        }

        public override void Serialize(ref byte[] bytes, ref int offset, DateTimeOffset value, IResolver resolver)
        {
            JsonUtility.WriteString(ref bytes, ref offset, value.ToString(formatString));
        }

        public override DateTimeOffset Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var str = JsonUtility.ReadString(bytes, ref offset);
            return formatString == null
                ? DateTimeOffset.Parse(str, CultureInfo.InvariantCulture)
                : DateTimeOffset.ParseExact(str, formatString, CultureInfo.InvariantCulture);
        }
    }

    public sealed class TimeSpanFormatter : FormatterBase<TimeSpan>
    {
        public static readonly IFormatter<TimeSpan> Default = new TimeSpanFormatter();
        public static readonly IFormatter<TimeSpan> ISO8601 = new TimeSpanFormatter("o");

        private readonly string formatString;

        public TimeSpanFormatter()
        {
            this.formatString = null;
        }

        public TimeSpanFormatter(string formatString)
        {
            this.formatString = formatString;
        }

        public override void Serialize(ref byte[] bytes, ref int offset, TimeSpan value, IResolver resolver)
        {
            JsonUtility.WriteString(ref bytes, ref offset, value.ToString(formatString));
        }

        public override TimeSpan Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            var str = JsonUtility.ReadString(bytes, ref offset);
            return formatString == null
                ? TimeSpan.Parse(str, CultureInfo.InvariantCulture)
                : TimeSpan.ParseExact(str, formatString, CultureInfo.InvariantCulture);
        }
    }
}
