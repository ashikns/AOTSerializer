using AOTSerializer.Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace AOTSerializer.Tests
{
    public class CollectionTest : IClassFixture<ResolverFixture>
    {
        private IResolver Resolver { get; }

        public CollectionTest(ResolverFixture fixture)
        {
            Resolver = fixture.Resolver;
        }

        private T Convert<T>(T value)
        {
            return Serializer.Deserialize<T>(Serializer.Serialize(value, Resolver), Resolver);
        }

        public static object[][] collectionTestData = new object[][]
        {
            new object[]{ new int[]{ 1,10, 100 } , null },
            new object[]{ new List<int>{ 1,10, 100 } , null },
        };

        [Theory]
        [MemberData(nameof(collectionTestData))]
        public void ConcreteCollectionTest<T>(T x, T y)
        {
            Convert(x).IsStructuralEqual(x);
            Convert(y).IsStructuralEqual(y);
        }

        [Fact]
        public void ArraySegmentTest()
        {
            var test = new ArraySegment<byte>(new byte[] { 1, 10, 100 });
            Convert(test).Is(test);
            ArraySegment<byte>? nullableTest = new ArraySegment<byte>(new byte[] { 1, 10, 100 });
            Convert(nullableTest).Is(nullableTest);
            nullableTest = null;
            Convert(nullableTest).IsNull();
        }
    }
}