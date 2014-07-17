// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventSourceMapper.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventSourceMapper interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;

    /// <summary>
    ///     The EventSourceMapper interface.
    /// </summary>
    public interface IEventSourceMapper
    {
        #region Public Methods and Operators

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
        /// The type of the event source
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;

        /// <summary>
        /// The set.
        /// </summary>
        /// <param name="commandContext">
        /// The command context.
        /// </param>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <typeparam name="T">
        /// The type of the event source
        /// </typeparam>
        void Set<T>(CommandContext commandContext, T eventSource) where T : class, IEventSource;

        #endregion
    }
}