using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using NES.Contracts;
using NEventStore;

namespace NES.NEventStore
{
    using global::NEventStore.Logging;

    public class EventStoreAdapter : IEventStore
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(BsonSerializer));
        private readonly IStoreEvents _eventStore;

        public EventStoreAdapter(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public TMemento Read<TMemento>(string bucketId, string id) where TMemento : class, IMementoBase
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            var snapshot = _eventStore.Advanced.GetSnapshot(bucketId, id, int.MaxValue);
            return snapshot != null ? (TMemento)snapshot.Payload : null;
        }

        public IEnumerable<object> Read(string bucketId, string id, int version)
        {
            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            using (var stream = _eventStore.OpenStream(bucketId, id, version, int.MaxValue))
            {
                Logger.Debug("Stream with bucketId {0} id {1} has revision {2}. CommitedEvents count {3}", bucketId, id, stream.StreamRevision, stream.CommittedEvents.Count);
                return stream.CommittedEvents.Select(e => e.Body);
            }
        }

        public void Write(string bucketId, string id, int version, IEnumerable<object> events, Guid commitId, Dictionary<string, object> headers, Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            Logger.Debug("Write eventstream with bucketId {0} id {1} version {2} commitId {3} events {4}", bucketId, id, version, events.Count());

            bucketId = this.ChangeBucketIdIfRequired(bucketId);
            using (var stream = _eventStore.OpenStream(bucketId, id, version, int.MaxValue))
            {
                Logger.Debug("Opened stream has StreamRevision {0}", stream.StreamRevision);

                if (version != stream.StreamRevision && Transaction.Current != null)
                {
                    Logger.Warn("Opened stream version {0} is not equal to the actual eventSource version {1}. EventSource has been modified between the read and this write");
                    throw new ConflictingCommandException(string.Format("EventSource {0} has the version {1} and the stream has version {2}", id, version, stream.StreamRevision));
                }

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
                    Logger.Warn("DuplicateCommitException occured for the stream {0} with commitId {1}", id, commitId);
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