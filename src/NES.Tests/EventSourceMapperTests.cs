// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSourceMapperTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event source mapper tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System;
    using System.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    /// <summary>
    ///     The event source mapper tests.
    /// </summary>
    public static class EventSourceMapperTests
    {
        /// <summary>
        ///     The when_getting_event_source_and_snapshot_and_events_dont_exist.
        /// </summary>
        [TestClass]
        public class When_getting_event_source_and_snapshot_and_events_dont_exist : Test
        {
            #region Fields

            private readonly Mock<IEventSource> _eventSource = new Mock<IEventSource>();

            private readonly Mock<IEventSourceFactory> _eventSourceFactory = new Mock<IEventSourceFactory>();

            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();

            private IEventSourceMapper _eventSourceMapper;

            private IEventSource _returnedEventSource;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_create_event_source.
            /// </summary>
            [TestMethod]
            public void Should_create_event_source()
            {
                this._eventSourceFactory.Verify(f => f.Create<IEventSource>());
            }

            /// <summary>
            ///     The should_hydrate_event_source_with_events.
            /// </summary>
            [TestMethod]
            public void Should_hydrate_event_source_with_events()
            {
                this._eventSource.Verify(s => s.Hydrate(It.IsAny<IEnumerable<object>>()));
            }

            /// <summary>
            ///     The should_not_restore_snapshot.
            /// </summary>
            [TestMethod]
            public void Should_not_restore_snapshot()
            {
                this._eventSource.Verify(s => s.RestoreSnapshot(It.IsAny<IMemento>()), Times.Never());
            }

            /// <summary>
            ///     The should_not_return_hydrated_event_source.
            /// </summary>
            [TestMethod]
            public void Should_not_return_hydrated_event_source()
            {
                Assert.IsNull(this._returnedEventSource);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSourceMapper = new EventSourceMapper(this._eventSourceFactory.Object, this._eventStore.Object);

                this._eventSourceFactory.Setup(f => f.Create<IEventSource>()).Returns(this._eventSource.Object);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._returnedEventSource = this._eventSourceMapper.Get<IEventSource>(BucketSupport.DefaultBucketId, Guid.NewGuid());
            }

            #endregion
        }

        /// <summary>
        ///     The when_getting_event_source_and_snapshot_and_events_exist.
        /// </summary>
        [TestClass]
        public class When_getting_event_source_and_snapshot_and_events_exist : Test
        {
            #region Constants

            private const int _version = 0;

            #endregion

            #region Fields

            private readonly Mock<IEventSource> _eventSource = new Mock<IEventSource>();

            private readonly Mock<IEventSourceFactory> _eventSourceFactory = new Mock<IEventSourceFactory>();

            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();

            private readonly List<object> _events = new List<object> { new object() };

            private readonly Guid _id = Guid.NewGuid();

            private readonly Mock<IMemento> _memento = new Mock<IMemento>();

            private IEventSourceMapper _eventSourceMapper;

            private IEventSource _returnedEventSource;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_create_event_source.
            /// </summary>
            [TestMethod]
            public void Should_create_event_source()
            {
                this._eventSourceFactory.Verify(f => f.Create<IEventSource>());
            }

            /// <summary>
            ///     The should_hydrate_event_source_with_events.
            /// </summary>
            [TestMethod]
            public void Should_hydrate_event_source_with_events()
            {
                this._eventSource.Verify(s => s.Hydrate(this._events));
            }

            /// <summary>
            ///     The should_restore_snapshot.
            /// </summary>
            [TestMethod]
            public void Should_restore_snapshot()
            {
                this._eventSource.Verify(s => s.RestoreSnapshot(this._memento.Object));
            }

            /// <summary>
            ///     The should_return_hydrated_event_source.
            /// </summary>
            [TestMethod]
            public void Should_return_hydrated_event_source()
            {
                Assert.AreSame(this._eventSource.Object, this._returnedEventSource);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSourceMapper = new EventSourceMapper(this._eventSourceFactory.Object, this._eventStore.Object);

                this._eventSourceFactory.Setup(f => f.Create<IEventSource>()).Returns(this._eventSource.Object);
                this._eventSource.Setup(s => s.Id).Returns(this._id);
                this._eventSource.Setup(s => s.Version).Returns(_version);
                this._eventSource.Setup(s => s.BucketId).Returns(BucketSupport.DefaultBucketId);
                this._eventStore.Setup(a => a.Read(BucketSupport.DefaultBucketId, this._id)).Returns(this._memento.Object);
                this._eventStore.Setup(a => a.Read(BucketSupport.DefaultBucketId, this._id, _version)).Returns(this._events);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._returnedEventSource = this._eventSourceMapper.Get<IEventSource>(this._id);
            }

            #endregion
        }

        /// <summary>
        ///     The when_setting_event_source_and_events_exist.
        /// </summary>
        [TestClass]
        public class When_setting_event_source_and_events_exist : Test
        {
            #region Constants

            private const int _version = 2;

            #endregion

            #region Fields

            private readonly CommandContext _commandContext = new CommandContext();

            private readonly Guid _commitId = GuidComb.NewGuidComb();

            private readonly Mock<IEventSource> _eventSource = new Mock<IEventSource>();

            private readonly Mock<IEventStore> _eventStore = new Mock<IEventStore>();

            private readonly List<object> _events = new List<object> { new object(), new object() };

            private readonly Guid _id = GuidComb.NewGuidComb();

            private Dictionary<object, Dictionary<string, object>> _committedEventHeaders;

            private Dictionary<string, object> _committedHeaders;

            private IEventSourceMapper _eventSourceMapper;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_commit_event_headers_to_event_store.
            /// </summary>
            [TestMethod]
            public void Should_commit_event_headers_to_event_store()
            {
                for (int i = 0; i < this._events.Count; i++)
                {
                    Assert.AreEqual(_version + i + 1, this._committedEventHeaders[this._events[i]]["EventVersion"]);
                }
            }

            /// <summary>
            ///     The should_commit_headers_to_event_store.
            /// </summary>
            [TestMethod]
            public void Should_commit_headers_to_event_store()
            {
                Assert.AreEqual("TestValue", this._committedHeaders["TestKey"]);
                Assert.AreEqual(this._id, this._committedHeaders["AggregateId"]);
                Assert.AreEqual(BucketSupport.DefaultBucketId, this._committedHeaders["AggregateBucketId"]);
                Assert.AreEqual(_version + this._events.Count, this._committedHeaders["AggregateVersion"]);
                Assert.AreEqual(this._eventSource.Object.GetType().FullName, this._committedHeaders["AggregateType"]);
            }

            /// <summary>
            ///     The should_commit_to_event_store.
            /// </summary>
            [TestMethod]
            public void Should_commit_to_event_store()
            {
                this._eventStore.Verify(
                    s => s.Write(
                        BucketSupport.DefaultBucketId,
                        this._id,
                        _version,
                        this._events,
                        this._commitId,
                        It.IsAny<Dictionary<string, object>>(),
                        It.IsAny<Dictionary<object, Dictionary<string, object>>>()));
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSourceMapper = new EventSourceMapper(null, this._eventStore.Object);

                this._commandContext.Id = this._commitId;
                this._commandContext.Headers = new Dictionary<string, object> { { "TestKey", "TestValue" } };
                this._eventSource.Setup(s => s.Id).Returns(this._id);
                this._eventSource.Setup(s => s.BucketId).Returns(BucketSupport.DefaultBucketId);
                this._eventSource.Setup(s => s.Version).Returns(_version);
                this._eventSource.Setup(s => s.Flush()).Returns(this._events).Callback(() => this._eventSource.Setup(s => s.Version).Returns(_version + this._events.Count));
                
                this._eventStore.Setup(
                    s => s.Write(
                        It.IsAny<string>(),
                        It.IsAny<Guid>(),
                        It.IsAny<int>(),
                        It.IsAny<IEnumerable<object>>(),
                        It.IsAny<Guid>(),
                        It.IsAny<Dictionary<string, object>>(),
                        It.IsAny<Dictionary<object, Dictionary<string, object>>>()))
                    .Callback<string, Guid, int, IEnumerable<object>, Guid, Dictionary<string, object>, Dictionary<object, Dictionary<string, object>>>(
                        (a, b, c, d, e, f, g) =>
                        {
                            this._committedHeaders = f;
                            this._committedEventHeaders = g;
                        });
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._eventSourceMapper.Set(this._commandContext, this._eventSource.Object);
            }

            #endregion
        }
    }
}