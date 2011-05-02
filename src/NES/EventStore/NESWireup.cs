using EventStore;
namespace NES.EventStore
{
    public class NESWireup : Wireup
    {
        public NESWireup(Wireup inner)
            : base(inner)
        {
            inner.UsingAsynchronousDispatcher(new MessagePublisher(() => DI.Current.Resolve<IEventPublisher>()));
            
            DI.Current.Register<IEventStore, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
            DI.Current.Register(() => Container.Resolve<IStoreEvents>());
        }
    }
}