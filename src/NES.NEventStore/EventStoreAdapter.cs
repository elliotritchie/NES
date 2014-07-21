using System;
using System.Collections.Generic;
using System.Linq;
using NEventStore;

namespace NES.NEventStore
{
    public class EventStoreAdapter : IEventStore
    {
        private readonly IStoreEvents _eventStore;

        public EventStoreAdapter(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public IMemento Read(string bucketId, Guid id)
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            var snapshot = _eventStore.Advanced.GetSnapshot(bucketId, id, int.MaxValue);
            return snapshot != null ? (IMemento)snapshot.Payload : null;
        }

        public IEnumerable<object> Read(string bucketId, Guid id, int version)
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            using (var stream = _eventStore.OpenStream(bucketId, id, version, int.MaxValue))
            {
                return stream.CommittedEvents.Select(e => e.Body);
            }
        }

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
            using (var stream = _eventStore.OpenStream(bucketId, id, version, int.MaxValue))
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

        private string ChangeBucketIdIfRequired(string bucketId)
        {
            if (string.IsNullOrEmpty(bucketId) || string.IsNullOrWhiteSpace(bucketId))
            {
                return BucketSupport.DefaultBucketId;
            }

            return bucketId;
        }
    }
}