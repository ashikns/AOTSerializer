using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;

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

    public static class FormatterResolverExtensions
    {
        public static IFormatter GetFormatterWithVerify(this IResolver resolver, Type t)
        {
            IFormatter formatter;
            try
            {
                formatter = resolver.GetFormatter(t);
            }
            catch (TypeInitializationException ex)
            {
                // The fact that we're using static constructors to initialize this is an internal detail.
                // Rethrow the inner exception if there is one.
                // Do it carefully so as to not stomp on the original callstack.
                ExceptionDispatchInfo.Capture(ex.InnerException ?? ex).Throw();
                throw new InvalidOperationException("Unreachable"); // keep the compiler happy
            }

            if (formatter == null)
            {
                throw new FormatterNotRegisteredException(t.FullName + " is not registered in this resolver. resolver:" + resolver.GetType().Name);
            }

            return formatter;
        }

        public static IFormatter<T> GetFormatterWithVerify<T>(this IResolver resolver)
        {
            IFormatter<T> formatter;
            try
            {
                formatter = resolver.GetFormatter<T>();
            }
            catch (TypeInitializationException ex)
            {
                // The fact that we're using static constructors to initialize this is an internal detail.
                // Rethrow the inner exception if there is one.
                // Do it carefully so as to not stomp on the original callstack.
                ExceptionDispatchInfo.Capture(ex.InnerException ?? ex).Throw();
                throw new InvalidOperationException("Unreachable"); // keep the compiler happy
            }

            if (formatter == null)
            {
                throw new FormatterNotRegisteredException(typeof(T).FullName + " is not registered in this resolver. resolver:" + resolver.GetType().Name);
            }

            return formatter;
        }
    }
}
