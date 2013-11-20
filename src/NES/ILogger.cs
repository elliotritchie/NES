using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NES
{
    /// <summary>
    /// Has exactly the same methods like the logger of eventstore. This simplefies the 
    /// implemntation because the user can use the same logger
    /// </summary>
    public interface ILogger
    {
        void Verbose(string message, params object[] values);

        void Debug(string message, params object[] values);


        void Info(string message, params object[] values);


        void Warn(string message, params object[] values);


        void Error(string message, params object[] values);


        void Fatal(string message, params object[] values);

    }
}
