using System;
using NServiceBus;

namespace NES.NEventStore.Tests.TestDomain
{
    public interface ICarPriceChanged : IEvent, IBucketId
    {
        Guid Id { get; set; }

        double Price { get; set; }
    }
}