using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NES
{
    public class EventHandlerFactory<T> : IEventHandlerFactory<T>
    {
        private static readonly Dictionary<Type, Dictionary<Type, Action<AggregateBase<T>, T>>> _cache = new Dictionary<Type, Dictionary<Type, Action<AggregateBase<T>, T>>>();
        private static readonly object _cacheLock = new object();

        public Action<AggregateBase<T>, T> GetHandler(Type aggregateType, Type eventType)
        {
            lock (_cacheLock)
            {
                Dictionary<Type, Action<AggregateBase<T>, T>> handlers;
                Action<AggregateBase<T>, T> handler;

                if (!_cache.TryGetValue(aggregateType, out handlers) || !handlers.TryGetValue(eventType, out handler))
                {
                    var methodInfo = aggregateType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { eventType }, null);

                    if (methodInfo != null)
                    {
                        var parameterInfo = methodInfo.GetParameters().Single();
                        var instance = Expression.Parameter(typeof(AggregateBase<T>), "instance");
                        var argument = Expression.Parameter(typeof(T), "argument");
                        var methodCall = Expression.Call(Expression.Convert(instance, aggregateType), methodInfo, Expression.Convert(argument, parameterInfo.ParameterType));

                        handler = Expression.Lambda<Action<AggregateBase<T>, T>>(methodCall, instance, argument).Compile();
                    }
                    else
                    {
                        handler = (a, e) => {};
                    }

                    if (handlers == null)
                    {
                        _cache[aggregateType] = new Dictionary<Type, Action<AggregateBase<T>, T>>();
                    }

                    _cache[aggregateType][eventType] = handler;
                }

                return handler;
            }
        }
    }
}