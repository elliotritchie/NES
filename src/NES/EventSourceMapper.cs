using System;
using System.Collections.Generic;
using System.Linq;

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

        public T Get<T>(Guid id, Func<IEnumerable<object>, IEnumerable<object>> sortFilterAction) where T : class, IEventSource
        {
            Logger.Debug("Get event source Id '{0}', Type '{1}'", id, typeof(T).Name);

            var eventSource = _eventSourceFactory.Create<T>();

            RestoreSnapshot(id, eventSource);
            Hydrate(id, eventSource, sortFilterAction);

            return eventSource.Id != Guid.Empty ? eventSource : null;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return this.Get<T>(id, null);
        }

        public IEnumerable<object> GetEvents(Guid id)
        {
            return _eventStore.Read(id, int.MinValue);
        }

        public void Set<T>(CommandContext commandContext, T eventSource) where T : class, IEventSource
        {
            var id = eventSource.Id;
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
            headers["AggregateVersion"] = newVersion;
            headers["AggregateType"] = type.FullName;

            for (int i = 0; i < events.Count(); i++)
            {
                eventHeaders[events.ElementAt(i)] = new Dictionary<string, object> {{ "EventVersion", oldVersion + i + 1 }};
            }

            try
            {
                _eventStore.Write(id, oldVersion, events, commitId, headers, eventHeaders);
            }
            catch (ConflictingCommandException)
            {
                //TODO: Check if the events actually conflict
                throw;
            }
        }

        private void RestoreSnapshot<T>(Guid id, T eventSource) where T : IEventSource
        {
            Logger.Debug("Restore snapshot for event source Id '{0}', Type '{1}'", id, eventSource.GetType().Name);

            var memento = _eventStore.Read(id);

            if (memento != null)
            {
                eventSource.RestoreSnapshot(memento);
            }
        }

        private void Hydrate<T>(Guid id, T eventSource, Func<IEnumerable<object>, IEnumerable<object>> sortFilterAction) where T : IEventSource
        {
            Logger.Debug("Hydrate event source Id '{0}', Version '{1}' and Type '{2}'", id, eventSource.Version, eventSource.GetType().Name);

            var events = _eventStore.Read(id, eventSource.Version);

            if (sortFilterAction != null)
            {
                events = sortFilterAction(events);
            }

            eventSource.Hydrate(events);
        }
    }
}