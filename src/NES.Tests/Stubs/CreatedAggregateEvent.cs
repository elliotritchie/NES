// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreatedAggregateEvent.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The reatedAggregateEvent interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests.Stubs
{
    using System;

    /// <summary>
    ///     The reatedAggregateEvent interface.
    /// </summary>
    public interface ICreatedAggregateEvent : IEvent
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        Guid Id { get; set; }

        #endregion
    }
}