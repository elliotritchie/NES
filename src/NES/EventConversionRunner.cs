// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConversionRunner.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event conversion runner.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The event conversion runner.
    /// </summary>
    public class EventConversionRunner : IEventConversionRunner
    {
        #region Static Fields

        private static readonly Dictionary<Type, Type> _cache = new Dictionary<Type, Type>();

        private static readonly object _cacheLock = new object();

        #endregion

        #region Fields

        private readonly IEventConverterFactory _eventConverterFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventConversionRunner"/> class.
        /// </summary>
        /// <param name="eventConverterFactory">
        /// The event converter factory.
        /// </param>
        public EventConversionRunner(IEventConverterFactory eventConverterFactory)
        {
            this._eventConverterFactory = eventConverterFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Run(object @event)
        {
            var converter = this._eventConverterFactory.Get(this.GetInterfaceType(@event.GetType()));
            return converter != null ? this.Run(converter(@event)) : @event;
        }

        #endregion

        #region Methods

        private Type GetInterfaceType(Type type)
        {
            lock (_cacheLock)
            {
                Type interfaceType;

                if (!_cache.TryGetValue(type, out interfaceType))
                {
                    _cache[type] =
                        interfaceType =
                        type.FindInterfaces((t, o) => ((Type[])o).All(c => t == c || !t.IsAssignableFrom(c)), type.GetInterfaces()).Single();
                }

                return interfaceType;
            }
        }

        #endregion
    }
}