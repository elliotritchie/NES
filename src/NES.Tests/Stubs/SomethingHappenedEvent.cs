// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SomethingHappenedEvent.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The omethingHappenedEvent interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests.Stubs
{
    /// <summary>
    ///     The omethingHappenedEvent interface.
    /// </summary>
    public interface ISomethingHappenedEvent : IEvent
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the something.
        /// </summary>
        string Something { get; set; }

        #endregion
    }
}