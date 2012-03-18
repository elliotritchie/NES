using System;
using NServiceBus.MessageInterfaces;

namespace NES.NServiceBus
{
    public class MessageMapperAdapter : IEventMapper
    {
        private readonly IMessageMapper _messageMapper;

        public MessageMapperAdapter(IMessageMapper messageMapper)
        {
            _messageMapper = messageMapper;
        }

        public Type GetMappedTypeFor(Type type)
        {
            return _messageMapper.GetMappedTypeFor(type);
        }
    }
}