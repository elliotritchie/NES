// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventConverterFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventConverterFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The EventConverterFactory interface.
    /// </summary>
    public interface IEventConverterFactory
    {
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
        Func<object, object> Get(Type @eventType);

        #endregion
    }
}