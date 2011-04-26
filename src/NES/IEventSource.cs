using System;
using System.Collections;

namespace NES
{
    public interface IEventSource
    {
        Guid Id { get; }
        int Version { get; }
        void Hydrate(IEnumerable events);
        IEnumerable Flush();
    }
}