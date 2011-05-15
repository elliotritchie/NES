using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NES
{
    public class EventConverterFactory : IEventConverterFactory
    {
        private static readonly Dictionary<Type, Func<object, object>> _cache = new Dictionary<Type, Func<object, object>>();

        static EventConverterFactory()
        {
            foreach (var fileInfo in new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).GetFiles("*.dll", SearchOption.AllDirectories))
            foreach (var type in Assembly.LoadFrom(fileInfo.FullName).GetTypes().Where(t => t.IsClass && !t.IsAbstract))
            {
                var baseType = type.BaseType;

                if (baseType == null || !baseType.IsGenericType)
                {
                    continue;
                }

                var eventTypes = baseType.GetGenericArguments();

                if (eventTypes.Length != 2)
                {
                    continue;
                }

                var fromType = eventTypes[0];
                var toType = eventTypes[1];
                var eventConverterType = typeof(EventConverter<,>).MakeGenericType(fromType, toType);

                if (!eventConverterType.IsAssignableFrom(type))
                {
                    continue;
                }

                var eventFactory = DI.Current.Resolve<IEventFactory>();
                var eventFactoryMemberInfo = type.GetProperty("EventFactory");
                var eventConverterMemberInit = Expression.MemberInit(
                    Expression.New(type), 
                    Expression.Bind(eventFactoryMemberInfo, Expression.Constant(eventFactory)));
                var eventConverterMemberInitDelegate = Expression.Lambda<Func<object>>(eventConverterMemberInit).Compile();
                var eventConverter = eventConverterMemberInitDelegate.Invoke();

                var eventConverterParameter = Expression.Parameter(typeof(object), "eventConverter");
                var eventParameter = Expression.Parameter(typeof(object), "event");
                var eventConverterCall = Expression.Call(
                    Expression.Convert(eventConverterParameter, type),
                    type.GetMethod("Convert"),
                    Expression.Convert(eventParameter, fromType));
                var eventConverterCallDelegate = Expression.Lambda<Func<object, object, object>>(eventConverterCall, eventConverterParameter, eventParameter).Compile();

                _cache[fromType] = e => { return eventConverterCallDelegate(eventConverter, e); };
            }
        }

        public Func<object, object> Get(Type @eventType)
        {
            return _cache.ContainsKey(@eventType) ? _cache[@eventType] : null;
        }
    }
}