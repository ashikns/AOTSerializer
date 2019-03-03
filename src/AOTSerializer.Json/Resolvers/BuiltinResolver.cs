using AOTSerializer.Common;
using AOTSerializer.Json.Formatters;
using AOTSerializer.Json.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AOTSerializer.Json.Resolvers
{
    public sealed class BuiltinResolver : CachedResolver
    {
        public static readonly IResolver Instance = new BuiltinResolver();

        private BuiltinResolver()
        {

        }

        protected override IFormatter FindFormatter(Type t)
        {
            return BuiltinResolverGetFormatterHelper.GetFormatter(t);
        }
    }
}

namespace AOTSerializer.Json.Internal
{
    // used from PrimitiveObjectFormatter
    internal static class BuiltinResolverGetFormatterHelper
    {
        private static readonly Dictionary<Type, IFormatter> formatterMap = new Dictionary<Type, IFormatter>()
        {
            {typeof(object), ObjectFallbackFormatter.Default},

            // Primitive
            {typeof(Int16), Int16Formatter.Default},
            {typeof(Int32), Int32Formatter.Default},
            {typeof(Int64), Int64Formatter.Default},
            {typeof(UInt16), UInt16Formatter.Default},
            {typeof(UInt32), UInt32Formatter.Default},
            {typeof(UInt64), UInt64Formatter.Default},
            {typeof(Single), SingleFormatter.Default},
            {typeof(Double), DoubleFormatter.Default},
            {typeof(bool), BooleanFormatter.Default},
            {typeof(byte), ByteFormatter.Default},
            {typeof(sbyte), SByteFormatter.Default},
            {typeof(char), CharFormatter.Default},

            // Nulllable Primitive
            {typeof(Nullable<Int16>), NullableInt16Formatter.Default},
            {typeof(Nullable<Int32>), NullableInt32Formatter.Default},
            {typeof(Nullable<Int64>), NullableInt64Formatter.Default},
            {typeof(Nullable<UInt16>), NullableUInt16Formatter.Default},
            {typeof(Nullable<UInt32>), NullableUInt32Formatter.Default},
            {typeof(Nullable<UInt64>), NullableUInt64Formatter.Default},
            {typeof(Nullable<Single>), NullableSingleFormatter.Default},
            {typeof(Nullable<Double>), NullableDoubleFormatter.Default},
            {typeof(Nullable<bool>), NullableBooleanFormatter.Default},
            {typeof(Nullable<byte>), NullableByteFormatter.Default},
            {typeof(Nullable<sbyte>), NullableSByteFormatter.Default},
            {typeof(Nullable<char>), NullableCharFormatter.Default},
            
            // DateTime
            {typeof(DateTime), DateTimeFormatter.ISO8601},
            {typeof(TimeSpan), TimeSpanFormatter.Default},
            {typeof(DateTimeOffset), DateTimeOffsetFormatter.ISO8601},
            {typeof(DateTime?), new StaticNullableFormatter<DateTime>(DateTimeFormatter.ISO8601)}, // ISO8601
            {typeof(TimeSpan?), new StaticNullableFormatter<TimeSpan>(TimeSpanFormatter.Default)},
            {typeof(DateTimeOffset?),new StaticNullableFormatter<DateTimeOffset>(DateTimeOffsetFormatter.ISO8601)},

            {typeof(string), NullableStringFormatter.Default},
            {typeof(decimal), DecimalFormatter.Default},
            {typeof(decimal?), new StaticNullableFormatter<decimal>(DecimalFormatter.Default)},

            {typeof(Guid), GuidFormatter.Default},
            {typeof(Guid?), new StaticNullableFormatter<Guid>(GuidFormatter.Default)},
            {typeof(Uri), UriFormatter.Default},
            {typeof(Version), VersionFormatter.Default},
            {typeof(StringBuilder), StringBuilderFormatter.Default},
            {typeof(BitArray), BitArrayFormatter.Default},
            {typeof(Type), TypeFormatter.Default},
            
            // special primitive
            {typeof(byte[]), ByteArrayFormatter.Default},
            
            // otpmitized primitive array formatter
            {typeof(Int16[]), Int16ArrayFormatter.Default},
            {typeof(Int32[]), Int32ArrayFormatter.Default},
            {typeof(Int64[]), Int64ArrayFormatter.Default},
            {typeof(UInt16[]), UInt16ArrayFormatter.Default},
            {typeof(UInt32[]), UInt32ArrayFormatter.Default},
            {typeof(UInt64[]), UInt64ArrayFormatter.Default},
            {typeof(Single[]), SingleArrayFormatter.Default},
            {typeof(Double[]), DoubleArrayFormatter.Default},
            {typeof(Boolean[]), BooleanArrayFormatter.Default},
            {typeof(SByte[]), SByteArrayFormatter.Default},
            {typeof(Char[]), CharArrayFormatter.Default},
            {typeof(string[]), NullableStringArrayFormatter.Default},
            {typeof(DateTime[]), new ArrayFormatter<DateTime>()},

            // well known collections
            {typeof(List<Int16>), new ListFormatter<Int16>()},
            {typeof(List<Int32>), new ListFormatter<Int32>()},
            {typeof(List<Int64>), new ListFormatter<Int64>()},
            {typeof(List<UInt16>), new ListFormatter<UInt16>()},
            {typeof(List<UInt32>), new ListFormatter<UInt32>()},
            {typeof(List<UInt64>), new ListFormatter<UInt64>()},
            {typeof(List<Single>), new ListFormatter<Single>()},
            {typeof(List<Double>), new ListFormatter<Double>()},
            {typeof(List<Boolean>), new ListFormatter<Boolean>()},
            {typeof(List<byte>), new ListFormatter<byte>()},
            {typeof(List<SByte>), new ListFormatter<SByte>()},
            {typeof(List<Char>), new ListFormatter<Char>()},
            {typeof(List<string>), new ListFormatter<string>()},
            {typeof(List<DateTime>), new ListFormatter<DateTime>()},

            { typeof(ArraySegment<byte>), ByteArraySegmentFormatter.Default },
            { typeof(ArraySegment<byte>?),new StaticNullableFormatter<ArraySegment<byte>>(ByteArraySegmentFormatter.Default) },

            {typeof(System.Numerics.BigInteger), BigIntegerFormatter.Default},
            {typeof(System.Numerics.BigInteger?), new StaticNullableFormatter<System.Numerics.BigInteger>(BigIntegerFormatter.Default)},
            {typeof(System.Numerics.Complex), ComplexFormatter.Default},
            {typeof(System.Numerics.Complex?), new StaticNullableFormatter<System.Numerics.Complex>(ComplexFormatter.Default)},
            {typeof(System.Threading.Tasks.Task), TaskUnitFormatter.Default},
        };

        internal static IFormatter GetFormatter(Type t)
        {
            formatterMap.TryGetValue(t, out var formatter);
            return formatter;
        }
    }
}