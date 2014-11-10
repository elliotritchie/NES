using System.Collections.Generic;
using System.Linq;
using NES.Sample.Data;
using NES.Sample.Dtos;

namespace NES.Sample.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IDataRepository _dataRepository;

        public MessagesService()
            : this(new DataRepository())
        {
        }

        public MessagesService(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public IEnumerable<MessageDto> Get()
        {
            return _dataRepository.MessageDtos.OrderByDescending(m => m.Sent);
        }
    }
}