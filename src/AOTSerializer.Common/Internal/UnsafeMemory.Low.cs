#if NETSTANDARD || NETFRAMEWORK

using System;
using System.Runtime.CompilerServices;

namespace AOTSerializer.Internal
{
    // for string key property name write optimization.

    public static class UnsafeMemory
    {
        public static readonly bool Is32Bit = (IntPtr.Size == 4);

        public static void WriteRaw(ref byte[] buffer, ref int offset, byte[] src)
        {
            switch (src.Length)
            {
                case 0: break;
                case 1: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw1(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw1(ref buffer, offset, src); } break;
                case 2: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw2(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw2(ref buffer, offset, src); } break;
                case 3: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw3(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw3(ref buffer, offset, src); } break;
                case 4: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw4(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw4(ref buffer, offset, src); } break;
                case 5: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw5(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw5(ref buffer, offset, src); } break;
                case 6: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw6(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw6(ref buffer, offset, src); } break;
                case 7: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw7(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw7(ref buffer, offset, src); } break;
                case 8: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw8(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw8(ref buffer, offset, src); } break;
                case 9: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw9(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw9(ref buffer, offset, src); } break;
                case 10: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw10(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw10(ref buffer, offset, src); } break;
                case 11: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw11(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw11(ref buffer, offset, src); } break;
                case 12: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw12(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw12(ref buffer, offset, src); } break;
                case 13: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw13(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw13(ref buffer, offset, src); } break;
                case 14: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw14(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw14(ref buffer, offset, src); } break;
                case 15: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw15(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw15(ref buffer, offset, src); } break;
                case 16: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw16(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw16(ref buffer, offset, src); } break;
                case 17: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw17(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw17(ref buffer, offset, src); } break;
                case 18: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw18(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw18(ref buffer, offset, src); } break;
                case 19: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw19(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw19(ref buffer, offset, src); } break;
                case 20: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw20(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw20(ref buffer, offset, src); } break;
                case 21: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw21(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw21(ref buffer, offset, src); } break;
                case 22: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw22(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw22(ref buffer, offset, src); } break;
                case 23: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw23(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw23(ref buffer, offset, src); } break;
                case 24: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw24(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw24(ref buffer, offset, src); } break;
                case 25: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw25(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw25(ref buffer, offset, src); } break;
                case 26: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw26(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw26(ref buffer, offset, src); } break;
                case 27: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw27(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw27(ref buffer, offset, src); } break;
                case 28: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw28(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw28(ref buffer, offset, src); } break;
                case 29: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw29(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw29(ref buffer, offset, src); } break;
                case 30: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw30(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw30(ref buffer, offset, src); } break;
                case 31: if (Is32Bit) { offset += UnsafeMemory32.WriteRaw31(ref buffer, offset, src); } else { offset += UnsafeMemory64.WriteRaw31(ref buffer, offset, src); } break;
                default:
                    MemoryCopy(ref buffer, ref offset, src);
                    break;
            }
        }

        public static unsafe void MemoryCopy(ref byte[] buffer, ref int offset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref buffer, offset, src.Length);
#if !NET45
            fixed (void* dstP = &buffer[offset])
            fixed (void* srcP = &src[0])
            {
                Buffer.MemoryCopy(srcP, dstP, buffer.Length - offset, src.Length);
            }
#else
            Buffer.BlockCopy(src, 0, buffer, offset, src.Length);
#endif
            offset += src.Length;
        }
    }

    public static partial class UnsafeMemory32
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw1(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(byte*)pDst = *(byte*)pSrc;
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw2(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(short*)pDst = *(short*)pSrc;
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw3(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(byte*)pDst = *(byte*)pSrc;
                *(short*)(pDst + 1) = *(short*)(pSrc + 1);
            }

            return src.Length;
        }
    }

    public static partial class UnsafeMemory64
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw1(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(byte*)pDst = *(byte*)pSrc;
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw2(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(short*)pDst = *(short*)pSrc;
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw3(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(byte*)pDst = *(byte*)pSrc;
                *(short*)(pDst + 1) = *(short*)(pSrc + 1);
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw4(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(int*)(pDst + 0) = *(int*)(pSrc + 0);
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw5(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(int*)(pDst + 0) = *(int*)(pSrc + 0);
                *(int*)(pDst + 1) = *(int*)(pSrc + 1);
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw6(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(int*)(pDst + 0) = *(int*)(pSrc + 0);
                *(int*)(pDst + 2) = *(int*)(pSrc + 2);
            }

            return src.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteRaw7(ref byte[] dst, int dstOffset, byte[] src)
        {
            BinaryUtil.EnsureCapacity(ref dst, dstOffset, src.Length);

            fixed (byte* pSrc = &src[0])
            fixed (byte* pDst = &dst[dstOffset])
            {
                *(int*)(pDst + 0) = *(int*)(pSrc + 0);
                *(int*)(pDst + 3) = *(int*)(pSrc + 3);
            }

            return src.Length;
        }
    }
}

#endif