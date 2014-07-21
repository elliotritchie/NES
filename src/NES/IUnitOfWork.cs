using System;

namespace NES
{
    public interface IUnitOfWork
    {
        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;
        void Register<T>(T eventSource) where T : class, IEventSource;
        void Commit();
    }
}