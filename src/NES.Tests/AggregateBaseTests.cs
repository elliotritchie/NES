// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateBaseTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The aggregate base tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NES.Tests.Stubs;

    /// <summary>
    ///     The aggregate base tests.
    /// </summary>
    public static class AggregateBaseTests
    {
        /// <summary>
        ///     The when_applying_event.
        /// </summary>
        [TestClass]
        public class When_applying_event : Test
        {
            #region Constants

            private const string _value = "qwerty";

            #endregion

            #region Fields

            private AggregateStub _aggregate;

            private IEventSource _eventSource;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_raise_events.
            /// </summary>
            [TestMethod]
            public void Should_raise_events()
            {
                Assert.AreEqual(1, this._aggregate.HandledEvents.Count);
                Assert.AreEqual(_value, this._aggregate.HandledEvents.OfType<ISomethingHappenedEvent>().Single().Something);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSource = this._aggregate = new AggregateStub();
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._aggregate.DoSomething(_value);
            }

            #endregion
        }

        /// <summary>
        ///     The when_flushing_and_events_have_been_applied.
        /// </summary>
        [TestClass]
        public class When_flushing_and_events_have_been_applied : Test
        {
            #region Constants

            private const string _value = "qwerty";

            #endregion

            #region Fields

            private AggregateStub _aggregate;

            private IEventSource _eventSource;

            private IEnumerable<IEvent> _flushedEvents;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_increment_version.
            /// </summary>
            [TestMethod]
            public void Should_increment_version()
            {
                Assert.AreEqual(this._flushedEvents.Count(), this._eventSource.Version);
            }

            /// <summary>
            ///     The should_return_flushed_events.
            /// </summary>
            [TestMethod]
            public void Should_return_flushed_events()
            {
                Assert.IsTrue(this._flushedEvents.SequenceEqual(this._aggregate.HandledEvents));
                Assert.AreEqual(_value, this._flushedEvents.OfType<ISomethingHappenedEvent>().Single().Something);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSource = this._aggregate = new AggregateStub();
                this._aggregate.DoSomething(_value);
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._flushedEvents = this._eventSource.Flush().Cast<IEvent>();
            }

            #endregion
        }

        /// <summary>
        ///     The when_flushing_and_no_events_have_been_applied.
        /// </summary>
        [TestClass]
        public class When_flushing_and_no_events_have_been_applied : Test
        {
            #region Fields

            private AggregateStub _aggregate;

            private IEventSource _eventSource;

            private IEnumerable<IEvent> _flushedEvents;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_not_increment_version.
            /// </summary>
            [TestMethod]
            public void Should_not_increment_version()
            {
                Assert.AreEqual(this._flushedEvents.Count(), this._eventSource.Version);
            }

            /// <summary>
            ///     The should_return_no_events.
            /// </summary>
            [TestMethod]
            public void Should_return_no_events()
            {
                Assert.IsFalse(this._flushedEvents.Any());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSource = this._aggregate = new AggregateStub();
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._flushedEvents = this._eventSource.Flush().Cast<IEvent>();
            }

            #endregion
        }

        /// <summary>
        ///     The when_hydrating_and_events_are_not_supplied.
        /// </summary>
        [TestClass]
        public class When_hydrating_and_events_are_not_supplied : Test
        {
            #region Fields

            private readonly List<IEvent> _events = new List<IEvent>();

            private AggregateStub _aggregate;

            private IEventSource _eventSource;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_not_increment_version.
            /// </summary>
            [TestMethod]
            public void Should_not_increment_version()
            {
                Assert.AreEqual(this._events.Count, this._eventSource.Version);
            }

            /// <summary>
            ///     The should_not_raise_events.
            /// </summary>
            [TestMethod]
            public void Should_not_raise_events()
            {
                Assert.IsTrue(this._aggregate.HandledEvents.SequenceEqual(this._events));
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSource = this._aggregate = new AggregateStub();
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._eventSource.Hydrate(this._events);
            }

            #endregion
        }

        /// <summary>
        ///     The when_hydrating_and_events_are_supplied.
        /// </summary>
        [TestClass]
        public class When_hydrating_and_events_are_supplied : Test
        {
            #region Fields

            private readonly EventFactory _eventFactory = new EventFactory();

            private readonly List<IEvent> _events = new List<IEvent>();

            private AggregateStub _aggregate;

            private IEventSource _eventSource;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_increment_version.
            /// </summary>
            [TestMethod]
            public void Should_increment_version()
            {
                Assert.AreEqual(this._events.Count, this._eventSource.Version);
            }

            /// <summary>
            ///     The should_raise_events.
            /// </summary>
            [TestMethod]
            public void Should_raise_events()
            {
                Assert.IsTrue(this._aggregate.HandledEvents.SequenceEqual(this._events));
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventSource = this._aggregate = new AggregateStub();
                this._events.Add(this._eventFactory.Create<ISomethingHappenedEvent>(e => { }));
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._eventSource.Hydrate(this._events);
            }

            #endregion
        }
    }
}