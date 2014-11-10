using NES.Contracts;

namespace NES
{
    public static class DI
    {
        private static IDependencyInjectionContainer _current = new DependencyInjectionContainer();
        public static IDependencyInjectionContainer Current
        {
            get { return _current; }
            internal set { _current = value; }
        }

        static DI()
        {
            _current.Register<IUnitOfWork, ICommandContextProvider, IEventSourceMapper>((commandContextProvider, eventSourceMapper) =>
                new UnitOfWork(commandContextProvider, eventSourceMapper));
            
            _current.Register<IEventSourceMapper, IEventSourceFactory, IEventStore, IEventConversionRunner>((eventSourceFactory, eventStoreAdapter, eventConverterFactory) =>
                new EventSourceMapper(eventSourceFactory, eventStoreAdapter));

            _current.Register<IEventSourceFactory>(() => new EventSourceFactory());
            _current.Register<IEventFactory>(() => new EventFactory());
            _current.Register<IEventHandlerFactory>(() => new EventHandlerFactory());
            _current.Register<IEventConversionRunner, IEventConverterFactory>(eventConverterFactory => new EventConversionRunner(eventConverterFactory));
            _current.Register<IEventConverterFactory>(() => new EventConverterFactory());
        }
    }
}