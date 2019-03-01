using AOTSerializer.Internal;
using AOTSerializer.MessagePack.Decoders;
using AOTSerializer.MessagePack.Internal;
using System;
using System.Runtime.CompilerServices;

namespace AOTSerializer.MessagePack
{
    /// <summary>
    /// Encode/Decode Utility of MessagePack Spec.
    /// https://github.com/msgpack/msgpack/blob/master/spec.md
    /// </summary>
    public static partial class MessagePackBinary
    {
        private const int MaxSize = 256; // [0] ~ [255]

        private static readonly IMapHeaderDecoder[] mapHeaderDecoders = new IMapHeaderDecoder[MaxSize];
        private static readonly IArrayHeaderDecoder[] arrayHeaderDecoders = new IArrayHeaderDecoder[MaxSize];
        private static readonly IBooleanDecoder[] booleanDecoders = new IBooleanDecoder[MaxSize];
        private static readonly IByteDecoder[] byteDecoders = new IByteDecoder[MaxSize];
        private static readonly IBytesDecoder[] bytesDecoders = new IBytesDecoder[MaxSize];
        private static readonly IBytesSegmentDecoder[] bytesSegmentDecoders = new IBytesSegmentDecoder[MaxSize];
        private static readonly ISByteDecoder[] sbyteDecoders = new ISByteDecoder[MaxSize];
        private static readonly ISingleDecoder[] singleDecoders = new ISingleDecoder[MaxSize];
        private static readonly IDoubleDecoder[] doubleDecoders = new IDoubleDecoder[MaxSize];
        private static readonly IInt16Decoder[] int16Decoders = new IInt16Decoder[MaxSize];
        private static readonly IInt32Decoder[] int32Decoders = new IInt32Decoder[MaxSize];
        private static readonly IInt64Decoder[] int64Decoders = new IInt64Decoder[MaxSize];
        private static readonly IUInt16Decoder[] uint16Decoders = new IUInt16Decoder[MaxSize];
        private static readonly IUInt32Decoder[] uint32Decoders = new IUInt32Decoder[MaxSize];
        private static readonly IUInt64Decoder[] uint64Decoders = new IUInt64Decoder[MaxSize];
        private static readonly IStringDecoder[] stringDecoders = new IStringDecoder[MaxSize];
        private static readonly IStringSegmentDecoder[] stringSegmentDecoders = new IStringSegmentDecoder[MaxSize];
        private static readonly IExtDecoder[] extDecoders = new IExtDecoder[MaxSize];
        private static readonly IExtHeaderDecoder[] extHeaderDecoders = new IExtHeaderDecoder[MaxSize];
        private static readonly IDateTimeDecoder[] dateTimeDecoders = new IDateTimeDecoder[MaxSize];
        private static readonly IReadNextDecoder[] readNextDecoders = new IReadNextDecoder[MaxSize];

