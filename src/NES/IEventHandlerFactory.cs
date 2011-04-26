using System;

namespace NES
{
    public interface IEventHandlerFactory<T>
    {
        Action<AggregateBase<T>, T> Get(Type aggregateType, Type eventType);
    }
}