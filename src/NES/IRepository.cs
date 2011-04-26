using System;

namespace NES
{
    public interface IRepository
    {
        void Add<T>(T aggregate) where T : IEventSource;
        T Get<T>(Guid id) where T : IEventSource;
    }
}