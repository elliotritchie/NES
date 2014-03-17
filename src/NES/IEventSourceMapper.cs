using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventSourceMapper
    {
        T Get<T>(Guid id) where T : class, IEventSource;
        T Get<T>(Guid id, Func<IEnumerable<object>, IEnumerable<object>> sortFilterAction) where T : class, IEventSource;
        IEnumerable<object> GetEvents(Guid id);
        void Set<T>(CommandContext commandContext, T eventSource) where T : class, IEventSource;
    }
}