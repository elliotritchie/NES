using System.Collections.Generic;
using NServiceBus;

namespace NES.NServiceBus
{
    public class BusAdapter : IEventPublisher
    {
        private readonly IBus _bus;

        public BusAdapter(IBus bus)
        {
            _bus = bus;
        }

        public void Publish(IEnumerable<object> events, Dictionary<string, object> headers, Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            foreach (var header in headers)
            {
                _bus.OutgoingHeaders[header.Key] = header.Value != null ? header.Value.ToString() : null;
            }

            foreach (var @event in events)
            {
                foreach (var header in eventHeaders[@event])
                {
                    _bus.SetMessageHeader(@event, header.Key, header.Value.ToString());
                }

                _bus.Publish(@event);
            }
        }
    }
}