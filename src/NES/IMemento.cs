// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMemento.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The Memento interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The Memento interface.
    /// </summary>
    public interface IMemento
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the bucket id.
        /// </summary>
        string BucketId { get; set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets the version.
        /// </summary>
        int Version { get; set; }

        #endregion
    }
}