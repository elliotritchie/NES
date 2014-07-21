using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventSource
    {
        string BucketId { get; }

        Guid Id { get; }

        int Version { get; }

        IEnumerable<object> Flush();

        void Hydrate(IEnumerable<object> events);

        void RestoreSnapshot(IMemento memento);

        IMemento TakeSnapshot();
    }
}