using System;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public class EventSourceMapper : IEventSourceMapper
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(EventSourceMapper));
        private readonly IEventSourceFactory _eventSourceFactory;
        private readonly IEventStore _eventStore;

        public EventSourceMapper(IEventSourceFactory eventSourceFactory, IEventStore eventStore)
        {
            _eventSourceFactory = eventSourceFactory;
            _eventStore = eventStore;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            Logger.Debug("Get id {0}", id);

            var eventSource = _eventSourceFactory.Create<T>();

            RestoreSnapshot(id, eventSource);
            Hydrate(id, eventSource);
            
            return eventSource.Id != Guid.Empty ? eventSource : null;
        }

        public void Set<T>(CommandContext commandContext, T eventSource) where T : class, IEventSource
        {
            var id = eventSource.Id;
            var version = eventSource.Version;
            var type = eventSource.GetType();
            var events = eventSource.Flush().ToList();
            var commitId = commandContext.Id;
            var headers = commandContext.Headers;
            var eventHeaders = new Dictionary<object, Dictionary<string, object>>();

            if (!events.Any())
                return;

            var eventsCount = events.Count();

            Logger.Debug("commitId {0}eventSource Id {1} eventSource {2} events count {3}", commitId, id, eventSource.GetType().FullName, eventsCount);

            headers["AggregateId"] = id;
            headers["AggregateVersion"] = version + eventsCount;
            headers["AggregateType"] = type.FullName;

            for (int i = 0; i < eventsCount; i++)
            {
                eventHeaders[events.ElementAt(i)] = new Dictionary<string, object> {{ "EventVersion", version + i + 1 }};
            }

            try
            {
                _eventStore.Write(id, version, events, commitId, headers, eventHeaders);
            }
            catch (ConflictingCommandException conflictingCommandException)
            {
                //TODO: Check if the events actually conflict
                Logger.Error(conflictingCommandException.Message);
                throw;
            }
        }

        private void RestoreSnapshot<T>(Guid id, T eventSource) where T : IEventSource
        {
            Logger.Debug("RestoreSnaphost for id {0} and type {1}", id, eventSource.GetType().FullName);

            var memento = _eventStore.Read(id);

            if (memento != null)
            {
                Logger.Debug("Restore snapshot because memento exists");
                eventSource.RestoreSnapshot(memento);
                Logger.Debug("Restore snapshot done");
            }
        }

        private void Hydrate<T>(Guid id, T eventSource) where T : IEventSource
        {
            Logger.Debug("Hydrate id {0} eventSource {1} version {2}", id, eventSource, eventSource.Version);
            var events = _eventStore.Read(id, eventSource.Version);

            eventSource.Hydrate(events);
            
            Logger.Debug("Hydrate done");
        }
    }
}