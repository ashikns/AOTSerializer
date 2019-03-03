using System;
using Xunit;

namespace AOTSerializer.Tests
{
    public class TypeFormatterTest : IClassFixture<ResolverFixture>
    {
        [Fact]
        public void TypeTest()
        {
            var bin = Serializer.Serialize(typeof(int));
            var t = Serializer.Deserialize<Type>(bin);
            (t == typeof(int)).IsTrue();
        }
    }
}
