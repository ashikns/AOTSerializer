using AOTSerializer.Common;
using System;

namespace AOTSerializer.MessagePack.Formatters
{

    public sealed class TupleFormatter<T1> : FormatterBase<Tuple<T1>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 1);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
            }
        }

        public override Tuple<T1> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 1) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1>(item1);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2> : FormatterBase<Tuple<T1, T2>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 2);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
            }
        }

        public override Tuple<T1, T2> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 2) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2>(item1, item2);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3> : FormatterBase<Tuple<T1, T2, T3>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 3);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
                resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
            }
        }

        public override Tuple<T1, T2, T3> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 3) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2, T3>(item1, item2, item3);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4> : FormatterBase<Tuple<T1, T2, T3, T4>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 4);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
                resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
                resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
            }
        }

        public override Tuple<T1, T2, T3, T4> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 4) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5> : FormatterBase<Tuple<T1, T2, T3, T4, T5>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 5);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
                resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
                resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
                resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
            }
        }

        public override Tuple<T1, T2, T3, T4, T5> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 5) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5, T6> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 6);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
                resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
                resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
                resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
                resolver.GetFormatter<T6>().Serialize(ref bytes, ref offset, value.Item6, resolver);
            }
        }

        public override Tuple<T1, T2, T3, T4, T5, T6> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 6) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                var item6 = resolver.GetFormatter<T6>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5, T6, T7> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 7);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
                resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
                resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
                resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
                resolver.GetFormatter<T6>().Serialize(ref bytes, ref offset, value.Item6, resolver);
                resolver.GetFormatter<T7>().Serialize(ref bytes, ref offset, value.Item7, resolver);
            }
        }

        public override Tuple<T1, T2, T3, T4, T5, T6, T7> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 7) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                var item6 = resolver.GetFormatter<T6>().Deserialize(bytes, ref offset, resolver);
                var item7 = resolver.GetFormatter<T7>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7, TRest> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> value, IResolver resolver)
        {
            if (value == null)
            {
                MessagePackBinary.WriteNil(ref bytes, ref offset);
            }
            else
            {
                MessagePackBinary.WriteArrayHeader(ref bytes, ref offset, 8);

                resolver.GetFormatter<T1>().Serialize(ref bytes, ref offset, value.Item1, resolver);
                resolver.GetFormatter<T2>().Serialize(ref bytes, ref offset, value.Item2, resolver);
                resolver.GetFormatter<T3>().Serialize(ref bytes, ref offset, value.Item3, resolver);
                resolver.GetFormatter<T4>().Serialize(ref bytes, ref offset, value.Item4, resolver);
                resolver.GetFormatter<T5>().Serialize(ref bytes, ref offset, value.Item5, resolver);
                resolver.GetFormatter<T6>().Serialize(ref bytes, ref offset, value.Item6, resolver);
                resolver.GetFormatter<T7>().Serialize(ref bytes, ref offset, value.Item7, resolver);
                resolver.GetFormatter<TRest>().Serialize(ref bytes, ref offset, value.Rest, resolver);
            }
        }

        public override Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                offset += 1;
                return null;
            }
            else
            {
                var count = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
                if (count != 8) throw new InvalidOperationException("Invalid Tuple count");

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, ref offset, resolver);
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, ref offset, resolver);
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, ref offset, resolver);
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, ref offset, resolver);
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, ref offset, resolver);
                var item6 = resolver.GetFormatter<T6>().Deserialize(bytes, ref offset, resolver);
                var item7 = resolver.GetFormatter<T7>().Deserialize(bytes, ref offset, resolver);
                var item8 = resolver.GetFormatter<TRest>().Deserialize(bytes, ref offset, resolver);

                return new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, item8);
            }
        }
    }

}
