using System;
using System.Collections.Generic;

namespace NES.Contracts
{
    public interface IEventStore
    {
        TMemento Read<TMemento>(string bucketId, string id) where TMemento : class, IMementoBase;
        IEnumerable<object> Read(string bucketId, string id, int version);
        void Write(string bucketId, string id, int version, IEnumerable<object> events, Guid commitId, Dictionary<string, object> headers, Dictionary<object, Dictionary<string, object>> eventHeaders);
    }
}