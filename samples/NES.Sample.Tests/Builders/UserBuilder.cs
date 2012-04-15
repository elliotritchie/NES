using System;
using NES.Sample.Messages;
using NES.Sample.Model;

namespace NES.Sample.Tests.Builders
{
    public class UserBuilder : AggregateBuilder<User>
    {
        private readonly Guid _userId = GuidComb.NewGuidComb();
        private const string _username = "Bob";

        public UserBuilder CreatedUser()
        {
            return CreatedUser(_userId, _username);
        }

        public UserBuilder CreatedUser(Guid userId)
        {
            return CreatedUser(userId, _username);
        }

        public UserBuilder CreatedUser(string username)
        {
            return CreatedUser(_userId, username);
        }

        public UserBuilder CreatedUser(Guid userId, string username)
        {
            Apply<CreatedUserEvent>(e =>
            {
                e.UserId = userId;
                e.Username = username;
            });

            return this;
        }
    }
}