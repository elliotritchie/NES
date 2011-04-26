using System;

namespace NES
{
    public interface IEventFactory<T>
    {
        TEvent Create<TEvent>(Action<TEvent> action) where TEvent : T;
    }
}