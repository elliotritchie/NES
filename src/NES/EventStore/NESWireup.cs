using EventStore;
namespace NES.EventStore
{
    public class NESWireup : Wireup
    {
        public NESWireup(Wireup inner)
            : base(inner)
        {
            inner.UsingAsynchronousDispatcher(new MessagePublisher(() => DI.Current.Resolve<IBusAdapter>()));
            
            DI.Current.Register<IEventStoreAdapter, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
            DI.Current.Register(() => Container.Resolve<IStoreEvents>());
        }
    }
}