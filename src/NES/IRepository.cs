// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The Repository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The Repository interface.
    /// </summary>
    public interface IRepository
    {
        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="aggregate">
        /// The aggregate.
        /// </param>
        /// <typeparam name="T">
        /// Type of event source
        /// </typeparam>
        void Add<T>(T aggregate) where T : class, IEventSource;

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <typeparam name="T">
        /// Type of event source
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;

        #endregion
    }
}