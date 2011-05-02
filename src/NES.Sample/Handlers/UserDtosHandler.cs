using NES.Sample.Data;
using NES.Sample.Dtos;
using NES.Sample.Messages;
using NServiceBus;

namespace NES.Sample.Handlers
{
    public class UserDtosHandler : IHandleMessages<CreatedUserEvent>
    {
        private readonly ISession _session;

        public UserDtosHandler()
            : this(new Session())
        {
        }

        public UserDtosHandler(ISession session)
        {
            _session = session;
        }

        public void Handle(CreatedUserEvent @event)
        {
            _session.Add(new UserDto
                             {
                                 UserId = @event.UserId,
                                 Username = @event.Username
                             });
        }
    }
}