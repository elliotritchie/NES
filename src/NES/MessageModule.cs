using NServiceBus;

namespace NES
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