using NES.Sample.Data;
using NES.Sample.Dtos;
using NES.Sample.Messages;
using NServiceBus;

namespace NES.Sample.Handlers
{
    public class CreatedUserEventHandler : IHandleMessages<ICreatedUserEvent>
    {
        private readonly IDataRepository _dataRepository;

        public CreatedUserEventHandler(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void Handle(ICreatedUserEvent @event)
        {
            _dataRepository.Add(new UserDto
                             {
                                 UserId = @event.UserId,
                                 Username = @event.Username
                             });
        }
    }
}