using System;

namespace NES
{
    public interface IEventHandlerFactory<T> where T : class
    {
        Action<AggregateBase<T>, T> Get(Type aggregateType, Type eventType);
    }
}