using System;

namespace NES
{
    public interface IRepository
    {
        void Add<T>(T aggregate) where T : class, IEventSource;

        T Get<T>(string bucketId, Guid id) where T : class, IEventSource;
    }
}