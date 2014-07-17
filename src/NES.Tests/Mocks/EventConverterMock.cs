// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConverterMock.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event converter mock.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests.Mocks
{
    using NES.Tests.Stubs;

    /// <summary>
    ///     The event converter mock.
    /// </summary>
    public class EventConverterMock : EventConverter<ISomethingHappenedEvent, ISomethingElseHappenedEvent>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert.
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// The <see cref="ISomethingElseHappenedEvent"/>.
        /// </returns>
        public override ISomethingElseHappenedEvent Convert(ISomethingHappenedEvent @event)
        {
            return null;
        }

        #endregion
    }
}