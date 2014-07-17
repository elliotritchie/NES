// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DI.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The di.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    /// <summary>
    ///     The di.
    /// </summary>
    public static class DI
    {
        #region Static Fields

        private static IDependencyInjectionContainer _current = new DependencyInjectionContainer();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes static members of the <see cref="DI" /> class.
        /// </summary>
        static DI()
        {
            _current.Register<IUnitOfWork, ICommandContextProvider, IEventSourceMapper>(
                (commandContextProvider, eventSourceMapper) => new UnitOfWork(commandContextProvider, eventSourceMapper));

            _current.Register<IEventSourceMapper, IEventSourceFactory, IEventStore, IEventConversionRunner>(
                (eventSourceFactory, eventStoreAdapter, eventConverterFactory) =>
                new EventSourceMapper(eventSourceFactory, eventStoreAdapter));

            _current.Register(NESConfigure.Initialize);
            _current.Register<IEventSourceFactory>(() => new EventSourceFactory());
            _current.Register<IEventFactory>(() => new EventFactory());
            _current.Register<IEventHandlerFactory>(() => new EventHandlerFactory());
            _current.Register<IEventConversionRunner, IEventConverterFactory>(
                eventConverterFactory => new EventConversionRunner(eventConverterFactory));
            _current.Register<IEventConverterFactory>(() => new EventConverterFactory());
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the current.
        /// </summary>
        public static IDependencyInjectionContainer Current
        {
            get
            {
                return _current;
            }

            internal set
            {
                _current = value;
            }
        }

        #endregion
    }
}