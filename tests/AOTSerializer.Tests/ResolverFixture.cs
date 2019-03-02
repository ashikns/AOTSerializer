using AOTSerializer.Common;
using AOTSerializer.Resolvers;

namespace AOTSerializer.Tests
{
    public class ResolverFixture
    {
        public IResolver Resolver { get; }

        public ResolverFixture()
        {
            Resolver = new CompositeResolver(new[] { Json.Resolvers.BuiltinResolver.Instance }, null);
            //Resolver = new CompositeResolver(new[] { MessagePack.Resolvers.BuiltinResolver.Instance }, null);

            Serializer.DefaultResolver = Resolver;
        }
    }
}
