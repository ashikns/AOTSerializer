using AOTSerializer.Common;
using AOTSerializer.MessagePack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AOTSerializer.MessagePack.Formatters
{
    // unfortunately, can't use IDictionary<KVP> because supports IReadOnlyDictionary.
    public abstract class DictionaryFormatterBase<TKey, TValue, TIntermediate, TEnumerator, TDictionary> : FormatterBase<TDictionary>
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
        where TEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, TDictionary value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                var keyFormatter = resolver.GetFormatterWithVerify<TKey>();
                var valueFormatter = resolver.GetFormatterWithVerify<TValue>();

                int count;
                {
                    var col = value as ICollection<KeyValuePair<TKey, TValue>>;
                    if (col != null)
                    {
                        count = col.Count;
                    }
                    else
                    {
                        var col2 = value as IReadOnlyCollection<KeyValuePair<TKey, TValue>>;
                        if (col2 != null)
                        {
                            count = col2.Count;
                        }
                        else
                        {
                            throw new InvalidOperationException("DictionaryFormatterBase's TDictionary supports only ICollection<KVP> or IReadOnlyCollection<KVP>");
                        }
                    }
                }

                MessagePackBinary.WriteMapHeader(ref bytes, ref offset, count);

                var e = GetSourceEnumerator(value);
                try
                {
                    while (e.MoveNext())
                    {
                        var item = e.Current;
                        keyFormatter.Serialize(ref bytes, ref offset, item.Key, resolver);
                        valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);
                    }
                }
                finally
                {
                    e.Dispose();
                }
            }
        }

        public override TDictionary Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return default(TDictionary);
            }
            else
            {
                var keyFormatter = resolver.GetFormatterWithVerify<TKey>();
                var valueFormatter = resolver.GetFormatterWithVerify<TValue>();

                var len = MessagePackBinary.ReadMapHeader(bytes, ref offset);

                var dict = Create(len);
                for (int i = 0; i < len; i++)
                {
                    var key = keyFormatter.Deserialize(bytes, ref offset, resolver);
                    var value = valueFormatter.Deserialize(bytes, ref offset, resolver);

                    Add(dict, i, key, value);
                }

                return Complete(dict);
            }
        }

        // abstraction for serialize

        // Some collections can use struct iterator, this is optimization path
        protected abstract TEnumerator GetSourceEnumerator(TDictionary source);

        // abstraction for deserialize
        protected abstract TIntermediate Create(int count);
        protected abstract void Add(TIntermediate collection, int index, TKey key, TValue value);
        protected abstract TDictionary Complete(TIntermediate intermediateCollection);
    }

    public abstract class DictionaryFormatterBase<TKey, TValue, TIntermediate, TDictionary> : DictionaryFormatterBase<TKey, TValue, TIntermediate, IEnumerator<KeyValuePair<TKey, TValue>>, TDictionary>
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        protected override IEnumerator<KeyValuePair<TKey, TValue>> GetSourceEnumerator(TDictionary source)
        {
            return source.GetEnumerator();
        }
    }

    public abstract class DictionaryFormatterBase<TKey, TValue, TDictionary> : DictionaryFormatterBase<TKey, TValue, TDictionary, TDictionary>
        where TDictionary : IDictionary<TKey, TValue>
    {
        protected override TDictionary Complete(TDictionary intermediateCollection)
        {
            return intermediateCollection;
        }
    }


    public sealed class DictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, Dictionary<TKey, TValue>.Enumerator, Dictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override Dictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override Dictionary<TKey, TValue> Create(int count)
        {
            return new Dictionary<TKey, TValue>(count);
        }

        protected override Dictionary<TKey, TValue>.Enumerator GetSourceEnumerator(Dictionary<TKey, TValue> source)
        {
            return source.GetEnumerator();
        }
    }

    public sealed class GenericDictionaryFormatter<TKey, TValue, TDictionary> : DictionaryFormatterBase<TKey, TValue, TDictionary>
        where TDictionary : IDictionary<TKey, TValue>, new()
    {
        protected override void Add(TDictionary collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override TDictionary Create(int count)
        {
            return new TDictionary();
        }
    }

    public sealed class InterfaceDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, IDictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override Dictionary<TKey, TValue> Create(int count)
        {
            return new Dictionary<TKey, TValue>(count);
        }

        protected override IDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class SortedListFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, SortedList<TKey, TValue>>
    {
        protected override void Add(SortedList<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override SortedList<TKey, TValue> Create(int count)
        {
            return new SortedList<TKey, TValue>(count);
        }
    }

    public sealed class SortedDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, SortedDictionary<TKey, TValue>, SortedDictionary<TKey, TValue>.Enumerator, SortedDictionary<TKey, TValue>>
    {
        protected override void Add(SortedDictionary<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override SortedDictionary<TKey, TValue> Complete(SortedDictionary<TKey, TValue> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override SortedDictionary<TKey, TValue> Create(int count)
        {
            return new SortedDictionary<TKey, TValue>();
        }

        protected override SortedDictionary<TKey, TValue>.Enumerator GetSourceEnumerator(SortedDictionary<TKey, TValue> source)
        {
            return source.GetEnumerator();
        }
    }

    public sealed class ReadOnlyDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, ReadOnlyDictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override ReadOnlyDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return new ReadOnlyDictionary<TKey, TValue>(intermediateCollection);
        }

        protected override Dictionary<TKey, TValue> Create(int count)
        {
            return new Dictionary<TKey, TValue>(count);
        }
    }

    public sealed class InterfaceReadOnlyDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override IReadOnlyDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override Dictionary<TKey, TValue> Create(int count)
        {
            return new Dictionary<TKey, TValue>(count);
        }
    }

    public sealed class ConcurrentDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, System.Collections.Concurrent.ConcurrentDictionary<TKey, TValue>>
    {
        protected override void Add(ConcurrentDictionary<TKey, TValue> collection, int index, TKey key, TValue value)
        {
            collection.TryAdd(key, value);
        }

        protected override ConcurrentDictionary<TKey, TValue> Create(int count)
        {
            // concurrent dictionary can't access defaultConcurrecyLevel so does not use count overload.
            return new ConcurrentDictionary<TKey, TValue>();
        }
    }
}