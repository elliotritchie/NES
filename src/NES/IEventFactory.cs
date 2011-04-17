using System;

namespace NES
{
    public interface IEventFactory<T>
    {
        TEvent CreateEvent<TEvent>(Action<TEvent> action) where TEvent : T;
    }
}