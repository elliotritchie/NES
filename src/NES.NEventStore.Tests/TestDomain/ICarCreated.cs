// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICarCreated.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The CarCreated interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NES.NEventStore.Tests.TestDomain
{
    using System;

    /// <summary>
    /// The CarCreated interface.
    /// </summary>
    public interface ICarCreated : IBucketId
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}