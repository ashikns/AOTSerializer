using AOTSerializer.Internal;
using AOTSerializer.Json.Internal;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace AOTSerializer.Json
{
    // JSON RFC: https://www.ietf.org/rfc/rfc4627.txt

    public static class JsonUtility
    {
        private static readonly ArraySegment<byte> nullTokenSegment = new ArraySegment<byte>(new byte[] { 110, 117, 108, 108 }, 0, 4);
        private static readonly byte[] bom = Encoding.UTF8.GetPreamble();

        public static JsonToken GetCurrentJsonToken(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            var c = bytes[offset];
            switch (c)
            {
                case (byte)'{': return JsonToken.BeginObject;
                case (byte)'}': return JsonToken.EndObject;
                case (byte)'[': return JsonToken.BeginArray;
                case (byte)']': return JsonToken.EndArray;
                case (byte)'t': return JsonToken.True;
                case (byte)'f': return JsonToken.False;
                case (byte)'n': return JsonToken.Null;
                case (byte)',': return JsonToken.ValueSeparator;
                case (byte)':': return JsonToken.NameSeparator;
                case (byte)'-': return JsonToken.Number;
                case (byte)'0': return JsonToken.Number;
                case (byte)'1': return JsonToken.Number;
                case (byte)'2': return JsonToken.Number;
                case (byte)'3': return JsonToken.Number;
                case (byte)'4': return JsonToken.Number;
                case (byte)'5': return JsonToken.Number;
                case (byte)'6': return JsonToken.Number;
                case (byte)'7': return JsonToken.Number;
                case (byte)'8': return JsonToken.Number;
                case (byte)'9': return JsonToken.Number;
                case (byte)'\"': return JsonToken.String;
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 46:
                case 47:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 92:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                default:
                    return JsonToken.None;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SkipBom(byte[] bytes, ref int offset)
        {
            if (bytes.Length >= 3)
            {
                if (bytes[offset] == bom[0] && bytes[offset + 1] == bom[1] && bytes[offset + 2] == bom[2])
                {
                    offset += 3;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SkipWhiteSpace(byte[] bytes, ref int offset)
        {
            // eliminate array bound check
            for (int i = offset; i < bytes.Length; i++)
            {
                switch (bytes[i])
                {
                    case 0x20: // Space
                    case 0x09: // Horizontal tab
                    case 0x0A: // Line feed or New line
                    case 0x0D: // Carriage return
                        continue;
                    case (byte)'/': // BeginComment
                        ReadComment(bytes, ref i);
                        continue;
                    // optimize skip jumptable
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 11:
                    case 12:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    case 18:
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 24:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                    case 44:
                    case 45:
                    case 46:
                    default:
                        offset = i; // end
                        return;
                }
            }

            offset = bytes.Length;
        }

        public static bool ReadIsNull(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == 'n')
            {
                if (bytes[offset + 1] != 'u') goto ERROR;
                if (bytes[offset + 2] != 'l') goto ERROR;
                if (bytes[offset + 3] != 'l') goto ERROR;
                offset += 4;
                return true;
            }
            else
            {
                return false;
            }

        ERROR:
            throw CreateParsingException("null", bytes, ref offset);
        }

        public static void ReadIsNullWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsNull(bytes, ref offset)) throw CreateParsingException("null", bytes, ref offset);
        }

        public static bool ReadIsBeginArray(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == '[')
            {
                offset += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ReadIsBeginArrayWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsBeginArray(bytes, ref offset)) throw CreateParsingException("[", bytes, ref offset);
        }

        public static bool ReadIsEndArray(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == ']')
            {
                offset += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ReadIsEndArrayWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsEndArray(bytes, ref offset)) throw CreateParsingException("]", bytes, ref offset);
        }

        public static bool ReadIsBeginObject(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == '{')
            {
                offset += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ReadIsBeginObjectWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsBeginObject(bytes, ref offset)) throw CreateParsingException("{", bytes, ref offset);
        }

        public static bool ReadIsEndObject(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == '}')
            {
                offset += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ReadIsEndObjectWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsEndObject(bytes, ref offset)) throw CreateParsingException("}", bytes, ref offset);
        }

        public static bool ReadIsValueSeparator(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == ',')
            {
                offset += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ReadIsValueSeparatorWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsValueSeparator(bytes, ref offset)) throw CreateParsingException(",", bytes, ref offset);
        }

        public static bool ReadIsNameSeparator(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == ':')
            {
                offset += 1;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void ReadIsNameSeparatorWithVerify(byte[] bytes, ref int offset)
        {
            if (!ReadIsNameSeparator(bytes, ref offset)) throw CreateParsingException(":", bytes, ref offset);
        }

        /// <summary>
        /// Same as ReadStringSegment, but converts to UTF8 string.
        /// </summary>
        public static string ReadString(byte[] bytes, ref int offset)
        {
            if (ReadIsNull(bytes, ref offset)) { return null; }
            return StringEncoding.UTF8.GetString(ReadStringSegmentCore(bytes, ref offset));
        }

        /// <summary>
        /// Get string with code point conversion.
        /// Returned ArraySegment may have the original array as underlying Array. Don't modify!!
        /// </summary>
        public static ArraySegment<byte> ReadStringSegment(byte[] bytes, ref int offset)
        {
            if (ReadIsNull(bytes, ref offset)) { return nullTokenSegment; }
            return ReadStringSegmentCore(bytes, ref offset);
        }

        /// <summary>
        /// Get raw string-span(do not unescape).
        /// Returned ArraySegment has the original array as underlying Array. Don't modify!!
        /// </summary>
        public static ArraySegment<byte> ReadStringSegmentRaw(byte[] bytes, ref int offset)
        {
            if (ReadIsNull(bytes, ref offset))
            {
                return nullTokenSegment;
            }

            // SkipWhiteSpace is already called from IsNull
            if (bytes[offset++] != '\"') throw CreateParsingException("\"", bytes, ref offset);

            var from = offset;

            for (int i = offset; i < bytes.Length; i++)
            {
                if (bytes[i] == (char)'\"')
                {
                    // is escape?
                    if (bytes[i - 1] == (char)'\\')
                    {
                        continue;
                    }
                    else
                    {
                        offset = i + 1;
                        return new ArraySegment<byte>(bytes, from, offset - from - 1); // remove \"
                    }
                }
            }

            throw CreateParsingExceptionMessage("not found end string.", bytes, ref offset);
        }

        /// <summary> ReadStringSegment + ReadIsNameSeparatorWithVerify </summary>
        public static ArraySegment<byte> ReadPropertyNameSegment(byte[] bytes, ref int offset)
        {
            var key = ReadStringSegment(bytes, ref offset);
            ReadIsNameSeparatorWithVerify(bytes, ref offset);
            return key;
        }

        /// <summary> ReadStringSegmentRaw + ReadIsNameSeparatorWithVerify </summary>
        public static ArraySegment<byte> ReadPropertyNameSegmentRaw(byte[] bytes, ref int offset)
        {
            var key = ReadStringSegmentRaw(bytes, ref offset);
            ReadIsNameSeparatorWithVerify(bytes, ref offset);
            return key;
        }

        public static bool ReadBoolean(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            if (bytes[offset] == 't')
            {
                if (bytes[offset + 1] != 'r') goto ERROR_TRUE;
                if (bytes[offset + 2] != 'u') goto ERROR_TRUE;
                if (bytes[offset + 3] != 'e') goto ERROR_TRUE;
                offset += 4;
                return true;
            }
            else if (bytes[offset] == 'f')
            {
                if (bytes[offset + 1] != 'a') goto ERROR_FALSE;
                if (bytes[offset + 2] != 'l') goto ERROR_FALSE;
                if (bytes[offset + 3] != 's') goto ERROR_FALSE;
                if (bytes[offset + 4] != 'e') goto ERROR_FALSE;
                offset += 5;
                return false;
            }
            else
            {
                throw CreateParsingException("true | false", bytes, ref offset);
            }

        ERROR_TRUE:
            throw CreateParsingException("true", bytes, ref offset);
        ERROR_FALSE:
            throw CreateParsingException("false", bytes, ref offset);
        }

        public static void ReadNext(byte[] bytes, ref int offset)
        {
            var token = GetCurrentJsonToken(bytes, ref offset);
            ReadNextCore(token, bytes, ref offset);
        }

        public static void ReadNextBlock(byte[] bytes, ref int offset)
        {
            var stack = 0;

        AGAIN:
            var token = GetCurrentJsonToken(bytes, ref offset);
            switch (token)
            {
                case JsonToken.BeginObject:
                case JsonToken.BeginArray:
                    offset++;
                    stack++;
                    goto AGAIN;
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                    offset++;
                    stack--;
                    if (stack != 0) { goto AGAIN; }
                    break;
                case JsonToken.True:
                case JsonToken.False:
                case JsonToken.Null:
                case JsonToken.String:
                case JsonToken.Number:
                case JsonToken.NameSeparator:
                case JsonToken.ValueSeparator:
                    ReadNextCore(token, bytes, ref offset);
                    if (stack != 0) { goto AGAIN; }
                    break;
                case JsonToken.None:
                default:
                    break;
            }
        }

        /// <summary>
        /// Returned ArraySegment has the original array as underlying Array. Don't modify!!
        /// </summary>
        public static ArraySegment<byte> ReadNextBlockSegment(byte[] bytes, ref int offset)
        {
            var startOffset = offset;
            ReadNextBlock(bytes, ref offset);
            return new ArraySegment<byte>(bytes, startOffset, offset - startOffset);
        }

        public static sbyte ReadSByte(byte[] bytes, ref int offset)
        {
            return checked((sbyte)ReadInt64(bytes, ref offset));
        }

        public static short ReadInt16(byte[] bytes, ref int offset)
        {
            return checked((short)ReadInt64(bytes, ref offset));
        }

        public static int ReadInt32(byte[] bytes, ref int offset)
        {
            return checked((int)ReadInt64(bytes, ref offset));
        }

        public static long ReadInt64(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            int readCount;
            var v = NumberConverter.ReadInt64(bytes, offset, out readCount);
            if (readCount == 0)
            {
                throw CreateParsingException("Number Token", bytes, ref offset);
            }

            offset += readCount;
            return v;
        }

        public static byte ReadByte(byte[] bytes, ref int offset)
        {
            return checked((byte)ReadUInt64(bytes, ref offset));
        }

        public static ushort ReadUInt16(byte[] bytes, ref int offset)
        {
            return checked((ushort)ReadUInt64(bytes, ref offset));
        }

        public static uint ReadUInt32(byte[] bytes, ref int offset)
        {
            return checked((uint)ReadUInt64(bytes, ref offset));
        }

        public static ulong ReadUInt64(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            int readCount;
            var v = NumberConverter.ReadUInt64(bytes, offset, out readCount);
            if (readCount == 0)
            {
                throw CreateParsingException("Number Token", bytes, ref offset);
            }
            offset += readCount;
            return v;
        }

        public static Single ReadSingle(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            int readCount;
            var v = Internal.DoubleConversion.StringToDoubleConverter.ToSingle(bytes, offset, out readCount);
            if (readCount == 0)
            {
                throw CreateParsingException("Number Token", bytes, ref offset);
            }
            offset += readCount;
            return v;
        }

        public static Double ReadDouble(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            int readCount;
            var v = Internal.DoubleConversion.StringToDoubleConverter.ToDouble(bytes, offset, out readCount);
            if (readCount == 0)
            {
                throw CreateParsingException("Number Token", bytes, ref offset);
            }
            offset += readCount;
            return v;
        }

        /// <summary>
        /// Returned ArraySegment has the original array as underlying Array. Don't modify!!
        /// </summary>
        public static ArraySegment<byte> ReadNumberSegment(byte[] bytes, ref int offset)
        {
            SkipWhiteSpace(bytes, ref offset);

            var initialOffset = offset;
            for (int i = offset; i < bytes.Length; i++)
            {
                if (!NumberConverter.IsNumberRepresentation(bytes[i]))
                {
                    offset = i;
                    goto END;
                }
            }
            offset = bytes.Length;

        END:
            return new ArraySegment<byte>(bytes, initialOffset, offset - initialOffset);
        }

        public static ArraySegment<byte> GetEncodedPropertyName(string propertyName)
        {
            var buffer = new byte[StringEncoding.UTF8.GetMaxByteCount(propertyName.Length) + 2];
            int offset = 0;
            WritePropertyName(ref buffer, ref offset, propertyName);
            return new ArraySegment<byte>(buffer, 0, offset);
        }

        public static ArraySegment<byte> GetEncodedPropertyNameWithPrefixValueSeparator(string propertyName)
        {
            var buffer = new byte[StringEncoding.UTF8.GetMaxByteCount(propertyName.Length) + 2];
            int offset = 0;
            WriteValueSeparator(ref buffer, ref offset);
            WritePropertyName(ref buffer, ref offset, propertyName);
            return new ArraySegment<byte>(buffer, 0, offset);
        }

        public static ArraySegment<byte> GetEncodedPropertyNameWithBeginObject(string propertyName)
        {
            var buffer = new byte[StringEncoding.UTF8.GetMaxByteCount(propertyName.Length) + 2];
            int offset = 0;
            WriteBeginObject(ref buffer, ref offset);
            WritePropertyName(ref buffer, ref offset, propertyName);
            return new ArraySegment<byte>(buffer, 0, offset);
        }

        public static ArraySegment<byte> GetEncodedPropertyNameWithoutQuotation(string propertyName)
        {
            var buffer = new byte[StringEncoding.UTF8.GetMaxByteCount(propertyName.Length) + 2];
            int offset = 0;
            WriteString(ref buffer, ref offset, propertyName); // "propname"
            return new ArraySegment<byte>(buffer, 1, offset - 1); // without quotation
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw(ref byte[] buffer, ref int offset, byte rawValue)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = rawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRaw(ref byte[] buffer, ref int offset, byte[] rawValue)
        {
            UnsafeMemory.WriteRaw(ref buffer, ref offset, rawValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteRawUnsafe(ref byte[] buffer, ref int offset, byte rawValue)
        {
            buffer[offset++] = rawValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBeginArray(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)'[';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteEndArray(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)']';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBeginObject(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)'{';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteEndObject(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)'}';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteValueSeparator(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)',';
        }

        /// <summary>:</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteNameSeparator(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)':';
        }

        /// <summary>WriteString + WriteNameSeparator</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WritePropertyName(ref byte[] buffer, ref int offset, string propertyName)
        {
            WriteString(ref buffer, ref offset, propertyName);
            WriteNameSeparator(ref buffer, ref offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteQuotation(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 1);
            buffer[offset++] = (byte)'\"';
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteNull(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 4);
            buffer[offset + 0] = (byte)'n';
            buffer[offset + 1] = (byte)'u';
            buffer[offset + 2] = (byte)'l';
            buffer[offset + 3] = (byte)'l';
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBoolean(ref byte[] buffer, ref int offset, bool value)
        {
            if (value)
            {
                BinaryUtil.EnsureCapacity(ref buffer, offset, 4);
                buffer[offset + 0] = (byte)'t';
                buffer[offset + 1] = (byte)'r';
                buffer[offset + 2] = (byte)'u';
                buffer[offset + 3] = (byte)'e';
                offset += 4;
            }
            else
            {
                BinaryUtil.EnsureCapacity(ref buffer, offset, 5);
                buffer[offset + 0] = (byte)'f';
                buffer[offset + 1] = (byte)'a';
                buffer[offset + 2] = (byte)'l';
                buffer[offset + 3] = (byte)'s';
                buffer[offset + 4] = (byte)'e';
                offset += 5;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteTrue(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 4);
            buffer[offset + 0] = (byte)'t';
            buffer[offset + 1] = (byte)'r';
            buffer[offset + 2] = (byte)'u';
            buffer[offset + 3] = (byte)'e';
            offset += 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFalse(ref byte[] buffer, ref int offset)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, 5);
            buffer[offset + 0] = (byte)'f';
            buffer[offset + 1] = (byte)'a';
            buffer[offset + 2] = (byte)'l';
            buffer[offset + 3] = (byte)'s';
            buffer[offset + 4] = (byte)'e';
            offset += 5;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSingle(ref byte[] buffer, ref int offset, Single value)
        {
            offset += Internal.DoubleConversion.DoubleToStringConverter.GetBytes(ref buffer, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteDouble(ref byte[] buffer, ref int offset, double value)
        {
            offset += Internal.DoubleConversion.DoubleToStringConverter.GetBytes(ref buffer, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByte(ref byte[] buffer, ref int offset, byte value)
        {
            WriteUInt64(ref buffer, ref offset, (ulong)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt16(ref byte[] buffer, ref int offset, ushort value)
        {
            WriteUInt64(ref buffer, ref offset, (ulong)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteUInt32(ref byte[] buffer, ref int offset, uint value)
        {
            WriteUInt64(ref buffer, ref offset, (ulong)value);
        }

        public static void WriteUInt64(ref byte[] buffer, ref int offset, ulong value)
        {
            offset += NumberConverter.WriteUInt64(ref buffer, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteSByte(ref byte[] buffer, ref int offset, sbyte value)
        {
            WriteInt64(ref buffer, ref offset, (long)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16(ref byte[] buffer, ref int offset, short value)
        {
            WriteInt64(ref buffer, ref offset, (long)value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32(ref byte[] buffer, ref int offset, int value)
        {
            WriteInt64(ref buffer, ref offset, (long)value);
        }

        public static void WriteInt64(ref byte[] buffer, ref int offset, long value)
        {
            offset += NumberConverter.WriteInt64(ref buffer, offset, value);
        }

        public static void WriteString(ref byte[] buffer, ref int offset, string value)
        {
            if (value == null)
            {
                WriteNull(ref buffer, ref offset);
                return;
            }

            // single-path escape

            // nonescaped-ensure
            var startoffset = offset;
            var max = StringEncoding.UTF8.GetMaxByteCount(value.Length) + 2;
            BinaryUtil.EnsureCapacity(ref buffer, startoffset, max);

            var from = 0;
            var to = value.Length;

            buffer[offset++] = (byte)'\"';

            // for JIT Optimization, for-loop i < str.Length
            for (int i = 0; i < value.Length; i++)
            {
                byte escapeChar = default(byte);
                switch (value[i])
                {
                    case '"':
                        escapeChar = (byte)'"';
                        break;
                    case '\\':
                        escapeChar = (byte)'\\';
                        break;
                    case '\b':
                        escapeChar = (byte)'b';
                        break;
                    case '\f':
                        escapeChar = (byte)'f';
                        break;
                    case '\n':
                        escapeChar = (byte)'n';
                        break;
                    case '\r':
                        escapeChar = (byte)'r';
                        break;
                    case '\t':
                        escapeChar = (byte)'t';
                        break;
                    // use switch jumptable
                    case (char)0:
                    case (char)1:
                    case (char)2:
                    case (char)3:
                    case (char)4:
                    case (char)5:
                    case (char)6:
                    case (char)7:
                    case (char)11:
                    case (char)14:
                    case (char)15:
                    case (char)16:
                    case (char)17:
                    case (char)18:
                    case (char)19:
                    case (char)20:
                    case (char)21:
                    case (char)22:
                    case (char)23:
                    case (char)24:
                    case (char)25:
                    case (char)26:
                    case (char)27:
                    case (char)28:
                    case (char)29:
                    case (char)30:
                    case (char)31:
                    case (char)32:
                    case (char)33:
                    case (char)35:
                    case (char)36:
                    case (char)37:
                    case (char)38:
                    case (char)39:
                    case (char)40:
                    case (char)41:
                    case (char)42:
                    case (char)43:
                    case (char)44:
                    case (char)45:
                    case (char)46:
                    case (char)47:
                    case (char)48:
                    case (char)49:
                    case (char)50:
                    case (char)51:
                    case (char)52:
                    case (char)53:
                    case (char)54:
                    case (char)55:
                    case (char)56:
                    case (char)57:
                    case (char)58:
                    case (char)59:
                    case (char)60:
                    case (char)61:
                    case (char)62:
                    case (char)63:
                    case (char)64:
                    case (char)65:
                    case (char)66:
                    case (char)67:
                    case (char)68:
                    case (char)69:
                    case (char)70:
                    case (char)71:
                    case (char)72:
                    case (char)73:
                    case (char)74:
                    case (char)75:
                    case (char)76:
                    case (char)77:
                    case (char)78:
                    case (char)79:
                    case (char)80:
                    case (char)81:
                    case (char)82:
                    case (char)83:
                    case (char)84:
                    case (char)85:
                    case (char)86:
                    case (char)87:
                    case (char)88:
                    case (char)89:
                    case (char)90:
                    case (char)91:
                    default:
                        continue;
                }

                max += 2;
                BinaryUtil.EnsureCapacity(ref buffer, startoffset, max); // check +escape capacity

                offset += StringEncoding.UTF8.GetBytes(value, from, i - from, buffer, offset);
                from = i + 1;
                buffer[offset++] = (byte)'\\';
                buffer[offset++] = escapeChar;
            }

            if (from != value.Length)
            {
                offset += StringEncoding.UTF8.GetBytes(value, from, value.Length - from, buffer, offset);
            }

            buffer[offset++] = (byte)'\"';
        }

        private static ArraySegment<byte> ReadStringSegmentCore(byte[] bytes, ref int offset)
        {
            // SkipWhiteSpace is already called from IsNull

            byte[] builder = null;
            var builderOffset = 0;
            char[] codePointStringBuffer = null;
            var codePointStringOffet = 0;

            if (bytes[offset] != '\"') throw CreateParsingException("String Begin Token", bytes, ref offset);
            offset++;

            var from = offset;

            // eliminate array-bound check
            for (int i = offset; i < bytes.Length; i++)
            {
                byte escapeCharacter = 0;
                switch (bytes[i])
                {
                    case (byte)'\\': // escape character
                        switch ((char)bytes[i + 1])
                        {
                            case '"':
                            case '\\':
                            case '/':
                                escapeCharacter = bytes[i + 1];
                                goto COPY;
                            case 'b':
                                escapeCharacter = (byte)'\b';
                                goto COPY;
                            case 'f':
                                escapeCharacter = (byte)'\f';
                                goto COPY;
                            case 'n':
                                escapeCharacter = (byte)'\n';
                                goto COPY;
                            case 'r':
                                escapeCharacter = (byte)'\r';
                                goto COPY;
                            case 't':
                                escapeCharacter = (byte)'\t';
                                goto COPY;
                            case 'u':
                                if (codePointStringBuffer == null) codePointStringBuffer = StringBuilderCache.GetCodePointStringBuffer();

                                if (codePointStringOffet == 0)
                                {
                                    if (builder == null) builder = StringBuilderCache.GetBuffer();

                                    var copyCount = i - from;
                                    BinaryUtil.EnsureCapacity(ref builder, builderOffset, copyCount + 1); // require + 1
                                    Buffer.BlockCopy(bytes, from, builder, builderOffset, copyCount);
                                    builderOffset += copyCount;
                                }

                                if (codePointStringBuffer.Length == codePointStringOffet)
                                {
                                    Array.Resize(ref codePointStringBuffer, codePointStringBuffer.Length * 2);
                                }

                                var a = (char)bytes[i + 2];
                                var b = (char)bytes[i + 3];
                                var c = (char)bytes[i + 4];
                                var d = (char)bytes[i + 5];
                                var codepoint = GetCodePoint(a, b, c, d);
                                codePointStringBuffer[codePointStringOffet++] = (char)codepoint;
                                i += 5;
                                offset += 6;
                                from = offset;
                                continue;
                            default:
                                throw CreateParsingExceptionMessage("Bad JSON escape.", bytes, ref offset);
                        }
                    case (byte)'"': // endtoken
                        offset++;
                        goto END;
                    default: // string
                        if (codePointStringOffet != 0)
                        {
                            if (builder == null) builder = StringBuilderCache.GetBuffer();
                            BinaryUtil.EnsureCapacity(ref builder, builderOffset, StringEncoding.UTF8.GetMaxByteCount(codePointStringOffet));
                            builderOffset += StringEncoding.UTF8.GetBytes(codePointStringBuffer, 0, codePointStringOffet, builder, builderOffset);
                            codePointStringOffet = 0;
                        }
                        offset++;
                        continue;
                }

            COPY:
                {
                    if (builder == null) builder = StringBuilderCache.GetBuffer();
                    if (codePointStringOffet != 0)
                    {
                        BinaryUtil.EnsureCapacity(ref builder, builderOffset, StringEncoding.UTF8.GetMaxByteCount(codePointStringOffet));
                        builderOffset += StringEncoding.UTF8.GetBytes(codePointStringBuffer, 0, codePointStringOffet, builder, builderOffset);
                        codePointStringOffet = 0;
                    }

                    var copyCount = i - from;
                    BinaryUtil.EnsureCapacity(ref builder, builderOffset, copyCount + 1); // require + 1!
                    Buffer.BlockCopy(bytes, from, builder, builderOffset, copyCount);
                    builderOffset += copyCount;
                    builder[builderOffset++] = escapeCharacter;
                    i += 1;
                    offset += 2;
                    from = offset;
                }
            }

            throw CreateParsingException("String End Token", bytes, ref offset);

        END:
            if (builderOffset == 0 && codePointStringOffet == 0) // no escape
            {
                // offset -1 to skip last quote
                return new ArraySegment<byte>(bytes, from, offset - 1 - from);
            }
            else
            {
                if (builder == null) builder = StringBuilderCache.GetBuffer();
                if (codePointStringOffet != 0)
                {
                    BinaryUtil.EnsureCapacity(ref builder, builderOffset, StringEncoding.UTF8.GetMaxByteCount(codePointStringOffet));
                    builderOffset += StringEncoding.UTF8.GetBytes(codePointStringBuffer, 0, codePointStringOffet, builder, builderOffset);
                    codePointStringOffet = 0;
                }

                var copyCount = offset - from - 1;
                BinaryUtil.EnsureCapacity(ref builder, builderOffset, copyCount);
                Buffer.BlockCopy(bytes, from, builder, builderOffset, copyCount);
                builderOffset += copyCount;

                return new ArraySegment<byte>(builder, 0, builderOffset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReadNextCore(JsonToken token, byte[] bytes, ref int offset)
        {
            switch (token)
            {
                case JsonToken.BeginObject:
                case JsonToken.BeginArray:
                case JsonToken.ValueSeparator:
                case JsonToken.NameSeparator:
                case JsonToken.EndObject:
                case JsonToken.EndArray:
                    offset += 1;
                    break;
                case JsonToken.True:
                case JsonToken.Null:
                    offset += 4;
                    break;
                case JsonToken.False:
                    offset += 5;
                    break;
                case JsonToken.String:
                    offset += 1; // position is "\"";
                    for (int i = offset; i < bytes.Length; i++)
                    {
                        if (bytes[i] == (char)'\"')
                        {
                            // is escape?
                            if (bytes[i - 1] == (char)'\\')
                            {
                                continue;
                            }
                            else
                            {
                                offset = i + 1;
                                return; // end
                            }
                        }
                    }
                    throw CreateParsingExceptionMessage("not found end string.", bytes, ref offset);
                case JsonToken.Number:
                    for (int i = offset; i < bytes.Length; i++)
                    {
                        if (IsWordBreak(bytes[i]))
                        {
                            offset = i;
                            return;
                        }
                    }
                    offset = bytes.Length;
                    break;
                case JsonToken.None:
                default:
                    break;
            }
        }

        private static void ReadComment(byte[] bytes, ref int offset)
        {
            // current token is '/'
            if (bytes[offset + 1] == '/')
            {
                // single line
                offset += 2;
                for (int i = offset; i < bytes.Length; i++)
                {
                    if (bytes[i] == '\r' || bytes[i] == '\n')
                    {
                        offset = i;
                        return;
                    }
                }

                throw new JsonParsingException("Can not find end token of single line comment(\r or \n).");
            }
            else if (bytes[offset + 1] == '*')
            {

                offset += 2; // '/' + '*';
                for (int i = offset; i < bytes.Length; i++)
                {
                    if (bytes[i] == '*' && bytes[i + 1] == '/')
                    {
                        offset = i + 1;
                        return;
                    }
                }
                throw new JsonParsingException("Can not find end token of multi line comment(*/).");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetCodePoint(char a, char b, char c, char d)
        {
            return (((((ToNumber(a) * 16) + ToNumber(b)) * 16) + ToNumber(c)) * 16) + ToNumber(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int ToNumber(char x)
        {
            if ('0' <= x && x <= '9')
            {
                return x - '0';
            }
            else if ('a' <= x && x <= 'f')
            {
                return x - 'a' + 10;
            }
            else if ('A' <= x && x <= 'F')
            {
                return x - 'A' + 10;
            }
            throw new JsonParsingException("Invalid Character" + x);
        }

        private static bool IsWordBreak(byte c)
        {
            switch (c)
            {
                case (byte)' ':
                case (byte)'{':
                case (byte)'}':
                case (byte)'[':
                case (byte)']':
                case (byte)',':
                case (byte)':':
                case (byte)'\"':
                    return true;
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 33:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                case 41:
                case 42:
                case 43:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                case 81:
                case 82:
                case 83:
                case 84:
                case 85:
                case 86:
                case 87:
                case 88:
                case 89:
                case 90:
                case 92:
                case 94:
                case 95:
                case 96:
                case 97:
                case 98:
                case 99:
                case 100:
                case 101:
                case 102:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 108:
                case 109:
                case 110:
                case 111:
                case 112:
                case 113:
                case 114:
                case 115:
                case 116:
                case 117:
                case 118:
                case 119:
                case 120:
                case 121:
                case 122:
                default:
                    return false;
            }
        }

        private static JsonParsingException CreateParsingException(string expected, byte[] bytes, ref int offset)
        {
            var actual = ((char)bytes[offset]).ToString();
            var pos = offset;

            try
            {
                var token = GetCurrentJsonToken(bytes, ref offset);
                switch (token)
                {
                    case JsonToken.Number:
                        actual = StringEncoding.UTF8.GetString(ReadNumberSegment(bytes, ref offset));
                        break;
                    case JsonToken.String:
                        actual = "\"" + StringEncoding.UTF8.GetString(ReadStringSegment(bytes, ref offset)) + "\"";
                        break;
                    case JsonToken.True:
                        actual = "true";
                        break;
                    case JsonToken.False:
                        actual = "false";
                        break;
                    case JsonToken.Null:
                        actual = "null";
                        break;
                    default:
                        break;
                }
            }
            catch { }

            return new JsonParsingException("expected:'" + expected + "', actual:'" + actual + "', at offset:" + pos, bytes, pos, offset, actual);
        }

        private static JsonParsingException CreateParsingExceptionMessage(string message, byte[] bytes, ref int offset)
        {
            var actual = ((char)bytes[offset]).ToString();
            var pos = offset;

            return new JsonParsingException(message, bytes, pos, pos, actual);
        }

        internal static class StringBuilderCache
        {
            [ThreadStatic]
            private static byte[] buffer;

            [ThreadStatic]
            private static char[] codePointStringBuffer;

            public static byte[] GetBuffer()
            {
                if (buffer == null)
                {
                    buffer = new byte[65535];
                }
                return buffer;
            }

            public static char[] GetCodePointStringBuffer()
            {
                if (codePointStringBuffer == null)
                {
                    codePointStringBuffer = new char[65535];
                }
                return codePointStringBuffer;
            }
        }
    }

    public class JsonParsingException : Exception
    {
        private WeakReference underyingBytes;
        private int limit;
        public int Offset { get; private set; }
        public string ActualChar { get; set; }

        public JsonParsingException(string message)
            : base(message)
        {

        }

        public JsonParsingException(string message, byte[] underlyingBytes, int offset, int limit, string actualChar)
            : base(message)
        {
            this.underyingBytes = new WeakReference(underlyingBytes);
            this.Offset = offset;
            this.ActualChar = actualChar;
            this.limit = limit;
        }

        /// <summary>
        /// Underlying bytes is may be a pooling buffer, be careful to use it. If lost reference or can not handled byte[], return null.
        /// </summary>
        public byte[] GetUnderlyingByteArrayUnsafe()
        {
            return underyingBytes.Target as byte[];
        }

        /// <summary>
        /// Underlying bytes is may be a pooling buffer, be careful to use it. If lost reference or can not handled byte[], return null.
        /// </summary>
        public string GetUnderlyingStringUnsafe()
        {
            var bytes = underyingBytes.Target as byte[];
            if (bytes != null)
            {
                return StringEncoding.UTF8.GetString(bytes, 0, limit) + "...";
            }
            return null;
        }
    }
}
