using System.Collections.Generic;
using System.Collections.ObjectModel;
using NES.Contracts;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Logging;
using NEventStore.Persistence;
using NEventStore.Serialization;
using Newtonsoft.Json;

namespace NES.NEventStore
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
                Logger.Debug("Wrapping serializer of type '{0}' in '{1}'", serializer.GetType(), typeof(CompositeSerializer));

                this.Container.Register<ISerialize>(new CompositeSerializer(serializer, () => DI.Current.Resolve<IEventSerializer>()));
            }

            Logger.Debug("Configuring the store to dispatch messages synchronously.");
            Logger.Debug("Registering dispatcher of type '{0}'.", typeof(MessageDispatcher));

            this.Container.Register<IDispatchCommits>(new MessageDispatcher(() => DI.Current.Resolve<IEventPublisher>()));
            this.Container.Register<IScheduleDispatches>(c =>
            {
                var dispatchScheduler = new SynchronousDispatchScheduler(
                    c.Resolve<IDispatchCommits>(),
                    c.Resolve<IPersistStreams>());
                if (c.Resolve<DispatcherSchedulerStartup>() == DispatcherSchedulerStartup.Auto)
                {
                    dispatchScheduler.Start();
                }

                return dispatchScheduler;
            });

            DI.Current.Register<IEventStore, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
        }

        public SerializationWireup UsingJsonSerialization()
        {
            Logger.Debug("Configuring custom NES Json serializer to cope with payloads that contain messages as interfaces.");

            return new SerializationWireup(this, new JsonSerializer(() => DI.Current.Resolve<IEventMapper>(), () => DI.Current.Resolve<IEventFactory>()));
        }

        public SerializationWireup UsingJsonSerialization(JsonSerializerSettings serializerSettings)
        {
            Logger.Debug("Configuring custom NES Json serializer to cope with payloads that contain messages as interfaces. SerializerSettings are also passed");

            return new SerializationWireup(this, new JsonSerializer(() => DI.Current.Resolve<IEventMapper>(), () => DI.Current.Resolve<IEventFactory>(), serializerSettings));
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

            var store = base.Build();

            DI.Current.Register(() => store);

            return store;
        }
    }
}