using System;

namespace NES.Tests.Stubs
{
    public interface CreatedAggregateStubEvent : IEvent
    {
        Guid Id { get; set; }
    }
}