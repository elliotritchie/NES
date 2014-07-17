// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventStore.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The EventStore interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The EventStore interface.
    /// </summary>
    public interface IEventStore
    {
        #region Public Methods and Operators

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IMemento"/>.
        /// </returns>
        IMemento Read(string bucketId, Guid id);

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<object> Read(string bucketId, Guid id, int version);

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="version">
        /// The version.
        /// </param>
        /// <param name="events">
        /// The events.
        /// </param>
        /// <param name="commitId">
        /// The commit id.
        /// </param>
        /// <param name="headers">
        /// The headers.
        /// </param>
        /// <param name="eventHeaders">
        /// The event headers.
        /// </param>
        void Write(
            string bucketId, 
            Guid id, 
            int version, 
            IEnumerable<object> events, 
            Guid commitId, 
            Dictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders);

        #endregion
    }
}