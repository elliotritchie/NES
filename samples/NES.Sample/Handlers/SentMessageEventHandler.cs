using System.Linq;
using NES.Sample.Data;
using NES.Sample.Dtos;
using NES.Sample.Messages;
using NServiceBus;

namespace NES.Sample.Handlers
{
    public class SentMessageEventHandler : IHandleMessages<ISentMessageEvent>
    {
        private readonly IDataRepository _dataRepository;


        public SentMessageEventHandler(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void Handle(ISentMessageEvent @event)
        {
            _dataRepository.Add(new MessageDto
                             {
                                 Username = _dataRepository.UserDtos.Single(u => u.UserId == @event.UserId).Username,
                                 Message = @event.Message,
                                 Sent = @event.Sent
                             });
        }
    }
}