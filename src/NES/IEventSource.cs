using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventSource<T>
    {
        Guid Id { get; }
        int Version { get; }
        void Hydrate(IEnumerable<T> events);
        IEnumerable<T> Flush();
    }
}