using System;
using System.Linq;

namespace NES
{
    public class EventSourceMapper : IEventSourceMapper
    {
        private readonly IEventSourceFactory _eventSourceFactory;
        private readonly IEventStore _eventStore;
        private readonly IEventConversionRunner _eventConversionRunner;

        public EventSourceMapper(IEventSourceFactory eventSourceFactory, IEventStore eventStore, IEventConversionRunner eventConversionRunner)
        {
            _eventSourceFactory = eventSourceFactory;
            _eventStore = eventStore;
            _eventConversionRunner = eventConversionRunner;
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
            try
            {
                _eventStore.Write(eventSource.Id, eventSource.Version, eventSource.Flush(), commandContext.Id, commandContext.Headers);
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

            if (events.Any())
            {
                eventSource.Hydrate(_eventConversionRunner.Run(events));
            }
        }
    }
}