using System;

namespace NES
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public static Func<IUnitOfWork> Closure;

        [ThreadStatic]
        private static IUnitOfWork _unitOfWork;

        public void Begin()
        {
            _unitOfWork = Closure();
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