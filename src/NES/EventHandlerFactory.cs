using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NES
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(EventHandlerFactory));
        private static readonly Dictionary<Type, Dictionary<Type, Action<object, object>>> _cache = new Dictionary<Type, Dictionary<Type, Action<object, object>>>();
        private static readonly object _cacheLock = new object();

        public Action<object> Get(object aggregate, Type eventType)
        {
            var aggregateType = aggregate.GetType();

            lock (_cacheLock)
            {
                Dictionary<Type, Action<object, object>> handlers;
                Action<object, object> handler;

                if (!_cache.TryGetValue(aggregateType, out handlers) || !handlers.TryGetValue(eventType, out handler))
                {
                    Logger.Debug(string.Format("cache does not contain the aggregateType {0}", aggregateType.FullName));

                    var handlerMethodInfo = aggregateType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { eventType }, null);

                    if (handlerMethodInfo != null)
                    {
                        var aggregateParameter = Expression.Parameter(typeof(object), "aggregate");
                        var eventParameter = Expression.Parameter(typeof(object), "event");
                        var eventInterfaceType = handlerMethodInfo.GetParameters().Single().ParameterType;

                        var handlerCall = Expression.Call(
                            Expression.Convert(aggregateParameter, aggregateType), 
                            handlerMethodInfo, 
                            Expression.Convert(eventParameter, eventInterfaceType));
                        
                        handler = Expression.Lambda<Action<object, object>>(handlerCall, aggregateParameter, eventParameter).Compile();
                    }
                    else
                    {
                        Logger.Warn(string.Format("Aggregate {0} has not Handle methods", aggregateType.FullName));
                        handler = (a, e) => {};
                    }

                    if (handlers == null)
                    {
                        _cache[aggregateType] = new Dictionary<Type, Action<object, object>>();
                    }

                    _cache[aggregateType][eventType] = handler;
                }

                return e => handler(aggregate, e);
            }
        }
    }
}