using System;

namespace NES
{
    public interface IEventFactory<T> where T : class
    {
        TEvent Create<TEvent>(Action<TEvent> action) where TEvent : T;
    }
}