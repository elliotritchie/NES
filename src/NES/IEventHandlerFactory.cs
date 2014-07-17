// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventHandlerFactory.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventHandlerFactory interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The EventHandlerFactory interface.
    /// </summary>
    public interface IEventHandlerFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="aggregate">
        /// The aggregate.
        /// </param>
        /// <param name="eventType">
        /// The event type.
        /// </param>
        /// <returns>
        /// The <see cref="Action"/>.
        /// </returns>
        Action<object> Get(object aggregate, Type @eventType);

        #endregion
    }
}