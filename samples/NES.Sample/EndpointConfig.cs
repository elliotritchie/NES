using NES.NEventStore;
using NES.NServiceBus;
using NEventStore;
using NServiceBus;

namespace NES.Sample
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Wireup.Init()
                .UsingInMemoryPersistence()
                .NES()
                .Build();

            Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .JsonSerializer()
                .NES();
        }
    }
}