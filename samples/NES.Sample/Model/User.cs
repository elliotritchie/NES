using System;
using NES.Sample.Messages;

namespace NES.Sample.Model
{
    public class User : AggregateBase
    {
        public User(Guid userId, string username)
        {
            Apply<CreatedUserEvent>(e =>
            {
                e.UserId = userId;
                e.Username = username;
            });
        }

        private User()
        {
        }

        public Message SendMessage(Guid messageId, string message)
        {
            return new Message(this, messageId, message);
        }

        private void Handle(CreatedUserEvent @event)
        {
            Id = @event.UserId;
        }
    }
}