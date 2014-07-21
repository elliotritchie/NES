using System.Collections.Generic;

namespace NES
{
    public interface IEventPublisher
    {
        void Publish(
            IEnumerable<object> events, 
            IDictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders);
    }
}