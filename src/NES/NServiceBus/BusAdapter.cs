using System.Collections.Generic;
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

        public void Publish(IEnumerable<object> events)
        {
            foreach (var @event in events.Cast<IMessage>())
            {
                _bus.Publish(@event);
            }
        }
    }
}