        static MessagePackBinary()
        {
            // Init LookupTable.
            for (int i = 0; i < MaxSize; i++)
            {
                mapHeaderDecoders[i] = InvalidMapHeader.Instance;
                arrayHeaderDecoders[i] = InvalidArrayHeader.Instance;
                booleanDecoders[i] = InvalidBoolean.Instance;
                byteDecoders[i] = InvalidByte.Instance;
                bytesDecoders[i] = InvalidBytes.Instance;
                bytesSegmentDecoders[i] = InvalidBytesSegment.Instance;
                sbyteDecoders[i] = InvalidSByte.Instance;
                singleDecoders[i] = InvalidSingle.Instance;
                doubleDecoders[i] = InvalidDouble.Instance;
                int16Decoders[i] = InvalidInt16.Instance;
                int32Decoders[i] = InvalidInt32.Instance;
                int64Decoders[i] = InvalidInt64.Instance;
                uint16Decoders[i] = InvalidUInt16.Instance;
                uint32Decoders[i] = InvalidUInt32.Instance;
                uint64Decoders[i] = InvalidUInt64.Instance;
                stringDecoders[i] = InvalidString.Instance;
                stringSegmentDecoders[i] = InvalidStringSegment.Instance;
                extDecoders[i] = InvalidExt.Instance;
                extHeaderDecoders[i] = InvalidExtHeader.Instance;
                dateTimeDecoders[i] = InvalidDateTime.Instance;
            }

            // Number
            for (int i = MessagePackCode.MinNegativeFixInt; i <= MessagePackCode.MaxNegativeFixInt; i++)
            {
                sbyteDecoders[i] = FixSByte.Instance;
                int16Decoders[i] = FixNegativeInt16.Instance;
                int32Decoders[i] = FixNegativeInt32.Instance;
                int64Decoders[i] = FixNegativeInt64.Instance;
                singleDecoders[i] = FixNegativeFloat.Instance;
                doubleDecoders[i] = FixNegativeDouble.Instance;
                readNextDecoders[i] = ReadNext1.Instance;
            }
            for (int i = MessagePackCode.MinFixInt; i <= MessagePackCode.MaxFixInt; i++)
            {
                byteDecoders[i] = FixByte.Instance;
                sbyteDecoders[i] = FixSByte.Instance;
                int16Decoders[i] = FixInt16.Instance;
                int32Decoders[i] = FixInt32.Instance;
                int64Decoders[i] = FixInt64.Instance;
                uint16Decoders[i] = FixUInt16.Instance;
                uint32Decoders[i] = FixUInt32.Instance;
                uint64Decoders[i] = FixUInt64.Instance;
                singleDecoders[i] = FixFloat.Instance;
                doubleDecoders[i] = FixDouble.Instance;
                readNextDecoders[i] = ReadNext1.Instance;
            }

            byteDecoders[MessagePackCode.UInt8] = UInt8Byte.Instance;
            sbyteDecoders[MessagePackCode.Int8] = Int8SByte.Instance;
            int16Decoders[MessagePackCode.UInt8] = UInt8Int16.Instance;
            int16Decoders[MessagePackCode.UInt16] = UInt16Int16.Instance;
            int16Decoders[MessagePackCode.Int8] = Int8Int16.Instance;
            int16Decoders[MessagePackCode.Int16] = Int16Int16.Instance;
            int32Decoders[MessagePackCode.UInt8] = UInt8Int32.Instance;
            int32Decoders[MessagePackCode.UInt16] = UInt16Int32.Instance;
            int32Decoders[MessagePackCode.UInt32] = UInt32Int32.Instance;
            int32Decoders[MessagePackCode.Int8] = Int8Int32.Instance;
            int32Decoders[MessagePackCode.Int16] = Int16Int32.Instance;
            int32Decoders[MessagePackCode.Int32] = Int32Int32.Instance;
            int64Decoders[MessagePackCode.UInt8] = UInt8Int64.Instance;
            int64Decoders[MessagePackCode.UInt16] = UInt16Int64.Instance;
            int64Decoders[MessagePackCode.UInt32] = UInt32Int64.Instance;
            int64Decoders[MessagePackCode.UInt64] = UInt64Int64.Instance;
            int64Decoders[MessagePackCode.Int8] = Int8Int64.Instance;
            int64Decoders[MessagePackCode.Int16] = Int16Int64.Instance;
            int64Decoders[MessagePackCode.Int32] = Int32Int64.Instance;
            int64Decoders[MessagePackCode.Int64] = Int64Int64.Instance;
            uint16Decoders[MessagePackCode.UInt8] = UInt8UInt16.Instance;
            uint16Decoders[MessagePackCode.UInt16] = UInt16UInt16.Instance;
            uint32Decoders[MessagePackCode.UInt8] = UInt8UInt32.Instance;
            uint32Decoders[MessagePackCode.UInt16] = UInt16UInt32.Instance;
            uint32Decoders[MessagePackCode.UInt32] = UInt32UInt32.Instance;
            uint64Decoders[MessagePackCode.UInt8] = UInt8UInt64.Instance;
            uint64Decoders[MessagePackCode.UInt16] = UInt16UInt64.Instance;
            uint64Decoders[MessagePackCode.UInt32] = UInt32UInt64.Instance;
            uint64Decoders[MessagePackCode.UInt64] = UInt64UInt64.Instance;

            singleDecoders[MessagePackCode.Float32] = Float32Single.Instance;
            singleDecoders[MessagePackCode.Int8] = Int8Single.Instance;
            singleDecoders[MessagePackCode.Int16] = Int16Single.Instance;
            singleDecoders[MessagePackCode.Int32] = Int32Single.Instance;
            singleDecoders[MessagePackCode.Int64] = Int64Single.Instance;
            singleDecoders[MessagePackCode.UInt8] = UInt8Single.Instance;
            singleDecoders[MessagePackCode.UInt16] = UInt16Single.Instance;
            singleDecoders[MessagePackCode.UInt32] = UInt32Single.Instance;
            singleDecoders[MessagePackCode.UInt64] = UInt64Single.Instance;

            doubleDecoders[MessagePackCode.Float32] = Float32Double.Instance;
            doubleDecoders[MessagePackCode.Float64] = Float64Double.Instance;
            doubleDecoders[MessagePackCode.Int8] = Int8Double.Instance;
            doubleDecoders[MessagePackCode.Int16] = Int16Double.Instance;
            doubleDecoders[MessagePackCode.Int32] = Int32Double.Instance;
            doubleDecoders[MessagePackCode.Int64] = Int64Double.Instance;
            doubleDecoders[MessagePackCode.UInt8] = UInt8Double.Instance;
            doubleDecoders[MessagePackCode.UInt16] = UInt16Double.Instance;
            doubleDecoders[MessagePackCode.UInt32] = UInt32Double.Instance;
            doubleDecoders[MessagePackCode.UInt64] = UInt64Double.Instance;

            readNextDecoders[MessagePackCode.Int8] = ReadNext2.Instance;
            readNextDecoders[MessagePackCode.Int16] = ReadNext3.Instance;
            readNextDecoders[MessagePackCode.Int32] = ReadNext5.Instance;
            readNextDecoders[MessagePackCode.Int64] = ReadNext9.Instance;
            readNextDecoders[MessagePackCode.UInt8] = ReadNext2.Instance;
            readNextDecoders[MessagePackCode.UInt16] = ReadNext3.Instance;
            readNextDecoders[MessagePackCode.UInt32] = ReadNext5.Instance;
            readNextDecoders[MessagePackCode.UInt64] = ReadNext9.Instance;
            readNextDecoders[MessagePackCode.Float32] = ReadNext5.Instance;
            readNextDecoders[MessagePackCode.Float64] = ReadNext9.Instance;

            // Map
            for (int i = MessagePackCode.MinFixMap; i <= MessagePackCode.MaxFixMap; i++)
            {
                mapHeaderDecoders[i] = FixMapHeader.Instance;
                readNextDecoders[i] = ReadNext1.Instance;
            }
            mapHeaderDecoders[MessagePackCode.Map16] = Map16Header.Instance;
            mapHeaderDecoders[MessagePackCode.Map32] = Map32Header.Instance;
            readNextDecoders[MessagePackCode.Map16] = ReadNextMap.Instance;
            readNextDecoders[MessagePackCode.Map32] = ReadNextMap.Instance;

            // Array
            for (int i = MessagePackCode.MinFixArray; i <= MessagePackCode.MaxFixArray; i++)
            {
                arrayHeaderDecoders[i] = FixArrayHeader.Instance;
                readNextDecoders[i] = ReadNext1.Instance;
            }
            arrayHeaderDecoders[MessagePackCode.Array16] = Array16Header.Instance;
            arrayHeaderDecoders[MessagePackCode.Array32] = Array32Header.Instance;
            readNextDecoders[MessagePackCode.Array16] = ReadNextArray.Instance;
            readNextDecoders[MessagePackCode.Array32] = ReadNextArray.Instance;

            // Str
            for (int i = MessagePackCode.MinFixStr; i <= MessagePackCode.MaxFixStr; i++)
            {
                stringDecoders[i] = FixString.Instance;
                stringSegmentDecoders[i] = FixStringSegment.Instance;
                readNextDecoders[i] = ReadNextFixStr.Instance;
            }

            stringDecoders[MessagePackCode.Str8] = Str8String.Instance;
            stringDecoders[MessagePackCode.Str16] = Str16String.Instance;
            stringDecoders[MessagePackCode.Str32] = Str32String.Instance;
            stringSegmentDecoders[MessagePackCode.Str8] = Str8StringSegment.Instance;
            stringSegmentDecoders[MessagePackCode.Str16] = Str16StringSegment.Instance;
            stringSegmentDecoders[MessagePackCode.Str32] = Str32StringSegment.Instance;
            readNextDecoders[MessagePackCode.Str8] = ReadNextStr8.Instance;
            readNextDecoders[MessagePackCode.Str16] = ReadNextStr16.Instance;
            readNextDecoders[MessagePackCode.Str32] = ReadNextStr32.Instance;

            // Others
            stringDecoders[MessagePackCode.Nil] = NilString.Instance;
            stringSegmentDecoders[MessagePackCode.Nil] = NilStringSegment.Instance;
            bytesDecoders[MessagePackCode.Nil] = NilBytes.Instance;
            bytesSegmentDecoders[MessagePackCode.Nil] = NilBytesSegment.Instance;
            readNextDecoders[MessagePackCode.Nil] = ReadNext1.Instance;

            booleanDecoders[MessagePackCode.False] = False.Instance;
            booleanDecoders[MessagePackCode.True] = True.Instance;
            readNextDecoders[MessagePackCode.False] = ReadNext1.Instance;
            readNextDecoders[MessagePackCode.True] = ReadNext1.Instance;

            bytesDecoders[MessagePackCode.Bin8] = Bin8Bytes.Instance;
            bytesDecoders[MessagePackCode.Bin16] = Bin16Bytes.Instance;
            bytesDecoders[MessagePackCode.Bin32] = Bin32Bytes.Instance;
            bytesSegmentDecoders[MessagePackCode.Bin8] = Bin8BytesSegment.Instance;
            bytesSegmentDecoders[MessagePackCode.Bin16] = Bin16BytesSegment.Instance;
            bytesSegmentDecoders[MessagePackCode.Bin32] = Bin32BytesSegment.Instance;
            readNextDecoders[MessagePackCode.Bin8] = ReadNextBin8.Instance;
            readNextDecoders[MessagePackCode.Bin16] = ReadNextBin16.Instance;
            readNextDecoders[MessagePackCode.Bin32] = ReadNextBin32.Instance;

            // Ext
            extDecoders[MessagePackCode.FixExt1] = FixExt1.Instance;
            extDecoders[MessagePackCode.FixExt2] = FixExt2.Instance;
            extDecoders[MessagePackCode.FixExt4] = FixExt4.Instance;
            extDecoders[MessagePackCode.FixExt8] = FixExt8.Instance;
            extDecoders[MessagePackCode.FixExt16] = FixExt16.Instance;
            extDecoders[MessagePackCode.Ext8] = Ext8.Instance;
            extDecoders[MessagePackCode.Ext16] = Ext16.Instance;
            extDecoders[MessagePackCode.Ext32] = Ext32.Instance;

            extHeaderDecoders[MessagePackCode.FixExt1] = FixExt1Header.Instance;
            extHeaderDecoders[MessagePackCode.FixExt2] = FixExt2Header.Instance;
            extHeaderDecoders[MessagePackCode.FixExt4] = FixExt4Header.Instance;
            extHeaderDecoders[MessagePackCode.FixExt8] = FixExt8Header.Instance;
            extHeaderDecoders[MessagePackCode.FixExt16] = FixExt16Header.Instance;
            extHeaderDecoders[MessagePackCode.Ext8] = Ext8Header.Instance;
            extHeaderDecoders[MessagePackCode.Ext16] = Ext16Header.Instance;
            extHeaderDecoders[MessagePackCode.Ext32] = Ext32Header.Instance;


            readNextDecoders[MessagePackCode.FixExt1] = ReadNext3.Instance;
            readNextDecoders[MessagePackCode.FixExt2] = ReadNext4.Instance;
            readNextDecoders[MessagePackCode.FixExt4] = ReadNext6.Instance;
            readNextDecoders[MessagePackCode.FixExt8] = ReadNext10.Instance;
            readNextDecoders[MessagePackCode.FixExt16] = ReadNext18.Instance;
            readNextDecoders[MessagePackCode.Ext8] = ReadNextExt8.Instance;
            readNextDecoders[MessagePackCode.Ext16] = ReadNextExt16.Instance;
            readNextDecoders[MessagePackCode.Ext32] = ReadNextExt32.Instance;

            // DateTime
            dateTimeDecoders[MessagePackCode.FixExt4] = FixExt4DateTime.Instance;
            dateTimeDecoders[MessagePackCode.FixExt8] = FixExt8DateTime.Instance;
            dateTimeDecoders[MessagePackCode.Ext8] = Ext8DateTime.Instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MessagePackType GetMessagePackType(byte[] bytes, int offset)
        {
            return MessagePackCode.ToMessagePackType(bytes[offset]);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadNext(byte[] bytes, ref int offset)
        {
            readNextDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ReadNextBlock(byte[] bytes, ref int offset)
        {
            switch (GetMessagePackType(bytes, offset))
            {
                case MessagePackType.Unknown:
                case MessagePackType.Integer:
                case MessagePackType.Nil:
                case MessagePackType.Boolean:
                case MessagePackType.Float:
                case MessagePackType.String:
                case MessagePackType.Binary:
                case MessagePackType.Extension:
                default:
                    ReadNext(bytes, ref offset);
                    return;
                case MessagePackType.Array:
                    {
                        var header = ReadArrayHeader(bytes, ref offset);
                        for (int i = 0; i < header; i++)
                        {
                            ReadNextBlock(bytes, ref offset);
                        }
                        return;
                    }
                case MessagePackType.Map:
                    {
                        var header = ReadMapHeader(bytes, ref offset);
                        for (int i = 0; i < header; i++)
                        {
                            ReadNextBlock(bytes, ref offset); // read key block
                            ReadNextBlock(bytes, ref offset); // read value block
                        }
                        return;
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteNil(ref byte[] bytes, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 1);

            bytes[offset++] = MessagePackCode.Nil;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Nil ReadNil(byte[] bytes, ref int offset)
        {
            if (bytes[offset] == MessagePackCode.Nil)
            {
                offset++;
                return Nil.Default;
            }
            else
            {
                throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNil(byte[] bytes, int offset)
        {
            return bytes[offset] == MessagePackCode.Nil;
        }

        public static void WriteRaw(ref byte[] bytes, ref int offset, byte[] rawMessagePackBlock)
        {
            UnsafeMemory.WriteRaw(ref bytes, ref offset, rawMessagePackBlock);
        }

        /// <summary>
        /// Unsafe. If value is guranteed 0 ~ MessagePackRange.MaxFixMapCount(15), can use this method.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFixedMapHeaderUnsafe(ref byte[] bytes, ref int offset, int count)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
            bytes[offset] = (byte)(MessagePackCode.MinFixMap | count);
            offset++;
        }

        /// <summary>
        /// Write map count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteMapHeader(ref byte[] bytes, ref int offset, int count)
        {
            checked
            {
                WriteMapHeader(ref bytes, ref offset, (uint)count);
            }
        }

        /// <summary>
        /// Write map count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteMapHeader(ref byte[] bytes, ref int offset, uint count)
        {
            if (count <= MessagePackRange.MaxFixMapCount)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = (byte)(MessagePackCode.MinFixMap | count);
                offset += 1;
                return;
            }
            else if (count <= ushort.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                unchecked
                {
                    bytes[offset] = MessagePackCode.Map16;
                    bytes[offset + 1] = (byte)(count >> 8);
                    bytes[offset + 2] = (byte)(count);
                }
                offset += 3;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                unchecked
                {
                    bytes[offset] = MessagePackCode.Map32;
                    bytes[offset + 1] = (byte)(count >> 24);
                    bytes[offset + 2] = (byte)(count >> 16);
                    bytes[offset + 3] = (byte)(count >> 8);
                    bytes[offset + 4] = (byte)(count);
                }
                offset += 5;
                return;
            }
        }

        /// <summary>
        /// Write map format header, always use map32 format(length is fixed, 5).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteMapHeaderForceMap32Block(ref byte[] bytes, ref int offset, uint count)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
            unchecked
            {
                bytes[offset] = MessagePackCode.Map32;
                bytes[offset + 1] = (byte)(count >> 24);
                bytes[offset + 2] = (byte)(count >> 16);
                bytes[offset + 3] = (byte)(count >> 8);
                bytes[offset + 4] = (byte)(count);
            }
            offset += 5;
        }

        /// <summary>
        /// Return map count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadMapHeader(byte[] bytes, ref int offset)
        {
            checked
            {
                return (int)mapHeaderDecoders[bytes[offset]].Read(bytes, ref offset);
            }
        }

        /// <summary>
        /// Return map count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadMapHeaderRaw(byte[] bytes, ref int offset)
        {
            return mapHeaderDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetArrayHeaderLength(int count)
        {
            if (count <= MessagePackRange.MaxFixArrayCount)
            {
                return 1;
            }
            else if (count <= ushort.MaxValue)
            {
                return 3;
            }
            else
            {
                return 5;
            }
        }

        /// <summary>
        /// Unsafe. If value is guranteed 0 ~ MessagePackRange.MaxFixArrayCount(15), can use this method.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFixedArrayHeaderUnsafe(ref byte[] bytes, ref int offset, int count)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
            bytes[offset] = (byte)(MessagePackCode.MinFixArray | count);
            offset++;
        }

        /// <summary>
        /// Write array count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteArrayHeader(ref byte[] bytes, ref int offset, int count)
        {
            checked
            {
                WriteArrayHeader(ref bytes, ref offset, (uint)count);
            }
        }

        /// <summary>
        /// Write array count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteArrayHeader(ref byte[] bytes, ref int offset, uint count)
        {
            if (count <= MessagePackRange.MaxFixArrayCount)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = (byte)(MessagePackCode.MinFixArray | count);
                offset += 1;
                return;
            }
            else if (count <= ushort.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                unchecked
                {
                    bytes[offset] = MessagePackCode.Array16;
                    bytes[offset + 1] = (byte)(count >> 8);
                    bytes[offset + 2] = (byte)(count);
                }
                offset += 3;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                unchecked
                {
                    bytes[offset] = MessagePackCode.Array32;
                    bytes[offset + 1] = (byte)(count >> 24);
                    bytes[offset + 2] = (byte)(count >> 16);
                    bytes[offset + 3] = (byte)(count >> 8);
                    bytes[offset + 4] = (byte)(count);
                }
                offset += 5;
                return;
            }
        }

        /// <summary>
        /// Write array format header, always use array32 format(length is fixed, 5).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteArrayHeaderForceArray32Block(ref byte[] bytes, ref int offset, uint count)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
            unchecked
            {
                bytes[offset] = MessagePackCode.Array32;
                bytes[offset + 1] = (byte)(count >> 24);
                bytes[offset + 2] = (byte)(count >> 16);
                bytes[offset + 3] = (byte)(count >> 8);
                bytes[offset + 4] = (byte)(count);
            }
            offset += 5;
            return;
        }

        /// <summary>
        /// Return array count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadArrayHeader(byte[] bytes, ref int offset)
        {
            checked
            {
                return (int)arrayHeaderDecoders[bytes[offset]].Read(bytes, ref offset);
            }
        }

        /// <summary>
        /// Return array count.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadArrayHeaderRaw(byte[] bytes, ref int offset)
        {
            return arrayHeaderDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBoolean(ref byte[] bytes, ref int offset, bool value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = (value ? MessagePackCode.True : MessagePackCode.False);
            offset++;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolean(byte[] bytes, ref int offset)
        {
            return booleanDecoders[bytes[offset]].Read(ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByte(ref byte[] bytes, ref int offset, byte value)
        {
            if (value <= MessagePackCode.MaxFixInt)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = value;
                offset += 1;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                bytes[offset] = MessagePackCode.UInt8;
                bytes[offset + 1] = value;
                offset += 2;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByteForceByteBlock(ref byte[] bytes, ref int offset, byte value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
            bytes[offset] = MessagePackCode.UInt8;
            bytes[offset + 1] = value;
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReadByte(byte[] bytes, ref int offset)
        {
            return byteDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBytes(ref byte[] bytes, ref int offset, byte[] value)
        {
            if (value == null)
            {
                WriteNil(ref bytes, ref offset);
            }
            else
            {
                WriteBytes(ref bytes, ref offset, value, 0, value.Length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBytes(ref byte[] dest, ref int dstOffset, byte[] src, int srcOffset, int count)
        {
            if (src == null)
            {
                WriteNil(ref dest, ref dstOffset);
                return;
            }

            if (count <= byte.MaxValue)
            {
                var size = count + 2;
                BinaryUtil.EnsureCapacity(ref dest, dstOffset, size);

                dest[dstOffset] = MessagePackCode.Bin8;
                dest[dstOffset + 1] = (byte)count;

                Buffer.BlockCopy(src, srcOffset, dest, dstOffset + 2, count);
                dstOffset += size;
                return;
            }
            else if (count <= UInt16.MaxValue)
            {
                var size = count + 3;
                BinaryUtil.EnsureCapacity(ref dest, dstOffset, size);

                unchecked
                {
                    dest[dstOffset] = MessagePackCode.Bin16;
                    dest[dstOffset + 1] = (byte)(count >> 8);
                    dest[dstOffset + 2] = (byte)(count);
                }

                Buffer.BlockCopy(src, srcOffset, dest, dstOffset + 3, count);
                dstOffset += size;
                return;
            }
            else
            {
                var size = count + 5;
                BinaryUtil.EnsureCapacity(ref dest, dstOffset, size);

                unchecked
                {
                    dest[dstOffset] = MessagePackCode.Bin32;
                    dest[dstOffset + 1] = (byte)(count >> 24);
                    dest[dstOffset + 2] = (byte)(count >> 16);
                    dest[dstOffset + 3] = (byte)(count >> 8);
                    dest[dstOffset + 4] = (byte)(count);
                }

                Buffer.BlockCopy(src, srcOffset, dest, dstOffset + 5, count);
                dstOffset += size;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadBytes(byte[] bytes, ref int offset)
        {
            return bytesDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<byte> ReadBytesSegment(byte[] bytes, ref int offset)
        {
            return bytesSegmentDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSByte(ref byte[] bytes, ref int offset, sbyte value)
        {
            if (value < MessagePackRange.MinFixNegativeInt)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                bytes[offset] = MessagePackCode.Int8;
                bytes[offset + 1] = unchecked((byte)(value));
                offset += 2;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = unchecked((byte)value);
                offset += 1;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSByteForceSByteBlock(ref byte[] bytes, int offset, sbyte value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
            bytes[offset] = MessagePackCode.Int8;
            bytes[offset + 1] = unchecked((byte)(value));
            offset += 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSByte(byte[] bytes, ref int offset)
        {
            return sbyteDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSingle(ref byte[] bytes, ref int offset, float value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 5);

            bytes[offset] = MessagePackCode.Float32;

            var num = new Float32Bits(value);
            if (BitConverter.IsLittleEndian)
            {
                bytes[offset + 1] = num.Byte3;
                bytes[offset + 2] = num.Byte2;
                bytes[offset + 3] = num.Byte1;
                bytes[offset + 4] = num.Byte0;
            }
            else
            {
                bytes[offset + 1] = num.Byte0;
                bytes[offset + 2] = num.Byte1;
                bytes[offset + 3] = num.Byte2;
                bytes[offset + 4] = num.Byte3;
            }

            offset += 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ReadSingle(byte[] bytes, ref int offset)
        {
            return singleDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDouble(ref byte[] bytes, ref int offset, double value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 9);

            bytes[offset] = MessagePackCode.Float64;

            var num = new Float64Bits(value);
            if (BitConverter.IsLittleEndian)
            {
                bytes[offset + 1] = num.Byte7;
                bytes[offset + 2] = num.Byte6;
                bytes[offset + 3] = num.Byte5;
                bytes[offset + 4] = num.Byte4;
                bytes[offset + 5] = num.Byte3;
                bytes[offset + 6] = num.Byte2;
                bytes[offset + 7] = num.Byte1;
                bytes[offset + 8] = num.Byte0;
            }
            else
            {
                bytes[offset + 1] = num.Byte0;
                bytes[offset + 2] = num.Byte1;
                bytes[offset + 3] = num.Byte2;
                bytes[offset + 4] = num.Byte3;
                bytes[offset + 5] = num.Byte4;
                bytes[offset + 6] = num.Byte5;
                bytes[offset + 7] = num.Byte6;
                bytes[offset + 8] = num.Byte7;
            }

            offset += 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ReadDouble(byte[] bytes, ref int offset)
        {
            return doubleDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16(ref byte[] bytes, ref int offset, short value)
        {
            if (value >= 0)
            {
                // positive int(use uint)
                if (value <= MessagePackRange.MaxFixPositiveInt)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                    bytes[offset] = unchecked((byte)value);
                    offset += 1;
                    return;
                }
                else if (value <= byte.MaxValue)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                    bytes[offset] = MessagePackCode.UInt8;
                    bytes[offset + 1] = unchecked((byte)value);
                    offset += 2;
                    return;
                }
                else
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.UInt16;
                    bytes[offset + 1] = unchecked((byte)(value >> 8));
                    bytes[offset + 2] = unchecked((byte)value);
                    offset += 3;
                    return;
                }
            }
            else
            {
                // negative int(use int)
                if (MessagePackRange.MinFixNegativeInt <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                    bytes[offset] = unchecked((byte)value);
                    offset += 1;
                    return;
                }
                else if (sbyte.MinValue <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                    bytes[offset] = MessagePackCode.Int8;
                    bytes[offset + 1] = unchecked((byte)value);
                    offset += 2;
                    return;
                }
                else
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.Int16;
                    bytes[offset + 1] = unchecked((byte)(value >> 8));
                    bytes[offset + 2] = unchecked((byte)value);
                    offset += 3;
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16ForceInt16Block(ref byte[] bytes, ref int offset, short value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
            bytes[offset] = MessagePackCode.Int16;
            bytes[offset + 1] = unchecked((byte)(value >> 8));
            bytes[offset + 2] = unchecked((byte)value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReadInt16(byte[] bytes, ref int offset)
        {
            return int16Decoders[bytes[offset]].Read(bytes, ref offset);
        }

        /// <summary>
        /// Unsafe. If value is guranteed 0 ~ MessagePackCode.MaxFixInt(127), can use this method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePositiveFixedIntUnsafe(ref byte[] bytes, ref int offset, int value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
            bytes[offset] = (byte)value;
            offset += 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32(ref byte[] bytes, ref int offset, int value)
        {
            if (value >= 0)
            {
                // positive int(use uint)
                if (value <= MessagePackRange.MaxFixPositiveInt)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                    bytes[offset] = unchecked((byte)value);
                    offset += 1;
                    return;
                }
                else if (value <= byte.MaxValue)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                    bytes[offset] = MessagePackCode.UInt8;
                    bytes[offset + 1] = unchecked((byte)value);
                    offset += 2;
                    return;
                }
                else if (value <= ushort.MaxValue)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.UInt16;
                    bytes[offset + 1] = unchecked((byte)(value >> 8));
                    bytes[offset + 2] = unchecked((byte)value);
                    offset += 3;
                    return;
                }
                else
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                    bytes[offset] = MessagePackCode.UInt32;
                    bytes[offset + 1] = unchecked((byte)(value >> 24));
                    bytes[offset + 2] = unchecked((byte)(value >> 16));
                    bytes[offset + 3] = unchecked((byte)(value >> 8));
                    bytes[offset + 4] = unchecked((byte)value);
                    offset += 5;
                    return;
                }
            }
            else
            {
                // negative int(use int)
                if (MessagePackRange.MinFixNegativeInt <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                    bytes[offset] = unchecked((byte)value);
                    offset += 1;
                    return;
                }
                else if (sbyte.MinValue <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                    bytes[offset] = MessagePackCode.Int8;
                    bytes[offset + 1] = unchecked((byte)value);
                    offset += 2;
                    return;
                }
                else if (short.MinValue <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.Int16;
                    bytes[offset + 1] = unchecked((byte)(value >> 8));
                    bytes[offset + 2] = unchecked((byte)value);
                    offset += 3;
                    return;
                }
                else
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                    bytes[offset] = MessagePackCode.Int32;
                    bytes[offset + 1] = unchecked((byte)(value >> 24));
                    bytes[offset + 2] = unchecked((byte)(value >> 16));
                    bytes[offset + 3] = unchecked((byte)(value >> 8));
                    bytes[offset + 4] = unchecked((byte)value);
                    offset += 5;
                    return;
                }
            }
        }

        /// <summary>
        /// Acquire static message block(always 5 bytes).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32ForceInt32Block(ref byte[] bytes, ref int offset, int value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
            bytes[offset] = MessagePackCode.Int32;
            bytes[offset + 1] = unchecked((byte)(value >> 24));
            bytes[offset + 2] = unchecked((byte)(value >> 16));
            bytes[offset + 3] = unchecked((byte)(value >> 8));
            bytes[offset + 4] = unchecked((byte)value);
            offset += 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReadInt32(byte[] bytes, ref int offset)
        {
            return int32Decoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt64(ref byte[] bytes, ref int offset, long value)
        {
            if (value >= 0)
            {
                // positive int(use uint)
                if (value <= MessagePackRange.MaxFixPositiveInt)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                    bytes[offset] = unchecked((byte)value);
                    offset += 1;
                    return;
                }
                else if (value <= byte.MaxValue)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                    bytes[offset] = MessagePackCode.UInt8;
                    bytes[offset + 1] = unchecked((byte)value);
                    offset += 2;
                    return;
                }
                else if (value <= ushort.MaxValue)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.UInt16;
                    bytes[offset + 1] = unchecked((byte)(value >> 8));
                    bytes[offset + 2] = unchecked((byte)value);
                    offset += 3;
                    return;
                }
                else if (value <= uint.MaxValue)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                    bytes[offset] = MessagePackCode.UInt32;
                    bytes[offset + 1] = unchecked((byte)(value >> 24));
                    bytes[offset + 2] = unchecked((byte)(value >> 16));
                    bytes[offset + 3] = unchecked((byte)(value >> 8));
                    bytes[offset + 4] = unchecked((byte)value);
                    offset += 5;
                    return;
                }
                else
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 9);
                    bytes[offset] = MessagePackCode.UInt64;
                    bytes[offset + 1] = unchecked((byte)(value >> 56));
                    bytes[offset + 2] = unchecked((byte)(value >> 48));
                    bytes[offset + 3] = unchecked((byte)(value >> 40));
                    bytes[offset + 4] = unchecked((byte)(value >> 32));
                    bytes[offset + 5] = unchecked((byte)(value >> 24));
                    bytes[offset + 6] = unchecked((byte)(value >> 16));
                    bytes[offset + 7] = unchecked((byte)(value >> 8));
                    bytes[offset + 8] = unchecked((byte)value);
                    offset += 9;
                    return;
                }
            }
            else
            {
                // negative int(use int)
                if (MessagePackRange.MinFixNegativeInt <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                    bytes[offset] = unchecked((byte)value);
                    offset += 1;
                    return;
                }
                else if (sbyte.MinValue <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                    bytes[offset] = MessagePackCode.Int8;
                    bytes[offset + 1] = unchecked((byte)value);
                    offset += 2;
                    return;
                }
                else if (short.MinValue <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.Int16;
                    bytes[offset + 1] = unchecked((byte)(value >> 8));
                    bytes[offset + 2] = unchecked((byte)value);
                    offset += 3;
                    return;
                }
                else if (int.MinValue <= value)
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                    bytes[offset] = MessagePackCode.Int32;
                    bytes[offset + 1] = unchecked((byte)(value >> 24));
                    bytes[offset + 2] = unchecked((byte)(value >> 16));
                    bytes[offset + 3] = unchecked((byte)(value >> 8));
                    bytes[offset + 4] = unchecked((byte)value);
                    offset += 5;
                    return;
                }
                else
                {
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 9);
                    bytes[offset] = MessagePackCode.Int64;
                    bytes[offset + 1] = unchecked((byte)(value >> 56));
                    bytes[offset + 2] = unchecked((byte)(value >> 48));
                    bytes[offset + 3] = unchecked((byte)(value >> 40));
                    bytes[offset + 4] = unchecked((byte)(value >> 32));
                    bytes[offset + 5] = unchecked((byte)(value >> 24));
                    bytes[offset + 6] = unchecked((byte)(value >> 16));
                    bytes[offset + 7] = unchecked((byte)(value >> 8));
                    bytes[offset + 8] = unchecked((byte)value);
                    offset += 9;
                    return;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt64ForceInt64Block(ref byte[] bytes, ref int offset, long value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 9);
            bytes[offset] = MessagePackCode.Int64;
            bytes[offset + 1] = unchecked((byte)(value >> 56));
            bytes[offset + 2] = unchecked((byte)(value >> 48));
            bytes[offset + 3] = unchecked((byte)(value >> 40));
            bytes[offset + 4] = unchecked((byte)(value >> 32));
            bytes[offset + 5] = unchecked((byte)(value >> 24));
            bytes[offset + 6] = unchecked((byte)(value >> 16));
            bytes[offset + 7] = unchecked((byte)(value >> 8));
            bytes[offset + 8] = unchecked((byte)value);
            offset += 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReadInt64(byte[] bytes, ref int offset)
        {
            return int64Decoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16(ref byte[] bytes, ref int offset, ushort value)
        {
            if (value <= MessagePackRange.MaxFixPositiveInt)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = unchecked((byte)value);
                offset += 1;
                return;
            }
            else if (value <= byte.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                bytes[offset] = MessagePackCode.UInt8;
                bytes[offset + 1] = unchecked((byte)value);
                offset += 2;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                bytes[offset] = MessagePackCode.UInt16;
                bytes[offset + 1] = unchecked((byte)(value >> 8));
                bytes[offset + 2] = unchecked((byte)value);
                offset += 3;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16ForceUInt16Block(ref byte[] bytes, ref int offset, ushort value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
            bytes[offset] = MessagePackCode.UInt16;
            bytes[offset + 1] = unchecked((byte)(value >> 8));
            bytes[offset + 2] = unchecked((byte)value);
            offset += 3;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReadUInt16(byte[] bytes, ref int offset)
        {
            return uint16Decoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32(ref byte[] bytes, ref int offset, uint value)
        {
            if (value <= MessagePackRange.MaxFixPositiveInt)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = unchecked((byte)value);
                offset += 1;
                return;
            }
            else if (value <= byte.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                bytes[offset] = MessagePackCode.UInt8;
                bytes[offset + 1] = unchecked((byte)value);
                offset += 2;
                return;
            }
            else if (value <= ushort.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                bytes[offset] = MessagePackCode.UInt16;
                bytes[offset + 1] = unchecked((byte)(value >> 8));
                bytes[offset + 2] = unchecked((byte)value);
                offset += 3;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                bytes[offset] = MessagePackCode.UInt32;
                bytes[offset + 1] = unchecked((byte)(value >> 24));
                bytes[offset + 2] = unchecked((byte)(value >> 16));
                bytes[offset + 3] = unchecked((byte)(value >> 8));
                bytes[offset + 4] = unchecked((byte)value);
                offset += 5;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32ForceUInt32Block(ref byte[] bytes, ref int offset, uint value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
            bytes[offset] = MessagePackCode.UInt32;
            bytes[offset + 1] = unchecked((byte)(value >> 24));
            bytes[offset + 2] = unchecked((byte)(value >> 16));
            bytes[offset + 3] = unchecked((byte)(value >> 8));
            bytes[offset + 4] = unchecked((byte)value);
            offset += 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReadUInt32(byte[] bytes, ref int offset)
        {
            return uint32Decoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64(ref byte[] bytes, ref int offset, ulong value)
        {
            if (value <= MessagePackRange.MaxFixPositiveInt)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 1);
                bytes[offset] = unchecked((byte)value);
                offset += 1;
                return;
            }
            else if (value <= byte.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 2);
                bytes[offset] = MessagePackCode.UInt8;
                bytes[offset + 1] = unchecked((byte)value);
                offset += 2;
                return;
            }
            else if (value <= ushort.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                bytes[offset] = MessagePackCode.UInt16;
                bytes[offset + 1] = unchecked((byte)(value >> 8));
                bytes[offset + 2] = unchecked((byte)value);
                offset += 3;
                return;
            }
            else if (value <= uint.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 5);
                bytes[offset] = MessagePackCode.UInt32;
                bytes[offset + 1] = unchecked((byte)(value >> 24));
                bytes[offset + 2] = unchecked((byte)(value >> 16));
                bytes[offset + 3] = unchecked((byte)(value >> 8));
                bytes[offset + 4] = unchecked((byte)value);
                offset += 5;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, 9);
                bytes[offset] = MessagePackCode.UInt64;
                bytes[offset + 1] = unchecked((byte)(value >> 56));
                bytes[offset + 2] = unchecked((byte)(value >> 48));
                bytes[offset + 3] = unchecked((byte)(value >> 40));
                bytes[offset + 4] = unchecked((byte)(value >> 32));
                bytes[offset + 5] = unchecked((byte)(value >> 24));
                bytes[offset + 6] = unchecked((byte)(value >> 16));
                bytes[offset + 7] = unchecked((byte)(value >> 8));
                bytes[offset + 8] = unchecked((byte)value);
                offset += 9;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt64ForceUInt64Block(ref byte[] bytes, ref int offset, ulong value)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, 9);
            bytes[offset] = MessagePackCode.UInt64;
            bytes[offset + 1] = unchecked((byte)(value >> 56));
            bytes[offset + 2] = unchecked((byte)(value >> 48));
            bytes[offset + 3] = unchecked((byte)(value >> 40));
            bytes[offset + 4] = unchecked((byte)(value >> 32));
            bytes[offset + 5] = unchecked((byte)(value >> 24));
            bytes[offset + 6] = unchecked((byte)(value >> 16));
            bytes[offset + 7] = unchecked((byte)(value >> 8));
            bytes[offset + 8] = unchecked((byte)value);
            offset += 9;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReadUInt64(byte[] bytes, ref int offset)
        {
            return uint64Decoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteChar(ref byte[] bytes, ref int offset, char value)
        {
            WriteUInt16(ref bytes, ref offset, (ushort)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadChar(byte[] bytes, ref int offset)
        {
            return (char)ReadUInt16(bytes, ref offset);
        }

        /// <summary>
        /// Unsafe. If value is guranteed length is 0 ~ 31, can use this method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFixedStringUnsafe(ref byte[] bytes, ref int offset, string value, int byteCount)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 1);
            bytes[offset] = (byte)(MessagePackCode.MinFixStr | byteCount);
            StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset + 1);

            offset += byteCount + 1;
        }

        /// <summary>
        /// Unsafe. If pre-calculated byteCount of target string, can use this method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringUnsafe(ref byte[] bytes, ref int offset, string value, int byteCount)
        {
            if (byteCount <= MessagePackRange.MaxFixStringLength)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 1);
                bytes[offset] = (byte)(MessagePackCode.MinFixStr | byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset + 1);
                offset += byteCount + 1;
                return;
            }
            else if (byteCount <= byte.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 2);
                bytes[offset] = MessagePackCode.Str8;
                bytes[offset + 1] = unchecked((byte)byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset + 2);
                offset += byteCount + 2;
                return;
            }
            else if (byteCount <= ushort.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 3);
                bytes[offset] = MessagePackCode.Str16;
                bytes[offset + 1] = unchecked((byte)(byteCount >> 8));
                bytes[offset + 2] = unchecked((byte)byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset + 3);
                offset += byteCount + 3;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 5);
                bytes[offset] = MessagePackCode.Str32;
                bytes[offset + 1] = unchecked((byte)(byteCount >> 24));
                bytes[offset + 2] = unchecked((byte)(byteCount >> 16));
                bytes[offset + 3] = unchecked((byte)(byteCount >> 8));
                bytes[offset + 4] = unchecked((byte)byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset + 5);
                offset += byteCount + 5;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteStringBytes(ref byte[] bytes, ref int offset, byte[] utf8stringBytes)
        {
            var byteCount = utf8stringBytes.Length;
            if (byteCount <= MessagePackRange.MaxFixStringLength)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 1);
                bytes[offset] = (byte)(MessagePackCode.MinFixStr | byteCount);
                Buffer.BlockCopy(utf8stringBytes, 0, bytes, offset + 1, byteCount);
                offset += byteCount + 1;
                return;
            }
            else if (byteCount <= byte.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 2);
                bytes[offset] = MessagePackCode.Str8;
                bytes[offset + 1] = unchecked((byte)byteCount);
                Buffer.BlockCopy(utf8stringBytes, 0, bytes, offset + 2, byteCount);
                offset += byteCount + 2;
                return;
            }
            else if (byteCount <= ushort.MaxValue)
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 3);
                bytes[offset] = MessagePackCode.Str16;
                bytes[offset + 1] = unchecked((byte)(byteCount >> 8));
                bytes[offset + 2] = unchecked((byte)byteCount);
                Buffer.BlockCopy(utf8stringBytes, 0, bytes, offset + 3, byteCount);
                offset += byteCount + 3;
                return;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref bytes, offset, byteCount + 5);
                bytes[offset] = MessagePackCode.Str32;
                bytes[offset + 1] = unchecked((byte)(byteCount >> 24));
                bytes[offset + 2] = unchecked((byte)(byteCount >> 16));
                bytes[offset + 3] = unchecked((byte)(byteCount >> 8));
                bytes[offset + 4] = unchecked((byte)byteCount);
                Buffer.BlockCopy(utf8stringBytes, 0, bytes, offset + 5, byteCount);
                offset += byteCount + 5;
                return;
            }
        }

        public static byte[] GetEncodedStringBytes(string value)
        {
            var byteCount = StringEncoding.UTF8.GetByteCount(value);
            if (byteCount <= MessagePackRange.MaxFixStringLength)
            {
                var bytes = new byte[byteCount + 1];
                bytes[0] = (byte)(MessagePackCode.MinFixStr | byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, 1);
                return bytes;
            }
            else if (byteCount <= byte.MaxValue)
            {
                var bytes = new byte[byteCount + 2];
                bytes[0] = MessagePackCode.Str8;
                bytes[1] = unchecked((byte)byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, 2);
                return bytes;
            }
            else if (byteCount <= ushort.MaxValue)
            {
                var bytes = new byte[byteCount + 3];
                bytes[0] = MessagePackCode.Str16;
                bytes[1] = unchecked((byte)(byteCount >> 8));
                bytes[2] = unchecked((byte)byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, 3);
                return bytes;
            }
            else
            {
                var bytes = new byte[byteCount + 5];
                bytes[0] = MessagePackCode.Str32;
                bytes[1] = unchecked((byte)(byteCount >> 24));
                bytes[2] = unchecked((byte)(byteCount >> 16));
                bytes[3] = unchecked((byte)(byteCount >> 8));
                bytes[4] = unchecked((byte)byteCount);
                StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, 5);
                return bytes;
            }
        }

        public static void WriteString(ref byte[] bytes, ref int offset, string value)
        {
            if (value == null)
            {
                WriteNil(ref bytes, ref offset);
                return;
            }

            // MaxByteCount -> WritePrefix -> GetBytes has some overheads of `MaxByteCount`
            // solves heuristic length check

            // ensure buffer by MaxByteCount(faster than GetByteCount)
            BinaryUtil.EnsureCapacity(ref bytes, offset, StringEncoding.UTF8.GetMaxByteCount(value.Length) + 5);

            int useOffset;
            if (value.Length <= MessagePackRange.MaxFixStringLength)
            {
                useOffset = 1;
            }
            else if (value.Length <= byte.MaxValue)
            {
                useOffset = 2;
            }
            else if (value.Length <= ushort.MaxValue)
            {
                useOffset = 3;
            }
            else
            {
                useOffset = 5;
            }

            // skip length area
            var writeBeginOffset = offset + useOffset;
            var byteCount = StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, writeBeginOffset);

            // move body and write prefix
            if (byteCount <= MessagePackRange.MaxFixStringLength)
            {
                if (useOffset != 1)
                {
                    Buffer.BlockCopy(bytes, writeBeginOffset, bytes, offset + 1, byteCount);
                }
                bytes[offset] = (byte)(MessagePackCode.MinFixStr | byteCount);
                offset += byteCount + 1;
                return;
            }
            else if (byteCount <= byte.MaxValue)
            {
                if (useOffset != 2)
                {
                    Buffer.BlockCopy(bytes, writeBeginOffset, bytes, offset + 2, byteCount);
                }

                bytes[offset] = MessagePackCode.Str8;
                bytes[offset + 1] = unchecked((byte)byteCount);
                offset += byteCount + 2;
                return;
            }
            else if (byteCount <= ushort.MaxValue)
            {
                if (useOffset != 3)
                {
                    Buffer.BlockCopy(bytes, writeBeginOffset, bytes, offset + 3, byteCount);
                }

                bytes[offset] = MessagePackCode.Str16;
                bytes[offset + 1] = unchecked((byte)(byteCount >> 8));
                bytes[offset + 2] = unchecked((byte)byteCount);
                offset += byteCount + 3;
                return;
            }
            else
            {
                if (useOffset != 5)
                {
                    Buffer.BlockCopy(bytes, writeBeginOffset, bytes, offset + 5, byteCount);
                }

                bytes[offset] = MessagePackCode.Str32;
                bytes[offset + 1] = unchecked((byte)(byteCount >> 24));
                bytes[offset + 2] = unchecked((byte)(byteCount >> 16));
                bytes[offset + 3] = unchecked((byte)(byteCount >> 8));
                bytes[offset + 4] = unchecked((byte)byteCount);
                offset += byteCount + 5;
                return;
            }
        }

        public static void WriteStringForceStr32Block(ref byte[] bytes, ref int offset, string value)
        {
            if (value == null)
            {
                WriteNil(ref bytes, ref offset);
                return;
            }

            BinaryUtil.EnsureCapacity(ref bytes, offset, StringEncoding.UTF8.GetMaxByteCount(value.Length) + 5);

            var byteCount = StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset + 5);

            bytes[offset] = MessagePackCode.Str32;
            bytes[offset + 1] = unchecked((byte)(byteCount >> 24));
            bytes[offset + 2] = unchecked((byte)(byteCount >> 16));
            bytes[offset + 3] = unchecked((byte)(byteCount >> 8));
            bytes[offset + 4] = unchecked((byte)byteCount);
            offset += byteCount + 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(byte[] bytes, ref int offset)
        {
            return stringDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArraySegment<byte> ReadStringSegment(byte[] bytes, ref int offset)
        {
            return stringSegmentDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExtensionFormatHeader(ref byte[] bytes, ref int offset, sbyte typeCode, int dataLength)
        {
            switch (dataLength)
            {
                case 1:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.FixExt1;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    offset += 2;
                    return;
                case 2:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
                    bytes[offset] = MessagePackCode.FixExt2;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    offset += 2;
                    return;
                case 4:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 6);
                    bytes[offset] = MessagePackCode.FixExt4;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    offset += 2;
                    return;
                case 8:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 10);
                    bytes[offset] = MessagePackCode.FixExt8;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    offset += 2;
                    return;
                case 16:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 18);
                    bytes[offset] = MessagePackCode.FixExt16;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    offset += 2;
                    return;
                default:
                    unchecked
                    {
                        if (dataLength <= byte.MaxValue)
                        {
                            BinaryUtil.EnsureCapacity(ref bytes, offset, dataLength + 3);
                            bytes[offset] = MessagePackCode.Ext8;
                            bytes[offset + 1] = unchecked((byte)(dataLength));
                            bytes[offset + 2] = unchecked((byte)typeCode);
                            offset += 3;
                            return;
                        }
                        else if (dataLength <= UInt16.MaxValue)
                        {
                            BinaryUtil.EnsureCapacity(ref bytes, offset, dataLength + 4);
                            bytes[offset] = MessagePackCode.Ext16;
                            bytes[offset + 1] = unchecked((byte)(dataLength >> 8));
                            bytes[offset + 2] = unchecked((byte)(dataLength));
                            bytes[offset + 3] = unchecked((byte)typeCode);
                            offset += 4;
                            return;
                        }
                        else
                        {
                            BinaryUtil.EnsureCapacity(ref bytes, offset, dataLength + 6);
                            bytes[offset] = MessagePackCode.Ext32;
                            bytes[offset + 1] = unchecked((byte)(dataLength >> 24));
                            bytes[offset + 2] = unchecked((byte)(dataLength >> 16));
                            bytes[offset + 3] = unchecked((byte)(dataLength >> 8));
                            bytes[offset + 4] = unchecked((byte)dataLength);
                            bytes[offset + 5] = unchecked((byte)typeCode);
                            offset += 6;
                            return;
                        }
                    }
            }
        }

        /// <summary>
        /// Write extension format header, always use ext32 format(length is fixed, 6).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExtensionFormatHeaderForceExt32Block(ref byte[] bytes, ref int offset, sbyte typeCode, int dataLength)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, dataLength + 6);
            bytes[offset] = MessagePackCode.Ext32;
            bytes[offset + 1] = unchecked((byte)(dataLength >> 24));
            bytes[offset + 2] = unchecked((byte)(dataLength >> 16));
            bytes[offset + 3] = unchecked((byte)(dataLength >> 8));
            bytes[offset + 4] = unchecked((byte)dataLength);
            bytes[offset + 5] = unchecked((byte)typeCode);
            offset += 6;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteExtensionFormat(ref byte[] bytes, ref int offset, sbyte typeCode, byte[] data)
        {
            var length = data.Length;
            switch (length)
            {
                case 1:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 3);
                    bytes[offset] = MessagePackCode.FixExt1;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    bytes[offset + 2] = data[0];
                    offset += 3;
                    return;
                case 2:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 4);
                    bytes[offset] = MessagePackCode.FixExt2;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    bytes[offset + 2] = data[0];
                    bytes[offset + 3] = data[1];
                    offset += 4;
                    return;
                case 4:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 6);
                    bytes[offset] = MessagePackCode.FixExt4;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    bytes[offset + 2] = data[0];
                    bytes[offset + 3] = data[1];
                    bytes[offset + 4] = data[2];
                    bytes[offset + 5] = data[3];
                    offset += 6;
                    return;
                case 8:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 10);
                    bytes[offset] = MessagePackCode.FixExt8;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    bytes[offset + 2] = data[0];
                    bytes[offset + 3] = data[1];
                    bytes[offset + 4] = data[2];
                    bytes[offset + 5] = data[3];
                    bytes[offset + 6] = data[4];
                    bytes[offset + 7] = data[5];
                    bytes[offset + 8] = data[6];
                    bytes[offset + 9] = data[7];
                    offset += 10;
                    return;
                case 16:
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 18);
                    bytes[offset] = MessagePackCode.FixExt16;
                    bytes[offset + 1] = unchecked((byte)typeCode);
                    bytes[offset + 2] = data[0];
                    bytes[offset + 3] = data[1];
                    bytes[offset + 4] = data[2];
                    bytes[offset + 5] = data[3];
                    bytes[offset + 6] = data[4];
                    bytes[offset + 7] = data[5];
                    bytes[offset + 8] = data[6];
                    bytes[offset + 9] = data[7];
                    bytes[offset + 10] = data[8];
                    bytes[offset + 11] = data[9];
                    bytes[offset + 12] = data[10];
                    bytes[offset + 13] = data[11];
                    bytes[offset + 14] = data[12];
                    bytes[offset + 15] = data[13];
                    bytes[offset + 16] = data[14];
                    bytes[offset + 17] = data[15];
                    offset += 18;
                    return;
                default:
                    unchecked
                    {
                        if (data.Length <= byte.MaxValue)
                        {
                            BinaryUtil.EnsureCapacity(ref bytes, offset, length + 3);
                            bytes[offset] = MessagePackCode.Ext8;
                            bytes[offset + 1] = unchecked((byte)(length));
                            bytes[offset + 2] = unchecked((byte)typeCode);
                            Buffer.BlockCopy(data, 0, bytes, offset + 3, length);
                            offset += length + 3;
                            return;
                        }
                        else if (data.Length <= UInt16.MaxValue)
                        {
                            BinaryUtil.EnsureCapacity(ref bytes, offset, length + 4);
                            bytes[offset] = MessagePackCode.Ext16;
                            bytes[offset + 1] = unchecked((byte)(length >> 8));
                            bytes[offset + 2] = unchecked((byte)(length));
                            bytes[offset + 3] = unchecked((byte)typeCode);
                            Buffer.BlockCopy(data, 0, bytes, offset + 4, length);
                            offset += length + 4;
                            return;
                        }
                        else
                        {
                            BinaryUtil.EnsureCapacity(ref bytes, offset, length + 6);
                            bytes[offset] = MessagePackCode.Ext32;
                            bytes[offset + 1] = unchecked((byte)(length >> 24));
                            bytes[offset + 2] = unchecked((byte)(length >> 16));
                            bytes[offset + 3] = unchecked((byte)(length >> 8));
                            bytes[offset + 4] = unchecked((byte)length);
                            bytes[offset + 5] = unchecked((byte)typeCode);
                            Buffer.BlockCopy(data, 0, bytes, offset + 6, length);
                            offset += length + 6;
                            return;
                        }
                    }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ExtensionResult ReadExtensionFormat(byte[] bytes, ref int offset)
        {
            return extDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        /// <summary>
        /// return byte length of ExtensionFormat.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ExtensionHeader ReadExtensionFormatHeader(byte[] bytes, ref int offset)
        {
            return extHeaderDecoders[bytes[offset]].Read(bytes, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetExtensionFormatHeaderLength(int dataLength)
        {
            switch (dataLength)
            {
                case 1:
                case 2:
                case 4:
                case 8:
                case 16:
                    return 2;
                default:
                    if (dataLength <= byte.MaxValue)
                    {
                        return 3;
                    }
                    else if (dataLength <= UInt16.MaxValue)
                    {
                        return 4;
                    }
                    else
                    {
                        return 6;
                    }
            }
        }

        // Timestamp spec
        // https://github.com/msgpack/msgpack/pull/209
        // FixExt4(-1) => seconds |  [1970-01-01 00:00:00 UTC, 2106-02-07 06:28:16 UTC) range
        // FixExt8(-1) => nanoseconds + seconds | [1970-01-01 00:00:00.000000000 UTC, 2514-05-30 01:53:04.000000000 UTC) range
        // Ext8(12,-1) => nanoseconds + seconds | [-584554047284-02-23 16:59:44 UTC, 584554051223-11-09 07:00:16.000000000 UTC) range

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDateTime(ref byte[] bytes, ref int offset, DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();

            var secondsSinceBclEpoch = dateTime.Ticks / TimeSpan.TicksPerSecond;
            var seconds = secondsSinceBclEpoch - DateTimeConstants.BclSecondsAtUnixEpoch;
            var nanoseconds = (dateTime.Ticks % TimeSpan.TicksPerSecond) * DateTimeConstants.NanosecondsPerTick;

            // reference pseudo code.
            /*
            struct timespec {
                long tv_sec;  // seconds
                long tv_nsec; // nanoseconds
            } time;
            if ((time.tv_sec >> 34) == 0)
            {
                uint64_t data64 = (time.tv_nsec << 34) | time.tv_sec;
                if (data & 0xffffffff00000000L == 0)
                {
                    // timestamp 32
                    uint32_t data32 = data64;
                    serialize(0xd6, -1, data32)
                }
                else
                {
                    // timestamp 64
                    serialize(0xd7, -1, data64)
                }
            }
            else
            {
                // timestamp 96
                serialize(0xc7, 12, -1, time.tv_nsec, time.tv_sec)
            }
            */

            if ((seconds >> 34) == 0)
            {
                var data64 = unchecked((ulong)((nanoseconds << 34) | seconds));
                if ((data64 & 0xffffffff00000000L) == 0)
                {
                    // timestamp 32(seconds in 32-bit unsigned int)
                    var data32 = (UInt32)data64;
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 6);
                    bytes[offset] = MessagePackCode.FixExt4;
                    bytes[offset + 1] = unchecked((byte)ReservedMessagePackExtensionTypeCode.DateTime);
                    bytes[offset + 2] = unchecked((byte)(data32 >> 24));
                    bytes[offset + 3] = unchecked((byte)(data32 >> 16));
                    bytes[offset + 4] = unchecked((byte)(data32 >> 8));
                    bytes[offset + 5] = unchecked((byte)data32);

                    offset += 6;
                    return;
                }
                else
                {
                    // timestamp 64(nanoseconds in 30-bit unsigned int | seconds in 34-bit unsigned int)
                    BinaryUtil.EnsureCapacity(ref bytes, offset, 10);
                    bytes[offset] = MessagePackCode.FixExt8;
                    bytes[offset + 1] = unchecked((byte)ReservedMessagePackExtensionTypeCode.DateTime);
                    bytes[offset + 2] = unchecked((byte)(data64 >> 56));
                    bytes[offset + 3] = unchecked((byte)(data64 >> 48));
                    bytes[offset + 4] = unchecked((byte)(data64 >> 40));
                    bytes[offset + 5] = unchecked((byte)(data64 >> 32));
                    bytes[offset + 6] = unchecked((byte)(data64 >> 24));
                    bytes[offset + 7] = unchecked((byte)(data64 >> 16));
                    bytes[offset + 8] = unchecked((byte)(data64 >> 8));
                    bytes[offset + 9] = unchecked((byte)data64);

                    offset += 10;
                    return;
                }
            }
            else
            {
                // timestamp 96( nanoseconds in 32-bit unsigned int | seconds in 64-bit signed int )
                BinaryUtil.EnsureCapacity(ref bytes, offset, 15);
                bytes[offset] = MessagePackCode.Ext8;
                bytes[offset + 1] = (byte)12;
                bytes[offset + 2] = unchecked((byte)ReservedMessagePackExtensionTypeCode.DateTime);
                bytes[offset + 3] = unchecked((byte)(nanoseconds >> 24));
                bytes[offset + 4] = unchecked((byte)(nanoseconds >> 16));
                bytes[offset + 5] = unchecked((byte)(nanoseconds >> 8));
                bytes[offset + 6] = unchecked((byte)nanoseconds);
                bytes[offset + 7] = unchecked((byte)(seconds >> 56));
                bytes[offset + 8] = unchecked((byte)(seconds >> 48));
                bytes[offset + 9] = unchecked((byte)(seconds >> 40));
                bytes[offset + 10] = unchecked((byte)(seconds >> 32));
                bytes[offset + 11] = unchecked((byte)(seconds >> 24));
                bytes[offset + 12] = unchecked((byte)(seconds >> 16));
                bytes[offset + 13] = unchecked((byte)(seconds >> 8));
                bytes[offset + 14] = unchecked((byte)seconds);

                offset += 15;
                return;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime ReadDateTime(byte[] bytes, ref int offset)
        {
            return dateTimeDecoders[bytes[offset]].Read(bytes, ref offset);
        }
    }

    public struct ExtensionResult
    {
        public sbyte TypeCode { get; private set; }
        public byte[] Data { get; private set; }

        public ExtensionResult(sbyte typeCode, byte[] data)
        {
            TypeCode = typeCode;
            Data = data;
        }
    }

    public struct ExtensionHeader
    {
        public sbyte TypeCode { get; private set; }
        public uint Length { get; private set; }

        public ExtensionHeader(sbyte typeCode, uint length)
        {
            TypeCode = typeCode;
            Length = length;
        }
    }
}

namespace AOTSerializer.MessagePack.Internal
{
    internal static class DateTimeConstants
    {
        internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal const long BclSecondsAtUnixEpoch = 62135596800;
        internal const int NanosecondsPerTick = 100;
    }
}

namespace AOTSerializer.MessagePack.Decoders
{
    internal interface IMapHeaderDecoder
    {
        uint Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixMapHeader : IMapHeaderDecoder
    {
        internal static readonly IMapHeaderDecoder Instance = new FixMapHeader();

        private FixMapHeader()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            return (uint)(bytes[offset++] & 0xF);
        }
    }

    internal sealed class Map16Header : IMapHeaderDecoder
    {
        internal static readonly IMapHeaderDecoder Instance = new Map16Header();

        private Map16Header()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                offset += 3;
                return (uint)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class Map32Header : IMapHeaderDecoder
    {
        internal static readonly IMapHeaderDecoder Instance = new Map32Header();

        private Map32Header()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                offset += 5;
                return (uint)((bytes[offset - 4] << 24) | (bytes[offset - 3] << 16) | (bytes[offset - 2] << 8) | bytes[offset - 1]);
            }
        }
    }

    internal sealed class InvalidMapHeader : IMapHeaderDecoder
    {
        internal static readonly IMapHeaderDecoder Instance = new InvalidMapHeader();

        private InvalidMapHeader()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IArrayHeaderDecoder
    {
        uint Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixArrayHeader : IArrayHeaderDecoder
    {
        internal static readonly IArrayHeaderDecoder Instance = new FixArrayHeader();

        private FixArrayHeader()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            return (uint)(bytes[offset++] & 0xF);
        }
    }

    internal sealed class Array16Header : IArrayHeaderDecoder
    {
        internal static readonly IArrayHeaderDecoder Instance = new Array16Header();

        private Array16Header()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                offset += 3;
                return (uint)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class Array32Header : IArrayHeaderDecoder
    {
        internal static readonly IArrayHeaderDecoder Instance = new Array32Header();

        private Array32Header()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                offset += 5;
                return (uint)((bytes[offset - 4] << 24) | (bytes[offset - 3] << 16) | (bytes[offset - 2] << 8) | bytes[offset - 1]);
            }
        }
    }

    internal sealed class InvalidArrayHeader : IArrayHeaderDecoder
    {
        internal static readonly IArrayHeaderDecoder Instance = new InvalidArrayHeader();

        private InvalidArrayHeader()
        {

        }

        public uint Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IBooleanDecoder
    {
        bool Read(ref int offset);
    }

    internal sealed class True : IBooleanDecoder
    {
        internal static IBooleanDecoder Instance = new True();

        private True() { }

        public bool Read(ref int offset)
        {
            offset++;
            return true;
        }
    }

    internal sealed class False : IBooleanDecoder
    {
        internal static IBooleanDecoder Instance = new False();

        private False() { }

        public bool Read(ref int offset)
        {
            offset++;
            return false;
        }
    }

    internal sealed class InvalidBoolean : IBooleanDecoder
    {
        internal static IBooleanDecoder Instance = new InvalidBoolean();

        private InvalidBoolean() { }

        public bool Read(ref int offset)
        {
            throw new InvalidOperationException("code is invalid.");
        }
    }

    internal interface IByteDecoder
    {
        byte Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixByte : IByteDecoder
    {
        internal static readonly IByteDecoder Instance = new FixByte();

        private FixByte()
        {

        }

        public byte Read(byte[] bytes, ref int offset)
        {
            return bytes[offset++];
        }
    }

    internal sealed class UInt8Byte : IByteDecoder
    {
        internal static readonly IByteDecoder Instance = new UInt8Byte();

        private UInt8Byte()
        {

        }

        public byte Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return bytes[offset - 1];
        }
    }

    internal sealed class InvalidByte : IByteDecoder
    {
        internal static readonly IByteDecoder Instance = new InvalidByte();

        private InvalidByte()
        {

        }

        public byte Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IBytesDecoder
    {
        byte[] Read(byte[] bytes, ref int offset);
    }

    internal sealed class NilBytes : IBytesDecoder
    {
        internal static readonly IBytesDecoder Instance = new NilBytes();

        private NilBytes()
        {

        }

        public byte[] Read(byte[] bytes, ref int offset)
        {
            offset++;
            return null;
        }
    }

    internal sealed class Bin8Bytes : IBytesDecoder
    {
        internal static readonly IBytesDecoder Instance = new Bin8Bytes();

        private Bin8Bytes()
        {

        }

        public byte[] Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset + 1];
            var newBytes = new byte[length];
            Buffer.BlockCopy(bytes, offset + 2, newBytes, 0, length);

            offset += length + 2;
            return newBytes;
        }
    }

    internal sealed class Bin16Bytes : IBytesDecoder
    {
        internal static readonly IBytesDecoder Instance = new Bin16Bytes();

        private Bin16Bytes()
        {

        }

        public byte[] Read(byte[] bytes, ref int offset)
        {
            var length = (bytes[offset + 1] << 8) + (bytes[offset + 2]);
            var newBytes = new byte[length];
            Buffer.BlockCopy(bytes, offset + 3, newBytes, 0, length);

            offset += length + 3;
            return newBytes;
        }
    }

    internal sealed class Bin32Bytes : IBytesDecoder
    {
        internal static readonly IBytesDecoder Instance = new Bin32Bytes();

        private Bin32Bytes()
        {

        }

        public byte[] Read(byte[] bytes, ref int offset)
        {
            var length = (bytes[offset + 1] << 24) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 8) | (bytes[offset + 4]);
            var newBytes = new byte[length];
            Buffer.BlockCopy(bytes, offset + 5, newBytes, 0, length);

            offset += length + 5;
            return newBytes;
        }
    }

    internal sealed class InvalidBytes : IBytesDecoder
    {
        internal static readonly IBytesDecoder Instance = new InvalidBytes();

        private InvalidBytes()
        {

        }

        public byte[] Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IBytesSegmentDecoder
    {
        ArraySegment<byte> Read(byte[] bytes, ref int offset);
    }

    internal sealed class NilBytesSegment : IBytesSegmentDecoder
    {
        internal static readonly IBytesSegmentDecoder Instance = new NilBytesSegment();

        private NilBytesSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            offset++;
            return default;
        }
    }

    internal sealed class Bin8BytesSegment : IBytesSegmentDecoder
    {
        internal static readonly IBytesSegmentDecoder Instance = new Bin8BytesSegment();

        private Bin8BytesSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset + 1];

            offset += length + 2;
            return new ArraySegment<byte>(bytes, offset - length, length);
        }
    }

    internal sealed class Bin16BytesSegment : IBytesSegmentDecoder
    {
        internal static readonly IBytesSegmentDecoder Instance = new Bin16BytesSegment();

        private Bin16BytesSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            var length = (bytes[offset + 1] << 8) + (bytes[offset + 2]);

            offset += length + 3;
            return new ArraySegment<byte>(bytes, offset - length, length);
        }
    }

    internal sealed class Bin32BytesSegment : IBytesSegmentDecoder
    {
        internal static readonly IBytesSegmentDecoder Instance = new Bin32BytesSegment();

        private Bin32BytesSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            var length = (bytes[offset + 1] << 24) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 8) | (bytes[offset + 4]);
            offset += length + 5;
            return new ArraySegment<byte>(bytes, offset - length, length);
        }
    }

