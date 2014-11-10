using System;
using NES.Contracts;

namespace NES
{
    public class EventSourceFactory : IEventSourceFactory
    {
        public T Create<T>() where T : IEventSourceBase
        {
            return (T)Activator.CreateInstance(typeof(T), true);
        }
    }
}