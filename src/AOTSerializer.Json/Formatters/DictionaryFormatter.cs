using AOTSerializer.Common;
using AOTSerializer.Internal;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AOTSerializer.Json.Formatters
{
    // unfortunately, can't use IDictionary<KVP> because supports IReadOnlyDictionary.
    public abstract class DictionaryFormatterBase<TKey, TValue, TIntermediate, TEnumerator, TDictionary> : FormatterBase<TDictionary>
        where TDictionary : class, IEnumerable<KeyValuePair<TKey, TValue>>
        where TEnumerator : IEnumerator<KeyValuePair<TKey, TValue>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, TDictionary value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteBeginObject(ref bytes, ref offset);

            using (var e = GetSourceEnumerator(value))
            {
                if (!e.MoveNext())
                {
                    JsonUtility.WriteEndObject(ref bytes, ref offset);
                    return;
                }

                var valueFormatter = resolver.GetFormatter<TValue>();

                if (resolver.GetFormatter<TKey>() is IObjectPropertyNameFormatter<TKey> keyFormatter)
                {
                    var item = e.Current;
                    keyFormatter.SerializeToPropertyName(ref bytes, ref offset, item.Key, resolver);
                    JsonUtility.WriteNameSeparator(ref bytes, ref offset);
                    valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);

                    while (e.MoveNext())
                    {
                        JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        item = e.Current;
                        keyFormatter.SerializeToPropertyName(ref bytes, ref offset, item.Key, resolver);
                        JsonUtility.WriteNameSeparator(ref bytes, ref offset);
                        valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);
                    }
                }
                else
                {
                    var item = e.Current;
                    JsonUtility.WriteString(ref bytes, ref offset, item.Key.ToString());
                    JsonUtility.WriteNameSeparator(ref bytes, ref offset);
                    valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);

                    while (e.MoveNext())
                    {
                        JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        item = e.Current;
                        JsonUtility.WriteString(ref bytes, ref offset, item.Key.ToString());
                        JsonUtility.WriteNameSeparator(ref bytes, ref offset);
                        valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);
                    }
                }
            }

            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override TDictionary Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            if (!(resolver.GetFormatter<TKey>() is IObjectPropertyNameFormatter<TKey> keyFormatter))
            {
                throw new InvalidOperationException(typeof(TKey) + " does not support dictionary key deserialize.");
            }

            var valueFormatter = resolver.GetFormatter<TValue>();
            var dict = Create();

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var key = keyFormatter.DeserializeFromPropertyName(bytes, ref offset, resolver);
                JsonUtility.ReadIsNameSeparatorWithVerify(bytes, ref offset);
                var value = valueFormatter.Deserialize(bytes, ref offset, resolver);
                Add(dict, key, value);

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return Complete(dict);
        }

        // abstraction for serialize

        // Some collections can use struct iterator, this is optimization path
        protected abstract TEnumerator GetSourceEnumerator(TDictionary source);

        // abstraction for deserialize
        protected abstract TIntermediate Create();
        protected abstract void Add(TIntermediate collection, TKey key, TValue value);
        protected abstract TDictionary Complete(TIntermediate intermediateCollection);
    }

    public abstract class DictionaryFormatterBase<TKey, TValue, TIntermediate, TDictionary> : DictionaryFormatterBase<TKey, TValue, TIntermediate, IEnumerator<KeyValuePair<TKey, TValue>>, TDictionary>
        where TDictionary : class, IEnumerable<KeyValuePair<TKey, TValue>>
    {
        protected override IEnumerator<KeyValuePair<TKey, TValue>> GetSourceEnumerator(TDictionary source)
        {
            return source.GetEnumerator();
        }
    }

    public abstract class DictionaryFormatterBase<TKey, TValue, TDictionary> : DictionaryFormatterBase<TKey, TValue, TDictionary, TDictionary>
        where TDictionary : class, IDictionary<TKey, TValue>
    {
        protected override TDictionary Complete(TDictionary intermediateCollection)
        {
            return intermediateCollection;
        }
    }


    public sealed class DictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override Dictionary<TKey, TValue> Create()
        {
            return new Dictionary<TKey, TValue>();
        }
    }

    public sealed class GenericDictionaryFormatter<TKey, TValue, TDictionary> : DictionaryFormatterBase<TKey, TValue, TDictionary>
        where TDictionary : class, IDictionary<TKey, TValue>, new()
    {
        protected override void Add(TDictionary collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override TDictionary Create()
        {
            return new TDictionary();
        }
    }

    public sealed class InterfaceDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, IDictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override Dictionary<TKey, TValue> Create()
        {
            return new Dictionary<TKey, TValue>();
        }

        protected override IDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return intermediateCollection;
        }
    }

    public sealed class SortedListFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, SortedList<TKey, TValue>>
    {
        protected override void Add(SortedList<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override SortedList<TKey, TValue> Create()
        {
            return new SortedList<TKey, TValue>();
        }
    }

    public sealed class SortedDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, SortedDictionary<TKey, TValue>>
    {
        protected override void Add(SortedDictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override SortedDictionary<TKey, TValue> Create()
        {
            return new SortedDictionary<TKey, TValue>();
        }
    }

    public sealed class ReadOnlyDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, ReadOnlyDictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override ReadOnlyDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return new ReadOnlyDictionary<TKey, TValue>(intermediateCollection);
        }

        protected override Dictionary<TKey, TValue> Create()
        {
            return new Dictionary<TKey, TValue>();
        }
    }

    public sealed class InterfaceReadOnlyDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, Dictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>>
    {
        protected override void Add(Dictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.Add(key, value);
        }

        protected override IReadOnlyDictionary<TKey, TValue> Complete(Dictionary<TKey, TValue> intermediateCollection)
        {
            return intermediateCollection;
        }

        protected override Dictionary<TKey, TValue> Create()
        {
            return new Dictionary<TKey, TValue>();
        }
    }

    public sealed class ConcurrentDictionaryFormatter<TKey, TValue> : DictionaryFormatterBase<TKey, TValue, ConcurrentDictionary<TKey, TValue>>
    {
        protected override void Add(ConcurrentDictionary<TKey, TValue> collection, TKey key, TValue value)
        {
            collection.TryAdd(key, value);
        }

        protected override ConcurrentDictionary<TKey, TValue> Create()
        {
            return new ConcurrentDictionary<TKey, TValue>();
        }
    }

    public sealed class NonGenericDictionaryFormatter<T> : FormatterBase<T>
        where T : class, IDictionary, new()
    {
        public override void Serialize(ref byte[] bytes, ref int offset, T value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var valueFormatter = resolver.GetFormatter<object>();

            JsonUtility.WriteBeginObject(ref bytes, ref offset);

            var e = value.GetEnumerator();
            try
            {
                if (!e.MoveNext())
                {
                    JsonUtility.WriteEndObject(ref bytes, ref offset);
                    return;
                }

                var item = (DictionaryEntry)e.Current;
                JsonUtility.WritePropertyName(ref bytes, ref offset, item.Key.ToString());
                valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);

                while (e.MoveNext())
                {
                    JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                    item = (DictionaryEntry)e.Current;
                    JsonUtility.WritePropertyName(ref bytes, ref offset, item.Key.ToString());
                    valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);
                }
            }
            finally
            {
                if (e is IDisposable d) { d.Dispose(); }
            }

            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override T Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return default;
            }

            var valueFormatter = resolver.GetFormatter<object>();
            var dict = new T();

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var key = StringEncoding.UTF8.GetString(JsonUtility.ReadPropertyNameSegment(bytes, ref offset));
                var value = valueFormatter.Deserialize(bytes, ref offset, resolver);
                dict.Add(key, value);

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return dict;
        }
    }

    public sealed class NonGenericInterfaceDictionaryFormatter : FormatterBase<IDictionary>
    {
        public static readonly IFormatter<IDictionary> Default = new NonGenericInterfaceDictionaryFormatter();

        public override void Serialize(ref byte[] bytes, ref int offset, IDictionary value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var valueFormatter = resolver.GetFormatter<object>();

            JsonUtility.WriteBeginObject(ref bytes, ref offset);

            var e = value.GetEnumerator();
            try
            {
                if (!e.MoveNext())
                {
                    JsonUtility.WriteEndObject(ref bytes, ref offset);
                    return;
                }

                var item = (DictionaryEntry)e.Current;
                JsonUtility.WritePropertyName(ref bytes, ref offset, item.Key.ToString());
                valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);

                while (e.MoveNext())
                {
                    JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                    item = (DictionaryEntry)e.Current;
                    JsonUtility.WritePropertyName(ref bytes, ref offset, item.Key.ToString());
                    valueFormatter.Serialize(ref bytes, ref offset, item.Value, resolver);
                }
            }
            finally
            {
                if (e is IDisposable d) { d.Dispose(); }
            }

            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override IDictionary Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }

            var valueFormatter = resolver.GetFormatter<object>();
            var dict = new Dictionary<object, object>();

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var key = StringEncoding.UTF8.GetString(JsonUtility.ReadPropertyNameSegment(bytes, ref offset));
                var value = valueFormatter.Deserialize(bytes, ref offset, resolver);
                dict.Add(key, value);

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return dict;
        }
    }
}