    internal sealed class InvalidBytesSegment : IBytesSegmentDecoder
    {
        internal static readonly IBytesSegmentDecoder Instance = new InvalidBytesSegment();

        private InvalidBytesSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface ISByteDecoder
    {
        sbyte Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixSByte : ISByteDecoder
    {
        internal static readonly ISByteDecoder Instance = new FixSByte();

        private FixSByte()
        {

        }

        public sbyte Read(byte[] bytes, ref int offset)
        {
            return unchecked((sbyte)bytes[offset++]);
        }
    }

    internal sealed class Int8SByte : ISByteDecoder
    {
        internal static readonly ISByteDecoder Instance = new Int8SByte();

        private Int8SByte()
        {

        }

        public sbyte Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((sbyte)(bytes[offset - 1]));
        }
    }

    internal sealed class InvalidSByte : ISByteDecoder
    {
        internal static readonly ISByteDecoder Instance = new InvalidSByte();

        private InvalidSByte()
        {

        }

        public sbyte Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface ISingleDecoder
    {
        float Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixNegativeFloat : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new FixNegativeFloat();

        private FixNegativeFloat()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return FixSByte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class FixFloat : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new FixFloat();

        private FixFloat()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return FixByte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int8Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new Int8Single();

        private Int8Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return Int8SByte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int16Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new Int16Single();

        private Int16Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return Int16Int16.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int32Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new Int32Single();

        private Int32Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return Int32Int32.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int64Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new Int64Single();

        private Int64Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return Int64Int64.Instance.Read(bytes, ref offset);
        }
    }


