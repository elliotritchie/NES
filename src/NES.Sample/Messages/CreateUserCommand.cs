using System;
using System.ComponentModel.DataAnnotations;
using NServiceBus;

namespace NES.Sample.Messages
{
    public class CreateUserCommand : IMessage
    {
        [Required]
        public Guid? UserId { get; set; }

        [DataType(DataType.Text)]
        [Required]
        public string Username { get; set; }

        public CreateUserCommand()
        {
            UserId = GuidComb.NewGuidComb();
        }
    }
}