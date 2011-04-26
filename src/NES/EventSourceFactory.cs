using System;

namespace NES
{
    public class EventSourceFactory : IEventSourceFactory
    {
        public T Create<T>() where T : IEventSource
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}