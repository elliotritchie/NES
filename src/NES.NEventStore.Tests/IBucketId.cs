// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBucketId.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The BucketId interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NES.NEventStore.Tests
{
    /// <summary>
    /// The BucketId interface.
    /// </summary>
    public interface IBucketId
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the bucket id.
        /// </summary>
        string BucketId { get; set; }

        #endregion
    }
}