using System;

namespace NES
{
    public interface IMemento
    {
        Guid Id { get; set; }
        string BucketId { get; set; }
        int Version { get; set; }
    }
}