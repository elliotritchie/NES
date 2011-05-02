using System.Collections.Generic;
using System.Linq;
using NES.Sample.Data;
using NES.Sample.Dtos;

namespace NES.Sample.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly ISession _session;

        public MessagesService()
            : this(new Session())
        {
        }

        public MessagesService(ISession session)
        {
            _session = session;
        }

        public IEnumerable<MessageDto> Get()
        {
            return _session.MessageDtos.OrderByDescending(m => m.Sent);
        }
    }
}