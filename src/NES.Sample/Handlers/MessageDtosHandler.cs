using System.Linq;
using NES.Sample.Data;
using NES.Sample.Dtos;
using NES.Sample.Messages;
using NServiceBus;

namespace NES.Sample.Handlers
{
    public class MessageDtosHandler : IHandleMessages<SentMessageEvent>
    {
        private readonly ISession _session;

        public MessageDtosHandler()
            : this(new Session())
        {
        }

        public MessageDtosHandler(ISession session)
        {
            _session = session;
        }

        public void Handle(SentMessageEvent @event)
        {
            _session.Add(new MessageDto
                             {
                                 Username = _session.UserDtos.Single(u => u.UserId == @event.UserId).Username,
                                 Message = @event.Message,
                                 Sent = @event.Sent
                             });
        }
    }
}