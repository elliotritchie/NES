using System;

namespace NES
{
    public class Repository : IRepository
    {
        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T>(id);
        }

        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            UnitOfWorkFactory.Current.Register(aggregate);
        }
    }
}