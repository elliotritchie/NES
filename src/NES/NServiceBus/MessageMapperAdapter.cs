using System;
using NServiceBus.MessageInterfaces;

namespace NES.NServiceBus
{
    public class MessageMapperAdapter : IEventFactory
    {
        private readonly IMessageMapper _messageMapper;

        public MessageMapperAdapter(IMessageMapper messageMapper)
        {
            _messageMapper = messageMapper;
        }

        public T Create<T>(Action<T> action)
        {
            var @event = (T)_messageMapper.CreateInstance(typeof(T));

            action(@event);

            return @event;
        }
    }
}