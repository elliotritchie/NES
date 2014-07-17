// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConverterFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event converter factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     The event converter factory.
    /// </summary>
    public class EventConverterFactory : IEventConverterFactory
    {
        #region Static Fields

        private static readonly Dictionary<Type, Func<object, object>> _cache = new Dictionary<Type, Func<object, object>>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="EventConverterFactory" /> class.
        /// </summary>
        static EventConverterFactory()
        {
            foreach (var type in Global.TypesToScan)
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    continue;
                }

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

                var eventConverter = Expression.Lambda<Func<object>>(eventConverterMemberInit).Compile().Invoke();
                var eventConverterParameter = Expression.Constant(eventConverter);
                var eventParameter = Expression.Parameter(typeof(object), "event");

                var eventConverterCall = Expression.Call(
                    Expression.Convert(eventConverterParameter, type), 
                    type.GetMethod("Convert"), 
                    Expression.Convert(eventParameter, fromType));

                _cache[fromType] = Expression.Lambda<Func<object, object>>(eventConverterCall, eventParameter).Compile();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <returns>
        /// The <see cref="Func"/>.
        /// </returns>
        public Func<object, object> Get(Type eventType)
        {
            return _cache.ContainsKey(eventType) ? _cache[eventType] : null;
        }

        #endregion
    }
}