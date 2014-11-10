using NES.Sample.Messages;
using NES.Sample.Model;
using NES.Sample.Services;
using NServiceBus;

namespace NES.Sample.Handlers
{
    using NES.Contracts;

    public class CreateUserCommandHandler : IHandleMessages<CreateUserCommand>
    {
        private readonly IRepository _repository;

        public CreateUserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(CreateUserCommand command)
        {
            var user = new User(command.UserId.Value, command.Username);

            _repository.Add(user);
        }
    }
}