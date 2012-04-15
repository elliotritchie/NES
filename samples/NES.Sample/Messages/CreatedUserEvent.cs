using System;
using NServiceBus;

namespace NES.Sample.Messages
{
    public interface CreatedUserEvent : IEvent
    {
        Guid UserId { get; set; }
        string Username { get; set; }
    }
}