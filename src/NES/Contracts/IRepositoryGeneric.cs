using System;

namespace NES.Contracts
{
    public interface IRepositoryGeneric<TId> : IRepositoryAdd
    {
        T Get<T>(string bucketId, TId id) where T : class, IEventSource<TId>;
        T Get<T>(string bucketId, TId id, int version) where T : class, IEventSource<TId>;
        T Get<T>(TId id) where T : class, IEventSource<TId>;
        T Get<T>(TId id, int version) where T : class, IEventSource<TId>;
    }
}