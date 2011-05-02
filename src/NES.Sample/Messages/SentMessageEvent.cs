using System;

namespace NES.Sample.Messages
{
    public interface SentMessageEvent : IEvent
    {
        Guid MessageId { get; set; }
        Guid UserId { get; set; }
        string Message { get; set; }
        DateTime Sent { get; set; }
    }
}