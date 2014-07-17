// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSourceFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event source factory.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The event source factory.
    /// </summary>
    public class EventSourceFactory : IEventSourceFactory
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The create.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the event to create
        /// </typeparam>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public T Create<T>() where T : IEventSource
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }

        #endregion
    }
}