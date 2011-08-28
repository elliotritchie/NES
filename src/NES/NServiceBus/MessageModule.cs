using System.Transactions;
using NServiceBus;

namespace NES.NServiceBus
{
    public class MessageModule : IMessageModule
    {
        public void HandleBeginMessage()
        {
            UnitOfWorkFactory.Begin();
        }

        public void HandleEndMessage()
        {
            var unitOfWork = UnitOfWorkFactory.Current;

            if (Transaction.Current == null && unitOfWork != null)
            {
                unitOfWork.Commit();
            }

            UnitOfWorkFactory.End();
        }

        public void HandleError()
        {
            UnitOfWorkFactory.End();
        }
    }
}