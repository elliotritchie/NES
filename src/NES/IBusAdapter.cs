using System.Collections.Generic;

namespace NES
{
    public interface IBusAdapter
    {
        void Publish(IEnumerable<object> events);
    }
}