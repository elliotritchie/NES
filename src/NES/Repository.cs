using System;

namespace NES
{
    public class Repository : IRepository
    {
        private readonly UnitOfWorkFactory _unitOfWorkFactory;

        public Repository()
        {
            _unitOfWorkFactory = UnitOfWorkFactory.Current;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return _unitOfWorkFactory.GetUnitOfWork().Get<T>(id);
        }

        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            _unitOfWorkFactory.GetUnitOfWork().Register(aggregate);
        }
    }
}