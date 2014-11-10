using System.Collections.Generic;

namespace NES.Contracts
{
    public interface IEventSourceBase : IStringId
    {

        string BucketId { get; }

        int Version { get; }

        void Hydrate(IEnumerable<object> events);
        IEnumerable<object> Flush();
    }
}