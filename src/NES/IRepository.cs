using System;

namespace NES
{
    public interface IRepository
    {
        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;
        void Add<T>(T aggregate) where T : class, IEventSource;
    }
}