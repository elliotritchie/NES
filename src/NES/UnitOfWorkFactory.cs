using System;
using NES.Contracts;

namespace NES
{
    public static class UnitOfWorkFactory
    {
        [ThreadStatic]
        private static IUnitOfWork _current;
        public static IUnitOfWork Current
        {
            get { return _current; }
            internal set { _current = value; }
        }

        public static void Begin()
        {
            _current = DI.Current.Resolve<IUnitOfWork>();
        }

        public static void End()
        {
            _current = null;
        }
    }
}