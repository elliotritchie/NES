using EventStore;
using NES.EventStore;
using NES.NServiceBus;
using NServiceBus;

namespace NES.Sample
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Wireup.Init()
                .UsingInMemoryPersistence()
                .UsingBinarySerialization()
                .NES()
                .Build();

            Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .XmlSerializer("http://getnes.net")
                .NES();
        }
    }
}