﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AOTSerializer.Common;
using AOTSerializer.Internal;

#if NETSTANDARD || NETFRAMEWORK
using System.Collections.Concurrent;
#endif

namespace AOTSerializer.MessagePack.Formatters
{
    public sealed class ArrayFormatter<T> : FormatterBase<T[]>
    {
        public override int Serialize(ref byte[] bytes, int offset, T[] value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                var formatter = resolver.GetFormatter<T>();

                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Length);

                for (int i = 0; i < value.Length; i++)
                {
                    offset += formatter.Serialize(ref bytes, offset, value[i], resolver);
                }

                return offset - startOffset;
            }
        }

        public override T[] Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;
                var formatter = resolver.GetFormatter<T>();

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var array = new T[len];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = formatter.Deserialize(bytes, offset, out readSize, resolver);
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return array;
            }
        }
    }

    public sealed class ByteArraySegmentFormatter : FormatterBase<ArraySegment<byte>>
    {
        public static readonly ByteArraySegmentFormatter Instance = new ByteArraySegmentFormatter();

        private ByteArraySegmentFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, ArraySegment<byte> value, IResolver resolver)
        {
            if (value.Array == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                return MessagePackBinary.WriteBytes(ref bytes, offset, value.Array, value.Offset, value.Count);
            }
        }

        public override ArraySegment<byte> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return default(ArraySegment<byte>);
            }
            else
            {
                // use ReadBytesSegment? But currently straem api uses memory pool so can't save arraysegment...
                var binary = MessagePackBinary.ReadBytes(bytes, offset, out readSize);
                return new ArraySegment<byte>(binary, 0, binary.Length);
            }
        }
    }

    public sealed class ArraySegmentFormatter<T> : FormatterBase<ArraySegment<T>>
    {
        public override int Serialize(ref byte[] bytes, int offset, ArraySegment<T> value, IResolver resolver)
        {
            if (value.Array == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                var formatter = resolver.GetFormatter<T>();

                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Count);

                var array = value.Array;
                for (int i = 0; i < value.Count; i++)
                {
                    var item = array[value.Offset + i];
                    offset += formatter.Serialize(ref bytes, offset, item, resolver);
                }

                return offset - startOffset;
            }
        }

        public override ArraySegment<T> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return default(ArraySegment<T>);
            }
            else
            {
                var array = resolver.GetFormatter<T[]>().Deserialize(bytes, offset, out readSize, resolver);
                return new ArraySegment<T>(array, 0, array.Length);
            }
        }
    }

    // List<T> is popular format, should avoid abstraction.
    public sealed class ListFormatter<T> : FormatterBase<List<T>>
    {
        public override int Serialize(ref byte[] bytes, int offset, List<T> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                var formatter = resolver.GetFormatter<T>();

                var c = value.Count;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, c);

                for (int i = 0; i < c; i++)
                {
                    offset += formatter.Serialize(ref bytes, offset, value[i], resolver);
                }

                return offset - startOffset;
            }
        }

        public override List<T> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;
                var formatter = resolver.GetFormatter<T>();

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;
                var list = new List<T>(len);
                for (int i = 0; i < len; i++)
                {
                    list.Add(formatter.Deserialize(bytes, offset, out readSize, resolver));
                    offset += readSize;
                }
                readSize = offset - startOffset;
                return list;
            }
        }
    }

    public abstract class CollectionFormatterBase<TElement, TIntermediate, TEnumerator, TCollection> : FormatterBase<TCollection>
        where TCollection : IEnumerable<TElement>
        where TEnumerator : IEnumerator<TElement>
    {
        public override int Serialize(ref byte[] bytes, int offset, TCollection value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                // Optimize iteration(array is fastest)
                var array = value as TElement[];
                if (array != null)
                {
                    var startOffset = offset;
                    var formatter = resolver.GetFormatter<TElement>();

                    offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, array.Length);

                    foreach (var item in array)
                    {
                        offset += formatter.Serialize(ref bytes, offset, item, resolver);
                    }

                    return offset - startOffset;
                }
                else
                {
                    var startOffset = offset;
                    var formatter = resolver.GetFormatter<TElement>();

                    // knows count or not.
                    var seqCount = GetCount(value);
                    if (seqCount != null)
                    {
                        offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, seqCount.Value);

                        // Unity's foreach struct enumerator causes boxing so iterate manually.
                        var e = GetSourceEnumerator(value);
                        try
                        {
                            while (e.MoveNext())
                            {
                                offset += formatter.Serialize(ref bytes, offset, e.Current, resolver);
                            }
                        }
                        finally
                        {
                            e.Dispose();
                        }

                        return offset - startOffset;
                    }
                    else
                    {
                        // write message first -> open header space -> write header
                        var writeStarOffset = offset;

                        var count = 0;
                        var moveCount = 0;

                        // count = 16 <= 65535, header len is "3" so choose default space.
                        offset += 3;

                        var e = GetSourceEnumerator(value);
                        try
                        {
                            while (e.MoveNext())
                            {
                                count++;
                                var writeSize = formatter.Serialize(ref bytes, offset, e.Current, resolver);
                                moveCount += writeSize;
                                offset += writeSize;
                            }
                        }
                        finally
                        {
                            e.Dispose();
                        }

                        var headerLength = MessagePackBinary.GetArrayHeaderLength(count);
                        if (headerLength != 3)
                        {
                            if (headerLength == 1) offset -= 2; // 1
                            else offset += 2; // 5

                            BinaryUtil.EnsureCapacity(ref bytes, offset, headerLength);
                            Buffer.BlockCopy(bytes, writeStarOffset + 3, bytes, writeStarOffset + headerLength, moveCount);
                        }
                        MessagePackBinary.WriteArrayHeader(ref bytes, writeStarOffset, count);

                        return offset - startOffset;
                    }
                }
            }
        }

        public override TCollection Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return default(TCollection);
            }
            else
            {
                var startOffset = offset;
                var formatter = resolver.GetFormatter<TElement>();

                var len = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;

                var list = Create(len);
                for (int i = 0; i < len; i++)
                {
                    Add(list, i, formatter.Deserialize(bytes, offset, out readSize, resolver));
                    offset += readSize;
                }
                readSize = offset - startOffset;

                return Complete(list);
            }
        }

        // abstraction for serialize
        protected virtual int? GetCount(TCollection sequence)
        {
            var collection = sequence as ICollection<TElement>;
            if (collection != null)
            {
                return collection.Count;
            }
            else
            {
                var c2 = sequence as IReadOnlyCollection<TElement>;
                if (c2 != null)
                {
                    return c2.Count;
                }
            }

            return null;
        }

        // Some collections can use struct iterator, this is optimization path
        protected abstract TEnumerator GetSourceEnumerator(TCollection source);

        // abstraction for deserialize
        protected abstract TIntermediate Create(int count);
        protected abstract void Add(TIntermediate collection, int index, TElement value);
        protected abstract TCollection Complete(TIntermediate intermediateCollection);
    }

    public abstract class CollectionFormatterBase<TElement, TIntermediate, TCollection> : CollectionFormatterBase<TElement, TIntermediate, IEnumerator<TElement>, TCollection>
        where TCollection : IEnumerable<TElement>
    {
        protected override IEnumerator<TElement> GetSourceEnumerator(TCollection source)
        {
            return source.GetEnumerator();
        }
    }

    public abstract class CollectionFormatterBase<TElement, TCollection> : CollectionFormatterBase<TElement, TCollection, TCollection>
        where TCollection : IEnumerable<TElement>
    {
        protected sealed override TCollection Complete(TCollection intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class GenericCollectionFormatter<TElement, TCollection> : CollectionFormatterBase<TElement, TCollection>
         where TCollection : ICollection<TElement>, new()
    {
        protected override TCollection Create(int count)
        {
            return new TCollection();
        }

        protected override void Add(TCollection collection, int index, TElement value)
        {
            collection.Add(value);
        }
    }

    public sealed class LinkedListFormatter<T> : CollectionFormatterBase<T, LinkedList<T>, LinkedList<T>.Enumerator, LinkedList<T>>
    {
        protected override void Add(LinkedList<T> collection, int index, T value)
        {
            collection.AddLast(value);
        }

        protected override LinkedList<T> Complete(LinkedList<T> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override LinkedList<T> Create(int count)
        {
            return new LinkedList<T>();
        }

        protected override LinkedList<T>.Enumerator GetSourceEnumerator(LinkedList<T> source)
        {
            return source.GetEnumerator();
        }
    }

    public sealed class QeueueFormatter<T> : CollectionFormatterBase<T, Queue<T>, Queue<T>.Enumerator, Queue<T>>
    {
        protected override int? GetCount(Queue<T> sequence)
        {
            return sequence.Count;
        }

        protected override void Add(Queue<T> collection, int index, T value)
        {
            collection.Enqueue(value);
        }

        protected override Queue<T> Create(int count)
        {
            return new Queue<T>(count);
        }

        protected override Queue<T>.Enumerator GetSourceEnumerator(Queue<T> source)
        {
            return source.GetEnumerator();
        }

        protected override Queue<T> Complete(Queue<T> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    // should deserialize reverse order.
    public sealed class StackFormatter<T> : CollectionFormatterBase<T, T[], Stack<T>.Enumerator, Stack<T>>
    {
        protected override int? GetCount(Stack<T> sequence)
        {
            return sequence.Count;
        }

        protected override void Add(T[] collection, int index, T value)
        {
            // add reverse
            collection[collection.Length - 1 - index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override Stack<T>.Enumerator GetSourceEnumerator(Stack<T> source)
        {
            return source.GetEnumerator();
        }

        protected override Stack<T> Complete(T[] intermediateCollection)
        {
            return new Stack<T>(intermediateCollection);
        }
    }

    public sealed class HashSetFormatter<T> : CollectionFormatterBase<T, HashSet<T>, HashSet<T>.Enumerator, HashSet<T>>
    {
        protected override int? GetCount(HashSet<T> sequence)
        {
            return sequence.Count;
        }

        protected override void Add(HashSet<T> collection, int index, T value)
        {
            collection.Add(value);
        }

        protected override HashSet<T> Complete(HashSet<T> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override HashSet<T> Create(int count)
        {
            return new HashSet<T>();
        }

        protected override HashSet<T>.Enumerator GetSourceEnumerator(HashSet<T> source)
        {
            return source.GetEnumerator();
        }
    }

    public sealed class ReadOnlyCollectionFormatter<T> : CollectionFormatterBase<T, T[], ReadOnlyCollection<T>>
    {
        protected override void Add(T[] collection, int index, T value)
        {
            collection[index] = value;
        }

        protected override ReadOnlyCollection<T> Complete(T[] intermediateCollection)
        {
            return new ReadOnlyCollection<T>(intermediateCollection);
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }
    }

    public sealed class InterfaceListFormatter<T> : CollectionFormatterBase<T, T[], IList<T>>
    {
        protected override void Add(T[] collection, int index, T value)
        {
            collection[index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override IList<T> Complete(T[] intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceCollectionFormatter<T> : CollectionFormatterBase<T, T[], ICollection<T>>
    {
        protected override void Add(T[] collection, int index, T value)
        {
            collection[index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override ICollection<T> Complete(T[] intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceEnumerableFormatter<T> : CollectionFormatterBase<T, T[], IEnumerable<T>>
    {
        protected override void Add(T[] collection, int index, T value)
        {
            collection[index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override IEnumerable<T> Complete(T[] intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    // [Key, [Array]]
    public sealed class InterfaceGroupingFormatter<TKey, TElement> : FormatterBase<IGrouping<TKey, TElement>>
    {
        public override int Serialize(ref byte[] bytes, int offset, IGrouping<TKey, TElement> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);
                offset += resolver.GetFormatter<TKey>().Serialize(ref bytes, offset, value.Key, resolver);
                offset += resolver.GetFormatter<IEnumerable<TElement>>().Serialize(ref bytes, offset, value, resolver);
                return offset - startOffset;
            }
        }

        public override IGrouping<TKey, TElement> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }
            else
            {
                var startOffset = offset;
                var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
                offset += readSize;

                if (count != 2) throw new InvalidOperationException("Invalid Grouping format.");

                var key = resolver.GetFormatter<TKey>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;

                var value = resolver.GetFormatter<IEnumerable<TElement>>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;

                readSize = offset - startOffset;
                return new Grouping<TKey, TElement>(key, value);
            }
        }
    }

    public sealed class InterfaceLookupFormatter<TKey, TElement> : CollectionFormatterBase<IGrouping<TKey, TElement>, Dictionary<TKey, IGrouping<TKey, TElement>>, ILookup<TKey, TElement>>
    {
        protected override void Add(Dictionary<TKey, IGrouping<TKey, TElement>> collection, int index, IGrouping<TKey, TElement> value)
        {
            collection.Add(value.Key, value);
        }

        protected override ILookup<TKey, TElement> Complete(Dictionary<TKey, IGrouping<TKey, TElement>> intermediateCollection)
        {
            return new Lookup<TKey, TElement>(intermediateCollection);
        }

        protected override Dictionary<TKey, IGrouping<TKey, TElement>> Create(int count)
        {
            return new Dictionary<TKey, IGrouping<TKey, TElement>>(count);
        }
    }

    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        private readonly TKey key;
        private readonly IEnumerable<TElement> elements;

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            this.key = key;
            this.elements = elements;
        }

        public TKey Key
        {
            get
            {
                return key;
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }

    internal class Lookup<TKey, TElement> : ILookup<TKey, TElement>
    {
        private readonly Dictionary<TKey, IGrouping<TKey, TElement>> groupings;

        public Lookup(Dictionary<TKey, IGrouping<TKey, TElement>> groupings)
        {
            this.groupings = groupings;
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                return groupings[key];
            }
        }

        public int Count
        {
            get
            {
                return groupings.Count;
            }
        }

        public bool Contains(TKey key)
        {
            return groupings.ContainsKey(key);
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            return groupings.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return groupings.Values.GetEnumerator();
        }
    }

    // NonGenerics

    public sealed class NonGenericListFormatter<T> : FormatterBase<T>
        where T : class, IList, new()
    {
        public override int Serialize(ref byte[] bytes, int offset, T value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, offset);
                return 1;
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Count);
            foreach (var item in value)
            {
                offset += formatter.Serialize(ref bytes, offset, item, resolver);
            }

            return offset - startOffset;
        }

        public override T Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return default(T);
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var list = new T();
            for (int i = 0; i < count; i++)
            {
                list.Add(formatter.Deserialize(bytes, offset, out readSize, resolver));
                offset += readSize;
            }

            readSize = offset - startOffset;
            return list;
        }
    }

    public sealed class NonGenericInterfaceListFormatter : FormatterBase<IList>
    {
        public static readonly IFormatter<IList> Instance = new NonGenericInterfaceListFormatter();

        private NonGenericInterfaceListFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, IList value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, offset);
                return 1;
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, value.Count);
            foreach (var item in value)
            {
                offset += formatter.Serialize(ref bytes, offset, item, resolver);
            }

            return offset - startOffset;
        }

        public override IList Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return default(IList);
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            var count = MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var list = new object[count];
            for (int i = 0; i < count; i++)
            {
                list[i] = formatter.Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            }

            readSize = offset - startOffset;
            return list;
        }
    }

    public sealed class NonGenericDictionaryFormatter<T> : FormatterBase<T>
        where T : class, IDictionary, new()
    {
        public override int Serialize(ref byte[] bytes, int offset, T value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, offset);
                return 1;
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            offset += MessagePackBinary.WriteMapHeader(ref bytes, offset, value.Count);
            foreach (DictionaryEntry item in value)
            {
                offset += formatter.Serialize(ref bytes, offset, item.Key, resolver);
                offset += formatter.Serialize(ref bytes, offset, item.Value, resolver);
            }

            return offset - startOffset;
        }

        public override T Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            var count = MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var dict = new T();
            for (int i = 0; i < count; i++)
            {
                var key = formatter.Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var value = formatter.Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                dict.Add(key, value);
            }

            readSize = offset - startOffset;
            return dict;
        }
    }

    public sealed class NonGenericInterfaceDictionaryFormatter : FormatterBase<IDictionary>
    {
        public static readonly IFormatter<IDictionary> Instance = new NonGenericInterfaceDictionaryFormatter();

        private NonGenericInterfaceDictionaryFormatter()
        {

        }

        public override int Serialize(ref byte[] bytes, int offset, IDictionary value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, offset);
                return 1;
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            offset += MessagePackBinary.WriteMapHeader(ref bytes, offset, value.Count);
            foreach (DictionaryEntry item in value)
            {
                offset += formatter.Serialize(ref bytes, offset, item.Key, resolver);
                offset += formatter.Serialize(ref bytes, offset, item.Value, resolver);
            }

            return offset - startOffset;
        }

        public override IDictionary Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var formatter = resolver.GetFormatter<object>();
            var startOffset = offset;

            var count = MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var dict = new Dictionary<object, object>(count);
            for (int i = 0; i < count; i++)
            {
                var key = formatter.Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var value = formatter.Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                dict.Add(key, value);
            }

            readSize = offset - startOffset;
            return dict;
        }
    }

    public sealed class ObservableCollectionFormatter<T> : CollectionFormatterBase<T, ObservableCollection<T>>
    {
        protected override void Add(ObservableCollection<T> collection, int index, T value)
        {
            collection.Add(value);
        }

        protected override ObservableCollection<T> Create(int count)
        {
            return new ObservableCollection<T>();
        }
    }

    public sealed class ReadOnlyObservableCollectionFormatter<T> : CollectionFormatterBase<T, ObservableCollection<T>, ReadOnlyObservableCollection<T>>
    {
        protected override void Add(ObservableCollection<T> collection, int index, T value)
        {
            collection.Add(value);
        }

        protected override ObservableCollection<T> Create(int count)
        {
            return new ObservableCollection<T>();
        }

        protected override ReadOnlyObservableCollection<T> Complete(ObservableCollection<T> intermediateCollection)
        {
            return new ReadOnlyObservableCollection<T>(intermediateCollection);
        }
    }

    public sealed class InterfaceReadOnlyListFormatter<T> : CollectionFormatterBase<T, T[], IReadOnlyList<T>>
    {
        protected override void Add(T[] collection, int index, T value)
        {
            collection[index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override IReadOnlyList<T> Complete(T[] intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceReadOnlyCollectionFormatter<T> : CollectionFormatterBase<T, T[], IReadOnlyCollection<T>>
    {
        protected override void Add(T[] collection, int index, T value)
        {
            collection[index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override IReadOnlyCollection<T> Complete(T[] intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceSetFormatter<T> : CollectionFormatterBase<T, HashSet<T>, ISet<T>>
    {
        protected override void Add(HashSet<T> collection, int index, T value)
        {
            collection.Add(value);
        }

        protected override ISet<T> Complete(HashSet<T> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override HashSet<T> Create(int count)
        {
            return new HashSet<T>();
        }
    }

    public sealed class ConcurrentBagFormatter<T> : CollectionFormatterBase<T, System.Collections.Concurrent.ConcurrentBag<T>>
    {
        protected override int? GetCount(ConcurrentBag<T> sequence)
        {
            return sequence.Count;
        }

        protected override void Add(ConcurrentBag<T> collection, int index, T value)
        {
            collection.Add(value);
        }

        protected override ConcurrentBag<T> Create(int count)
        {
            return new ConcurrentBag<T>();
        }
    }

    public sealed class ConcurrentQueueFormatter<T> : CollectionFormatterBase<T, System.Collections.Concurrent.ConcurrentQueue<T>>
    {
        protected override int? GetCount(ConcurrentQueue<T> sequence)
        {
            return sequence.Count;
        }

        protected override void Add(ConcurrentQueue<T> collection, int index, T value)
        {
            collection.Enqueue(value);
        }

        protected override ConcurrentQueue<T> Create(int count)
        {
            return new ConcurrentQueue<T>();
        }
    }

    public sealed class ConcurrentStackFormatter<T> : CollectionFormatterBase<T, T[], ConcurrentStack<T>>
    {
        protected override int? GetCount(ConcurrentStack<T> sequence)
        {
            return sequence.Count;
        }

        protected override void Add(T[] collection, int index, T value)
        {
            // add reverse
            collection[collection.Length - 1 - index] = value;
        }

        protected override T[] Create(int count)
        {
            return new T[count];
        }

        protected override ConcurrentStack<T> Complete(T[] intermediateCollection)
        {
            return new ConcurrentStack<T>(intermediateCollection);
        }
    }
}