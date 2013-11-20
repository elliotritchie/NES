using System;
using NServiceBus;

namespace NES.NServiceBus
{
    public class MessageCreatorAdapter : IEventFactory
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(MessageCreatorAdapter));
        private readonly IMessageCreator _messageCreator;

        public MessageCreatorAdapter(IMessageCreator messageCreator)
        {
            _messageCreator = messageCreator;
        }

        public T Create<T>(Action<T> action)
        {
            Logger.Debug(string.Format("Creating instance of the message {0}", typeof(T).FullName));
            return _messageCreator.CreateInstance(action);
        }

        public object Create(Type type)
        {
            return _messageCreator.CreateInstance(type);
        }
    }
}