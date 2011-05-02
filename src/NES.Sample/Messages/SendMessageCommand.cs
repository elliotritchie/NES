using System;
using System.ComponentModel.DataAnnotations;
using NServiceBus;

namespace NES.Sample.Messages
{
    public class SendMessageCommand : IMessage
    {
        [Required]
        public Guid? MessageId { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Message { get; set; }

        public SendMessageCommand()
        {
            MessageId = GuidComb.NewGuidComb();
        }
    }
}