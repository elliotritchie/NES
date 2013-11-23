using System;
using System.Collections.Generic;
using System.Linq;
using NEventStore;

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
            var snapshot = _eventStore.Advanced.GetSnapshot(id, int.MaxValue);
            return snapshot != null ? (IMemento)snapshot.Payload : null;
        }

        public IEnumerable<object> Read(Guid id, int version)
        {
            using (var stream = _eventStore.OpenStream(id, version, int.MaxValue))
            {
                return stream.CommittedEvents.Select(e => e.Body);
            }
        }

        public void Write(Guid id, int version, IEnumerable<object> events, Guid commitId, Dictionary<string, object> headers, Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            using (var stream = _eventStore.OpenStream(id, version, int.MaxValue))
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
                catch (DuplicateCommitException ex)
                {
                    stream.ClearChanges();
                }
                catch (ConcurrencyException ex)
                {
                    throw new ConflictingCommandException(ex.Message, ex);
                }
            }
        }
    }
}