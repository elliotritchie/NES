using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Container.Register<IDispatchCommits>(c => new SynchronousDispatcher(c.Resolve<IPublishMessages>(), c.Resolve<IPersistStreams>()));

            DI.Current.Register<IEventStore, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
            DI.Current.Register(() => Container.Resolve<IStoreEvents>());
        }

        public override IStoreEvents Build()
        {
            var pipelineHooks = Container.Resolve<ICollection<IPipelineHook>>();
            var eventConverterPipelineHook = new EventConverterPipelineHook(() => DI.Current.Resolve<IEventConversionRunner>());

            if (pipelineHooks == null)
            {
                Container.Register((pipelineHooks = new Collection<IPipelineHook>()));
            }

            pipelineHooks.Add(eventConverterPipelineHook);

            return base.Build();
        }
    }
}