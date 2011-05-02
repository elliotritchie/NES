using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace NES.Tests
{
    public static class EventSourceMapperTests
    {
        [TestClass]
        public class When_getting_event_source_and_snapshot_and_events_exist : Test
        {
            private IEventSourceMapper _eventSourceMapper;
            private readonly Mock<IEventSourceFactory> _eventSourceFactory = new Mock<IEventSourceFactory>();
            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();
            private readonly Mock<IEventSource> _eventSource = new Mock<IEventSource>();
            private readonly Guid _id = Guid.NewGuid();
            private const int _version = 0;
            private readonly Mock<IMemento> _memento = new Mock<IMemento>();
            private readonly List<object> _events = new List<object> { new object() };
            private IEventSource _returnedEventSource;

            protected override void Context()
            {
                _eventSourceMapper = new EventSourceMapper(_eventSourceFactory.Object, _eventStore.Object);

                _eventSourceFactory.Setup(f => f.Create<IEventSource>()).Returns(_eventSource.Object);
                _eventSource.Setup(s => s.Id).Returns(_id);
                _eventSource.Setup(s => s.Version).Returns(_version);
                _eventStore.Setup(a => a.Read(_id)).Returns(_memento.Object);
                _eventStore.Setup(a => a.Read(_id, _version)).Returns(_events);
            }

            protected override void Event()
            {
                _returnedEventSource = _eventSourceMapper.Get<IEventSource>(_id);
            }

            [TestMethod]
            public void Should_create_event_source()
            {
                _eventSourceFactory.Verify(f => f.Create<IEventSource>());
            }

            [TestMethod]
            public void Should_restore_snapshot()
            {
                _eventSource.Verify(s => s.RestoreSnapshot(_memento.Object));
            }

            [TestMethod]
            public void Should_hydrate_event_source_with_events()
            {
                _eventSource.Verify(s => s.Hydrate(_events));
            }

            [TestMethod]
            public void Should_return_hydrated_event_source()
            {
                Assert.AreSame(_eventSource.Object, _returnedEventSource);
            }
        }

        [TestClass]
        public class When_getting_event_source_and_snapshot_and_events_dont_exist : Test
        {
            private IEventSourceMapper _eventSourceMapper;
            private readonly Mock<IEventSourceFactory> _eventSourceFactory = new Mock<IEventSourceFactory>();
            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();
            private readonly Mock<IEventSource> _eventSource = new Mock<IEventSource>();
            private IEventSource _returnedEventSource;

            protected override void Context()
            {
                _eventSourceMapper = new EventSourceMapper(_eventSourceFactory.Object, _eventStore.Object);

                _eventSourceFactory.Setup(f => f.Create<IEventSource>()).Returns(_eventSource.Object);
            }

            protected override void Event()
            {
                _returnedEventSource = _eventSourceMapper.Get<IEventSource>(Guid.NewGuid());
            }

            [TestMethod]
            public void Should_create_event_source()
            {
                _eventSourceFactory.Verify(f => f.Create<IEventSource>());
            }

            [TestMethod]
            public void Should_not_restore_snapshot()
            {
                _eventSource.Verify(s => s.RestoreSnapshot(It.IsAny<IMemento>()), Times.Never());
            }

            [TestMethod]
            public void Should_not_hydrate_event_source_with_events()
            {
                _eventSource.Verify(s => s.Hydrate(It.IsAny<IEnumerable<object>>()), Times.Never());
            }

            [TestMethod]
            public void Should_not_return_hydrated_event_source()
            {
                Assert.IsNull(_returnedEventSource);
            }
        }
    }
}