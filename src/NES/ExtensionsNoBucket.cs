using System;
using System.Collections.Generic;

namespace NES
{
    public static class ExtensionsNoBucket
    {
        public static T Get<T>(this IEventSourceMapper eventSourceMapper, Guid id) where T : class, IEventSource
        {
            return eventSourceMapper.Get<T>(BucketSupport.DefaultBucketId, id);
        }

        public static T Get<T>(this IUnitOfWork unitOfWork, Guid id) where T : class, IEventSource
        {
            return unitOfWork.Get<T>(id);
        }

        public static T Get<T>(this IRepository repository, Guid id) where T : class, IEventSource
        {
            return repository.Get<T>(BucketSupport.DefaultBucketId, id);
        }

        public static IMemento Read(this IEventStore eventStore, Guid id)
        {
            return eventStore.Read(BucketSupport.DefaultBucketId, id);
        }

        public static IEnumerable<object> Read(this IEventStore eventStore, Guid id, int version)
        {
            return eventStore.Read(BucketSupport.DefaultBucketId, id, version);
        }

        public static void Write(
            this IEventStore eventStore, 
            Guid id, 
            int version, 
            IEnumerable<object> events, 
            Guid commitId, 
            Dictionary<string, object> headers, 
            Dictionary<object, Dictionary<string, object>> eventHeaders)
        {
            eventStore.Write(BucketSupport.DefaultBucketId, id, version, events, commitId, headers, eventHeaders);
        }
    }
}