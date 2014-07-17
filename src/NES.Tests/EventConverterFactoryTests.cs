// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConverterFactoryTests.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event converter factory tests.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NES.Tests.Stubs;

    /// <summary>
    ///     The event converter factory tests.
    /// </summary>
    public static class EventConverterFactoryTests
    {
        /// <summary>
        ///     The when_converter_for_event_is_requested_and_event_converter_doesnt_exist.
        /// </summary>
        [TestClass]
        public class When_converter_for_event_is_requested_and_event_converter_doesnt_exist : Test
        {
            #region Fields

            private Func<object, object> _converter;

            private IEventConverterFactory _eventConverterFactory;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_return_null.
            /// </summary>
            [TestMethod]
            public void Should_return_null()
            {
                Assert.IsNull(this._converter);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                Global.TypesToScan = typeof(Test).Assembly.GetTypes();

                this._eventConverterFactory = new EventConverterFactory();
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                this._converter = this._eventConverterFactory.Get(typeof(ISomethingElseHappenedEvent));
            }

            #endregion
        }

        /// <summary>
        ///     The when_converter_for_event_is_requested_and_event_converter_exists.
        /// </summary>
        [TestClass]
        public class When_converter_for_event_is_requested_and_event_converter_exists : Test
        {
            #region Fields

            private readonly EventFactory _eventFactory = new EventFactory();

            private Func<object, object> _converter;

            private ISomethingHappenedEvent _event;

            private IEventConverterFactory _eventConverterFactory;

            private Exception _ex;

            #endregion

            #region Public Methods and Operators

            /// <summary>
            ///     The should_return_event_converter.
            /// </summary>
            [TestMethod]
            public void Should_return_event_converter()
            {
                Assert.IsNotNull(this._converter);
                Assert.IsNull(this._ex);
            }

            #endregion

            #region Methods

            /// <summary>
            ///     The context.
            /// </summary>
            protected override void Context()
            {
                Global.TypesToScan = typeof(Test).Assembly.GetTypes();

                this._eventConverterFactory = new EventConverterFactory();
                this._event = this._eventFactory.Create<ISomethingHappenedEvent>(e => { });
            }

            /// <summary>
            ///     The event.
            /// </summary>
            protected override void Event()
            {
                try
                {
                    this._converter = this._eventConverterFactory.Get(typeof(ISomethingHappenedEvent));
                    this._converter(this._event);
                }
                catch (Exception ex)
                {
                    this._ex = ex;
                }
            }

            #endregion
        }
    }
}