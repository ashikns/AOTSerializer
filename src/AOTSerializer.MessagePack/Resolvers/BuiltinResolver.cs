﻿using AOTSerializer.Common;
using AOTSerializer.MessagePack.Formatters;
using AOTSerializer.MessagePack.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AOTSerializer.MessagePack.Resolvers
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

namespace AOTSerializer.MessagePack.Internal
{
    internal static class BuiltinResolverGetFormatterHelper
    {
        private static readonly Dictionary<Type, IFormatter> formatterMap = new Dictionary<Type, IFormatter>()
        {
            {typeof(object), ObjectFallbackFormatter.Default},

            // Primitive
            {typeof(Int16), Int16Formatter.Instance},
            {typeof(Int32), Int32Formatter.Instance},
            {typeof(Int64), Int64Formatter.Instance},
            {typeof(UInt16), UInt16Formatter.Instance},
            {typeof(UInt32), UInt32Formatter.Instance},
            {typeof(UInt64), UInt64Formatter.Instance},
            {typeof(Single), SingleFormatter.Instance},
            {typeof(Double), DoubleFormatter.Instance},
            {typeof(bool), BooleanFormatter.Instance},
            {typeof(byte), ByteFormatter.Instance},
            {typeof(sbyte), SByteFormatter.Instance},
            {typeof(char), CharFormatter.Instance},

            // Nulllable Primitive
            {typeof(Nullable<Int16>), NullableInt16Formatter.Instance},
            {typeof(Nullable<Int32>), NullableInt32Formatter.Instance},
            {typeof(Nullable<Int64>), NullableInt64Formatter.Instance},
            {typeof(Nullable<UInt16>), NullableUInt16Formatter.Instance},
            {typeof(Nullable<UInt32>), NullableUInt32Formatter.Instance},
            {typeof(Nullable<UInt64>), NullableUInt64Formatter.Instance},
            {typeof(Nullable<Single>), NullableSingleFormatter.Instance},
            {typeof(Nullable<Double>), NullableDoubleFormatter.Instance},
            {typeof(Nullable<bool>), NullableBooleanFormatter.Instance},
            {typeof(Nullable<byte>), NullableByteFormatter.Instance},
            {typeof(Nullable<sbyte>), NullableSByteFormatter.Instance},
            {typeof(Nullable<char>), NullableCharFormatter.Instance},
            
            // DateTime
            {typeof(DateTime), DateTimeFormatter.Instance},
            {typeof(DateTime?), NullableDateTimeFormatter.Instance},
            {typeof(TimeSpan), TimeSpanFormatter.Instance},
            {typeof(TimeSpan?), new StaticNullableFormatter<TimeSpan>(TimeSpanFormatter.Instance)},
            {typeof(DateTimeOffset), DateTimeOffsetFormatter.Instance},
            {typeof(DateTimeOffset?), new StaticNullableFormatter<DateTimeOffset>(DateTimeOffsetFormatter.Instance)},

            // StandardClassLibraryFormatter
            {typeof(string), NullableStringFormatter.Instance},
            {typeof(decimal), DecimalFormatter.Instance},
            {typeof(decimal?), new StaticNullableFormatter<decimal>(DecimalFormatter.Instance)},

            {typeof(Guid), GuidFormatter.Instance},
            {typeof(Guid?), new StaticNullableFormatter<Guid>(GuidFormatter.Instance)},
            {typeof(Uri), UriFormatter.Instance},
            {typeof(Version), VersionFormatter.Instance},
            {typeof(StringBuilder), StringBuilderFormatter.Instance},
            {typeof(BitArray), BitArrayFormatter.Instance},
            {typeof(Type), TypeFormatter.Default},
            
            // special primitive
            {typeof(byte[]), ByteArrayFormatter.Instance},
            
            // Nil
            {typeof(Nil), NilFormatter.Instance},
            {typeof(Nil?), NullableNilFormatter.Instance},
            
            // otpmitized primitive array formatter
            {typeof(Int16[]), Int16ArrayFormatter.Instance},
            {typeof(Int32[]), Int32ArrayFormatter.Instance},
            {typeof(Int64[]), Int64ArrayFormatter.Instance},
            {typeof(UInt16[]), UInt16ArrayFormatter.Instance},
            {typeof(UInt32[]), UInt32ArrayFormatter.Instance},
            {typeof(UInt64[]), UInt64ArrayFormatter.Instance},
            {typeof(Single[]), SingleArrayFormatter.Instance},
            {typeof(Double[]), DoubleArrayFormatter.Instance},
            {typeof(Boolean[]), BooleanArrayFormatter.Instance},
            {typeof(SByte[]), SByteArrayFormatter.Instance},
            {typeof(Char[]), CharArrayFormatter.Instance},
            {typeof(string[]), NullableStringArrayFormatter.Instance},
            {typeof(DateTime[]), DateTimeArrayFormatter.Instance},

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

            { typeof(ArraySegment<byte>), ByteArraySegmentFormatter.Instance },
            { typeof(ArraySegment<byte>?),new StaticNullableFormatter<ArraySegment<byte>>(ByteArraySegmentFormatter.Instance) },

            {typeof(System.Numerics.BigInteger), BigIntegerFormatter.Instance},
            {typeof(System.Numerics.BigInteger?), new StaticNullableFormatter<System.Numerics.BigInteger>(BigIntegerFormatter.Instance)},
            {typeof(System.Numerics.Complex), ComplexFormatter.Instance},
            {typeof(System.Numerics.Complex?), new StaticNullableFormatter<System.Numerics.Complex>(ComplexFormatter.Instance)},
            {typeof(System.Threading.Tasks.Task), TaskUnitFormatter.Instance},
        };

        internal static IFormatter GetFormatter(Type t)
        {
            formatterMap.TryGetValue(t, out var formatter);
            return formatter;
        }
    }
}