using AOTSerializer.Common;
using System;
using System.Reflection;

namespace AOTSerializer.Resolvers
{
    public sealed class CompositeResolver : CachedResolver
    {
        private readonly IResolver[] _resolvers;
        private readonly IFormatter[] _formatters;

        public CompositeResolver(IResolver[] resolvers, IFormatter[] formatters)
        {
            _resolvers = resolvers;
            _formatters = formatters;
        }

        protected override IFormatter FindFormatter(Type t)
        {
            foreach (var item in _formatters)
            {
                foreach (var implInterface in item.GetType().GetTypeInfo().ImplementedInterfaces)
                {
                    var ti = implInterface.GetTypeInfo();
                    if (ti.IsGenericType && ti.GenericTypeArguments[0] == t)
                    {
                        return item;
                    }
                }
            }

            foreach (var item in _resolvers)
            {
                var formatter = item.GetFormatter(t);
                if (formatter != null) { return formatter; }
            }

            return null;
        }
    }
}
