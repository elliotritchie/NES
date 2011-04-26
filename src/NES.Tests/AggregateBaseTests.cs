using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NES.Tests
{
    public static class AggregateBaseTests
    {
        [TestClass]
        public class When_hydrating_and_events_are_supplied : Test
        {
            private AggregateStub _aggregate;
            private IEventSource _eventSource;
            private readonly EventFactory<IEvent> _eventFactory = new EventFactory<IEvent>();
            private readonly List<IEvent> _events = new List<IEvent>();
            
            protected override void Context()
            {
                _eventSource = _aggregate = new AggregateStub();
                _events.Add(_eventFactory.Create<SomethingHappenedEvent>(e => {}));
            }

            protected override void Event()
            {
                _eventSource.Hydrate(_events);
            }

            [TestMethod]
            public void Should_raise_events()
            {
                Assert.IsTrue(_aggregate.Events.SequenceEqual(_events));
            }

            [TestMethod]
            public void Should_increment_version()
            {
                Assert.AreEqual(_events.Count, _eventSource.Version);
            }
        }

        [TestClass]
        public class When_hydrating_and_events_are_not_supplied : Test
        {
            private AggregateStub _aggregate;
            private IEventSource _eventSource;
            private readonly List<IEvent> _events = new List<IEvent>();

            protected override void Context()
            {
                _eventSource = _aggregate = new AggregateStub();
            }

            protected override void Event()
            {
                _eventSource.Hydrate(_events);
            }

            [TestMethod]
            public void Should_not_raise_events()
            {
                Assert.IsTrue(_aggregate.Events.SequenceEqual(_events));
            }

            [TestMethod]
            public void Should_not_increment_version()
            {
                Assert.AreEqual(_events.Count, _eventSource.Version);
            }
        }

        [TestClass]
        public class When_applying_event : Test
        {
            private AggregateStub _aggregate;
            private IEventSource _eventSource;
            private const string _value = "qwerty";

            protected override void Context()
            {
                _eventSource = _aggregate = new AggregateStub();
            }

            protected override void Event()
            {
                _aggregate.DoSomething(_value);
            }

            [TestMethod]
            public void Should_raise_events()
            {
                Assert.AreEqual(1, _aggregate.Events.Count);
                Assert.AreEqual(_value, _aggregate.Events.OfType<SomethingHappenedEvent>().Single().Something);
            }
        }

        [TestClass]
        public class When_flushing_and_events_have_been_applied : Test
        {
            private AggregateStub _aggregate;
            private IEventSource _eventSource;
            private IEnumerable<IEvent> _flushedEvents;
            private const string _value = "qwerty";

            protected override void Context()
            {
                _eventSource = _aggregate = new AggregateStub();
                _aggregate.DoSomething(_value);
            }

            protected override void Event()
            {
                _flushedEvents = _eventSource.Flush().Cast<IEvent>();
            }

            [TestMethod]
            public void Should_return_flushed_events()
            {
                Assert.IsTrue(_flushedEvents.SequenceEqual(_aggregate.Events));
                Assert.AreEqual(_value, _flushedEvents.OfType<SomethingHappenedEvent>().Single().Something);
            }

            [TestMethod]
            public void Should_increment_version()
            {
                Assert.AreEqual(_flushedEvents.Count(), _eventSource.Version);
            }
        }

        [TestClass]
        public class When_flushing_and_no_events_have_been_applied : Test
        {
            private AggregateStub _aggregate;
            private IEventSource _eventSource;
            private IEnumerable<IEvent> _flushedEvents;

            protected override void Context()
            {
                _eventSource = _aggregate = new AggregateStub();
            }

            protected override void Event()
            {
                _flushedEvents = _eventSource.Flush().Cast<IEvent>();
            }

            [TestMethod]
            public void Should_return_no_events()
            {
                Assert.IsFalse(_flushedEvents.Any());
            }

            [TestMethod]
            public void Should_not_increment_version()
            {
                Assert.AreEqual(_flushedEvents.Count(), _eventSource.Version);
            }
        }

        public interface IEvent
        {
        }

        public interface SomethingHappenedEvent : IEvent
        {
            string Something { get; set; }
        }

        public class AggregateStub : AggregateBase<IEvent>
        {
            public readonly List<IEvent> Events = new List<IEvent>();

            public void DoSomething(string value)
            {
                Apply<SomethingHappenedEvent>(e =>
                {
                    e.Something = value;
                });
            }

            private void Handle(SomethingHappenedEvent @event)
            {
                Events.Add(@event);
            }
        }
    }
}