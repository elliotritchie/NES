using System;
using System.Collections.Generic;
using System.Linq;
using EventStore;

namespace NES.EventStore
{
    public class EventStoreAdapter : IEventStore
    {
        private readonly IStoreEvents _eventStore;

        public EventStoreAdapter(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public IMemento Read(Guid id)
        {
            var snapshot = _eventStore.GetSnapshot(id, int.MaxValue);
            return snapshot != null ? (IMemento)snapshot.Payload : null;
        }

        public IEnumerable<object> Read(Guid id, int version)
        {
            using (var stream = _eventStore.OpenStream(id, version, int.MaxValue))
            {
                return stream.CommittedEvents.Select(e => e.Body);
            }
        }

        public void Write(Guid id, int version, IEnumerable<object> events)
        {
            using (var stream = _eventStore.OpenStream(id, version, int.MaxValue))
            {
                foreach (var @event in events)
                {
                    stream.Add(new EventMessage { Body = @event });
                }

                try
                {
                    stream.CommitChanges(Guid.NewGuid());
                }
                catch (ConcurrencyException ex)
                {
                    throw new ConflictingCommandException(ex.Message, ex);
                }
            }
        }
    }
}