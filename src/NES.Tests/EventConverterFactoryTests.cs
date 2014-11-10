using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES.Contracts;
using NES.Tests.Stubs;

namespace NES.Tests
{
    public static class EventConverterFactoryTests
    {
        [TestClass]
        public class When_converter_for_event_is_requested_and_event_converter_exists : Test
        {
            private readonly EventFactory _eventFactory = new EventFactory();
            private IEventConverterFactory _eventConverterFactory;
            private SomethingHappenedEvent _event;
            private Func<object, object> _converter;
            private Exception _ex;

            protected override void Context()
            {
                Global.TypesToScan = typeof(Test).Assembly.GetTypes();
                
                _eventConverterFactory = new EventConverterFactory();
                _event = _eventFactory.Create<SomethingHappenedEvent>(e => {});
            }

            protected override void Event()
            {
                try
                {
                    _converter = _eventConverterFactory.Get(typeof(SomethingHappenedEvent));
                    _converter(_event);
                }
                catch(Exception ex)
                {
                    _ex = ex;
                }
            }

            [TestMethod]
            public void Should_return_event_converter()
            {
                Assert.IsNotNull(_converter);
                Assert.IsNull(_ex);
            }
        }

        [TestClass]
        public class When_converter_for_event_is_requested_and_event_converter_doesnt_exist : Test
        {
            private IEventConverterFactory _eventConverterFactory;
            private Func<object, object> _converter;

            protected override void Context()
            {
                Global.TypesToScan = typeof(Test).Assembly.GetTypes();

                _eventConverterFactory = new EventConverterFactory();
            }

            protected override void Event()
            {
                _converter = _eventConverterFactory.Get(typeof(SomethingElseHappenedEvent));
            }

            [TestMethod]
            public void Should_return_null()
            {
                Assert.IsNull(_converter);
            }
        }
    }
}