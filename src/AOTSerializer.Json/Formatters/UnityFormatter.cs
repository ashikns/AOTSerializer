#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace AOTSerializer.Json.Formatters.UnityEngine
{
    using AOTSerializer.Common;
    using AOTSerializer.Json;
    using System;

    public sealed class WeightedModeFormatter : FormatterBase<global::UnityEngine.WeightedMode>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.WeightedMode value, IResolver resolver)
        {
            JsonUtility.WriteInt32(ref bytes, ref offset, (Int32)value);
        }

        public override global::UnityEngine.WeightedMode Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return (global::UnityEngine.WeightedMode)JsonUtility.ReadInt32(bytes, ref offset);
        }
    }

    public sealed class WrapModeFormatter : FormatterBase<global::UnityEngine.WrapMode>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.WrapMode value, IResolver resolver)
        {
            JsonUtility.WriteInt32(ref bytes, ref offset, (Int32)value);
        }

        public override global::UnityEngine.WrapMode Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return (global::UnityEngine.WrapMode)JsonUtility.ReadInt32(bytes, ref offset);
        }
    }

    public sealed class GradientModeFormatter : FormatterBase<global::UnityEngine.GradientMode>
    {
        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.GradientMode value, IResolver resolver)
        {
            JsonUtility.WriteInt32(ref bytes, ref offset, (Int32)value);
        }

        public override global::UnityEngine.GradientMode Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            return (global::UnityEngine.GradientMode)JsonUtility.ReadInt32(bytes, ref offset);
        }
    }


}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612


