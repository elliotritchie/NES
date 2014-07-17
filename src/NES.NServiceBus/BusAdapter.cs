// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BusAdapter.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The bus adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using System.Collections.Generic;

    using global::NServiceBus;

    /// <summary>
    ///     The bus adapter.
    /// </summary>
    public class BusAdapter : IEventPublisher
    {
        #region Fields

        private readonly IBus _bus;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BusAdapter"/> class.
        /// </summary>
        /// <param name="bus">
        /// The bus.
        /// </param>
        public BusAdapter(IBus bus)
        {
            this._bus = bus;
        }

        #endregion

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
        public void Publish(
            IEnumerable<object> events, 
            IDictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            foreach (var header in headers)
            {
                this._bus.OutgoingHeaders[header.Key] = header.Value != null ? header.Value.ToString() : null;
            }

            foreach (var @event in events)
            {
                foreach (var header in eventHeaders[@event])
                {
                    this._bus.SetMessageHeader(@event, header.Key, header.Value.ToString());
                }

                this._bus.Publish(@event);
            }
        }

        #endregion
    }
}