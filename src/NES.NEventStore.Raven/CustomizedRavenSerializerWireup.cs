using System.Reflection;
using NES.Contracts;
using NEventStore;
using NEventStore.Logging;
using NEventStore.Persistence;
using Raven.Client;
using NEventStore.Persistence.RavenDB;

namespace NES.NEventStore.Raven
{
    public class CustomizedRavenSerializerWireup : Wireup
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(CustomizedRavenSerializerWireup));

        public CustomizedRavenSerializerWireup(NESWireup wireup) : base(wireup)
        {
        }

        public override IStoreEvents Build()
        {
            Logger.Debug("Configuring customized Raven serializer to cope with payloads that contain messages as interfaces.");

            var engine = (RavenPersistenceEngine)Container.Resolve<IPersistStreams>();
            var store = (IDocumentStore)engine.GetType().GetField("store", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(engine);

            store.Conventions.CustomizeJsonSerializer = s =>
            {
                s.Binder = new EventSerializationBinder(DI.Current.Resolve<IEventMapper>());
                s.ContractResolver = new EventContractResolver(DI.Current.Resolve<IEventMapper>(), DI.Current.Resolve<IEventFactory>());
            };

            return base.Build();
        }
    }
}