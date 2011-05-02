using System;

namespace NES
{
    public interface IEventHandlerFactory
    {
        Action Get<TAggregate, TEvent>(TAggregate aggregate, TEvent @event)
            where TAggregate : AggregateBase<TEvent>
            where TEvent : class;
    }
}