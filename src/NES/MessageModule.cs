using NServiceBus;

namespace NES
{
    public class MessageModule : IMessageModule
    {
        private readonly UnitOfWorkFactory _unitOfWorkFactory;

        public MessageModule()
        {
            _unitOfWorkFactory = UnitOfWorkFactory.Current;
        }

        public void HandleBeginMessage()
        {
            _unitOfWorkFactory.Begin();
        }

        public void HandleEndMessage()
        {
            try
            {
                _unitOfWorkFactory.GetUnitOfWork().Commit();
            }
            finally
            {
                _unitOfWorkFactory.End();
            }
        }

        public void HandleError()
        {
        }
    }
}