#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace AOTSerializer.Json.Formatters.UnityEngine
{
    using AOTSerializer.Common;
    using AOTSerializer.Internal;
    using AOTSerializer.Json;
    using System;
    using System.Linq;


    public sealed class Vector2Formatter : FormatterBase<global::UnityEngine.Vector2>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public Vector2Formatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("x")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("y")), 1 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("x").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("y").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Vector2 value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.x);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.y);
        }

        public override global::UnityEngine.Vector2 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __x__ = default(float);
            var __x__b__ = false;
            var __y__ = default(float);
            var __y__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Vector2(__x__, __y__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Vector2(__x__, __y__);
            if (__x__b__) ____result.x = __x__;
            if (__y__b__) ____result.y = __y__;

            return ____result;
        }
    }


    public sealed class Vector3Formatter : FormatterBase<global::UnityEngine.Vector3>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public Vector3Formatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("x")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("y")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("z")), 2 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("x").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("y").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("z").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Vector3 value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.x);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.y);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.z);
        }

        public override global::UnityEngine.Vector3 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __x__ = default(float);
            var __x__b__ = false;
            var __y__ = default(float);
            var __y__b__ = false;
            var __z__ = default(float);
            var __z__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Vector3(__x__, __y__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 2:
                        __z__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __z__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 2:
                        __z__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __z__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Vector3(__x__, __y__);
            if (__x__b__) ____result.x = __x__;
            if (__y__b__) ____result.y = __y__;
            if (__z__b__) ____result.z = __z__;

            return ____result;
        }
    }


    public sealed class Vector4Formatter : FormatterBase<global::UnityEngine.Vector4>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public Vector4Formatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("x")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("y")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("z")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("w")), 3 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("x").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("y").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("z").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("w").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Vector4 value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.x);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.y);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.z);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.w);
        }

        public override global::UnityEngine.Vector4 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __x__ = default(float);
            var __x__b__ = false;
            var __y__ = default(float);
            var __y__b__ = false;
            var __z__ = default(float);
            var __z__b__ = false;
            var __w__ = default(float);
            var __w__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Vector4(__x__, __y__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 2:
                        __z__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __z__b__ = true;
                        break;
                    case 3:
                        __w__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __w__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 2:
                        __z__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __z__b__ = true;
                        break;
                    case 3:
                        __w__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __w__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Vector4(__x__, __y__);
            if (__x__b__) ____result.x = __x__;
            if (__y__b__) ____result.y = __y__;
            if (__z__b__) ____result.z = __z__;
            if (__w__b__) ____result.w = __w__;

            return ____result;
        }
    }


    public sealed class QuaternionFormatter : FormatterBase<global::UnityEngine.Quaternion>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public QuaternionFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("eulerAngles")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("x")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("y")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("z")), 3 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("w")), 4 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("eulerAngles").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("x").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("y").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("z").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("w").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Quaternion value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, ref offset, value.eulerAngles, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.x);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.y);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.z);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[4]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.w);
        }

        public override global::UnityEngine.Quaternion Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __eulerAngles__ = default(global::UnityEngine.Vector3);
            var __eulerAngles__b__ = false;
            var __x__ = default(float);
            var __x__b__ = false;
            var __y__ = default(float);
            var __y__b__ = false;
            var __z__ = default(float);
            var __z__b__ = false;
            var __w__ = default(float);
            var __w__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Quaternion(__x__, __y__, __z__, __w__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __eulerAngles__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __eulerAngles__b__ = true;
                        break;
                    case 1:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 2:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 3:
                        __z__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __z__b__ = true;
                        break;
                    case 4:
                        __w__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __w__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __eulerAngles__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __eulerAngles__b__ = true;
                        break;
                    case 1:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 2:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 3:
                        __z__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __z__b__ = true;
                        break;
                    case 4:
                        __w__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __w__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Quaternion(__x__, __y__, __z__, __w__);
            if (__eulerAngles__b__) ____result.eulerAngles = __eulerAngles__;
            if (__x__b__) ____result.x = __x__;
            if (__y__b__) ____result.y = __y__;
            if (__z__b__) ____result.z = __z__;
            if (__w__b__) ____result.w = __w__;

            return ____result;
        }
    }


    public sealed class ColorFormatter : FormatterBase<global::UnityEngine.Color>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public ColorFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("r")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("g")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("b")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("a")), 3 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("r").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("g").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("b").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("a").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Color value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.r);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.g);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.b);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.a);
        }

        public override global::UnityEngine.Color Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __r__ = default(float);
            var __r__b__ = false;
            var __g__ = default(float);
            var __g__b__ = false;
            var __b__ = default(float);
            var __b__b__ = false;
            var __a__ = default(float);
            var __a__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Color(__r__, __g__, __b__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __r__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __r__b__ = true;
                        break;
                    case 1:
                        __g__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __g__b__ = true;
                        break;
                    case 2:
                        __b__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __b__b__ = true;
                        break;
                    case 3:
                        __a__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __a__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __r__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __r__b__ = true;
                        break;
                    case 1:
                        __g__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __g__b__ = true;
                        break;
                    case 2:
                        __b__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __b__b__ = true;
                        break;
                    case 3:
                        __a__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __a__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Color(__r__, __g__, __b__);
            if (__r__b__) ____result.r = __r__;
            if (__g__b__) ____result.g = __g__;
            if (__b__b__) ____result.b = __b__;
            if (__a__b__) ____result.a = __a__;

            return ____result;
        }
    }


    public sealed class BoundsFormatter : FormatterBase<global::UnityEngine.Bounds>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public BoundsFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("center")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("size")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("extents")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("min")), 3 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("max")), 4 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("center").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("size").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("extents").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("min").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("max").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Bounds value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, ref offset, value.center, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, ref offset, value.size, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, ref offset, value.extents, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, ref offset, value.min, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[4]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Serialize(ref bytes, ref offset, value.max, resolver);
        }

        public override global::UnityEngine.Bounds Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __center__ = default(global::UnityEngine.Vector3);
            var __center__b__ = false;
            var __size__ = default(global::UnityEngine.Vector3);
            var __size__b__ = false;
            var __extents__ = default(global::UnityEngine.Vector3);
            var __extents__b__ = false;
            var __min__ = default(global::UnityEngine.Vector3);
            var __min__b__ = false;
            var __max__ = default(global::UnityEngine.Vector3);
            var __max__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Bounds(__center__, __size__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __center__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __center__b__ = true;
                        break;
                    case 1:
                        __size__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __size__b__ = true;
                        break;
                    case 2:
                        __extents__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __extents__b__ = true;
                        break;
                    case 3:
                        __min__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __min__b__ = true;
                        break;
                    case 4:
                        __max__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __max__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __center__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __center__b__ = true;
                        break;
                    case 1:
                        __size__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __size__b__ = true;
                        break;
                    case 2:
                        __extents__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __extents__b__ = true;
                        break;
                    case 3:
                        __min__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __min__b__ = true;
                        break;
                    case 4:
                        __max__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector3>().Deserialize(bytes, ref offset, resolver);
                        __max__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Bounds(__center__, __size__);
            if (__center__b__) ____result.center = __center__;
            if (__size__b__) ____result.size = __size__;
            if (__extents__b__) ____result.extents = __extents__;
            if (__min__b__) ____result.min = __min__;
            if (__max__b__) ____result.max = __max__;

            return ____result;
        }
    }


    public sealed class RectFormatter : FormatterBase<global::UnityEngine.Rect>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public RectFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("x")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("y")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("position")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("center")), 3 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("min")), 4 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("max")), 5 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("width")), 6 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("height")), 7 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("size")), 8 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("xMin")), 9 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("yMin")), 10 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("xMax")), 11 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("yMax")), 12 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("x").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("y").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("position").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("center").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("min").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("max").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("width").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("height").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("size").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("xMin").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("yMin").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("xMax").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("yMax").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Rect value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.x);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.y);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Serialize(ref bytes, ref offset, value.position, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Serialize(ref bytes, ref offset, value.center, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[4]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Serialize(ref bytes, ref offset, value.min, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[5]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Serialize(ref bytes, ref offset, value.max, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[6]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.width);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[7]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.height);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[8]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Serialize(ref bytes, ref offset, value.size, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[9]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.xMin);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[10]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.yMin);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[11]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.xMax);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[12]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.yMax);
        }

        public override global::UnityEngine.Rect Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __x__ = default(float);
            var __x__b__ = false;
            var __y__ = default(float);
            var __y__b__ = false;
            var __position__ = default(global::UnityEngine.Vector2);
            var __position__b__ = false;
            var __center__ = default(global::UnityEngine.Vector2);
            var __center__b__ = false;
            var __min__ = default(global::UnityEngine.Vector2);
            var __min__b__ = false;
            var __max__ = default(global::UnityEngine.Vector2);
            var __max__b__ = false;
            var __width__ = default(float);
            var __width__b__ = false;
            var __height__ = default(float);
            var __height__b__ = false;
            var __size__ = default(global::UnityEngine.Vector2);
            var __size__b__ = false;
            var __xMin__ = default(float);
            var __xMin__b__ = false;
            var __yMin__ = default(float);
            var __yMin__b__ = false;
            var __xMax__ = default(float);
            var __xMax__b__ = false;
            var __yMax__ = default(float);
            var __yMax__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Rect(__position__, __size__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 2:
                        __position__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __position__b__ = true;
                        break;
                    case 3:
                        __center__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __center__b__ = true;
                        break;
                    case 4:
                        __min__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __min__b__ = true;
                        break;
                    case 5:
                        __max__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __max__b__ = true;
                        break;
                    case 6:
                        __width__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __width__b__ = true;
                        break;
                    case 7:
                        __height__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __height__b__ = true;
                        break;
                    case 8:
                        __size__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __size__b__ = true;
                        break;
                    case 9:
                        __xMin__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __xMin__b__ = true;
                        break;
                    case 10:
                        __yMin__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __yMin__b__ = true;
                        break;
                    case 11:
                        __xMax__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __xMax__b__ = true;
                        break;
                    case 12:
                        __yMax__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __yMax__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __x__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __x__b__ = true;
                        break;
                    case 1:
                        __y__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __y__b__ = true;
                        break;
                    case 2:
                        __position__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __position__b__ = true;
                        break;
                    case 3:
                        __center__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __center__b__ = true;
                        break;
                    case 4:
                        __min__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __min__b__ = true;
                        break;
                    case 5:
                        __max__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __max__b__ = true;
                        break;
                    case 6:
                        __width__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __width__b__ = true;
                        break;
                    case 7:
                        __height__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __height__b__ = true;
                        break;
                    case 8:
                        __size__ = resolver.GetFormatterWithVerify<global::UnityEngine.Vector2>().Deserialize(bytes, ref offset, resolver);
                        __size__b__ = true;
                        break;
                    case 9:
                        __xMin__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __xMin__b__ = true;
                        break;
                    case 10:
                        __yMin__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __yMin__b__ = true;
                        break;
                    case 11:
                        __xMax__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __xMax__b__ = true;
                        break;
                    case 12:
                        __yMax__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __yMax__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Rect(__position__, __size__);
            if (__x__b__) ____result.x = __x__;
            if (__y__b__) ____result.y = __y__;
            if (__position__b__) ____result.position = __position__;
            if (__center__b__) ____result.center = __center__;
            if (__min__b__) ____result.min = __min__;
            if (__max__b__) ____result.max = __max__;
            if (__width__b__) ____result.width = __width__;
            if (__height__b__) ____result.height = __height__;
            if (__size__b__) ____result.size = __size__;
            if (__xMin__b__) ____result.xMin = __xMin__;
            if (__yMin__b__) ____result.yMin = __yMin__;
            if (__xMax__b__) ____result.xMax = __xMax__;
            if (__yMax__b__) ____result.yMax = __yMax__;

            return ____result;
        }
    }


    public sealed class KeyframeFormatter : FormatterBase<global::UnityEngine.Keyframe>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public KeyframeFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("time")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("value")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("inTangent")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("outTangent")), 3 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("inWeight")), 4 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("outWeight")), 5 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("weightedMode")), 6 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("tangentMode")), 7 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("time").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("value").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("inTangent").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("outTangent").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("inWeight").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("outWeight").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("weightedMode").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("tangentMode").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Keyframe value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.time);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.value);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.inTangent);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.outTangent);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[4]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.inWeight);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[5]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.outWeight);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[6]);
            resolver.GetFormatterWithVerify<global::UnityEngine.WeightedMode>().Serialize(ref bytes, ref offset, value.weightedMode, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[7]);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.tangentMode);
        }

        public override global::UnityEngine.Keyframe Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __time__ = default(float);
            var __time__b__ = false;
            var __value__ = default(float);
            var __value__b__ = false;
            var __inTangent__ = default(float);
            var __inTangent__b__ = false;
            var __outTangent__ = default(float);
            var __outTangent__b__ = false;
            var __inWeight__ = default(float);
            var __inWeight__b__ = false;
            var __outWeight__ = default(float);
            var __outWeight__b__ = false;
            var __weightedMode__ = default(global::UnityEngine.WeightedMode);
            var __weightedMode__b__ = false;
            var __tangentMode__ = default(int);
            var __tangentMode__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Keyframe(__time__, __value__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __time__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __time__b__ = true;
                        break;
                    case 1:
                        __value__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __value__b__ = true;
                        break;
                    case 2:
                        __inTangent__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __inTangent__b__ = true;
                        break;
                    case 3:
                        __outTangent__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __outTangent__b__ = true;
                        break;
                    case 4:
                        __inWeight__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __inWeight__b__ = true;
                        break;
                    case 5:
                        __outWeight__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __outWeight__b__ = true;
                        break;
                    case 6:
                        __weightedMode__ = resolver.GetFormatterWithVerify<global::UnityEngine.WeightedMode>().Deserialize(bytes, ref offset, resolver);
                        __weightedMode__b__ = true;
                        break;
                    case 7:
                        __tangentMode__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __tangentMode__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __time__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __time__b__ = true;
                        break;
                    case 1:
                        __value__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __value__b__ = true;
                        break;
                    case 2:
                        __inTangent__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __inTangent__b__ = true;
                        break;
                    case 3:
                        __outTangent__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __outTangent__b__ = true;
                        break;
                    case 4:
                        __inWeight__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __inWeight__b__ = true;
                        break;
                    case 5:
                        __outWeight__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __outWeight__b__ = true;
                        break;
                    case 6:
                        __weightedMode__ = resolver.GetFormatterWithVerify<global::UnityEngine.WeightedMode>().Deserialize(bytes, ref offset, resolver);
                        __weightedMode__b__ = true;
                        break;
                    case 7:
                        __tangentMode__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __tangentMode__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Keyframe(__time__, __value__);
            if (__time__b__) ____result.time = __time__;
            if (__value__b__) ____result.value = __value__;
            if (__inTangent__b__) ____result.inTangent = __inTangent__;
            if (__outTangent__b__) ____result.outTangent = __outTangent__;
            if (__inWeight__b__) ____result.inWeight = __inWeight__;
            if (__outWeight__b__) ____result.outWeight = __outWeight__;
            if (__weightedMode__b__) ____result.weightedMode = __weightedMode__;
            if (__tangentMode__b__) ____result.tangentMode = __tangentMode__;

            return ____result;
        }
    }


    public sealed class AnimationCurveFormatter : FormatterBase<global::UnityEngine.AnimationCurve>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public AnimationCurveFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("keys")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("preWrapMode")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("postWrapMode")), 2 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("keys").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("preWrapMode").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("postWrapMode").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.AnimationCurve value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Keyframe[]>().Serialize(ref bytes, ref offset, value.keys, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            resolver.GetFormatterWithVerify<global::UnityEngine.WrapMode>().Serialize(ref bytes, ref offset, value.preWrapMode, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            resolver.GetFormatterWithVerify<global::UnityEngine.WrapMode>().Serialize(ref bytes, ref offset, value.postWrapMode, resolver);
        }

        public override global::UnityEngine.AnimationCurve Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }


            var __keys__ = default(global::UnityEngine.Keyframe[]);
            var __keys__b__ = false;
            var __preWrapMode__ = default(global::UnityEngine.WrapMode);
            var __preWrapMode__b__ = false;
            var __postWrapMode__ = default(global::UnityEngine.WrapMode);
            var __postWrapMode__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.AnimationCurve();
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __keys__ = resolver.GetFormatterWithVerify<global::UnityEngine.Keyframe[]>().Deserialize(bytes, ref offset, resolver);
                        __keys__b__ = true;
                        break;
                    case 1:
                        __preWrapMode__ = resolver.GetFormatterWithVerify<global::UnityEngine.WrapMode>().Deserialize(bytes, ref offset, resolver);
                        __preWrapMode__b__ = true;
                        break;
                    case 2:
                        __postWrapMode__ = resolver.GetFormatterWithVerify<global::UnityEngine.WrapMode>().Deserialize(bytes, ref offset, resolver);
                        __postWrapMode__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __keys__ = resolver.GetFormatterWithVerify<global::UnityEngine.Keyframe[]>().Deserialize(bytes, ref offset, resolver);
                        __keys__b__ = true;
                        break;
                    case 1:
                        __preWrapMode__ = resolver.GetFormatterWithVerify<global::UnityEngine.WrapMode>().Deserialize(bytes, ref offset, resolver);
                        __preWrapMode__b__ = true;
                        break;
                    case 2:
                        __postWrapMode__ = resolver.GetFormatterWithVerify<global::UnityEngine.WrapMode>().Deserialize(bytes, ref offset, resolver);
                        __postWrapMode__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.AnimationCurve();
            if (__keys__b__) ____result.keys = __keys__;
            if (__preWrapMode__b__) ____result.preWrapMode = __preWrapMode__;
            if (__postWrapMode__b__) ____result.postWrapMode = __postWrapMode__;

            return ____result;
        }
    }


    public sealed class RectOffsetFormatter : FormatterBase<global::UnityEngine.RectOffset>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public RectOffsetFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("left")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("right")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("top")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("bottom")), 3 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("left").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("right").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("top").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("bottom").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.RectOffset value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.left);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.right);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.top);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.bottom);
        }

        public override global::UnityEngine.RectOffset Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }


            var __left__ = default(int);
            var __left__b__ = false;
            var __right__ = default(int);
            var __right__b__ = false;
            var __top__ = default(int);
            var __top__b__ = false;
            var __bottom__ = default(int);
            var __bottom__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.RectOffset();
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __left__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __left__b__ = true;
                        break;
                    case 1:
                        __right__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __right__b__ = true;
                        break;
                    case 2:
                        __top__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __top__b__ = true;
                        break;
                    case 3:
                        __bottom__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __bottom__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __left__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __left__b__ = true;
                        break;
                    case 1:
                        __right__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __right__b__ = true;
                        break;
                    case 2:
                        __top__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __top__b__ = true;
                        break;
                    case 3:
                        __bottom__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __bottom__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.RectOffset();
            if (__left__b__) ____result.left = __left__;
            if (__right__b__) ____result.right = __right__;
            if (__top__b__) ____result.top = __top__;
            if (__bottom__b__) ____result.bottom = __bottom__;

            return ____result;
        }
    }


    public sealed class GradientColorKeyFormatter : FormatterBase<global::UnityEngine.GradientColorKey>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public GradientColorKeyFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("color")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("time")), 1 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("color").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("time").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.GradientColorKey value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            resolver.GetFormatterWithVerify<global::UnityEngine.Color>().Serialize(ref bytes, ref offset, value.color, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.time);
        }

        public override global::UnityEngine.GradientColorKey Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __color__ = default(global::UnityEngine.Color);
            var __color__b__ = false;
            var __time__ = default(float);
            var __time__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.GradientColorKey();
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __color__ = resolver.GetFormatterWithVerify<global::UnityEngine.Color>().Deserialize(bytes, ref offset, resolver);
                        __color__b__ = true;
                        break;
                    case 1:
                        __time__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __time__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __color__ = resolver.GetFormatterWithVerify<global::UnityEngine.Color>().Deserialize(bytes, ref offset, resolver);
                        __color__b__ = true;
                        break;
                    case 1:
                        __time__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __time__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.GradientColorKey();
            if (__color__b__) ____result.color = __color__;
            if (__time__b__) ____result.time = __time__;

            return ____result;
        }
    }


    public sealed class GradientAlphaKeyFormatter : FormatterBase<global::UnityEngine.GradientAlphaKey>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public GradientAlphaKeyFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("alpha")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("time")), 1 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("alpha").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("time").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.GradientAlphaKey value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.alpha);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.time);
        }

        public override global::UnityEngine.GradientAlphaKey Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __alpha__ = default(float);
            var __alpha__b__ = false;
            var __time__ = default(float);
            var __time__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.GradientAlphaKey(__alpha__, __time__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __alpha__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __alpha__b__ = true;
                        break;
                    case 1:
                        __time__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __time__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __alpha__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __alpha__b__ = true;
                        break;
                    case 1:
                        __time__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __time__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.GradientAlphaKey(__alpha__, __time__);
            if (__alpha__b__) ____result.alpha = __alpha__;
            if (__time__b__) ____result.time = __time__;

            return ____result;
        }
    }


    public sealed class GradientFormatter : FormatterBase<global::UnityEngine.Gradient>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public GradientFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("colorKeys")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("alphaKeys")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("mode")), 2 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("colorKeys").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("alphaKeys").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("mode").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Gradient value, IResolver resolver)
        {
            if (value == null)
            {
                JsonUtility.WriteNull(ref bytes, ref offset);
                return;
            }
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            resolver.GetFormatterWithVerify<global::UnityEngine.GradientColorKey[]>().Serialize(ref bytes, ref offset, value.colorKeys, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            resolver.GetFormatterWithVerify<global::UnityEngine.GradientAlphaKey[]>().Serialize(ref bytes, ref offset, value.alphaKeys, resolver);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            resolver.GetFormatterWithVerify<global::UnityEngine.GradientMode>().Serialize(ref bytes, ref offset, value.mode, resolver);
        }

        public override global::UnityEngine.Gradient Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                return null;
            }


            var __colorKeys__ = default(global::UnityEngine.GradientColorKey[]);
            var __colorKeys__b__ = false;
            var __alphaKeys__ = default(global::UnityEngine.GradientAlphaKey[]);
            var __alphaKeys__b__ = false;
            var __mode__ = default(global::UnityEngine.GradientMode);
            var __mode__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Gradient();
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __colorKeys__ = resolver.GetFormatterWithVerify<global::UnityEngine.GradientColorKey[]>().Deserialize(bytes, ref offset, resolver);
                        __colorKeys__b__ = true;
                        break;
                    case 1:
                        __alphaKeys__ = resolver.GetFormatterWithVerify<global::UnityEngine.GradientAlphaKey[]>().Deserialize(bytes, ref offset, resolver);
                        __alphaKeys__b__ = true;
                        break;
                    case 2:
                        __mode__ = resolver.GetFormatterWithVerify<global::UnityEngine.GradientMode>().Deserialize(bytes, ref offset, resolver);
                        __mode__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __colorKeys__ = resolver.GetFormatterWithVerify<global::UnityEngine.GradientColorKey[]>().Deserialize(bytes, ref offset, resolver);
                        __colorKeys__b__ = true;
                        break;
                    case 1:
                        __alphaKeys__ = resolver.GetFormatterWithVerify<global::UnityEngine.GradientAlphaKey[]>().Deserialize(bytes, ref offset, resolver);
                        __alphaKeys__b__ = true;
                        break;
                    case 2:
                        __mode__ = resolver.GetFormatterWithVerify<global::UnityEngine.GradientMode>().Deserialize(bytes, ref offset, resolver);
                        __mode__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Gradient();
            if (__colorKeys__b__) ____result.colorKeys = __colorKeys__;
            if (__alphaKeys__b__) ____result.alphaKeys = __alphaKeys__;
            if (__mode__b__) ____result.mode = __mode__;

            return ____result;
        }
    }


    public sealed class Matrix4x4Formatter : FormatterBase<global::UnityEngine.Matrix4x4>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public Matrix4x4Formatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m00")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m10")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m20")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m30")), 3 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m01")), 4 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m11")), 5 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m21")), 6 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m31")), 7 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m02")), 8 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m12")), 9 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m22")), 10 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m32")), 11 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m03")), 12 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m13")), 13 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m23")), 14 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("m33")), 15 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("m00").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m10").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m20").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m30").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m01").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m11").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m21").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m31").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m02").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m12").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m22").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m32").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m03").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m13").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m23").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("m33").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Matrix4x4 value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m00);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m10);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m20);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m30);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[4]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m01);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[5]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m11);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[6]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m21);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[7]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m31);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[8]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m02);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[9]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m12);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[10]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m22);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[11]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m32);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[12]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m03);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[13]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m13);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[14]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m23);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[15]);
            JsonUtility.WriteSingle(ref bytes, ref offset, value.m33);
        }

        public override global::UnityEngine.Matrix4x4 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __m00__ = default(float);
            var __m00__b__ = false;
            var __m10__ = default(float);
            var __m10__b__ = false;
            var __m20__ = default(float);
            var __m20__b__ = false;
            var __m30__ = default(float);
            var __m30__b__ = false;
            var __m01__ = default(float);
            var __m01__b__ = false;
            var __m11__ = default(float);
            var __m11__b__ = false;
            var __m21__ = default(float);
            var __m21__b__ = false;
            var __m31__ = default(float);
            var __m31__b__ = false;
            var __m02__ = default(float);
            var __m02__b__ = false;
            var __m12__ = default(float);
            var __m12__b__ = false;
            var __m22__ = default(float);
            var __m22__b__ = false;
            var __m32__ = default(float);
            var __m32__b__ = false;
            var __m03__ = default(float);
            var __m03__b__ = false;
            var __m13__ = default(float);
            var __m13__b__ = false;
            var __m23__ = default(float);
            var __m23__b__ = false;
            var __m33__ = default(float);
            var __m33__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Matrix4x4();
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __m00__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m00__b__ = true;
                        break;
                    case 1:
                        __m10__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m10__b__ = true;
                        break;
                    case 2:
                        __m20__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m20__b__ = true;
                        break;
                    case 3:
                        __m30__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m30__b__ = true;
                        break;
                    case 4:
                        __m01__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m01__b__ = true;
                        break;
                    case 5:
                        __m11__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m11__b__ = true;
                        break;
                    case 6:
                        __m21__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m21__b__ = true;
                        break;
                    case 7:
                        __m31__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m31__b__ = true;
                        break;
                    case 8:
                        __m02__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m02__b__ = true;
                        break;
                    case 9:
                        __m12__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m12__b__ = true;
                        break;
                    case 10:
                        __m22__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m22__b__ = true;
                        break;
                    case 11:
                        __m32__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m32__b__ = true;
                        break;
                    case 12:
                        __m03__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m03__b__ = true;
                        break;
                    case 13:
                        __m13__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m13__b__ = true;
                        break;
                    case 14:
                        __m23__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m23__b__ = true;
                        break;
                    case 15:
                        __m33__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m33__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __m00__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m00__b__ = true;
                        break;
                    case 1:
                        __m10__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m10__b__ = true;
                        break;
                    case 2:
                        __m20__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m20__b__ = true;
                        break;
                    case 3:
                        __m30__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m30__b__ = true;
                        break;
                    case 4:
                        __m01__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m01__b__ = true;
                        break;
                    case 5:
                        __m11__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m11__b__ = true;
                        break;
                    case 6:
                        __m21__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m21__b__ = true;
                        break;
                    case 7:
                        __m31__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m31__b__ = true;
                        break;
                    case 8:
                        __m02__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m02__b__ = true;
                        break;
                    case 9:
                        __m12__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m12__b__ = true;
                        break;
                    case 10:
                        __m22__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m22__b__ = true;
                        break;
                    case 11:
                        __m32__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m32__b__ = true;
                        break;
                    case 12:
                        __m03__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m03__b__ = true;
                        break;
                    case 13:
                        __m13__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m13__b__ = true;
                        break;
                    case 14:
                        __m23__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m23__b__ = true;
                        break;
                    case 15:
                        __m33__ = JsonUtility.ReadSingle(bytes, ref offset);
                        __m33__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Matrix4x4();
            if (__m00__b__) ____result.m00 = __m00__;
            if (__m10__b__) ____result.m10 = __m10__;
            if (__m20__b__) ____result.m20 = __m20__;
            if (__m30__b__) ____result.m30 = __m30__;
            if (__m01__b__) ____result.m01 = __m01__;
            if (__m11__b__) ____result.m11 = __m11__;
            if (__m21__b__) ____result.m21 = __m21__;
            if (__m31__b__) ____result.m31 = __m31__;
            if (__m02__b__) ____result.m02 = __m02__;
            if (__m12__b__) ____result.m12 = __m12__;
            if (__m22__b__) ____result.m22 = __m22__;
            if (__m32__b__) ____result.m32 = __m32__;
            if (__m03__b__) ____result.m03 = __m03__;
            if (__m13__b__) ____result.m13 = __m13__;
            if (__m23__b__) ____result.m23 = __m23__;
            if (__m33__b__) ____result.m33 = __m33__;

            return ____result;
        }
    }


    public sealed class Color32Formatter : FormatterBase<global::UnityEngine.Color32>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public Color32Formatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("r")), 0 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("g")), 1 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("b")), 2 },
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("a")), 3 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("r").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("g").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("b").ToArray(),
                JsonUtility.GetEncodedPropertyNameWithPrefixValueSeparator("a").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.Color32 value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteByte(ref bytes, ref offset, value.r);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[1]);
            JsonUtility.WriteByte(ref bytes, ref offset, value.g);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[2]);
            JsonUtility.WriteByte(ref bytes, ref offset, value.b);
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[3]);
            JsonUtility.WriteByte(ref bytes, ref offset, value.a);
        }

        public override global::UnityEngine.Color32 Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __r__ = default(byte);
            var __r__b__ = false;
            var __g__ = default(byte);
            var __g__b__ = false;
            var __b__ = default(byte);
            var __b__b__ = false;
            var __a__ = default(byte);
            var __a__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.Color32(__r__, __g__, __b__, __a__);
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __r__ = JsonUtility.ReadByte(bytes, ref offset);
                        __r__b__ = true;
                        break;
                    case 1:
                        __g__ = JsonUtility.ReadByte(bytes, ref offset);
                        __g__b__ = true;
                        break;
                    case 2:
                        __b__ = JsonUtility.ReadByte(bytes, ref offset);
                        __b__b__ = true;
                        break;
                    case 3:
                        __a__ = JsonUtility.ReadByte(bytes, ref offset);
                        __a__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __r__ = JsonUtility.ReadByte(bytes, ref offset);
                        __r__b__ = true;
                        break;
                    case 1:
                        __g__ = JsonUtility.ReadByte(bytes, ref offset);
                        __g__b__ = true;
                        break;
                    case 2:
                        __b__ = JsonUtility.ReadByte(bytes, ref offset);
                        __b__b__ = true;
                        break;
                    case 3:
                        __a__ = JsonUtility.ReadByte(bytes, ref offset);
                        __a__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.Color32(__r__, __g__, __b__, __a__);
            if (__r__b__) ____result.r = __r__;
            if (__g__b__) ____result.g = __g__;
            if (__b__b__) ____result.b = __b__;
            if (__a__b__) ____result.a = __a__;

            return ____result;
        }
    }


    public sealed class LayerMaskFormatter : FormatterBase<global::UnityEngine.LayerMask>
    {
        private readonly AutomataDictionary ____keyMapping;
        private readonly byte[][] ____stringByteKeys;

        public LayerMaskFormatter()
        {
            this.____keyMapping = new AutomataDictionary()
            {
                { System.Text.Encoding.UTF8.GetString(JsonUtility.GetEncodedPropertyNameWithoutQuotation("value")), 0 },
            };

            this.____stringByteKeys = new byte[][]
            {
                JsonUtility.GetEncodedPropertyNameWithBeginObject("value").ToArray(),
            };
        }

        public override void Serialize(ref byte[] bytes, ref int offset, global::UnityEngine.LayerMask value, IResolver resolver)
        {
            JsonUtility.WriteRaw(ref bytes, ref offset, this.____stringByteKeys[0]);
            JsonUtility.WriteInt32(ref bytes, ref offset, value.value);
        }

        public override global::UnityEngine.LayerMask Deserialize(byte[] bytes, ref int offset, IResolver resolver)
        {
            if (JsonUtility.ReadIsNull(bytes, ref offset))
            {
                throw new InvalidOperationException("typecode is null, struct not supported");
            }


            var __value__ = default(int);
            var __value__b__ = false;

            JsonUtility.ReadIsBeginObjectWithVerify(bytes, ref offset);
            if (JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                return new global::UnityEngine.LayerMask();
            }
            else
            {
                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __value__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __value__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            while (!JsonUtility.ReadIsEndObject(bytes, ref offset))
            {
                JsonUtility.ReadIsValueSeparatorWithVerify(bytes, ref offset);

                var stringKey = JsonUtility.ReadPropertyNameSegmentRaw(bytes, ref offset);
                ____keyMapping.TryGetValueSafe(stringKey, out var key);

                switch (key)
                {
                    case 0:
                        __value__ = JsonUtility.ReadInt32(bytes, ref offset);
                        __value__b__ = true;
                        break;
                    default:
                        JsonUtility.ReadNextBlock(bytes, ref offset);
                        break;
                }
            }

            var ____result = new global::UnityEngine.LayerMask();
            if (__value__b__) ____result.value = __value__;

            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
