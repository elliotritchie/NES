using System;

namespace NES.Contracts
{
    public interface IRepository : IRepositoryAdd
    {
        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;
        T Get<T>(Guid id) where T : class, IEventSource;
    }
}