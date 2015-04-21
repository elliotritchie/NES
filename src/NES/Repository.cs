using System;
using NES.Contracts;

namespace NES
{
    public class Repository : IRepository, StringWay.IRepository
    {
        public void Add<T>(T aggregate) where T : class, IEventSourceBase
        {
            UnitOfWorkFactory.Current.Register(aggregate);
        }

        public T Get<T>(string bucketId, string id) where T : class, IEventSource<string>
        {
            return UnitOfWorkFactory.Current.Get<T, string>(bucketId, id, int.MaxValue);
        }

        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource<Guid>
        {
            return UnitOfWorkFactory.Current.Get<T, Guid>(bucketId, id.ToString(), int.MaxValue);
        }

        public T Get<T>(Guid id) where T : class, IEventSource<Guid>
        {
            return UnitOfWorkFactory.Current.Get<T, Guid>(BucketSupport.DefaultBucketId, id.ToString(), int.MaxValue);
        }

        public T Get<T>(string id) where T : class, IEventSource<string>
        {
            return UnitOfWorkFactory.Current.Get<T, string>(BucketSupport.DefaultBucketId, id, Int32.MaxValue);
        }


        public T Get<T>(Guid id, int version) where T : class, IEventSource<Guid>
        {
            return UnitOfWorkFactory.Current.Get<T, Guid>(BucketSupport.DefaultBucketId, id.ToString(), version);
        }


        public T Get<T>(string id, int version) where T : class, IEventSource<string>
        {
            return UnitOfWorkFactory.Current.Get<T, string>(BucketSupport.DefaultBucketId, id, version);
        }


        public T Get<T>(string bucketId, string id, int version) where T : class, IEventSource<string>
        {
            return UnitOfWorkFactory.Current.Get<T, string>(bucketId, id, version);
        }


        public T Get<T>(string bucketId, Guid id, int version) where T : class, IEventSource<Guid>
        {
            return UnitOfWorkFactory.Current.Get<T, Guid>(bucketId, id.ToString(), version);
        }
    }
}