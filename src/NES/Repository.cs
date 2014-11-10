using System;
using NES.Contracts;

namespace NES
{
    public class Repository : IRepository, StringWay.IRepository
    {
        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            UnitOfWorkFactory.Current.Register(aggregate);
        }

        public T Get<T>(string bucketId, string id) where T : class, StringWay.IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T, string, StringWay.IMemento>(bucketId, id);
        }

        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T, Guid, IMemento>(bucketId, id.ToString());
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return this.Get<T>(BucketSupport.DefaultBucketId, id);
        }

        public T Get<T>(string id) where T : class, StringWay.IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T, string, StringWay.IMemento>(BucketSupport.DefaultBucketId, id);
        }
    }
}