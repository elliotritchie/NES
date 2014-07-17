// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICarPriceChanged.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The CarPriceChanged interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NES.NEventStore.Tests.TestDomain
{
    using System;

    using global::NServiceBus;

    /// <summary>
    /// The CarPriceChanged interface.
    /// </summary>
    public interface ICarPriceChanged : IEvent, IBucketId
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        double Price { get; set; }

        #endregion
    }
}