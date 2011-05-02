using NES.Sample.Messages;
using NES.Sample.Model;
using NES.Sample.Services;
using NServiceBus;

namespace NES.Sample.Handlers
{
    public class SendMessageCommandHandler : IHandleMessages<SendMessageCommand>
    {
        private readonly IValidationService _validationService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepository _repository;

        public SendMessageCommandHandler()
            : this(new ValidationService(), new AuthenticationService(), new Repository())
        {
        }

        public SendMessageCommandHandler(IValidationService validationService, IAuthenticationService authenticationService, IRepository repository)
        {
            _validationService = validationService;
            _authenticationService = authenticationService;
            _repository = repository;
        }

        public void Handle(SendMessageCommand command)
        {
            _validationService.Validate(command);

            var user = _repository.Get<User>(_authenticationService.UserId);
            var message = user.SendMessage(command.MessageId.Value, command.Message);

            _repository.Add(message);
        }
    }
}