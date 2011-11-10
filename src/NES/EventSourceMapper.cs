using System;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public class EventSourceMapper : IEventSourceMapper
    {
        private readonly IEventSourceFactory _eventSourceFactory;
        private readonly IEventStore _eventStore;

        public EventSourceMapper(IEventSourceFactory eventSourceFactory, IEventStore eventStore)
        {
            _eventSourceFactory = eventSourceFactory;
            _eventStore = eventStore;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
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
            var events = eventSource.Flush();
            var commitId = commandContext.Id;
            var headers = commandContext.Headers;
            var eventHeaders = new Dictionary<object, Dictionary<string, object>>();

            if (!events.Any())
                return;

            headers["AggregateId"] = id;
            headers["AggregateVersion"] = version + events.Count();
            headers["AggregateType"] = type.FullName;

            for (int i = 0; i < events.Count(); i++)
            {
                eventHeaders[events.ElementAt(i)] = new Dictionary<string, object> {{ "EventVersion", version + i + 1 }};
            }

            try
            {
                _eventStore.Write(id, version, events, commitId, headers, eventHeaders);
            }
            catch (ConflictingCommandException)
            {
                //TODO: Check if the events actually conflict
                throw;
            }
        }

        private void RestoreSnapshot<T>(Guid id, T eventSource) where T : IEventSource
        {
            var memento = _eventStore.Read(id);

            if (memento != null)
            {
                eventSource.RestoreSnapshot(memento);
            }
        }

        private void Hydrate<T>(Guid id, T eventSource) where T : IEventSource
        {
            var events = _eventStore.Read(id, eventSource.Version);

            eventSource.Hydrate(events);
        }
    }
}