using System;

namespace NES.NEventStore.Tests.TestDomain
{
    public interface ICarCreated : IBucketId
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}