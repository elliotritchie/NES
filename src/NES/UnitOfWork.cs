using System;

namespace NES
{
    public class UnitOfWork : IUnitOfWork
    {
        public T Get<T>(Guid id) where T : IEventSource
        {
            return default(T);
        }

        public void Register<T>(T source) where T : IEventSource
        {
        }

        public void Commit()
        {
        }
    }
}