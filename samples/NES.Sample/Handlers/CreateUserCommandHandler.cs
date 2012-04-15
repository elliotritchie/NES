using NES.Sample.Messages;
using NES.Sample.Model;
using NES.Sample.Services;
using NServiceBus;

namespace NES.Sample.Handlers
{
    public class CreateUserCommandHandler : IHandleMessages<CreateUserCommand>
    {
        private readonly IValidationService _validationService;
        private readonly IRepository _repository;

        public CreateUserCommandHandler()
            : this(new ValidationService(), new Repository())
        {
        }

        public CreateUserCommandHandler(IValidationService validationService, IRepository repository)
        {
            _validationService = validationService;
            _repository = repository;
        }

        public void Handle(CreateUserCommand command)
        {
            _validationService.Validate(command);

            var user = new User(command.UserId.Value, command.Username);

            _repository.Add(user);
        }
    }
}