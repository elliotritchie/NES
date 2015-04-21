using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Contracts;

namespace NES.Tests
{
    public static class EventSourceMapperTests
    {
        [TestClass]
        public class When_setting_event_source_and_events_exist : Test
        {
            private IEventSourceMapper _eventSourceMapper;
            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();
            private readonly CommandContext _commandContext = new CommandContext();
            private readonly Guid _commitId = GuidComb.NewGuidComb();
            private readonly Mock<IEventSource<Guid>> _eventSource = new Mock<IEventSource<Guid>>();
            private readonly Guid _id = GuidComb.NewGuidComb();
            private const int _version = 2;
            private readonly List<object> _events = new List<object> { new object(), new object() };
            private Dictionary<string, object> _committedHeaders;
            private Dictionary<object, Dictionary<string, object>> _committedEventHeaders;

            protected override void Context()
            {
                _eventSourceMapper = new EventSourceMapper(null, _eventStore.Object);

                _commandContext.Id = _commitId;
                _commandContext.Headers = new Dictionary<string, object> { { "TestKey", "TestValue" } };
                _eventSource.Setup(s => s.Id).Returns(_id);
                _eventSource.Setup(s => s.StringId).Returns(_id.ToString);
                _eventSource.Setup(s => s.Version).Returns(_version);
                _eventSource.Setup(s => s.Flush()).Returns(this._events).Callback(() => _eventSource.Setup(s => s.Version).Returns(_version + _events.Count));
                _eventSource.Setup(s => s.BucketId).Returns(BucketSupport.DefaultBucketId);

                _eventStore
                    .Setup(s => s.Write(
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<int>(),
                        It.IsAny<IEnumerable<object>>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Dictionary<string, object>>(),
                        It.IsAny<Dictionary<object, Dictionary<string, object>>>()))
                    .Callback<string, string, int, IEnumerable<object>, Guid, Dictionary<string, object>, Dictionary<object, Dictionary<string, object>>>((a, b, c, d, e, f, g) =>
                    {
                        _committedHeaders = f;
                        _committedEventHeaders = g;
                    });
            }

            protected override void Event()
            {
                _eventSourceMapper.Set(_commandContext, _eventSource.Object);
            }

            [TestMethod]
            public void Should_commit_to_event_store()
            {
                _eventStore.Verify(s => s.Write(BucketSupport.DefaultBucketId, _id.ToString(), _version, _events, _commitId, It.IsAny<Dictionary<string, object>>(), It.IsAny<Dictionary<object, Dictionary<string, object>>>()));
            }

            [TestMethod]
            public void Should_commit_headers_to_event_store()
            {
                Assert.AreEqual("TestValue", _committedHeaders["TestKey"]);
                Assert.AreEqual(_id.ToString(), _committedHeaders["AggregateId"]);
                Assert.AreEqual(_version + _events.Count, _committedHeaders["AggregateVersion"]);
                Assert.AreEqual(_eventSource.Object.GetType().FullName, _committedHeaders["AggregateType"]);
                Assert.AreEqual(BucketSupport.DefaultBucketId, _committedHeaders["AggregateBucketId"]);
            }

            [TestMethod]
            public void Should_commit_event_headers_to_event_store()
            {
                for (int i = 0; i < _events.Count; i++)
                {
                    Assert.AreEqual(_version + i + 1, _committedEventHeaders[_events[i]]["EventVersion"]);
                }
            }
        }

        [TestClass]
        public class When_getting_event_source_and_snapshot_and_events_exist : Test
        {
            private IEventSourceMapper _eventSourceMapper;
            private readonly Mock<IEventSourceFactory> _eventSourceFactory = new Mock<IEventSourceFactory>();
            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();
            private readonly Mock<IEventSource<Guid>> _eventSource = new Mock<IEventSource<Guid>>();
            private readonly Guid _id = Guid.NewGuid();
            private const int _version = 0;
            private readonly Mock<Memento<Guid>> _memento = new Mock<Memento<Guid>>();
            private readonly List<object> _events = new List<object> { new object() };
            private IEventSourceBase _returnedEventSource;

            protected override void Context()
            {
                _eventSourceMapper = new EventSourceMapper(_eventSourceFactory.Object, _eventStore.Object);

                _eventSourceFactory.Setup(f => f.Create<IEventSource<Guid>>()).Returns(_eventSource.Object);
                _eventSource.Setup(s => s.Id).Returns(_id);
                _eventSource.Setup(s => s.StringId).Returns(_id.ToString);
                _eventSource.Setup(s => s.Version).Returns(_version);
                _eventSource.Setup(s => s.BucketId).Returns(BucketSupport.DefaultBucketId);
                _eventStore.Setup(a => a.Read<Guid>(BucketSupport.DefaultBucketId, _id.ToString(), int.MaxValue)).Returns(this._memento.Object);
                _eventStore.Setup(a => a.Read(BucketSupport.DefaultBucketId, _id.ToString(), int.MaxValue)).Returns(this._events);
            }

            protected override void Event()
            {
                _returnedEventSource = _eventSourceMapper.Get<IEventSource<Guid>, Guid>(BucketSupport.DefaultBucketId, _id.ToString(), int.MaxValue);
            }

            [TestMethod]
            public void Should_create_event_source()
            {
                _eventSourceFactory.Verify(f => f.Create<IEventSource<Guid>>());
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
            private readonly Mock<IEventSource<Guid>> _eventSource = new Mock<IEventSource<Guid>>();
            private IEventSourceBase _returnedEventSource;

            protected override void Context()
            {
                _eventSourceMapper = new EventSourceMapper(_eventSourceFactory.Object, _eventStore.Object);

                _eventSourceFactory.Setup(f => f.Create<IEventSource<Guid>>()).Returns(_eventSource.Object);
            }

            protected override void Event()
            {
                _returnedEventSource = _eventSourceMapper.Get<IEventSource<Guid>, Guid>(BucketSupport.DefaultBucketId, Guid.NewGuid().ToString(), int.MaxValue);
            }

            [TestMethod]
            public void Should_create_event_source()
            {
                _eventSourceFactory.Verify(f => f.Create<IEventSource<Guid>>());
            }

            [TestMethod]
            public void Should_not_restore_snapshot()
            {
                _eventSource.Verify(s => s.RestoreSnapshot(It.IsAny<Memento<Guid>>()), Times.Never());
            }

            [TestMethod]
            public void Should_hydrate_event_source_with_events()
            {
                _eventSource.Verify(s => s.Hydrate(It.IsAny<IEnumerable<object>>()));
            }

            [TestMethod]
            public void Should_not_return_hydrated_event_source()
            {
                Assert.IsNull(_returnedEventSource);
            }
        }
    }
}