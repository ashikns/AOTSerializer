using AOTSerializer.Common;
using System.Collections.Generic;

namespace AOTSerializer.Json.Formatters
{
    // multi dimensional array serialize to [[seq], [seq]]

    public sealed class TwoDimensionalArrayFormatter<T> : FormatterBase<T[,]>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, T[,] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatterWithVerify<T>();

            var iLength = value.GetLength(0);
            var jLength = value.GetLength(1);

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            for (int i = 0; i < iLength; i++)
            {
                if (i != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteBeginArray(ref bytes, ref offset);
                for (int j = 0; j < jLength; j++)
                {
                    if (j != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                    formatter.Serialize(ref bytes, ref offset, value[i, j], resolver);
                }
                JsonUtility.WriteEndArray(ref bytes, ref offset);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override T[,] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var buffer = new List<List<T>>(4);
            var formatter = resolver.GetFormatterWithVerify<T>();

            var guessInnerLength = 0;
            var outerCount = 0;
            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                if (outerCount++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }

                var innerArray = new List<T>(guessInnerLength);
                var innerCount = 0;
                JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
                while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
                {
                    if (innerCount++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }
                    innerArray.Add(formatter.Deserialize(bytes, ref offset, resolver));
                }

                guessInnerLength = innerArray.Count;
                buffer.Add(innerArray);
            }

            var t = new T[buffer.Count, guessInnerLength];
            for (int i = 0; i < buffer.Count; i++)
            {
                for (int j = 0; j < guessInnerLength; j++)
                {
                    t[i, j] = buffer[i][j];
                }
            }

            return t;
        }
    }

    public sealed class ThreeDimensionalArrayFormatter<T> : FormatterBase<T[,,]>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, T[,,] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatterWithVerify<T>();

            var iLength = value.GetLength(0);
            var jLength = value.GetLength(1);
            var kLength = value.GetLength(2);

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            for (int i = 0; i < iLength; i++)
            {
                if (i != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteBeginArray(ref bytes, ref offset);
                for (int j = 0; j < jLength; j++)
                {
                    if (j != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                    JsonUtility.WriteBeginArray(ref bytes, ref offset);
                    for (int k = 0; k < kLength; k++)
                    {
                        if (k != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        formatter.Serialize(ref bytes, ref offset, value[i, j, k], resolver);
                    }
                    JsonUtility.WriteEndArray(ref bytes, ref offset);
                }
                JsonUtility.WriteEndArray(ref bytes, ref offset);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override T[,,] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var buffer = new List<List<List<T>>>(4);
            var formatter = resolver.GetFormatterWithVerify<T>();

            var guessInnerLength2 = 0;
            var guessInnerLength = 0;
            var outerCount = 0;
            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                if (outerCount++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }

                var innerArray = new List<List<T>>(guessInnerLength);
                var innerCount = 0;
                JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
                while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
                {
                    if (innerCount++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }

                    var innerArray2 = new List<T>(guessInnerLength2);
                    var innerCount2 = 0;
                    JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
                    while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
                    {
                        if (innerCount2++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }
                        innerArray2.Add(formatter.Deserialize(bytes, ref offset, resolver));
                    }

                    guessInnerLength2 = innerArray2.Count;
                    innerArray.Add(innerArray2);
                }

                guessInnerLength = innerArray.Count;
                buffer.Add(innerArray);
            }

            var t = new T[buffer.Count, guessInnerLength, guessInnerLength2];
            for (int i = 0; i < buffer.Count; i++)
            {
                for (int j = 0; j < guessInnerLength; j++)
                {
                    for (int k = 0; k < guessInnerLength2; k++)
                    {
                        t[i, j, k] = buffer[i][j][k];
                    }
                }
            }

            return t;
        }
    }

    public sealed class FourDimensionalArrayFormatter<T> : FormatterBase<T[,,,]>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, T[,,,] value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }

            var formatter = resolver.GetFormatterWithVerify<T>();

            var iLength = value.GetLength(0);
            var jLength = value.GetLength(1);
            var kLength = value.GetLength(2);
            var lLength = value.GetLength(3);

            JsonUtility.WriteBeginArray(ref bytes, ref offset);
            for (int i = 0; i < iLength; i++)
            {
                if (i != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                JsonUtility.WriteBeginArray(ref bytes, ref offset);
                for (int j = 0; j < jLength; j++)
                {
                    if (j != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                    JsonUtility.WriteBeginArray(ref bytes, ref offset);
                    for (int k = 0; k < kLength; k++)
                    {
                        if (k != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                        JsonUtility.WriteBeginArray(ref bytes, ref offset);
                        for (int l = 0; l < lLength; l++)
                        {
                            if (l != 0) JsonUtility.WriteValueSeparator(ref bytes, ref offset);
                            formatter.Serialize(ref bytes, ref offset, value[i, j, k, l], resolver);
                        }
                        JsonUtility.WriteEndArray(ref bytes, ref offset);
                    }
                    JsonUtility.WriteEndArray(ref bytes, ref offset);
                }
                JsonUtility.WriteEndArray(ref bytes, ref offset);
            }
            JsonUtility.WriteEndArray(ref bytes, ref offset);
        }

        public override T[,,,] Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset)) { return null; }

            var buffer = new List<List<List<List<T>>>>(4);
            var formatter = resolver.GetFormatterWithVerify<T>();

            var guessInnerLength3 = 0;
            var guessInnerLength2 = 0;
            var guessInnerLength = 0;
            var outerCount = 0;
            JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
            while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
            {
                if (outerCount++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }

                var innerArray = new List<List<List<T>>>(guessInnerLength);
                var innerCount = 0;
                JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
                while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
                {
                    if (innerCount++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }

                    var innerArray2 = new List<List<T>>(guessInnerLength2);
                    var innerCount2 = 0;
                    JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
                    while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
                    {
                        if (innerCount2++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }

                        var innerArray3 = new List<T>(guessInnerLength3);
                        var innerCount3 = 0;
                        JsonUtility.ReadIsBeginArrayWithVerify(bytes, ref offset);
                        while (!JsonUtility.ReadIsEndArray(bytes, ref offset))
                        {
                            if (innerCount3++ != 0) { JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset); }
                            innerArray3.Add(formatter.Deserialize(bytes, ref offset, resolver));
                        }
                        guessInnerLength3 = innerArray3.Count;
                        innerArray2.Add(innerArray3);
                    }

                    guessInnerLength2 = innerArray2.Count;
                    innerArray.Add(innerArray2);
                }

                guessInnerLength = innerArray.Count;
                buffer.Add(innerArray);
            }

            var t = new T[buffer.Count, guessInnerLength, guessInnerLength2, guessInnerLength3];
            for (int i = 0; i < buffer.Count; i++)
            {
                for (int j = 0; j < guessInnerLength; j++)
                {
                    for (int k = 0; k < guessInnerLength2; k++)
                    {
                        for (int l = 0; l < guessInnerLength3; l++)
                        {
                            t[i, j, k, l] = buffer[i][j][k][l];
                        }
                    }
                }
            }

            return t;
        }
    }
}