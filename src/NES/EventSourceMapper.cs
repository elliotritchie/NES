using System;
using System.Linq;

namespace NES
{
    public class EventSourceMapper : IEventSourceMapper
    {
        private readonly IEventSourceFactory _eventSourceFactory;
        private readonly IEventStoreAdapter _eventStoreAdapter;

        public EventSourceMapper(IEventSourceFactory eventSourceFactory, IEventStoreAdapter eventStoreAdapter)
        {
            _eventSourceFactory = eventSourceFactory;
            _eventStoreAdapter = eventStoreAdapter;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            var eventSource = _eventSourceFactory.Create<T>();

            RestoreSnapshot(id, eventSource);
            Hydrate(id, eventSource);
            
            return eventSource.Id != Guid.Empty ? eventSource : null;
        }

        public void Set<T>(T eventSource) where T : class, IEventSource
        {
            try
            {
                _eventStoreAdapter.Write(eventSource.Id, eventSource.Version, eventSource.Flush());
            }
            catch (ConflictingCommandException)
            {
                //TODO: Check if the events actually conflict
                throw;
            }
        }

        private void RestoreSnapshot<T>(Guid id, T eventSource) where T : IEventSource
        {
            var memento = _eventStoreAdapter.Read(id);

            if (memento != null)
            {
                eventSource.RestoreSnapshot(memento);
            }
        }

        private void Hydrate<T>(Guid id, T eventSource) where T : IEventSource
        {
            var events = _eventStoreAdapter.Read(id, eventSource.Version);

            if (events.Any())
            {
                eventSource.Hydrate(events);
            }
        }
    }
}