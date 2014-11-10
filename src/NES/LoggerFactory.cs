using System;
using NES.Contracts;

namespace NES
{
    public class LoggerFactory
    {
        public static Func<Type, ILogger> Create { get; set; }
    }
}