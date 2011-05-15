using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES.Tests.Stubs;

namespace NES.Tests
{
    public static class EventConverterFactoryTests
    {
        [TestClass]
        public class When_converter_for_event_is_requested_and_event_converter_exists : Test
        {
            private readonly IEventConverterFactory _eventConverterFactory = new EventConverterFactory();
            private readonly EventFactory _eventFactory = new EventFactory();
            private SomethingHappenedEvent _event;
            private Func<object, object> _delegate;
            private Exception _ex;

            protected override void Context()
            {
                _event = _eventFactory.Create<SomethingHappenedEvent>(e => {});
            }

            protected override void Event()
            {
                try
                {
                    _delegate = _eventConverterFactory.Get(typeof(SomethingHappenedEvent));
                    _delegate(_event);
                }
                catch(Exception ex)
                {
                    _ex = ex;
                }
            }

            [TestMethod]
            public void Should_return_event_converter_delegate()
            {
                Assert.IsNotNull(_delegate);
                Assert.IsNull(_ex);
            }
        }

        [TestClass]
        public class When_converter_for_event_is_requested_and_event_converter_doesnt_exist : Test
        {
            private readonly IEventConverterFactory _eventConverterFactory = new EventConverterFactory();
            private Func<object, object> _delegate;

            protected override void Context()
            {
            }

            protected override void Event()
            {
                _delegate = _eventConverterFactory.Get(typeof(SomethingElseHappenedEvent));
            }

            [TestMethod]
            public void Should_return_null()
            {
                Assert.IsNull(_delegate);
            }
        }
    }
}