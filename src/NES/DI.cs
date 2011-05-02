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
            _current.Register<IUnitOfWork, IEventSourceMapper>(eventSourceMapper => 
                new UnitOfWork(eventSourceMapper));
            
            _current.Register<IEventSourceMapper, IEventSourceFactory, IEventStore>((eventSourceFactory, eventStoreAdapter) => 
                new EventSourceMapper(eventSourceFactory, eventStoreAdapter));

            _current.Register<IEventSourceFactory>(() => new EventSourceFactory());
            _current.Register<IEventFactory>(() => new EventFactory());
            _current.Register<IEventHandlerFactory>(() => new EventHandlerFactory());
        }
    }
}