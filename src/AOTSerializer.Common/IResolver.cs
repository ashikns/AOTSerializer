using System;
using System.Collections.Concurrent;

namespace AOTSerializer.Common
{
    public interface IResolver
    {
        IFormatter GetFormatter(Type t);
        IFormatter<T> GetFormatter<T>();
    }

    public abstract class CachedResolver : IResolver
    {
        protected ConcurrentDictionary<Type, IFormatter> FormatterCache { get; private set; }

        protected CachedResolver()
        {
            FormatterCache = new ConcurrentDictionary<Type, IFormatter>();
        }

        public IFormatter<T> GetFormatter<T>()
        {
            var formatter = GetFormatter(typeof(T));
            if (formatter != null)
            {
                if (formatter is IFormatter<T> f)
                {
                    return f;
                }
                else
                {
                    throw new Exception($"Got non null formatter but could not cast it to {typeof(IFormatter<T>).Name}");
                }
            }

            return null;
        }

        public IFormatter GetFormatter(Type t)
        {
            if (!FormatterCache.TryGetValue(t, out var formatter))
            {
                formatter = FindFormatter(t);
                FormatterCache.TryAdd(t, formatter);
            }

            return formatter;
        }

        protected abstract IFormatter FindFormatter(Type t);
    }

    public class FormatterNotRegisteredException : Exception
    {
        public FormatterNotRegisteredException(string message) : base(message)
        {
        }
    }
}
