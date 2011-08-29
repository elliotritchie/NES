using System;
using System.Transactions;

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
            var unitOfWork = DI.Current.Resolve<IUnitOfWork>();

            if (Transaction.Current != null)
            {
                Transaction.Current.EnlistVolatile(new EnlistmentNotification(Transaction.Current.Clone(), unitOfWork), EnlistmentOptions.EnlistDuringPrepareRequired);
            }

            _current = unitOfWork;
        }

        public static void End()
        {
            _current = null;
        }
    }
}