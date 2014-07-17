// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUnitOfWork.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The UnitOfWork interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The UnitOfWork interface.
    /// </summary>
    public interface IUnitOfWork
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The commit.
        /// </summary>
        void Commit();

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

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <typeparam name="T">
        /// Type of event source
        /// </typeparam>
        void Register<T>(T eventSource) where T : class, IEventSource;

        #endregion
    }
}