    internal sealed class UInt8Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new UInt8Single();

        private UInt8Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return UInt8Byte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class UInt16Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new UInt16Single();

        private UInt16Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return UInt16UInt16.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class UInt32Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new UInt32Single();

        private UInt32Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return UInt32UInt32.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class UInt64Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new UInt64Single();

        private UInt64Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            return UInt64UInt64.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Float32Single : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new Float32Single();

        private Float32Single()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            return new Float32Bits(bytes, offset - 4).Value;
        }
    }

    internal sealed class InvalidSingle : ISingleDecoder
    {
        internal static readonly ISingleDecoder Instance = new InvalidSingle();

        private InvalidSingle()
        {

        }

        public Single Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IDoubleDecoder
    {
        double Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixNegativeDouble : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new FixNegativeDouble();

        private FixNegativeDouble()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return FixSByte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class FixDouble : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new FixDouble();

        private FixDouble()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return FixByte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int8Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new Int8Double();

        private Int8Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return Int8SByte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int16Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new Int16Double();

        private Int16Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return Int16Int16.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int32Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new Int32Double();

        private Int32Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return Int32Int32.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Int64Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new Int64Double();

        private Int64Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return Int64Int64.Instance.Read(bytes, ref offset);
        }
    }


    internal sealed class UInt8Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new UInt8Double();

        private UInt8Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return UInt8Byte.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class UInt16Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new UInt16Double();

        private UInt16Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return UInt16UInt16.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class UInt32Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new UInt32Double();

        private UInt32Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return UInt32UInt32.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class UInt64Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new UInt64Double();

        private UInt64Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            return UInt64UInt64.Instance.Read(bytes, ref offset);
        }
    }

    internal sealed class Float32Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new Float32Double();

        private Float32Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            return new Float32Bits(bytes, offset - 4).Value;
        }
    }

    internal sealed class Float64Double : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new Float64Double();

        private Float64Double()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            offset += 9;
            return new Float64Bits(bytes, offset - 8).Value;
        }
    }

    internal sealed class InvalidDouble : IDoubleDecoder
    {
        internal static readonly IDoubleDecoder Instance = new InvalidDouble();

        private InvalidDouble()
        {

        }

        public Double Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IInt16Decoder
    {
        Int16 Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixNegativeInt16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new FixNegativeInt16();

        private FixNegativeInt16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            return unchecked((short)(sbyte)bytes[offset++]);
        }
    }

    internal sealed class FixInt16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new FixInt16();

        private FixInt16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            return unchecked((short)bytes[offset++]);
        }
    }

    internal sealed class UInt8Int16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new UInt8Int16();

        private UInt8Int16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((short)(byte)(bytes[offset - 1]));
        }
    }

    internal sealed class UInt16Int16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new UInt16Int16();

        private UInt16Int16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            return checked((Int16)((bytes[offset - 2] << 8) + (bytes[offset - 1])));
        }
    }

    internal sealed class Int8Int16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new Int8Int16();

        private Int8Int16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((short)(sbyte)(bytes[offset - 1]));
        }
    }

    internal sealed class Int16Int16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new Int16Int16();

        private Int16Int16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            unchecked
            {
                return (short)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class InvalidInt16 : IInt16Decoder
    {
        internal static readonly IInt16Decoder Instance = new InvalidInt16();

        private InvalidInt16()
        {

        }

        public Int16 Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IInt32Decoder
    {
        Int32 Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixNegativeInt32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new FixNegativeInt32();

        private FixNegativeInt32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            return unchecked((int)(sbyte)bytes[offset++]);
        }
    }

    internal sealed class FixInt32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new FixInt32();

        private FixInt32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            return unchecked((int)bytes[offset++]);
        }
    }

    internal sealed class UInt8Int32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new UInt8Int32();

        private UInt8Int32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((int)(byte)(bytes[offset - 1]));
        }
    }

    internal sealed class UInt16Int32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new UInt16Int32();

        private UInt16Int32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            return (Int32)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
        }
    }

    internal sealed class UInt32Int32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new UInt32Int32();

        private UInt32Int32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            checked
            {
                return (Int32)((UInt32)(bytes[offset - 4] << 24) | (UInt32)(bytes[offset - 3] << 16) | (UInt32)(bytes[offset - 2] << 8) | (UInt32)bytes[offset - 1]);
            }
        }
    }

    internal sealed class Int8Int32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new Int8Int32();

        private Int8Int32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((int)(sbyte)(bytes[offset - 1]));
        }
    }

    internal sealed class Int16Int32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new Int16Int32();

        private Int16Int32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            unchecked
            {
                return (int)(short)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class Int32Int32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new Int32Int32();

        private Int32Int32()
        {

        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            unchecked
            {
                return (int)((bytes[offset - 4] << 24) | (bytes[offset - 3] << 16) | (bytes[offset - 2] << 8) | bytes[offset - 1]);
            }
        }
    }

    internal sealed class InvalidInt32 : IInt32Decoder
    {
        internal static readonly IInt32Decoder Instance = new InvalidInt32();

        private InvalidInt32()
        {
        }

        public Int32 Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IInt64Decoder
    {
        Int64 Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixNegativeInt64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new FixNegativeInt64();

        private FixNegativeInt64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            return unchecked((long)(sbyte)bytes[offset++]);
        }
    }

    internal sealed class FixInt64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new FixInt64();

        private FixInt64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            return unchecked((long)bytes[offset++]);
        }
    }

    internal sealed class UInt8Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new UInt8Int64();

        private UInt8Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((int)(byte)(bytes[offset - 1]));
        }
    }

    internal sealed class UInt16Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new UInt16Int64();

        private UInt16Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            return (Int64)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
        }
    }

    internal sealed class UInt32Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new UInt32Int64();

        private UInt32Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            return unchecked((Int64)((uint)(bytes[offset - 4] << 24) | ((uint)bytes[offset - 3] << 16) | ((uint)bytes[offset - 2] << 8) | (uint)bytes[offset - 1]));
        }
    }

    internal sealed class UInt64Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new UInt64Int64();

        private UInt64Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 9;
            checked
            {
                return (Int64)bytes[offset - 8] << 56 | (Int64)bytes[offset - 7] << 48 | (Int64)bytes[offset - 6] << 40 | (Int64)bytes[offset - 5] << 32
                     | (Int64)bytes[offset - 4] << 24 | (Int64)bytes[offset - 3] << 16 | (Int64)bytes[offset - 2] << 8 | (Int64)bytes[offset - 1];
            }
        }
    }


    internal sealed class Int8Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new Int8Int64();

        private Int8Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((long)(sbyte)(bytes[offset - 1]));
        }
    }

    internal sealed class Int16Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new Int16Int64();

        private Int16Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            unchecked
            {
                return (long)(short)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class Int32Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new Int32Int64();

        private Int32Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            unchecked
            {
                return (long)((long)(bytes[offset - 4] << 24) + (long)(bytes[offset - 3] << 16) + (long)(bytes[offset - 2] << 8) + (long)bytes[offset - 1]);
            }
        }
    }

    internal sealed class Int64Int64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new Int64Int64();

        private Int64Int64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            offset += 9;
            unchecked
            {
                return (long)bytes[offset - 8] << 56 | (long)bytes[offset - 7] << 48 | (long)bytes[offset - 6] << 40 | (long)bytes[offset - 5] << 32
                     | (long)bytes[offset - 4] << 24 | (long)bytes[offset - 3] << 16 | (long)bytes[offset - 2] << 8 | (long)bytes[offset - 1];
            }
        }
    }

    internal sealed class InvalidInt64 : IInt64Decoder
    {
        internal static readonly IInt64Decoder Instance = new InvalidInt64();

        private InvalidInt64()
        {

        }

        public Int64 Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IUInt16Decoder
    {
        UInt16 Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixUInt16 : IUInt16Decoder
    {
        internal static readonly IUInt16Decoder Instance = new FixUInt16();

        private FixUInt16()
        {

        }

        public UInt16 Read(byte[] bytes, ref int offset)
        {
            return unchecked((UInt16)bytes[offset++]);
        }
    }

    internal sealed class UInt8UInt16 : IUInt16Decoder
    {
        internal static readonly IUInt16Decoder Instance = new UInt8UInt16();

        private UInt8UInt16()
        {

        }

        public UInt16 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((UInt16)(bytes[offset - 1]));
        }
    }

    internal sealed class UInt16UInt16 : IUInt16Decoder
    {
        internal static readonly IUInt16Decoder Instance = new UInt16UInt16();

        private UInt16UInt16()
        {

        }

        public UInt16 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            unchecked
            {
                return (UInt16)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class InvalidUInt16 : IUInt16Decoder
    {
        internal static readonly IUInt16Decoder Instance = new InvalidUInt16();

        private InvalidUInt16()
        {

        }

        public UInt16 Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IUInt32Decoder
    {
        UInt32 Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixUInt32 : IUInt32Decoder
    {
        internal static readonly IUInt32Decoder Instance = new FixUInt32();

        private FixUInt32()
        {

        }

        public UInt32 Read(byte[] bytes, ref int offset)
        {
            return unchecked((UInt32)bytes[offset++]);
        }
    }

    internal sealed class UInt8UInt32 : IUInt32Decoder
    {
        internal static readonly IUInt32Decoder Instance = new UInt8UInt32();

        private UInt8UInt32()
        {

        }

        public UInt32 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((UInt32)(bytes[offset - 1]));
        }
    }

    internal sealed class UInt16UInt32 : IUInt32Decoder
    {
        internal static readonly IUInt32Decoder Instance = new UInt16UInt32();

        private UInt16UInt32()
        {

        }

        public UInt32 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            unchecked
            {
                return (UInt32)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class UInt32UInt32 : IUInt32Decoder
    {
        internal static readonly IUInt32Decoder Instance = new UInt32UInt32();

        private UInt32UInt32()
        {

        }

        public UInt32 Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            unchecked
            {
                return (UInt32)((UInt32)(bytes[offset - 4] << 24) | (UInt32)(bytes[offset - 3] << 16) | (UInt32)(bytes[offset - 2] << 8) | (UInt32)bytes[offset - 1]);
            }
        }
    }

    internal sealed class InvalidUInt32 : IUInt32Decoder
    {
        internal static readonly IUInt32Decoder Instance = new InvalidUInt32();

        private InvalidUInt32()
        {

        }

        public UInt32 Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IUInt64Decoder
    {
        UInt64 Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixUInt64 : IUInt64Decoder
    {
        internal static readonly IUInt64Decoder Instance = new FixUInt64();

        private FixUInt64()
        {

        }

        public UInt64 Read(byte[] bytes, ref int offset)
        {
            return unchecked((UInt64)bytes[offset++]);
        }
    }

    internal sealed class UInt8UInt64 : IUInt64Decoder
    {
        internal static readonly IUInt64Decoder Instance = new UInt8UInt64();

        private UInt8UInt64()
        {

        }

        public UInt64 Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            return unchecked((UInt64)(bytes[offset - 1]));
        }
    }

    internal sealed class UInt16UInt64 : IUInt64Decoder
    {
        internal static readonly IUInt64Decoder Instance = new UInt16UInt64();

        private UInt16UInt64()
        {

        }

        public UInt64 Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            unchecked
            {
                return (UInt64)((bytes[offset - 2] << 8) | (bytes[offset - 1]));
            }
        }
    }

    internal sealed class UInt32UInt64 : IUInt64Decoder
    {
        internal static readonly IUInt64Decoder Instance = new UInt32UInt64();

        private UInt32UInt64()
        {

        }

        public UInt64 Read(byte[] bytes, ref int offset)
        {
            offset += 5;
            unchecked
            {
                return (UInt64)(((UInt64)bytes[offset - 4] << 24) + (ulong)(bytes[offset - 3] << 16) + (UInt64)(bytes[offset - 2] << 8) + (UInt64)bytes[offset - 1]);
            }
        }
    }

    internal sealed class UInt64UInt64 : IUInt64Decoder
    {
        internal static readonly IUInt64Decoder Instance = new UInt64UInt64();

        private UInt64UInt64()
        {

        }

        public UInt64 Read(byte[] bytes, ref int offset)
        {
            offset += 9;
            unchecked
            {
                return (UInt64)bytes[offset - 8] << 56 | (UInt64)bytes[offset - 7] << 48 | (UInt64)bytes[offset - 6] << 40 | (UInt64)bytes[offset - 5] << 32
                     | (UInt64)bytes[offset - 4] << 24 | (UInt64)bytes[offset - 3] << 16 | (UInt64)bytes[offset - 2] << 8 | (UInt64)bytes[offset - 1];
            }
        }
    }

    internal sealed class InvalidUInt64 : IUInt64Decoder
    {
        internal static readonly IUInt64Decoder Instance = new InvalidUInt64();

        private InvalidUInt64()
        {

        }

        public UInt64 Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IStringDecoder
    {
        String Read(byte[] bytes, ref int offset);
    }

    internal sealed class NilString : IStringDecoder
    {
        internal static readonly IStringDecoder Instance = new NilString();

        private NilString()
        {

        }

        public String Read(byte[] bytes, ref int offset)
        {
            offset++;
            return null;
        }
    }

    internal sealed class FixString : IStringDecoder
    {
        internal static readonly IStringDecoder Instance = new FixString();

        private FixString()
        {

        }

        public String Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset] & 0x1F;
            offset += length + 1;
            return StringEncoding.UTF8.GetString(bytes, offset - length, length);
        }
    }

    internal sealed class Str8String : IStringDecoder
    {
        internal static readonly IStringDecoder Instance = new Str8String();

        private Str8String()
        {

        }

        public String Read(byte[] bytes, ref int offset)
        {
            var length = (int)bytes[offset + 1];
            offset += length + 2;
            return StringEncoding.UTF8.GetString(bytes, offset - length, length);
        }
    }

    internal sealed class Str16String : IStringDecoder
    {
        internal static readonly IStringDecoder Instance = new Str16String();

        private Str16String()
        {

        }

        public String Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (bytes[offset + 1] << 8) + (bytes[offset + 2]);
                offset += length + 3;
                return StringEncoding.UTF8.GetString(bytes, offset - length, length);
            }
        }
    }

    internal sealed class Str32String : IStringDecoder
    {
        internal static readonly IStringDecoder Instance = new Str32String();

        private Str32String()
        {

        }

        public String Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (int)((uint)(bytes[offset + 1] << 24) | (uint)(bytes[offset + 2] << 16) | (uint)(bytes[offset + 3] << 8) | (uint)bytes[offset + 4]);
                offset += length + 5;
                return StringEncoding.UTF8.GetString(bytes, offset - length, length);
            }
        }
    }

    internal sealed class InvalidString : IStringDecoder
    {
        internal static readonly IStringDecoder Instance = new InvalidString();

        private InvalidString()
        {

        }

        public String Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IStringSegmentDecoder
    {
        ArraySegment<byte> Read(byte[] bytes, ref int offset);
    }

    internal sealed class NilStringSegment : IStringSegmentDecoder
    {
        internal static readonly IStringSegmentDecoder Instance = new NilStringSegment();

        private NilStringSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            return new ArraySegment<byte>(bytes, offset++, 1);
        }
    }

    internal sealed class FixStringSegment : IStringSegmentDecoder
    {
        internal static readonly IStringSegmentDecoder Instance = new FixStringSegment();

        private FixStringSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset] & 0x1F;
            offset += length + 1;
            return new ArraySegment<byte>(bytes, offset - length, length);
        }
    }

    internal sealed class Str8StringSegment : IStringSegmentDecoder
    {
        internal static readonly IStringSegmentDecoder Instance = new Str8StringSegment();

        private Str8StringSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            var length = (int)bytes[offset + 1];
            offset += length + 2;
            return new ArraySegment<byte>(bytes, offset - length, length);
        }
    }

    internal sealed class Str16StringSegment : IStringSegmentDecoder
    {
        internal static readonly IStringSegmentDecoder Instance = new Str16StringSegment();

        private Str16StringSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (bytes[offset + 1] << 8) + (bytes[offset + 2]);
                offset += length + 3;
                return new ArraySegment<byte>(bytes, offset - length, length);
            }
        }
    }

    internal sealed class Str32StringSegment : IStringSegmentDecoder
    {
        internal static readonly IStringSegmentDecoder Instance = new Str32StringSegment();

        private Str32StringSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (int)((uint)(bytes[offset + 1] << 24) | (uint)(bytes[offset + 2] << 16) | (uint)(bytes[offset + 3] << 8) | (uint)bytes[offset + 4]);
                offset += length + 5;
                return new ArraySegment<byte>(bytes, offset - length, length);
            }
        }
    }

    internal sealed class InvalidStringSegment : IStringSegmentDecoder
    {
        internal static readonly IStringSegmentDecoder Instance = new InvalidStringSegment();

        private InvalidStringSegment()
        {

        }

        public ArraySegment<byte> Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IExtDecoder
    {
        ExtensionResult Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixExt1 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new FixExt1();

        private FixExt1()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            offset += 3;
            var typeCode = unchecked((sbyte)bytes[offset - 2]);
            var body = new byte[1] { bytes[offset - 1] }; // make new bytes is overhead?
            return new ExtensionResult(typeCode, body);
        }
    }

    internal sealed class FixExt2 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new FixExt2();

        private FixExt2()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            offset += 4;
            var typeCode = unchecked((sbyte)bytes[offset - 3]);
            var body = new byte[2]
            {
                bytes[offset - 2],
                bytes[offset - 1],
            };
            return new ExtensionResult(typeCode, body);
        }
    }

    internal sealed class FixExt4 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new FixExt4();

        private FixExt4()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            offset += 6;
            var typeCode = unchecked((sbyte)bytes[offset - 5]);
            var body = new byte[4]
            {
                bytes[offset - 4],
                bytes[offset - 3],
                bytes[offset - 2],
                bytes[offset - 1],
            };
            return new ExtensionResult(typeCode, body);
        }
    }

    internal sealed class FixExt8 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new FixExt8();

        private FixExt8()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            offset += 10;
            var typeCode = unchecked((sbyte)bytes[offset - 9]);
            var body = new byte[8]
            {
                bytes[offset - 8],
                bytes[offset - 7],
                bytes[offset - 6],
                bytes[offset - 5],
                bytes[offset - 4],
                bytes[offset - 3],
                bytes[offset - 2],
                bytes[offset - 1],
            };
            return new ExtensionResult(typeCode, body);
        }
    }

    internal sealed class FixExt16 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new FixExt16();

        private FixExt16()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            offset += 18;
            var typeCode = unchecked((sbyte)bytes[offset - 17]);
            var body = new byte[16]
            {
                bytes[offset - 16],
                bytes[offset - 15],
                bytes[offset - 14],
                bytes[offset - 13],
                bytes[offset - 12],
                bytes[offset - 11],
                bytes[offset - 10],
                bytes[offset - 9],
                bytes[offset - 8],
                bytes[offset - 7],
                bytes[offset - 6],
                bytes[offset - 5],
                bytes[offset - 4],
                bytes[offset - 3],
                bytes[offset - 2],
                bytes[offset - 1]
            };
            return new ExtensionResult(typeCode, body);
        }
    }

    internal sealed class Ext8 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new Ext8();

        private Ext8()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = bytes[offset + 1];
                var typeCode = unchecked((sbyte)bytes[offset + 2]);

                var body = new byte[length];
                offset += (int)length + 3;
                Buffer.BlockCopy(bytes, offset - length, body, 0, (int)length);
                return new ExtensionResult(typeCode, body);
            }
        }
    }

    internal sealed class Ext16 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new Ext16();

        private Ext16()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (int)((UInt16)(bytes[offset + 1] << 8) | (UInt16)bytes[offset + 2]);
                var typeCode = unchecked((sbyte)bytes[offset + 3]);

                var body = new byte[length];
                offset += length + 4;
                Buffer.BlockCopy(bytes, offset - length, body, 0, (int)length);
                return new ExtensionResult(typeCode, body);
            }
        }
    }

    internal sealed class Ext32 : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new Ext32();

        private Ext32()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (UInt32)((UInt32)(bytes[offset + 1] << 24) | (UInt32)(bytes[offset + 2] << 16) | (UInt32)(bytes[offset + 3] << 8) | (UInt32)bytes[offset + 4]);
                var typeCode = unchecked((sbyte)bytes[offset + 5]);

                var body = new byte[length];
                checked
                {
                    offset += (int)length + 6;
                    Buffer.BlockCopy(bytes, offset - (int)length, body, 0, (int)length);
                }
                return new ExtensionResult(typeCode, body);
            }
        }
    }

    internal sealed class InvalidExt : IExtDecoder
    {
        internal static readonly IExtDecoder Instance = new InvalidExt();

        private InvalidExt()
        {

        }

        public ExtensionResult Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IExtHeaderDecoder
    {
        ExtensionHeader Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixExt1Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new FixExt1Header();

        private FixExt1Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            var typeCode = unchecked((sbyte)bytes[offset - 1]);
            return new ExtensionHeader(typeCode, 1);
        }
    }

    internal sealed class FixExt2Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new FixExt2Header();

        private FixExt2Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            var typeCode = unchecked((sbyte)bytes[offset - 1]);
            return new ExtensionHeader(typeCode, 2);
        }
    }

    internal sealed class FixExt4Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new FixExt4Header();

        private FixExt4Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            var typeCode = unchecked((sbyte)bytes[offset - 1]);
            return new ExtensionHeader(typeCode, 4);
        }
    }

    internal sealed class FixExt8Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new FixExt8Header();

        private FixExt8Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            var typeCode = unchecked((sbyte)bytes[offset - 1]);
            return new ExtensionHeader(typeCode, 8);
        }
    }

    internal sealed class FixExt16Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new FixExt16Header();

        private FixExt16Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            offset += 2;
            var typeCode = unchecked((sbyte)bytes[offset - 1]);
            return new ExtensionHeader(typeCode, 16);
        }
    }

    internal sealed class Ext8Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new Ext8Header();

        private Ext8Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = bytes[offset + 1];
                var typeCode = unchecked((sbyte)bytes[offset + 2]);

                offset += 3;
                return new ExtensionHeader(typeCode, length);
            }
        }
    }

    internal sealed class Ext16Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new Ext16Header();

        private Ext16Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (UInt32)((UInt16)(bytes[offset + 1] << 8) | (UInt16)bytes[offset + 2]);
                var typeCode = unchecked((sbyte)bytes[offset + 3]);

                offset += 4;
                return new ExtensionHeader(typeCode, length);
            }
        }
    }

    internal sealed class Ext32Header : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new Ext32Header();

        private Ext32Header()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            unchecked
            {
                var length = (UInt32)((UInt32)(bytes[offset + 1] << 24) | (UInt32)(bytes[offset + 2] << 16) | (UInt32)(bytes[offset + 3] << 8) | (UInt32)bytes[offset + 4]);
                var typeCode = unchecked((sbyte)bytes[offset + 5]);

                offset += 6;
                return new ExtensionHeader(typeCode, length);
            }
        }
    }

    internal sealed class InvalidExtHeader : IExtHeaderDecoder
    {
        internal static readonly IExtHeaderDecoder Instance = new InvalidExtHeader();

        private InvalidExtHeader()
        {

        }

        public ExtensionHeader Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IDateTimeDecoder
    {
        DateTime Read(byte[] bytes, ref int offset);
    }

    internal sealed class FixExt4DateTime : IDateTimeDecoder
    {
        internal static readonly IDateTimeDecoder Instance = new FixExt4DateTime();

        private FixExt4DateTime()
        {

        }

        public DateTime Read(byte[] bytes, ref int offset)
        {
            var typeCode = unchecked((sbyte)bytes[offset + 1]);
            if (typeCode != ReservedMessagePackExtensionTypeCode.DateTime)
            {
                throw new InvalidOperationException(string.Format("typeCode is invalid. typeCode:{0}", typeCode));
            }

            unchecked
            {
                var seconds = (UInt32)((UInt32)(bytes[offset + 2] << 24) | (UInt32)(bytes[offset + 3] << 16) | (UInt32)(bytes[offset + 4] << 8) | (UInt32)bytes[offset + 5]);

                offset += 6;
                return DateTimeConstants.UnixEpoch.AddSeconds(seconds);
            }
        }
    }

    internal sealed class FixExt8DateTime : IDateTimeDecoder
    {
        internal static readonly IDateTimeDecoder Instance = new FixExt8DateTime();

        private FixExt8DateTime()
        {

        }

        public DateTime Read(byte[] bytes, ref int offset)
        {
            var typeCode = unchecked((sbyte)bytes[offset + 1]);
            if (typeCode != ReservedMessagePackExtensionTypeCode.DateTime)
            {
                throw new InvalidOperationException(string.Format("typeCode is invalid. typeCode:{0}", typeCode));
            }

            var data64 = (UInt64)bytes[offset + 2] << 56 | (UInt64)bytes[offset + 3] << 48 | (UInt64)bytes[offset + 4] << 40 | (UInt64)bytes[offset + 5] << 32
                       | (UInt64)bytes[offset + 6] << 24 | (UInt64)bytes[offset + 7] << 16 | (UInt64)bytes[offset + 8] << 8 | (UInt64)bytes[offset + 9];

            var nanoseconds = (long)(data64 >> 34);
            var seconds = data64 & 0x00000003ffffffffL;

            offset += 10;
            return DateTimeConstants.UnixEpoch.AddSeconds(seconds).AddTicks(nanoseconds / DateTimeConstants.NanosecondsPerTick);
        }
    }

    internal sealed class Ext8DateTime : IDateTimeDecoder
    {
        internal static readonly IDateTimeDecoder Instance = new Ext8DateTime();

        private Ext8DateTime()
        {

        }

        public DateTime Read(byte[] bytes, ref int offset)
        {
            var length = checked((byte)bytes[offset + 1]);
            var typeCode = unchecked((sbyte)bytes[offset + 2]);
            if (length != 12 || typeCode != ReservedMessagePackExtensionTypeCode.DateTime)
            {
                throw new InvalidOperationException(string.Format("typeCode is invalid. typeCode:{0}", typeCode));
            }

            var nanoseconds = (UInt32)((UInt32)(bytes[offset + 3] << 24) | (UInt32)(bytes[offset + 4] << 16) | (UInt32)(bytes[offset + 5] << 8) | (UInt32)bytes[offset + 6]);
            unchecked
            {
                var seconds = (long)bytes[offset + 7] << 56 | (long)bytes[offset + 8] << 48 | (long)bytes[offset + 9] << 40 | (long)bytes[offset + 10] << 32
                            | (long)bytes[offset + 11] << 24 | (long)bytes[offset + 12] << 16 | (long)bytes[offset + 13] << 8 | (long)bytes[offset + 14];

                offset += 15;
                return DateTimeConstants.UnixEpoch.AddSeconds(seconds).AddTicks(nanoseconds / DateTimeConstants.NanosecondsPerTick);
            }
        }
    }

    internal sealed class InvalidDateTime : IDateTimeDecoder
    {
        internal static readonly IDateTimeDecoder Instance = new InvalidDateTime();

        private InvalidDateTime()
        {

        }

        public DateTime Read(byte[] bytes, ref int offset)
        {
            throw new InvalidOperationException(string.Format("code is invalid. code:{0} format:{1}", bytes[offset], MessagePackCode.ToFormatName(bytes[offset])));
        }
    }

    internal interface IReadNextDecoder
    {
        void Read(byte[] bytes, ref int offset);
    }

    internal sealed class ReadNext1 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext1();

        private ReadNext1()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset++; }
    }

    internal sealed class ReadNext2 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext2();

        private ReadNext2()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 2; }
    }

    internal sealed class ReadNext3 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext3();

        private ReadNext3()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 3; }
    }

    internal sealed class ReadNext4 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext4();

        private ReadNext4()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 4; }
    }

    internal sealed class ReadNext5 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext5();

        private ReadNext5()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 5; }
    }

    internal sealed class ReadNext6 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext6();

        private ReadNext6()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 6; }
    }

    internal sealed class ReadNext9 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext9();

        private ReadNext9()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 9; }
    }

    internal sealed class ReadNext10 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext10();

        private ReadNext10()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 10; }
    }

    internal sealed class ReadNext18 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNext18();

        private ReadNext18()
        {

        }
        public void Read(byte[] bytes, ref int offset) { offset += 18; }
    }

    internal sealed class ReadNextMap : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextMap();

        private ReadNextMap()
        {

        }

        public void Read(byte[] bytes, ref int offset)
        {
            var length = MessagePackBinary.ReadMapHeader(bytes, ref offset);
            for (int i = 0; i < length; i++)
            {
                MessagePackBinary.ReadNext(bytes, ref offset); // key
                MessagePackBinary.ReadNext(bytes, ref offset); // value
            }
        }
    }

    internal sealed class ReadNextArray : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextArray();

        private ReadNextArray()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = MessagePackBinary.ReadArrayHeader(bytes, ref offset);
            for (int i = 0; i < length; i++)
            {
                MessagePackBinary.ReadNext(bytes, ref offset);
            }
        }
    }

    internal sealed class ReadNextFixStr : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextFixStr();

        private ReadNextFixStr()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset] & 0x1F;
            offset += length + 1;
        }
    }

    internal sealed class ReadNextStr8 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextStr8();

        private ReadNextStr8()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = (int)bytes[offset + 1];
            offset += length + 2;
        }
    }

    internal sealed class ReadNextStr16 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextStr16();

        private ReadNextStr16()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {

            var length = (bytes[offset + 1] << 8) | (bytes[offset + 2]);
            offset += length + 3;
        }
    }

    internal sealed class ReadNextStr32 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextStr32();

        private ReadNextStr32()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = (int)((uint)(bytes[offset + 1] << 24) | (uint)(bytes[offset + 2] << 16) | (uint)(bytes[offset + 3] << 8) | (uint)bytes[offset + 4]);
            offset += length + 5;
        }
    }

    internal sealed class ReadNextBin8 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextBin8();

        private ReadNextBin8()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset + 1];
            offset += length + 2;
        }
    }

    internal sealed class ReadNextBin16 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextBin16();

        private ReadNextBin16()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {

            var length = (bytes[offset + 1] << 8) | (bytes[offset + 2]);
            offset += length + 3;
        }
    }

    internal sealed class ReadNextBin32 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextBin32();

        private ReadNextBin32()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = (bytes[offset + 1] << 24) | (bytes[offset + 2] << 16) | (bytes[offset + 3] << 8) | (bytes[offset + 4]);
            offset += length + 5;
        }
    }

    internal sealed class ReadNextExt8 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextExt8();

        private ReadNextExt8()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = bytes[offset + 1];
            offset += (int)length + 3;
        }
    }

    internal sealed class ReadNextExt16 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextExt16();

        private ReadNextExt16()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = (int)((UInt16)(bytes[offset + 1] << 8) | (UInt16)bytes[offset + 2]);
            offset += length + 4;
        }
    }

    internal sealed class ReadNextExt32 : IReadNextDecoder
    {
        internal static readonly IReadNextDecoder Instance = new ReadNextExt32();

        private ReadNextExt32()
        {

        }
        public void Read(byte[] bytes, ref int offset)
        {
            var length = (UInt32)((UInt32)(bytes[offset + 1] << 24) | (UInt32)(bytes[offset + 2] << 16) | (UInt32)(bytes[offset + 3] << 8) | (UInt32)bytes[offset + 4]);
            offset += (int)length + 6;
        }
    }
}