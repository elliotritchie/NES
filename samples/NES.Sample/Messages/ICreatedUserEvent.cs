using System;
using NServiceBus;

namespace NES.Sample.Messages
{
    public interface ICreatedUserEvent : IEvent
    {
        Guid UserId { get; set; }
        string Username { get; set; }
    }
}