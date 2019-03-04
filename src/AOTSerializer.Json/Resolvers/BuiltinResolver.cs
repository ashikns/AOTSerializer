using AOTSerializer.Common;
using System;

namespace AOTSerializer.Json.Resolvers
{
    public sealed class BuiltinResolver : CachedResolver
    {
        public static readonly IResolver Instance = new BuiltinResolver();

        private BuiltinResolver()
        {
        }

        protected override IFormatter FindFormatter(Type t)
        {
            FormatterMap.ConcreteFormatterMap.TryGetValue(t, out var formatter);
            return formatter;
        }
    }
}