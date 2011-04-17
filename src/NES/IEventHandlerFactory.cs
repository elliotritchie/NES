using System;

namespace NES
{
    public interface IEventHandlerFactory<T>
    {
        Action<AggregateBase<T>, T> GetHandler(Type aggregateType, Type eventType);
    }
}