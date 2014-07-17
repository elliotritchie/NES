// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionsNoBucket.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The extensions no bucket.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The extensions no bucket.
    /// </summary>
    public static class ExtensionsNoBucket
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="eventSourceMapper">
        /// The event source mapper.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <typeparam name="T">
        /// Type of aggregate resp. EventSource
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Get<T>(this IEventSourceMapper eventSourceMapper, Guid id) where T : class, IEventSource
        {
            return eventSourceMapper.Get<T>(BucketSupport.DefaultBucketId, id);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <typeparam name="T">
        /// Type of aggregate resp. EventSource
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Get<T>(this IUnitOfWork unitOfWork, Guid id) where T : class, IEventSource
        {
            return unitOfWork.Get<T>(id);
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="repository">
        /// The repository.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <typeparam name="T">
        /// Type of aggregate resp. EventSource
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public static T Get<T>(this IRepository repository, Guid id) where T : class, IEventSource
        {
            return repository.Get<T>(BucketSupport.DefaultBucketId, id);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="eventStore">
        /// The event store.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="IMemento"/>.
        /// </returns>
        public static IMemento Read(this IEventStore eventStore, Guid id)
        {
            return eventStore.Read(BucketSupport.DefaultBucketId, id);
        }

        /// <summary>
        /// The read.
        /// </summary>
        /// <param name="eventStore">
        /// The event store.
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
        public static IEnumerable<object> Read(this IEventStore eventStore, Guid id, int version)
        {
            return eventStore.Read(BucketSupport.DefaultBucketId, id, version);
        }

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="eventStore">
        /// The event store.
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
        public static void Write(
            this IEventStore eventStore, 
            Guid id, 
            int version, 
            IEnumerable<object> events, 
            Guid commitId, 
            Dictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            eventStore.Write(BucketSupport.DefaultBucketId, id, version, events, commitId, headers, eventHeaders);
        }

        #endregion
    }
}