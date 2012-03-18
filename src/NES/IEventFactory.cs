using System;

namespace NES
{
    public interface IEventFactory
    {
        T Create<T>(Action<T> action);
        object Create(Type type);
    }
}