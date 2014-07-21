using System;

namespace NES
{
    public interface IMemento
    {
        string BucketId { get; set; }

        Guid Id { get; set; }

        int Version { get; set; }
    }
}