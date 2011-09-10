using System;

namespace NES
{
    [Serializable]
    public class ConflictingCommandException : Exception
    {
        public ConflictingCommandException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}