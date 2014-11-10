using System;

namespace NES.Contracts
{
    public interface IEventConverterFactory
    {
        Func<object, object> Get(Type @eventType);
    }
}