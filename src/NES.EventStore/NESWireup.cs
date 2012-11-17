using System.Collections.Generic;
using System.Collections.ObjectModel;
using EventStore;
using EventStore.Dispatcher;
using EventStore.Logging;
using EventStore.Persistence;
using EventStore.Serialization;

namespace NES.EventStore
{
    public class NESWireup : Wireup
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(NESWireup));

        public NESWireup(Wireup wireup) 
            : base(wireup)
        {
            var serializer = Container.Resolve<ISerialize>();

            if (serializer != null)
            {
                Logger.Debug("Configuring custom NES serializer to cope with payloads that contain messages as interfaces.");
                Logger.Debug("Wrapping serializer of type '" + serializer.GetType() + "' in '" + typeof(CompositeSerializer) + "'");

                Container.Register<ISerialize>(new CompositeSerializer(serializer, () => DI.Current.Resolve<IEventSerializer>()));
            }

            Logger.Debug("Configuring the store to dispatch messages synchronously.");
            Logger.Debug("Registering dispatcher of type '" + typeof(MessageDispatcher) + "'.");

            Container.Register<IScheduleDispatches>(c => new SynchronousDispatchScheduler(new MessageDispatcher(() => DI.Current.Resolve<IEventPublisher>()), c.Resolve<IPersistStreams>()));

            DI.Current.Register<IEventStore, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
            DI.Current.Register(() => Container.Resolve<IStoreEvents>());
        }

        public SerializationWireup UsingJsonSerialization()
        {
            Logger.Debug("Configuring custom NES Json serializer to cope with payloads that contain messages as interfaces.");

            return new SerializationWireup(this, new JsonSerializer(() => DI.Current.Resolve<IEventMapper>(), () => DI.Current.Resolve<IEventFactory>()));
        }

        public SerializationWireup UsingBsonSerialization()
        {
            Logger.Debug("Configuring custom NES Bson serializer to cope with payloads that contain messages as interfaces.");

            return new SerializationWireup(this, new BsonSerializer(() => DI.Current.Resolve<IEventMapper>(), () => DI.Current.Resolve<IEventFactory>()));
        }

        public override IStoreEvents Build()
        {
            Logger.Debug("Configuring the store to upconvert events when fetched.");

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