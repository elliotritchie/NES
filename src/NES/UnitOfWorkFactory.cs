using System;
using Microsoft.Practices.ServiceLocation;

namespace NES
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        [ThreadStatic]
        private static IUnitOfWork _unitOfWork;

        public void Begin()
        {
            _unitOfWork = ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }

        public void End()
        {
            _unitOfWork = null;
        }

        public IUnitOfWork GetUnitOfWork()
        {
            return _unitOfWork;
        }
    }
}