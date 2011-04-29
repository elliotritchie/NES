using System.Linq;
using NServiceBus;

namespace NES.NServiceBus
{
    public class BusAdapter : IBusAdapter
    {
        private readonly IBus _bus;

        public BusAdapter(IBus bus)
        {
            _bus = bus;
        }

        public void Publish(params object[] events)
        {
            _bus.Publish(events.Cast<IMessage>().ToArray());
        }
    }
}