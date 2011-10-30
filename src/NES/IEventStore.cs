using System;
using System.Collections.Generic;

namespace NES
{
    public interface IEventStore
    {
        IMemento Read(Guid id);
        IEnumerable<object> Read(Guid id, int version);
        void Write(Guid id, int version, IEnumerable<object> events, Guid commitId, Dictionary<string, string> headers);
    }
}