using System;
using NES.Sample.Messages;

namespace NES.Sample.Model
{
    public class Message : AggregateBase
    {
        public Message(User user, Guid messageId, string message)
        {
            Apply<ISentMessageEvent>(e =>
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

        private void Handle(ISentMessageEvent @event)
        {
            Id = @event.MessageId;
        }
    }
}