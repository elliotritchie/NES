using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventSource
    {
        Guid Id { get; }
        int Version { get; }
        void Hydrate(IMemento memento);
        void Hydrate(IEnumerable<object> events);
        IMemento TakeSnapshot();
        IEnumerable<object> Flush();
    }
}