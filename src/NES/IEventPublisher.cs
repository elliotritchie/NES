using System.Collections.Generic;

namespace NES
{
    public interface IEventPublisher
    {
        void Publish(IEnumerable<object> events);
    }
}