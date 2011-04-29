using EventStore;
using EventStore.Dispatcher;

namespace NES.EventStore
{
    public class NESWireup : Wireup
    {
        public NESWireup(Wireup inner)
			: base(inner)
		{
            DI.Current.Register<IEventStoreAdapter, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
            DI.Current.Register(() => Container.Resolve<IStoreEvents>());
		}
    }
}