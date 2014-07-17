// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSourceMapper.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event source mapper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The event source mapper.
    /// </summary>
    public class EventSourceMapper : IEventSourceMapper
    {
        #region Static Fields

        private static readonly ILogger Logger = LoggerFactory.Create(typeof(EventSourceMapper));

        #endregion

        #region Fields

        private readonly IEventSourceFactory _eventSourceFactory;

        private readonly IEventStore _eventStore;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSourceMapper"/> class.
        /// </summary>
        /// <param name="eventSourceFactory">
        /// The event source factory.
        /// </param>
        /// <param name="eventStore">
        /// The event store.
        /// </param>
        public EventSourceMapper(IEventSourceFactory eventSourceFactory, IEventStore eventStore)
        {
            this._eventSourceFactory = eventSourceFactory;
            this._eventStore = eventStore;
        }

        #endregion

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
        /// Type of aggregate resp. EventSource
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource
        {
            Logger.Debug("Get event source Id '{0}', Type '{1}'", id, typeof(T).Name);

            if (id == Guid.Empty)
            {
                Logger.Warn("Try to read event store with Guid.Empty");
                return null;
            }

            var eventSource = this._eventSourceFactory.Create<T>();

            bool hasSnaphot = this.RestoreSnapshot(bucketId, id, eventSource);
            bool hasEvents = this.Hydrate(bucketId, id, eventSource);

            if (!(hasSnaphot || hasEvents))
            {
                Logger.Debug(string.Format("No event source found using the id {0}", id));
                return null;
            }

            if (eventSource.Id == Guid.Empty)
            {
                Logger.Warn(
                    string.Format(
                        "Source with id {0} found in eventstore, but after hydration the id was not set properly {1}",
                        id,
                        eventSource.Id));
                return null;
            }

            return eventSource;
        }

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
        /// Type of aggregate resp. EventSource
        /// </typeparam>
        public void Set<T>(CommandContext commandContext, T eventSource) where T : class, IEventSource
        {
            var id = eventSource.Id;
            var bucketId = eventSource.BucketId;
            var type = eventSource.GetType();
            var oldVersion = eventSource.Version;
            var events = eventSource.Flush();
            var newVersion = eventSource.Version;
            var commitId = commandContext.Id;
            var headers = commandContext.Headers;
            var eventHeaders = new Dictionary<object, Dictionary<string, object>>();

            if (!events.Any())
            {
                return;
            }

            Logger.Debug(
                "Set event source Id '{0}', Version '{1}', Type '{2}', CommitId '{3}'",
                id,
                newVersion,
                eventSource.GetType().Name,
                commitId);

            headers["AggregateId"] = id;
            headers["AggregateBucketId"] = bucketId;
            headers["AggregateVersion"] = newVersion;
            headers["AggregateType"] = type.FullName;

            for (int i = 0; i < events.Count(); i++)
            {
                eventHeaders[events.ElementAt(i)] = new Dictionary<string, object> { { "EventVersion", oldVersion + i + 1 } };
            }

            try
            {
                this._eventStore.Write(bucketId, id, oldVersion, events, commitId, headers, eventHeaders);
            }
            catch (ConflictingCommandException)
            {
                //TODO: Check if the events actually conflict
                throw;
            }
        }

        #endregion

        #region Methods

        private bool Hydrate<T>(string bucketId, Guid id, T eventSource) where T : IEventSource
        {
            Logger.Debug(
                "Hydrate event source Id '{0}', BucketId '{1}', Version '{2}' and Type '{3}'",
                id,
                bucketId,
                eventSource.Version,
                eventSource.GetType().Name);

            var events = this._eventStore.Read(bucketId, id, eventSource.Version).ToList();

            eventSource.Hydrate(events);

            return events.Count > 0;
        }

        private bool RestoreSnapshot<T>(string bucketId, Guid id, T eventSource) where T : IEventSource
        {
            Logger.Debug("Restore snapshot for event source Id '{0}', BucketId '{1}', Type '{2}'", id, bucketId, eventSource.GetType().Name);

            var memento = this._eventStore.Read(bucketId, id);

            if (memento != null)
            {
                eventSource.RestoreSnapshot(memento);
                return true;
            }

            return false;
        }

        #endregion
    }
}