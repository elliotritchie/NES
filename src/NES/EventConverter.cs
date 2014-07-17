// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConverter.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    /// The event converter.
    /// </summary>
    /// <typeparam name="TFrom">
    /// The from type
    /// </typeparam>
    /// <typeparam name="TTo">
    /// The to type
    /// </typeparam>
    public abstract class EventConverter<TFrom, TTo>
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the event factory.
        /// </summary>
        public IEventFactory EventFactory { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// The <see cref="TTo"/>.
        /// </returns>
        public abstract TTo Convert(TFrom @event);

        #endregion
    }
}