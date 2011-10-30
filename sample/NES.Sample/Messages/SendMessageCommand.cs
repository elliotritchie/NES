using System;
using System.ComponentModel.DataAnnotations;

namespace NES.Sample.Messages
{
    public class SendMessageCommand : Command
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