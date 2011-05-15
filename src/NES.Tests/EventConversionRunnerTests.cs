using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Tests.Stubs;

namespace NES.Tests
{
    public static class EventConversionRunnerTests
    {
        [TestClass]
        public class When_event_runner_is_run_and_converters_exist : Test
        {
            private readonly Mock<IEventConverterFactory> _eventConverterFactory = new Mock<IEventConverterFactory>();
            private IEventConversionRunner _eventConversionRunner;
            private readonly EventFactory _eventFactory = new EventFactory();
            private SomethingHappenedEvent _somethingHappenedEvent;
            private SomethingElseHappenedEvent _somethingElseHappenedEvent;
            private readonly List<IEvent> _convertedEvents = new List<IEvent>();

            protected override void Context()
            {
                _eventConversionRunner = new EventConversionRunner(_eventConverterFactory.Object);
                _somethingHappenedEvent = _eventFactory.Create<SomethingHappenedEvent>(e => {});
                _somethingElseHappenedEvent = _eventFactory.Create<SomethingElseHappenedEvent>(e => {});

                _eventConverterFactory.Setup(f => f.Get(typeof(SomethingHappenedEvent))).Returns(e =>
                {
                    _convertedEvents.Add((IEvent)e);
                    return _somethingElseHappenedEvent;
                });
            }

            protected override void Event()
            {
                _eventConversionRunner.Run(new[] { _somethingHappenedEvent }).ToList();
            }

            [TestMethod]
            public void Should_get_delegate_from_event_converter_factory_for_SomethingHappenedEvent_once()
            {
                _eventConverterFactory.Verify(f => f.Get(typeof(SomethingHappenedEvent)), Times.Once());
            }

            [TestMethod]
            public void Should_invoke_delegate()
            {
                Assert.AreSame(_somethingHappenedEvent, _convertedEvents.Single());
            }

            [TestMethod]
            public void Should_try_and_get_delegate_from_event_converter_factory_for_SomethingElseHappenedEvent_once()
            {
                _eventConverterFactory.Verify(f => f.Get(typeof(SomethingElseHappenedEvent)), Times.Once());
            }
        }

        [TestClass]
        public class When_event_runner_is_run_and_converters_dont_exist : Test
        {
            private readonly Mock<IEventConverterFactory> _eventConverterFactory = new Mock<IEventConverterFactory>();
            private IEventConversionRunner _eventConversionRunner;
            private readonly EventFactory _eventFactory = new EventFactory();
            private SomethingHappenedEvent _somethingHappenedEvent;

            protected override void Context()
            {
                _eventConversionRunner = new EventConversionRunner(_eventConverterFactory.Object);
                _somethingHappenedEvent = _eventFactory.Create<SomethingHappenedEvent>(e => {});
            }

            protected override void Event()
            {
                _eventConversionRunner.Run(new[] { _somethingHappenedEvent }).ToList();
            }

            [TestMethod]
            public void Should_try_and_get_delegate_from_event_converter_factory_for_SomethingHappenedEvent_once()
            {
                _eventConverterFactory.Verify(f => f.Get(typeof(SomethingHappenedEvent)), Times.Once());
            }

            [TestMethod]
            public void Should_not_try_and_get_delegate_from_event_converter_factory_for_SomethingElseHappenedEvent()
            {
                _eventConverterFactory.Verify(f => f.Get(typeof(SomethingElseHappenedEvent)), Times.Never());
            }
        }
    }
}