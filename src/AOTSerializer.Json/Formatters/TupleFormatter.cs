using AOTSerializer.Common;
using AOTSerializer.Internal;
using AOTSerializer.Json.Formatters.Internal;
using System;
using System.Linq;

namespace AOTSerializer.Json.Formatters.Internal
{
    // reduce static constructor generate size on generics(especially IL2CPP on Unity)
    internal static class TupleFormatterHelper
    {
        internal static readonly byte[][] nameCache1;
        internal static readonly AutomataDictionary dictionary1;
        internal static readonly byte[][] nameCache2;
        internal static readonly AutomataDictionary dictionary2;
        internal static readonly byte[][] nameCache3;
        internal static readonly AutomataDictionary dictionary3;
        internal static readonly byte[][] nameCache4;
        internal static readonly AutomataDictionary dictionary4;
        internal static readonly byte[][] nameCache5;
        internal static readonly AutomataDictionary dictionary5;
        internal static readonly byte[][] nameCache6;
        internal static readonly AutomataDictionary dictionary6;
        internal static readonly byte[][] nameCache7;
        internal static readonly AutomataDictionary dictionary7;
        internal static readonly byte[][] nameCache8;
        internal static readonly AutomataDictionary dictionary8;

        static TupleFormatterHelper()
        {
            nameCache1 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
            };
            dictionary1 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
            };
            nameCache2 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
            };
            dictionary2 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
            };
            nameCache3 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item3").ToArray(),
            };
            dictionary3 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item3")), 2 },
            };
            nameCache4 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item3").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item4").ToArray(),
            };
            dictionary4 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item3")), 2 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item4")), 3 },
            };
            nameCache5 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item3").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item4").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item5").ToArray(),
            };
            dictionary5 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item3")), 2 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item4")), 3 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item5")), 4 },
            };
            nameCache6 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item3").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item4").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item5").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item6").ToArray(),
            };
            dictionary6 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item3")), 2 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item4")), 3 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item5")), 4 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item6")), 5 },
            };
            nameCache7 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item3").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item4").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item5").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item6").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item7").ToArray(),
            };
            dictionary7 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item3")), 2 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item4")), 3 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item5")), 4 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item6")), 5 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item7")), 6 },
            };
            nameCache8 = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("Item1").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item2").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item3").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item4").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item5").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item6").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Item7").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("Rest").ToArray(),
            };
            dictionary8 = new AutomataDictionary
            {
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item1")), 0 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item2")), 1 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item3")), 2 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item4")), 3 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item5")), 4 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item6")), 5 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Item7")), 6 },
                { StringEncoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("Rest")), 7 },
            };
        }
    }
}

namespace AOTSerializer.Json.Formatters
{

    public sealed class TupleFormatter<T1> : FormatterBase<Tuple<T1>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache1;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary1;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1>(item1);
        }
    }


    public sealed class TupleFormatter<T1, T2> : FormatterBase<Tuple<T1, T2>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache2;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary2;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2>(item1, item2);
        }
    }


    public sealed class TupleFormatter<T1, T2, T3> : FormatterBase<Tuple<T1, T2, T3>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache3;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary3;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[2]);
            resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2, T3> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;
            T3 item3 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 2:
                        item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2, T3>(item1, item2, item3);
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4> : FormatterBase<Tuple<T1, T2, T3, T4>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache4;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary4;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[2]);
            resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[3]);
            resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2, T3, T4> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;
            T3 item3 = default;
            T4 item4 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 2:
                        item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 3:
                        item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5> : FormatterBase<Tuple<T1, T2, T3, T4, T5>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache5;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary5;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[2]);
            resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[3]);
            resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[4]);
            resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2, T3, T4, T5> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;
            T3 item3 = default;
            T4 item4 = default;
            T5 item5 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 2:
                        item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 3:
                        item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 4:
                        item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache6;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary6;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5, T6> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[2]);
            resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[3]);
            resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[4]);
            resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[5]);
            resolver.GetFormatter<T6>().Serialize(ref bytes, ref offset, value.Item6, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2, T3, T4, T5, T6> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;
            T3 item3 = default;
            T4 item4 = default;
            T5 item5 = default;
            T6 item6 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 2:
                        item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 3:
                        item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 4:
                        item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 5:
                        item6 = resolver.GetFormatter<T6>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache7;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary7;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5, T6, T7> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[2]);
            resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[3]);
            resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[4]);
            resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[5]);
            resolver.GetFormatter<T6>().Serialize(ref bytes, ref offset, value.Item6, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[6]);
            resolver.GetFormatter<T7>().Serialize(ref bytes, ref offset, value.Item7, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2, T3, T4, T5, T6, T7> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;
            T3 item3 = default;
            T4 item4 = default;
            T5 item5 = default;
            T6 item6 = default;
            T7 item7 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 2:
                        item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 3:
                        item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 4:
                        item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 5:
                        item6 = resolver.GetFormatter<T6>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 6:
                        item7 = resolver.GetFormatter<T7>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7, TRest> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>
    {
        private static readonly byte[][] cache = TupleFormatterHelper.nameCache8;
        private static readonly AutomataDictionary dictionary = TupleFormatterHelper.dictionary8;

        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            JsonUtility.WriteRaw(ref bytes, ref offset, cache[0]);
            resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[1]);
            resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[2]);
            resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[3]);
            resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[4]);
            resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[5]);
            resolver.GetFormatter<T6>().Serialize(ref bytes, ref offset, value.Item6, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[6]);
            resolver.GetFormatter<T7>().Serialize(ref bytes, ref offset, value.Item7, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, cache[7]);
            resolver.GetFormatter<TRest>().Serialize(ref bytes, ref offset, value.Rest, resolver);
            JsonUtility.WriteEndObject(ref bytes, ref offset);
        }

        public override Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            T1 item1 = default;
            T2 item2 = default;
            T3 item3 = default;
            T4 item4 = default;
            T5 item5 = default;
            T6 item6 = default;
            T7 item7 = default;
            TRest item8 = default;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                var keyString = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                dictionary.TryGetValue(keyString, out var key);

                switch (key)
                {
                    case 0:
                        item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 1:
                        item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 2:
                        item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 3:
                        item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 4:
                        item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 5:
                        item6 = resolver.GetFormatter<T6>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 6:
                        item7 = resolver.GetFormatter<T7>().Deserialize(bytes, ref offset, resolver);
                        break;
                    case 7:
                        item8 = resolver.GetFormatter<TRest>().Deserialize(bytes, ref offset, resolver);
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset, out _);
                        break;
                }

                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);
            }

            return new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, item8);
        }
    }

}
