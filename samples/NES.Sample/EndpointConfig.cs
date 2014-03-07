using NES.NEventStore;
using NES.NServiceBus;
using NEventStore;
using NServiceBus;

namespace NES.Sample
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization, IWantToRunWhenBusStartsAndStops
    {
        public void Init()
        {
            Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .JsonSerializer()
                .NES();
        }

        public void Start()
        {
            Wireup.Init()
                .UsingInMemoryPersistence()
                .NES()
                .Build();
        }

        public void Stop()
        {
        }
    }
}