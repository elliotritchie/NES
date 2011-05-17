using EventStore;
using EventStore.Dispatcher;
using EventStore.Persistence;
using EventStore.Serialization;

namespace NES.EventStore
{
    public class NESWireup : Wireup
    {
        public NESWireup(Wireup inner)
            : base(inner)
        {
            Container.Register<ISerialize>(new Serializer(Container.Resolve<ISerialize>(), () => DI.Current.Resolve<IEventSerializer>()));
            Container.Register<IPublishMessages>(new MessagePublisher(() => DI.Current.Resolve<IEventPublisher>()));
            Container.Register<IDispatchCommits>(c => new AsynchronousDispatcher(c.Resolve<IPublishMessages>(), c.Resolve<IPersistStreams>()));

            DI.Current.Register<IEventStore, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
            DI.Current.Register(() => Container.Resolve<IStoreEvents>());
        }
    }
}