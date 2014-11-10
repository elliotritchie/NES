using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES.Tests
{
    using System.Collections.ObjectModel;
    using global::NEventStore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using NES.Contracts;
    using NEventStore;

    [TestClass]
    public class EventStoreAdapterTests : Test
    {
        private readonly Mock<IStoreEvents> _storeEvents = new Mock<IStoreEvents>();

        private Guid _streamId = Guid.NewGuid();
        private IEventStore _eventStore;

        protected override void Context()
        {
            var eventStreamInitial = new Mock<IEventStream>();
            eventStreamInitial.SetupGet(s => s.UncommittedHeaders).Returns(() => new Dictionary<string, object>());
            eventStreamInitial.SetupGet(s => s.UncommittedEvents).Returns(() => new Collection<EventMessage>());

            _storeEvents.Setup(s => s.OpenStream(BucketSupport.DefaultBucketId, _streamId.ToString(), 0, int.MaxValue)).Returns(eventStreamInitial.Object);

            this._eventStore = new EventStoreAdapter(_storeEvents.Object);

        }

        protected override void Event()
        {

        }

        [TestMethod]
        public void RunWithoutAnyException()
        {
            var eventHeaders = new Dictionary<object, Dictionary<string, object>>();
            eventHeaders["hi"] = new Dictionary<string, object>();
            eventHeaders["there"] = new Dictionary<string, object>();

            this._eventStore.Write(
                BucketSupport.DefaultBucketId,
                this._streamId.ToString(),
                0,
                new[] { "hi", "there" },
                Guid.NewGuid(),
                new Dictionary<string, object>(),
                eventHeaders);
        }
    }


}
