using System;
using AOTSerializer.Common;

namespace AOTSerializer.MessagePack.Formatters
{

    public sealed class TupleFormatter<T1> : FormatterBase<Tuple<T1>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 1);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 1) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1>(item1);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2> : FormatterBase<Tuple<T1, T2>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 2);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 2) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2>(item1, item2);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3> : FormatterBase<Tuple<T1, T2, T3>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2, T3> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 3);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);
                offset += resolver.GetFormatter<T3>().Serialize(ref bytes, offset, value.Item3, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2, T3> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 3) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2, T3>(item1, item2, item3);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4> : FormatterBase<Tuple<T1, T2, T3, T4>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2, T3, T4> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 4);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);
                offset += resolver.GetFormatter<T3>().Serialize(ref bytes, offset, value.Item3, resolver);
                offset += resolver.GetFormatter<T4>().Serialize(ref bytes, offset, value.Item4, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2, T3, T4> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 4) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5> : FormatterBase<Tuple<T1, T2, T3, T4, T5>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2, T3, T4, T5> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 5);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);
                offset += resolver.GetFormatter<T3>().Serialize(ref bytes, offset, value.Item3, resolver);
                offset += resolver.GetFormatter<T4>().Serialize(ref bytes, offset, value.Item4, resolver);
                offset += resolver.GetFormatter<T5>().Serialize(ref bytes, offset, value.Item5, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2, T3, T4, T5> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 5) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2, T3, T4, T5, T6> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 6);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);
                offset += resolver.GetFormatter<T3>().Serialize(ref bytes, offset, value.Item3, resolver);
                offset += resolver.GetFormatter<T4>().Serialize(ref bytes, offset, value.Item4, resolver);
                offset += resolver.GetFormatter<T5>().Serialize(ref bytes, offset, value.Item5, resolver);
                offset += resolver.GetFormatter<T6>().Serialize(ref bytes, offset, value.Item6, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2, T3, T4, T5, T6> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 6) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item6 = resolver.GetFormatter<T6>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6, T7>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2, T3, T4, T5, T6, T7> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 7);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);
                offset += resolver.GetFormatter<T3>().Serialize(ref bytes, offset, value.Item3, resolver);
                offset += resolver.GetFormatter<T4>().Serialize(ref bytes, offset, value.Item4, resolver);
                offset += resolver.GetFormatter<T5>().Serialize(ref bytes, offset, value.Item5, resolver);
                offset += resolver.GetFormatter<T6>().Serialize(ref bytes, offset, value.Item6, resolver);
                offset += resolver.GetFormatter<T7>().Serialize(ref bytes, offset, value.Item7, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2, T3, T4, T5, T6, T7> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 7) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item6 = resolver.GetFormatter<T6>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item7 = resolver.GetFormatter<T7>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
            }
        }
    }


    public sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7, TRest> : FormatterBase<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>
    {
        public override int Serialize(ref byte[] bytes, int offset, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> value, IResolver resolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }
            else
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteArrayHeader(ref bytes, offset, 8);

                offset += resolver.GetFormatter<T1>().Serialize(ref bytes, offset, value.Item1, resolver);
                offset += resolver.GetFormatter<T2>().Serialize(ref bytes, offset, value.Item2, resolver);
                offset += resolver.GetFormatter<T3>().Serialize(ref bytes, offset, value.Item3, resolver);
                offset += resolver.GetFormatter<T4>().Serialize(ref bytes, offset, value.Item4, resolver);
                offset += resolver.GetFormatter<T5>().Serialize(ref bytes, offset, value.Item5, resolver);
                offset += resolver.GetFormatter<T6>().Serialize(ref bytes, offset, value.Item6, resolver);
                offset += resolver.GetFormatter<T7>().Serialize(ref bytes, offset, value.Item7, resolver);
                offset += resolver.GetFormatter<TRest>().Serialize(ref bytes, offset, value.Rest, resolver);

                return offset - startOffset;
            }
        }

        public override Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Deserialize(byte[] bytes, int offset, out int readSize, IResolver resolver)
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
                if (count != 8) throw new InvalidOperationException("Invalid Tuple count");
                offset += readSize;

                var item1 = resolver.GetFormatter<T1>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item2 = resolver.GetFormatter<T2>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item3 = resolver.GetFormatter<T3>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item4 = resolver.GetFormatter<T4>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item5 = resolver.GetFormatter<T5>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item6 = resolver.GetFormatter<T6>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item7 = resolver.GetFormatter<T7>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
                var item8 = resolver.GetFormatter<TRest>().Deserialize(bytes, offset, out readSize, resolver);
                offset += readSize;
            
                readSize = offset - startOffset;
                return new Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>(item1, item2, item3, item4, item5, item6, item7, item8);
            }
        }
    }

}
