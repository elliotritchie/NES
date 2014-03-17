using System;
using System.Collections.Generic;

namespace NES
{
    public interface IUnitOfWork
    {
        T Get<T>(Guid id) where T : class, IEventSource;
        T Get<T>(Guid id, Func<IEnumerable<object>, IEnumerable<object>> sortFilterAction) where T : class, IEventSource;
        IEnumerable<object> GetEvents(Guid id);
        void Register<T>(T eventSource) where T : class, IEventSource;
        void Commit();
    }
}