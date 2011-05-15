using System;

namespace NES.Tests.Stubs
{
    public interface CreatedAggregateEvent : IEvent
    {
        Guid Id { get; set; }
    }
}