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
            try
            {
                UnitOfWorkFactory.Current.Commit();
            }
            finally
            {
                UnitOfWorkFactory.End();
            }
        }

        public void HandleError()
        {
        }
    }
}