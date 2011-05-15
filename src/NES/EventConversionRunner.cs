using System;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public class EventConversionRunner : IEventConversionRunner
    {
        private static readonly Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();
        private static readonly object _cacheLock = new object();
        private readonly IEventConverterFactory _eventConverterFactory;

        public EventConversionRunner(IEventConverterFactory eventConverterFactory)
        {
            _eventConverterFactory = eventConverterFactory;
        }

        public IEnumerable<object> Run(IEnumerable<object> events)
        {
            return events.Select(Run);
        }

        private object Run(object @event)
        {
            var @delegate = _eventConverterFactory.Get(GetInterfaceType(@event.GetType()));
            return @delegate != null ? Run(@delegate(@event)) : @event;
        }

        private Type GetInterfaceType(Type type)
        {
            lock (_cacheLock)
            {
                Type interfaceType;

                if (!_cache.TryGetValue(type, out interfaceType))
                {
                    _cache[type] = interfaceType = type.FindInterfaces((t, o) => ((Type[])o).All(c => t == c || !t.IsAssignableFrom(c)), type.GetInterfaces()).Single();
                }

                return interfaceType;
            }
        }
    }
}