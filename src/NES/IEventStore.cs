using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventStore
    {
        IMemento Read(string bucketId, Guid id);
        IEnumerable<object> Read(string bucketId, Guid id, int version);
        void Write(string bucketId, Guid id, int version, IEnumerable<object> events, Guid commitId, Dictionary<string, object> headers, Dictionary<object, Dictionary<string, object>> eventHeaders);
    }
}