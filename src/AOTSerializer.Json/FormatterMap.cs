﻿using AOTSerializer.Common;
using AOTSerializer.Json.Formatters;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOTSerializer.Json
{
    public static class FormatterMap
    {
        public static readonly Dictionary<Type, IFormatter> ConcreteFormatterMap = new Dictionary<Type, IFormatter>()
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
            {typeof(short?), NullableInt16Formatter.Default},
            {typeof(int?), NullableInt32Formatter.Default},
            {typeof(long?), NullableInt64Formatter.Default},
            {typeof(ushort?), NullableUInt16Formatter.Default},
            {typeof(uint?), NullableUInt32Formatter.Default},
            {typeof(ulong?), NullableUInt64Formatter.Default},
            {typeof(float?), NullableSingleFormatter.Default},
            {typeof(double?), NullableDoubleFormatter.Default},
            {typeof(bool?), NullableBooleanFormatter.Default},
            {typeof(byte?), NullableByteFormatter.Default},
            {typeof(sbyte?), NullableSByteFormatter.Default},
            {typeof(char?), NullableCharFormatter.Default},
            
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

            {typeof(BigInteger), BigIntegerFormatter.Default},
            {typeof(BigInteger?), new StaticNullableFormatter<BigInteger>(BigIntegerFormatter.Default)},
            {typeof(Complex), ComplexFormatter.Default},
            {typeof(Complex?), new StaticNullableFormatter<Complex>(ComplexFormatter.Default)},
            {typeof(Task), TaskUnitFormatter.Default},
        };

        public static readonly Dictionary<Type, Type> GenericFormatterMap = new Dictionary<Type, Type>
        {
            {typeof(List<>), typeof(ListFormatter<>) },
            {typeof(LinkedList<>), typeof(LinkedListFormatter<>)},
            {typeof(Queue<>), typeof(QueueFormatter<>)},
            {typeof(Stack<>), typeof(StackFormatter<>)},
            {typeof(HashSet<>), typeof(HashSetFormatter<>)},
            {typeof(ReadOnlyCollection<>), typeof(ReadOnlyCollectionFormatter<>)},
            {typeof(IList<>), typeof(InterfaceListFormatter<>)},
            {typeof(ICollection<>), typeof(InterfaceCollectionFormatter<>)},
            {typeof(IEnumerable<>), typeof(InterfaceEnumerableFormatter<>)},
            {typeof(Dictionary<,>), typeof(DictionaryFormatter<,>)},
            {typeof(IDictionary<,>), typeof(InterfaceDictionaryFormatter<,>)},
            {typeof(SortedDictionary<,>), typeof(SortedDictionaryFormatter<,>)},
            {typeof(SortedList<,>), typeof(SortedListFormatter<,>)},
            {typeof(ILookup<,>), typeof(InterfaceLookupFormatter<,>)},
            {typeof(IGrouping<,>), typeof(InterfaceGroupingFormatter<,>)},
            {typeof(ObservableCollection<>), typeof(ObservableCollectionFormatter<>)},
            {typeof(ReadOnlyObservableCollection<>), typeof(ReadOnlyObservableCollectionFormatter<>) },
            {typeof(IReadOnlyList<>), typeof(InterfaceReadOnlyListFormatter<>)},
            {typeof(IReadOnlyCollection<>), typeof(InterfaceReadOnlyCollectionFormatter<>)},
            {typeof(ISet<>), typeof(InterfaceSetFormatter<>)},
            {typeof(ConcurrentBag<>), typeof(ConcurrentBagFormatter<>)},
            {typeof(ConcurrentQueue<>), typeof(ConcurrentQueueFormatter<>)},
            {typeof(ConcurrentStack<>), typeof(ConcurrentStackFormatter<>)},
            {typeof(ReadOnlyDictionary<,>), typeof(ReadOnlyDictionaryFormatter<,>)},
            {typeof(IReadOnlyDictionary<,>), typeof(InterfaceReadOnlyDictionaryFormatter<,>)},
            {typeof(ConcurrentDictionary<,>), typeof(ConcurrentDictionaryFormatter<,>)},
            {typeof(Lazy<>), typeof(LazyFormatter<>)},
            {typeof(Task<>), typeof(TaskValueFormatter<>)},

            {typeof(Tuple<>), typeof(TupleFormatter<>)},
            {typeof(Tuple<,>), typeof(TupleFormatter<,>)},
            {typeof(Tuple<,,>), typeof(TupleFormatter<,,>)},
            {typeof(Tuple<,,,>), typeof(TupleFormatter<,,,>)},
            {typeof(Tuple<,,,,>), typeof(TupleFormatter<,,,,>)},
            {typeof(Tuple<,,,,,>), typeof(TupleFormatter<,,,,,>)},
            {typeof(Tuple<,,,,,,>), typeof(TupleFormatter<,,,,,,>)},
            {typeof(Tuple<,,,,,,,>), typeof(TupleFormatter<,,,,,,,>)},

            {typeof(ValueTuple<>), typeof(ValueTupleFormatter<>)},
            {typeof(ValueTuple<,>), typeof(ValueTupleFormatter<,>)},
            {typeof(ValueTuple<,,>), typeof(ValueTupleFormatter<,,>)},
            {typeof(ValueTuple<,,,>), typeof(ValueTupleFormatter<,,,>)},
            {typeof(ValueTuple<,,,,>), typeof(ValueTupleFormatter<,,,,>)},
            {typeof(ValueTuple<,,,,,>), typeof(ValueTupleFormatter<,,,,,>)},
            {typeof(ValueTuple<,,,,,,>), typeof(ValueTupleFormatter<,,,,,,>)},
            {typeof(ValueTuple<,,,,,,,>), typeof(ValueTupleFormatter<,,,,,,,>)},

            {typeof(KeyValuePair<,>), typeof(KeyValuePairFormatter<,>)},
            {typeof(ValueTask<>), typeof(ValueTaskFormatter<>)},
            {typeof(ArraySegment<>), typeof(ArraySegmentFormatter<>)},
        };
    }
}
