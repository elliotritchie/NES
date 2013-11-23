using System;
using NServiceBus;

namespace NES.NServiceBus
{
    public class MessageCreatorAdapter : IEventFactory
    {
        private readonly IMessageCreator _messageCreator;

        public MessageCreatorAdapter(IMessageCreator messageCreator)
        {
            _messageCreator = messageCreator;
        }

        public T Create<T>(Action<T> action)
        {
            return _messageCreator.CreateInstance(action);
        }

        public object Create(Type type)
        {
            return _messageCreator.CreateInstance(type);
        }
    }
}