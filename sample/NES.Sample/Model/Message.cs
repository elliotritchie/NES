using System;
using NES.Sample.Messages;

namespace NES.Sample.Model
{
    public class Message : Aggregate
    {
        public Message(User user, Guid messageId, string message)
        {
            Apply<SentMessageEvent>(e =>
            {
                e.MessageId = messageId;
                e.UserId = user.Id;
                e.Message = message;
                e.Sent = DateTime.UtcNow;
            });
        }

        private Message()
        {
        }

        private void Handle(SentMessageEvent @event)
        {
            Id = @event.MessageId;
        }
    }
}