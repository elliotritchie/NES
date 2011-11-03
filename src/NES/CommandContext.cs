using System;
using System.Collections.Generic;

namespace NES
{
    public class CommandContext
    {
        public Guid Id { get; set; }
        public Dictionary<string, object> Headers { get; set; }
    }
}