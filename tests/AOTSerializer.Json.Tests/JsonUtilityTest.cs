using AOTSerializer.Tests;
using System.Linq;
using System.Text;
using Xunit;

namespace AOTSerializer.Json.Tests
{
    public class JsonUtilityTest : IClassFixture<ResolverFixture>
    {
        [Fact]
        public void LargeString()
        {
            var origstr = new string('a', 99999);
            var str = "\\u0313" + origstr;
            str = "\"" + str + "\"";

            int offset = 0;
            var aaa = JsonUtility.ReadString(Encoding.UTF8.GetBytes(str), ref offset);

            aaa.Is("\u0313" + origstr);
        }


        [Fact]
        public void LargeString2()
        {
            var origstr = new string('a', 99999);
            var str = "\"" + origstr + "\"";

            int offset = 0;
            var aaa = JsonUtility.ReadString(Encoding.UTF8.GetBytes(str), ref offset);

            aaa.Is(origstr);
        }

        [Fact]
        public void LargeString3()
        {
            var origstr = new string('a', 999999);
            var str = "\"" + origstr + "\"";

            int offset = 0;
            var aaa = JsonUtility.ReadString(Encoding.UTF8.GetBytes(str), ref offset);

            aaa.Is(origstr);
        }

        [Fact]
        public void LargeString4()
        {
            var origstr = new string('a', 999999);
            var str = "\"" + origstr + "\"";

            var serialized = Serializer.Serialize(str);
            var deserialized = Serializer.Deserialize<string>(serialized);

            deserialized.Is(str);
        }

        [Fact]
        public void LargeArray()
        {
            var array = Enumerable.Range(1, 100000).ToArray();
            var bin = Serializer.Serialize(array);

            int offset = 0;
            JsonUtility.ReadNextBlock(bin, ref offset);

            // ok, can read.
            offset.Is(bin.Length);
        }
    }
}
