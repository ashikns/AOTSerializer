using System;
using System.Text;

namespace AOTSerializer.Internal
{
    public static class StringEncoding
    {
        public static readonly Encoding UTF8 = new UTF8Encoding(false);

        public static string GetString(this Encoding encoding, ArraySegment<byte> data)
        {
            return encoding.GetString(data.Array, data.Offset, data.Count);
        }
    }
}
