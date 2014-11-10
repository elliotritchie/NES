using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NES.Tests
{
    using System.Collections.ObjectModel;
    using global::NEventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NES.Contracts;
    using NEventStore;

    [TestClass]
    public class EventStoreAdapterConcurrencyTests : Test
    {
        private readonly Mock<IStoreEvents> _storeEvents = new Mock<IStoreEvents>();

        private Guid _streamId = Guid.NewGuid();
        private IEventStore _eventStore;

        protected override void Context()
        {
            var eventStreamRevision = new Mock<IEventStream>();
            eventStreamRevision.SetupGet(s => s.UncommittedHeaders).Returns(() => new Dictionary<string, object>());
            eventStreamRevision.SetupGet(s => s.UncommittedEvents).Returns(() => new Collection<EventMessage>());
            eventStreamRevision.SetupGet(s => s.StreamRevision).Returns(4);

            _storeEvents.Setup(s => s.OpenStream(BucketSupport.DefaultBucketId, _streamId.ToString(), 3, int.MaxValue)).Returns(eventStreamRevision.Object);

            this._eventStore = new EventStoreAdapter(_storeEvents.Object);

        }

        protected override void Event()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ConflictingCommandException))]
        public void MustRaiseConflictingCommandException()
        {
            var eventHeaders = new Dictionary<object, Dictionary<string, object>>();
            eventHeaders["hi"] = new Dictionary<string, object>();
            eventHeaders["there"] = new Dictionary<string, object>();

            using (var transactionScope = new TransactionScope())
            {
                this._eventStore.Write(BucketSupport.DefaultBucketId, this._streamId.ToString(), 3,
                    new[] { "hi", "there" }, Guid.NewGuid(), new Dictionary<string, object>(), eventHeaders);
            }
        }
    }


}
