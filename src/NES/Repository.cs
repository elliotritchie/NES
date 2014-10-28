using System;

namespace NES
{
    public class Repository : IRepository
    {
        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T>(bucketId, id);
        }

        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            UnitOfWorkFactory.Current.Register(aggregate);
        }
    }
}