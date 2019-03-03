using AOTSerializer.Internal;
using AOTSerializer.Json.Formatters;
using System;
using System.Text;
using Xunit;

namespace AOTSerializer.Tests
{
    public class NewGuidFormatterTest : IClassFixture<ResolverFixture>
    {
        // GuidBits is internal...

        [Fact]
        public void GuidBitsTest()
        {
            var original = Guid.NewGuid();

            var patternA = Encoding.UTF8.GetBytes(original.ToString().ToUpper());
            var patternB = Encoding.UTF8.GetBytes(original.ToString().ToLower());
            var patternC = Encoding.UTF8.GetBytes(original.ToString().ToUpper().Replace("-", ""));
            var patternD = Encoding.UTF8.GetBytes(original.ToString().ToLower().Replace("-", ""));

            new GuidBits(new ArraySegment<byte>(patternA, 0, patternA.Length)).Value.Is(original);
            new GuidBits(new ArraySegment<byte>(patternB, 0, patternB.Length)).Value.Is(original);
            new GuidBits(new ArraySegment<byte>(patternC, 0, patternC.Length)).Value.Is(original);
            new GuidBits(new ArraySegment<byte>(patternD, 0, patternD.Length)).Value.Is(original);
        }

        [Fact]
        public void FastGuid()
        {
            var original = Guid.NewGuid();
            byte[] bytes = null;
            int writeOff = 0;
            GuidFormatter.Default.Serialize(ref bytes, ref writeOff, original, null);
            Assert.True(writeOff == 38);

            int readOff = 0;
            GuidFormatter.Default.Deserialize(bytes, ref readOff, null).Is(original);
            Assert.True(readOff == 38);
        }
    }
}
