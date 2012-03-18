using System;
using System.Runtime.Serialization;

namespace NES.EventStore
{
    public class EventSerializationBinder : SerializationBinder
    {
        private readonly IEventMapper _eventMapper;

        public EventSerializationBinder(IEventMapper eventMapper)
        {
            _eventMapper = eventMapper;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            var mappedType = _eventMapper.GetMappedTypeFor(serializedType) ?? serializedType;

            assemblyName = null;
            typeName = mappedType.AssemblyQualifiedName;
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            throw new NotImplementedException();
        }
    }
}