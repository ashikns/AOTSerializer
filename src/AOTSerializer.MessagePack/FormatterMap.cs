using AOTSerializer.Common;
using AOTSerializer.MessagePack.Formatters;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOTSerializer.MessagePack
{
    public static class FormatterMap
    {
        public static readonly Dictionary<Type, (string WriteFunc, string ReadFunc)> PrimitiveFuncs = new Dictionary<Type, (string WriteFunc, string ReadFunc)>
        {
            { typeof(short), (nameof(MessagePackBinary.WriteInt16), nameof(MessagePackBinary.ReadInt16)) },
            { typeof(int), (nameof(MessagePackBinary.WriteInt32), nameof(MessagePackBinary.ReadInt32)) },
            { typeof(long), (nameof(MessagePackBinary.WriteInt64), nameof(MessagePackBinary.ReadInt64)) },
            { typeof(ushort), (nameof(MessagePackBinary.WriteUInt16), nameof(MessagePackBinary.ReadUInt16)) },
            { typeof(uint), (nameof(MessagePackBinary.WriteUInt32), nameof(MessagePackBinary.ReadUInt32)) },
            { typeof(ulong), (nameof(MessagePackBinary.WriteUInt64), nameof(MessagePackBinary.ReadUInt64)) },
            { typeof(float), (nameof(MessagePackBinary.WriteSingle), nameof(MessagePackBinary.ReadSingle)) },
            { typeof(double), (nameof(MessagePackBinary.WriteDouble), nameof(MessagePackBinary.ReadDouble)) },
            { typeof(bool), (nameof(MessagePackBinary.WriteBoolean), nameof(MessagePackBinary.ReadBoolean)) },
            { typeof(byte), (nameof(MessagePackBinary.WriteByte), nameof(MessagePackBinary.ReadByte)) },
            { typeof(sbyte), (nameof(MessagePackBinary.WriteSByte), nameof(MessagePackBinary.ReadSByte)) },
            { typeof(string), (nameof(MessagePackBinary.WriteString), nameof(MessagePackBinary.ReadString)) },

            { typeof(Nil), (nameof(MessagePackBinary.WriteNil), nameof(MessagePackBinary.ReadNil)) },
            { typeof(char), (nameof(MessagePackBinary.WriteChar), nameof(MessagePackBinary.ReadChar)) },
            { typeof(byte[]), (nameof(MessagePackBinary.WriteBytes), nameof(MessagePackBinary.ReadBytes)) },
            { typeof(DateTime), (nameof(MessagePackBinary.WriteDateTime), nameof(MessagePackBinary.ReadDateTime)) },
        };

        public static readonly Dictionary<Type, IFormatter> ConcreteFormatterMap = new Dictionary<Type, IFormatter>()
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
            {typeof(short?), NullableInt16Formatter.Instance},
            {typeof(int?), NullableInt32Formatter.Instance},
            {typeof(long?), NullableInt64Formatter.Instance},
            {typeof(ushort?), NullableUInt16Formatter.Instance},
            {typeof(uint?), NullableUInt32Formatter.Instance},
            {typeof(ulong?), NullableUInt64Formatter.Instance},
            {typeof(float?), NullableSingleFormatter.Instance},
            {typeof(double?), NullableDoubleFormatter.Instance},
            {typeof(bool?), NullableBooleanFormatter.Instance},
            {typeof(byte?), NullableByteFormatter.Instance},
            {typeof(sbyte?), NullableSByteFormatter.Instance},
            {typeof(char?), NullableCharFormatter.Instance},
            
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
            {typeof(Stream), StreamFormatter.Default},
            
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

            {typeof(BigInteger), BigIntegerFormatter.Instance},
            {typeof(BigInteger?), new StaticNullableFormatter<BigInteger>(BigIntegerFormatter.Instance)},
            {typeof(Complex), ComplexFormatter.Instance},
            {typeof(Complex?), new StaticNullableFormatter<Complex>(ComplexFormatter.Instance)},
            {typeof(Task), TaskUnitFormatter.Instance},
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
