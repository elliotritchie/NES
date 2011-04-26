using System;

namespace NES
{
    public interface IEventSourceMapper
    {
        T Get<T>(Guid id) where T : class, IEventSource;
        void Set<T>(T eventSource) where T : class, IEventSource;
    }
}