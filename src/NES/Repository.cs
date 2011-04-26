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

        public void Add<T>(T aggregate) where T : IEventSource
        {
            _unitOfWork.Register(aggregate);
        }

        public T Get<T>(Guid id) where T : IEventSource
        {
            return _unitOfWork.Get<T>(id);
        }
    }
}