using System.Collections.Generic;

namespace NES.Contracts
{
    public interface IEventPublisher
    {
        void Publish(IEnumerable<object> events, IDictionary<string, object> headers, Dictionary<object, Dictionary<string, object>> eventHeaders);
    }
}