using System;
using System.Collections.Generic;
using System.Linq;
using NES.Contracts;

namespace NES
{
    public class EventSourceMapper : IEventSourceMapper
    {
        private static readonly ILogger Logger = LoggerFactory.Create(typeof(EventSourceMapper));

        private readonly IEventSourceFactory _eventSourceFactory;

        private readonly IEventStore _eventStore;

        public EventSourceMapper(IEventSourceFactory eventSourceFactory, IEventStore eventStore)
        {
            _eventSourceFactory = eventSourceFactory;
            _eventStore = eventStore;
        }

        public T Get<T, TId>(string bucketId, string id, int version)
            where T : class, IEventSource<TId>
        {
            Logger.Debug("Get event source Id '{0}', Type '{1}'", id, typeof(T).Name);

            if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
            {
                Logger.Warn("Try to read event store with Guid.Empty");
                return null;
            }

            var eventSource = _eventSourceFactory.Create<T>();

            bool hasSnaphot = this.RestoreSnapshot<T, TId>(bucketId, id, eventSource, version);
            bool hasEvents = this.Hydrate(bucketId, id, eventSource, version);

            if (!(hasSnaphot || hasEvents))
            {
                Logger.Debug("No event source found using the id {0}", id);
                return null;
            }

            if (string.IsNullOrEmpty(eventSource.StringId))
            {
                Logger.Warn(string.Format("Source with id {0} found in eventstore, but after hydration the id was not set properly {1}", id, eventSource.StringId));
                return null;
            }

            return eventSource;
        }

        public void Set(CommandContext commandContext, IEventSourceBase eventSource)
        {
            var id = eventSource.StringId;
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

            Logger.Debug("Set event source Id '{0}', Version '{1}', Type '{2}', CommitId '{3}'", id, newVersion, eventSource.GetType().Name, commitId);

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
                _eventStore.Write(bucketId, id, oldVersion, events, commitId, headers, eventHeaders);
            }
            catch (ConflictingCommandException)
            {
                // TODO: Check if the events actually conflict
                throw;
            }
        }

        private bool Hydrate<T>(string bucketId, string id, T eventSource, int version) where T : IEventSourceBase
        {
            Logger.Debug("Hydrate event source Id '{0}', BucketId '{1}', Version '{2}' and Type '{3}'", id, bucketId, version, eventSource.GetType().Name);

            var events = _eventStore.Read(bucketId, id, version).ToList();

            eventSource.Hydrate(events);

            return events.Count > 0;
        }

        private bool RestoreSnapshot<T, TId>(string bucketId, string id, T eventSource, int version)
            where T : IEventSource<TId>
        {
            Logger.Debug("Restore snapshot for event source Id '{0}', BucketId '{1}', Type '{2}'", id, bucketId, eventSource.GetType().Name);

            var memento = _eventStore.Read<TId>(bucketId, id, version);

            if (memento != null)
            {
                eventSource.RestoreSnapshot(memento);
                return true;
            }

            return false;
        }
    }
}