using System;

namespace NES
{
    public interface IEventSourceMapper
    {
        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;

        void Set<T>(CommandContext commandContext, T eventSource) where T : class, IEventSource;
    }
}