using NES.Sample.Messages;
using NES.Sample.Model;
using NES.Sample.Services;
using NServiceBus;

namespace NES.Sample.Handlers
{
    using NES.Contracts;

    public class SendMessageCommandHandler : IHandleMessages<SendMessageCommand>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepository _repository;

        public SendMessageCommandHandler(IAuthenticationService authenticationService, IRepository repository)
        {
            _authenticationService = authenticationService;
            _repository = repository;
        }

        public void Handle(SendMessageCommand command)
        {
            var user = _repository.Get<User>(_authenticationService.UserId);
            var message = user.SendMessage(command.MessageId.Value, command.Message);

            _repository.Add(message);
        }
    }
}