// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventPublisher.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventPublisher interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System.Collections.Generic;

    /// <summary>
    ///     The EventPublisher interface.
    /// </summary>
    public interface IEventPublisher
    {
        #region Public Methods and Operators

        /// <summary>
        /// The publish.
        /// </summary>
        /// <param name="events">
        /// The events.
        /// </param>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <param name="eventHeaders">
        /// The event headers.
        /// </param>
        void Publish(
            IEnumerable<object> events, 
            IDictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders);

        #endregion
    }
}