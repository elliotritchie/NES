using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NES
{
    public class EventHandlerFactory : IEventHandlerFactory
    {
        private static readonly Dictionary<Type, Dictionary<Type, Action<object, object>>> _cache = new Dictionary<Type, Dictionary<Type, Action<object, object>>>();
        private static readonly object _cacheLock = new object();

        public Action Get<TAggregate, TEvent>(TAggregate aggregate, TEvent @event) 
            where TAggregate : AggregateBase<TEvent> 
            where TEvent : class
        {
            var aggregateType = aggregate.GetType();
            var eventType = @event.GetType();

            lock (_cacheLock)
            {
                Dictionary<Type, Action<object, object>> handlers;
                Action<object, object> handler;

                if (!_cache.TryGetValue(aggregateType, out handlers) || !handlers.TryGetValue(eventType, out handler))
                {
                    var methodInfo = aggregateType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { eventType }, null);

                    if (methodInfo != null)
                    {
                        var parameterInfo = methodInfo.GetParameters().Single();
                        var instance = Expression.Parameter(typeof(TAggregate), "instance");
                        var argument = Expression.Parameter(typeof(TEvent), "argument");
                        var methodCall = Expression.Call(Expression.Convert(instance, aggregateType), methodInfo, Expression.Convert(argument, parameterInfo.ParameterType));
                        var @delegate = Expression.Lambda<Action<TAggregate, TEvent>>(methodCall, instance, argument).Compile();
                        
                        handler = (a, e) => { @delegate((TAggregate)a, (TEvent)e); };
                    }
                    else
                    {
                        handler = (a, e) => {};
                    }

                    if (handlers == null)
                    {
                        _cache[aggregateType] = new Dictionary<Type, Action<object, object>>();
                    }

                    _cache[aggregateType][eventType] = handler;
                }

                return () => handler(aggregate, @event);
            }
        }
    }
}