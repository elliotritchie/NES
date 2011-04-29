namespace NES
{
    public static class DI
    {
        private static IDependencyInjectionContainer _current = new DependencyInjectionContainer();
        public static IDependencyInjectionContainer Current
        {
            get { return _current; }
            set { _current = value; }
        }

        static DI()
        {
            _current.Register<IUnitOfWork, IEventSourceMapper>(eventSourceMapper => 
                new UnitOfWork(eventSourceMapper));
            
            _current.Register<IEventSourceMapper, IEventSourceFactory, IEventStoreAdapter>((eventSourceFactory, eventStoreAdapter) => 
                new EventSourceMapper(eventSourceFactory, eventStoreAdapter));
        }
    }
}