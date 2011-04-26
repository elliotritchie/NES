using System;

namespace NES
{
    public interface IUnitOfWork
    {
        T Get<T>(Guid id) where T : IEventSource;
        void Register<T>(T source) where T : IEventSource;
        void Commit();
    }
}