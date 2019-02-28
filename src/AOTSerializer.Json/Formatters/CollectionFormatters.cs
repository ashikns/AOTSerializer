using AOTSerializer.Common;
using AOTSerializer.Internal;
using AOTSerializer.Json.Formatters.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AOTSerializer.Json.Formatters
{
    public class ArrayFormatter<T> : FormatterBase<T[]>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, T[] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<T>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            if (value.Length != 0)
            {
                formatter.Serialize(ref bytes, ref offset, value[0], resolver);
            }
            for (int i = 1; i < value.Length; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                formatter.Serialize(ref bytes, ref offset, value[i], resolver);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override T[] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) return null;

            var formatter = resolver.GetFormatter<T>();

            var segment = JsonUtility.ReadNextBlockSegment(bytes, ref offset, out var numElements);
            var segmentOffset = segment.Offset;

            var result = new T[numElements];
            int index = 0;

            JsonUtility.ReadIsBeginArrayWithVerify(segment.Array, ref segmentOffset);
            while (!JsonUtility.ReadIsEndArray(segment.Array, ref segmentOffset))
            {
                result[index++] = formatter.Deserialize(segment.Array, ref segmentOffset, resolver);
                JsonUtility.ReadIsValueSeparatorWithVerify(segment.Array, ref segmentOffset);
            }

            return result;
        }
    }

    public class ArraySegmentFormatter<T> : FormatterBase<ArraySegment<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, ArraySegment<T> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<T>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            if (value.Count != 0)
            {
                formatter.Serialize(ref bytes, ref offset, value.Array[value.Offset], resolver);
            }
            for (int i = 1; i < value.Count; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                formatter.Serialize(ref bytes, ref offset, value.Array[value.Offset + i], resolver);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override ArraySegment<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return default; }

            var formatter = resolver.GetFormatter<T>();

            var segment = JsonUtility.ReadNextBlockSegment(bytes, ref offset, out var numElements);
            var segmentOffset = segment.Offset;

            var result = new T[numElements];
            int index = 0;

            JsonUtility.ReadIsBeginArrayWithVerify(segment.Array, ref segmentOffset);
            while (!JsonUtility.ReadIsEndArray(segment.Array, ref segmentOffset))
            {
                result[index++] = formatter.Deserialize(segment.Array, ref segmentOffset, resolver);
                JsonUtility.ReadIsValueSeparatorWithVerify(segment.Array, ref segmentOffset);
            }

            return new ArraySegment<T>(result, 0, result.Length);
        }
    }

    public class ListFormatter<T> : FormatterBase<List<T>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, List<T> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<T>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            if (value.Count != 0)
            {
                formatter.Serialize(ref bytes, ref offset, value[0], resolver);
            }
            for (int i = 1; i < value.Count; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                formatter.Serialize(ref bytes, ref offset, value[i], resolver);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override List<T> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var formatter = resolver.GetFormatter<T>();
            var segment = JsonUtility.ReadNextBlockSegment(bytes, ref offset, out var numElements);
            var segmentOffset = segment.Offset;

            var result = new List<T>(numElements);

            JsonUtility.ReadIsBeginArrayWithVerify(segment.Array, ref segmentOffset);
            while (!JsonUtility.ReadIsEndArray(segment.Array, ref segmentOffset))
            {
                result.Add(formatter.Deserialize(segment.Array, ref segmentOffset, resolver));
                JsonUtility.ReadIsValueSeparatorWithVerify(segment.Array, ref segmentOffset);
            }

            return result;
        }
    }

    public abstract class CollectionFormatterBase<TElement, TIntermediate, TEnumerator, TCollection> : FormatterBase<TCollection>
        where TCollection : class, IEnumerable<TElement>
        where TEnumerator : IEnumerator<TElement>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, TCollection value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<TElement>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            // Unity's foreach struct enumerator causes boxing so iterate manually.
            using (var e = GetSourceEnumerator(value))
            {
                if (e.MoveNext())
                {
                    formatter.Serialize(ref bytes, ref offset, e.Current, resolver);

                    while (e.MoveNext())
                    {
                        JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        formatter.Serialize(ref bytes, ref offset, e.Current, resolver);
                    }
                }
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override TCollection Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var formatter = resolver.GetFormatter<TElement>();

            var segment = JsonUtility.ReadNextBlockSegment(bytes, ref offset, out var numElements);
            var segmentOffset = segment.Offset;

            var intermediate = Create(numElements);

            JsonUtility.ReadIsBeginArrayWithVerify(segment.Array, ref segmentOffset);
            while (!JsonUtility.ReadIsEndArray(segment.Array, ref segmentOffset))
            {
                Add(intermediate, formatter.Deserialize(segment.Array, ref segmentOffset, resolver));
                JsonUtility.ReadIsValueSeparatorWithVerify(segment.Array, ref segmentOffset);
            }

            return Complete(intermediate);
        }

        // Some collections can use struct iterator, this is optimization path
        protected abstract TEnumerator GetSourceEnumerator(TCollection source);

        // abstraction for deserialize
        protected abstract TIntermediate Create(int count);
        protected abstract void Add(TIntermediate collection, TElement value);
        protected abstract TCollection Complete(TIntermediate intermediateCollection);
    }

    public abstract class CollectionFormatterBase<TElement, TIntermediate, TCollection> : CollectionFormatterBase<TElement, TIntermediate, IEnumerator<TElement>, TCollection>
        where TCollection : class, IEnumerable<TElement>
    {
        protected override IEnumerator<TElement> GetSourceEnumerator(TCollection source)
        {
            return source.GetEnumerator();
        }
    }

    public abstract class CollectionFormatterBase<TElement, TCollection> : CollectionFormatterBase<TElement, TCollection, TCollection>
        where TCollection : class, IEnumerable<TElement>
    {
        protected sealed override TCollection Complete(TCollection intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class GenericCollectionFormatter<TElement, TCollection> : CollectionFormatterBase<TElement, TCollection>
         where TCollection : class, ICollection<TElement>, new()
    {
        protected override TCollection Create(int count)
        {
            return new TCollection();
        }

        protected override void Add(TCollection collection, TElement value)
        {
            collection.Add(value);
        }
    }

    public sealed class LinkedListFormatter<T> : CollectionFormatterBase<T, LinkedList<T>>
    {
        protected override void Add(LinkedList<T> collection, T value)
        {
            collection.AddLast(value);
        }

        protected override LinkedList<T> Create(int count)
        {
            return new LinkedList<T>();
        }
    }

    public sealed class QeueueFormatter<T> : CollectionFormatterBase<T, Queue<T>>
    {
        protected override void Add(Queue<T> collection, T value)
        {
            collection.Enqueue(value);
        }

        protected override Queue<T> Create(int count)
        {
            return new Queue<T>(count);
        }
    }

    // should deserialize reverse order.
    public sealed class StackFormatter<T> : CollectionFormatterBase<T, List<T>, Stack<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override Stack<T> Complete(List<T> intermediateCollection)
        {
            var stack = new Stack<T>(intermediateCollection.Count);
            for (int i = intermediateCollection.Count - 1; i >= 0; i--)
            {
                stack.Push(intermediateCollection[i]);
            }
            return stack;
        }
    }

    public sealed class HashSetFormatter<T> : CollectionFormatterBase<T, HashSet<T>>
    {
        protected override void Add(HashSet<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override HashSet<T> Create(int count)
        {
            return new HashSet<T>();
        }
    }

    public sealed class ReadOnlyCollectionFormatter<T> : CollectionFormatterBase<T, List<T>, ReadOnlyCollection<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override ReadOnlyCollection<T> Complete(List<T> intermediateCollection)
        {
            return new ReadOnlyCollection<T>(intermediateCollection);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }
    }

    public sealed class InterfaceListFormatter<T> : CollectionFormatterBase<T, List<T>, IList<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override IList<T> Complete(List<T> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceCollectionFormatter<T> : CollectionFormatterBase<T, List<T>, ICollection<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override ICollection<T> Complete(List<T> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceEnumerableFormatter<T> : CollectionFormatterBase<T, List<T>, IEnumerable<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override IEnumerable<T> Complete(List<T> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    // {Key:key, Elements:[Array]}  (not compatible with JSON.NET)
    public sealed class InterfaceGroupingFormatter<TKey, TElement> : FormatterBase<IGrouping<TKey, TElement>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, IGrouping<TKey, TElement> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, CollectionFormatterHelper.groupingName[0]);
            resolver.GetFormatter<TKey>().Serialize(ref bytes, ref offset, value.Key, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, CollectionFormatterHelper.groupingName[1]);
            resolver.GetFormatter<IEnumerable<TElement>>().Serialize(ref bytes, ref offset, value.AsEnumerable(), resolver);

            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override IGrouping<TKey, TElement> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            TKey resultKey = default;
            IEnumerable<TElement> resultValue = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                CollectionFormatterHelper.groupingAutomata.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        resultKey = resolver.GetFormatter<TKey>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        resultValue = resolver.GetFormatter<IEnumerable<TElement>>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Grouping<TKey, TElement>(resultKey, resultValue);
        }
    }

    public sealed class InterfaceLookupFormatter<TKey, TElement> : FormatterBase<ILookup<TKey, TElement>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, ILookup<TKey, TElement> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }
            else
            {
                resolver.GetFormatter<IEnumerable<IGrouping<TKey, TElement>>>().Serialize(ref bytes, ref offset, value.AsEnumerable(), resolver);
            }
        }

        public override ILookup<TKey, TElement> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var formatter = resolver.GetFormatter<IGrouping<TKey, TElement>>();
            var intermediateCollection = new Dictionary<TKey, IGrouping<TKey, TElement>>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                var g = formatter.Deserialize(bytes, ref offset, resolver);
                intermediateCollection.Add(g.Key, g);

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Lookup<TKey, TElement>(intermediateCollection);
        }
    }

    internal class Grouping<TKey, TElement> : IGrouping<TKey, TElement>
    {
        public TKey Key { get; }

        private readonly IEnumerable<TElement> _elements;

        public Grouping(TKey key, IEnumerable<TElement> elements)
        {
            this.Key = key;
            this._elements = elements;
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
        public override void Serialize(ref byte[] bytes, ref int offset, T value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<object>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            if (value.Count != 0)
            {
                formatter.Serialize(ref bytes, ref offset, value[0], resolver);
            }
            for (int i = 1; i < value.Count; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                formatter.Serialize(ref bytes, ref offset, value[i], resolver);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override T Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) return null;

            var formatter = resolver.GetFormatter<object>();
            var list = new T();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                list.Add(formatter.Deserialize(bytes, ref offset, resolver));
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return list;
        }
    }

    public sealed class NonGenericInterfaceEnumerableFormatter : FormatterBase<IEnumerable>
    {
        public static readonly IFormatter<IEnumerable> Default = new NonGenericInterfaceEnumerableFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, IEnumerable value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<object>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);

            var e = value.GetEnumerator();
            try
            {
                if (e.MoveNext())
                {
                    formatter.Serialize(ref bytes, ref offset, e.Current, resolver);

                    while (e.MoveNext())
                    {
                        JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        formatter.Serialize(ref bytes, ref offset, e.Current, resolver);
                    }
                }
            }
            finally
            {
                if (e is IDisposable d) { d.Dispose(); }
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override IEnumerable Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var formatter = resolver.GetFormatter<object>();
            var list = new List<object>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                list.Add(formatter.Deserialize(bytes, ref offset, resolver));
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return list;
        }
    }

    public sealed class NonGenericInterfaceCollectionFormatter : FormatterBase<ICollection>
    {
        public static readonly IFormatter<ICollection> Default = new NonGenericInterfaceCollectionFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, ICollection value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<object>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            var e = value.GetEnumerator();
            try
            {
                if (e.MoveNext())
                {
                    formatter.Serialize(ref bytes, ref offset, e.Current, resolver);

                    while (e.MoveNext())
                    {
                        JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        formatter.Serialize(ref bytes, ref offset, e.Current, resolver);
                    }
                }
            }
            finally
            {
                if (e is IDisposable d) { d.Dispose(); }
            }

            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override ICollection Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var formatter = resolver.GetFormatter<object>();
            var list = new List<object>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                list.Add(formatter.Deserialize(bytes, ref offset, resolver));
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return list;
        }
    }

    public sealed class NonGenericInterfaceListFormatter : FormatterBase<IList>
    {
        public static readonly IFormatter<IList> Default = new NonGenericInterfaceListFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, IList value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatter<object>();

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            if (value.Count != 0)
            {
                formatter.Serialize(ref bytes, ref offset, value[0], resolver);
            }
            for (int i = 1; i < value.Count; i++)
            {
                JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                formatter.Serialize(ref bytes, ref offset, value[i], resolver);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override IList Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var formatter = resolver.GetFormatter<object>();
            var list = new List<object>();

            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                list.Add(formatter.Deserialize(bytes, ref offset, resolver));
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return list;
        }
    }

    public sealed class ObservableCollectionFormatter<T> : CollectionFormatterBase<T, ObservableCollection<T>>
    {
        protected override void Add(ObservableCollection<T> collection, T value)
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
        protected override void Add(ObservableCollection<T> collection, T value)
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

    public sealed class InterfaceReadOnlyListFormatter<T> : CollectionFormatterBase<T, List<T>, IReadOnlyList<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override IReadOnlyList<T> Complete(List<T> intermediateCollection)
        {
            return new ReadOnlyCollection<T>(intermediateCollection);
        }
    }

    public sealed class InterfaceReadOnlyCollectionFormatter<T> : CollectionFormatterBase<T, List<T>, IReadOnlyCollection<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override IReadOnlyCollection<T> Complete(List<T> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class InterfaceSetFormatter<T> : CollectionFormatterBase<T, HashSet<T>, ISet<T>>
    {
        protected override void Add(HashSet<T> collection, T value)
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

    public sealed class ConcurrentBagFormatter<T> : CollectionFormatterBase<T, ConcurrentBag<T>>
    {
        protected override void Add(ConcurrentBag<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override ConcurrentBag<T> Create(int count)
        {
            return new ConcurrentBag<T>();
        }
    }

    public sealed class ConcurrentQueueFormatter<T> : CollectionFormatterBase<T, ConcurrentQueue<T>>
    {
        protected override void Add(ConcurrentQueue<T> collection, T value)
        {
            collection.Enqueue(value);
        }

        protected override ConcurrentQueue<T> Create(int count)
        {
            return new ConcurrentQueue<T>();
        }
    }

    public sealed class ConcurrentStackFormatter<T> : CollectionFormatterBase<T, List<T>, ConcurrentStack<T>>
    {
        protected override void Add(List<T> collection, T value)
        {
            collection.Add(value);
        }

        protected override List<T> Create(int count)
        {
            return new List<T>(count);
        }

        protected override ConcurrentStack<T> Complete(List<T> intermediateCollection)
        {
            var stack = new ConcurrentStack<T>();
            for (int i = intermediateCollection.Count - 1; i >= 0; i--)
            {
                stack.Push(intermediateCollection[i]);
            }
            return stack;
        }
    }
}


namespace AOTSerializer.Json.Formatters.Internal
{
    internal static class CollectionFormatterHelper
    {
        internal static readonly byte[][] groupingName;
        internal static readonly AutomataDictionary groupingAutomata;

        static CollectionFormatterHelper()
        {
            groupingName = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Key").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Elements").ToArray(),
            };
            groupingAutomata = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Key")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Elements")), 1 },
            };
        }
    }
}