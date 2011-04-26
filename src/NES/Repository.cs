using System;

namespace NES
{
    public class Repository : IRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public Repository(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWork = unitOfWorkFactory.GetUnitOfWork();
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return _unitOfWork.Get<T>(id);
        }

        public void Add<T>(T aggregate) where T : class, IEventSource
        {
            _unitOfWork.Register(aggregate);
        }
    }
}