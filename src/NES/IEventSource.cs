using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventSource
    {
        Guid Id { get; }
        int Version { get; }
        void RestoreSnapshot(IMemento memento);
        IMemento TakeSnapshot();
        void Hydrate(IEnumerable<object> events);
        IEnumerable<object> Flush();
    }
}