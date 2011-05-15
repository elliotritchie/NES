using System;

namespace NES
{
    public interface IEventHandlerFactory
    {
        Action<object> Get(object aggregate, Type @eventType);
    }
}