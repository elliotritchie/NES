// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventStoreAdapter.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event store adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using global::NEventStore;

    /// <summary>
    ///     The event store adapter.
    /// </summary>
    public class EventStoreAdapter : IEventStore
    {
        #region Fields

        private readonly IStoreEvents _eventStore;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventStoreAdapter"/> class.
        /// </summary>
        /// <param name="eventStore">
        /// The event store.
        /// </param>
        public EventStoreAdapter(IStoreEvents eventStore)
        {
            this._eventStore = eventStore;
        }

        #endregion

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
        public IMemento Read(string bucketId, Guid id)
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            var snapshot = this._eventStore.Advanced.GetSnapshot(bucketId, id, int.MaxValue);
            return snapshot != null ? (IMemento)snapshot.Payload : null;
        }

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
        public IEnumerable<object> Read(string bucketId, Guid id, int version)
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            using (var stream = this._eventStore.OpenStream(bucketId, id, version, int.MaxValue))
            {
                return stream.CommittedEvents.Select(e => e.Body);
            }
        }

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
        /// <exception cref="ConflictingCommandException">
        /// Is raised when ConcurrencyException occurs
        /// </exception>
        public void Write(
            string bucketId, 
            Guid id, 
            int version, 
            IEnumerable<object> events, 
            Guid commitId, 
            Dictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            using (var stream = this._eventStore.OpenStream(bucketId, id, version, int.MaxValue))
            {
                foreach (var header in headers)
                {
                    stream.UncommittedHeaders[header.Key] = header.Value;
                }

                foreach (var eventMessage in events.Select(e => new EventMessage { Body = e }))
                {
                    foreach (var header in eventHeaders[eventMessage.Body])
                    {
                        eventMessage.Headers[header.Key] = header.Value;
                    }

                    stream.Add(eventMessage);
                }

                try
                {
                    stream.CommitChanges(commitId);
                }
                catch (DuplicateCommitException)
                {
                    stream.ClearChanges();
                }
                catch (ConcurrencyException ex)
                {
                    throw new ConflictingCommandException(ex.Message, ex);
                }
            }
        }

        #endregion

        #region Methods

        private string ChangeBucketIdIfRequired(string bucketId)
        {
            if (string.IsNullOrEmpty(bucketId) || string.IsNullOrWhiteSpace(bucketId))
            {
                return BucketSupport.DefaultBucketId;
            }

            return bucketId;
        }

        #endregion
    }
}