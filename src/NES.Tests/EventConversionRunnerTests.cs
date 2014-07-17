// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConversionRunnerTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event conversion runner tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using NES.Tests.Stubs;

    /// <summary>
    ///     The event conversion runner tests.
    /// </summary>
    public static class EventConversionRunnerTests
    {
        /// <summary>
        ///     The when_event_runner_is_run_and_converters_dont_exist.
        /// </summary>
        [TestClass]
        public class When_event_runner_is_run_and_converters_dont_exist : Test
        {
            #region Fields

            private readonly Mock<IEventConverterFactory> _eventConverterFactory = new Mock<IEventConverterFactory>();

            private readonly EventFactory _eventFactory = new EventFactory();

            private IEventConversionRunner _eventConversionRunner;

            private ISomethingHappenedEvent _somethingHappenedEvent;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_not_try_and_get_delegate_from_event_converter_factory_for_ something else happened event.
            /// </summary>
            [TestMethod]
            public void Should_not_try_and_get_delegate_from_event_converter_factory_for_SomethingElseHappenedEvent()
            {
                this._eventConverterFactory.Verify(f => f.Get(typeof(ISomethingElseHappenedEvent)), Times.Never());
            }

            /// <summary>
            ///     The should_try_and_get_delegate_from_event_converter_factory_for_ something happened event_once.
            /// </summary>
            [TestMethod]
            public void Should_try_and_get_delegate_from_event_converter_factory_for_SomethingHappenedEvent_once()
            {
                this._eventConverterFactory.Verify(f => f.Get(typeof(ISomethingHappenedEvent)), Times.Once());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventConversionRunner = new EventConversionRunner(this._eventConverterFactory.Object);
                this._somethingHappenedEvent = this._eventFactory.Create<ISomethingHappenedEvent>(e => { });
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._eventConversionRunner.Run(this._somethingHappenedEvent);
            }

            #endregion
        }

        /// <summary>
        ///     The when_event_runner_is_run_and_converters_exist.
        /// </summary>
        [TestClass]
        public class When_event_runner_is_run_and_converters_exist : Test
        {
            #region Fields

            private readonly List<IEvent> _convertedEvents = new List<IEvent>();

            private readonly Mock<IEventConverterFactory> _eventConverterFactory = new Mock<IEventConverterFactory>();

            private readonly EventFactory _eventFactory = new EventFactory();

            private IEventConversionRunner _eventConversionRunner;

            private ISomethingElseHappenedEvent _somethingElseHappenedEvent;

            private ISomethingHappenedEvent _somethingHappenedEvent;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_get_delegate_from_event_converter_factory_for_ something happened event_once.
            /// </summary>
            [TestMethod]
            public void Should_get_delegate_from_event_converter_factory_for_SomethingHappenedEvent_once()
            {
                this._eventConverterFactory.Verify(f => f.Get(typeof(ISomethingHappenedEvent)), Times.Once());
            }

            /// <summary>
            ///     The should_invoke_delegate.
            /// </summary>
            [TestMethod]
            public void Should_invoke_delegate()
            {
                Assert.AreSame(this._somethingHappenedEvent, this._convertedEvents.Single());
            }

            /// <summary>
            ///     The should_try_and_get_delegate_from_event_converter_factory_for_ something else happened event_once.
            /// </summary>
            [TestMethod]
            public void Should_try_and_get_delegate_from_event_converter_factory_for_SomethingElseHappenedEvent_once()
            {
                this._eventConverterFactory.Verify(f => f.Get(typeof(ISomethingElseHappenedEvent)), Times.Once());
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                this._eventConversionRunner = new EventConversionRunner(this._eventConverterFactory.Object);
                this._somethingHappenedEvent = this._eventFactory.Create<ISomethingHappenedEvent>(e => { });
                this._somethingElseHappenedEvent = this._eventFactory.Create<ISomethingElseHappenedEvent>(e => { });

                this._eventConverterFactory.Setup(f => f.Get(typeof(ISomethingHappenedEvent))).Returns(
                    e =>
                        {
                            this._convertedEvents.Add((IEvent)e);
                            return this._somethingElseHappenedEvent;
                        });
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._eventConversionRunner.Run(this._somethingHappenedEvent);
            }

            #endregion
        }
    }
}