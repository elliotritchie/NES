// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The repository.
    /// </summary>
    public class Repository : IRepository
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
        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            UnitOfWorkFactory.Current.Register(aggregate);
        }

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
        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T>(bucketId, id);
        }

        #endregion
    }
}