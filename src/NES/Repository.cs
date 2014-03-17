using System;
using System.Collections.Generic;

namespace NES
{
    public class Repository : IRepository
    {
        public T Get<T>(Guid id, Func<IEnumerable<object>, IEnumerable<object>> sortFilterAction) where T : class, IEventSource
        {
            return UnitOfWorkFactory.Current.Get<T>(id, sortFilterAction);
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return this.Get<T>(id, null);
        }

        public IEnumerable<object> GetEvents(Guid id)
        {
            return UnitOfWorkFactory.Current.GetEvents(id);
        }

        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            UnitOfWorkFactory.Current.Register(aggregate);
        }
    }
}