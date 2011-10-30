using System;
using System.Collections.Generic;

namespace NES
{
    public class CommandContext
    {
        public Guid Id { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}