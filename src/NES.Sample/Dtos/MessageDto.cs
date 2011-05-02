using System;

namespace NES.Sample.Dtos
{
    public class MessageDto
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Sent { get; set; }
    }
}