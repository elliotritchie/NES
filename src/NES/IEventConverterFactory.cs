using System;

namespace NES
{
    public interface IEventConverterFactory
    {
        Func<object, object> Get(Type @eventType);
    }
}