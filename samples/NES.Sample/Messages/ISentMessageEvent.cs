using System;
using NServiceBus;

namespace NES.Sample.Messages
{
    public interface ISentMessageEvent : IEvent
    {
        Guid MessageId { get; set; }
        Guid UserId { get; set; }
        string Message { get; set; }
        DateTime Sent { get; set; }
    }
}