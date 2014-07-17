// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SomethingElseHappenedEvent.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The omethingElseHappenedEvent interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests.Stubs
{
    /// <summary>
    ///     The omethingElseHappenedEvent interface.
    /// </summary>
    public interface ISomethingElseHappenedEvent : IEvent
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the something else.
        /// </summary>
        string SomethingElse { get; set; }

        #endregion
    }
}