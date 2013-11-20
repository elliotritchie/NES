using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES
{
    public static class LoggingFactory
    {
        public static Func<Type, ILogger> BuildLogger { get; set; }

        static LoggingFactory()
        {
            LoggingFactory.NullLogger logger = new LoggingFactory.NullLogger();
            LoggingFactory.BuildLogger = (Func<Type, ILogger>)(type => (ILogger)logger);
        }

        public static void LogTo(Func<Type, ILogger> logger)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            LoggingFactory.BuildLogger = logger;
        }

        private class NullLogger : ILogger
        {
            public void Verbose(string message, params object[] values)
            {
            }

            public void Debug(string message, params object[] values)
            {
            }

            public void Info(string message, params object[] values)
            {
            }

            public void Warn(string message, params object[] values)
            {
            }

            public void Error(string message, params object[] values)
            {
            }

            public void Fatal(string message, params object[] values)
            {
            }
        }
    }
}
