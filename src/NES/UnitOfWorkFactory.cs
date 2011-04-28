using System;

namespace NES
{
    public sealed class UnitOfWorkFactory
    {
        private static readonly UnitOfWorkFactory _current = new UnitOfWorkFactory();
        public static UnitOfWorkFactory Current
        {
            get { return _current; }
        }

        [ThreadStatic]
        private static IUnitOfWork _unitOfWork;
        
        public void Begin()
        {
            _unitOfWork = DI.Current.Resolve<IUnitOfWork>();
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