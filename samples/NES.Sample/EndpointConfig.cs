using NES.NEventStore;
using NES.NServiceBus;
using NES.Sample.Data;
using NES.Sample.Services;
using NEventStore;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Log4Net;
using NServiceBus.Logging;
using NServiceBus.Persistence.Legacy;
using Json = NServiceBus.JsonSerializer;

namespace NES.Sample
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server, IWantToRunWhenBusStartsAndStops, IWantToRunWhenConfigurationIsComplete
    {
        public void Init()
        {
            LogManager.Use<Log4NetFactory>();
        }

        public void Start()
        {
            Wireup.Init()
                .UsingInMemoryPersistence()
                .EnlistInAmbientTransaction()
                .NES()
                .Build();
        }

        public void Stop()
        {
        }

        public void Customize(BusConfiguration configuration)
        {
            configuration.UseSerialization<Json>();
            configuration.EnableInstallers();
            configuration.UsePersistence<InMemoryPersistence>();
            configuration.UseTransport<MsmqTransport>();
            configuration.PurgeOnStartup(false);
            configuration.RegisterComponents(c =>
            {
                c.ConfigureComponent<AuthenticationService>(DependencyLifecycle.InstancePerUnitOfWork);
                c.ConfigureComponent<Repository>(DependencyLifecycle.InstancePerUnitOfWork);
                c.ConfigureComponent<ValidationService>(DependencyLifecycle.SingleInstance);
                c.ConfigureComponent<MutateIncomingTransportMessages>(DependencyLifecycle.InstancePerCall);
                c.ConfigureComponent<ValidateIncomingMessages>(DependencyLifecycle.InstancePerCall);
                c.ConfigureComponent<DataRepository>(DependencyLifecycle.InstancePerUnitOfWork);
            });
        }

        public void Run(Configure config)
        {
            config.NES();
        }
    }
}