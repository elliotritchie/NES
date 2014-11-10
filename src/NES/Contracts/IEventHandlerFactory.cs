using System;

namespace NES.Contracts
{
    public interface IEventHandlerFactory
    {
        Action<object> Get(object aggregate, Type @eventType);
    }
}