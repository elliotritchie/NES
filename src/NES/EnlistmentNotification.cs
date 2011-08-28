using System;
using System.Transactions;

namespace NES
{
    public class EnlistmentNotification : IEnlistmentNotification
    {
        private readonly Transaction _transaction;
        private readonly IUnitOfWork _unitOfWork;

        public EnlistmentNotification(Transaction transaction, IUnitOfWork unitOfWork)
        {
            _transaction = transaction;
            _unitOfWork = unitOfWork;
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                using (var ts = new TransactionScope(_transaction))
                {
                    _unitOfWork.Commit();

                    ts.Complete();
                }

                preparingEnlistment.Prepared();
            }
            catch (Exception ex)
            {
                preparingEnlistment.ForceRollback(ex);
            }
        }

        public void Commit(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            enlistment.Done();
        }
    }
}