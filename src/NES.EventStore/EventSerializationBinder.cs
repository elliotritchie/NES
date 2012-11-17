using System;
using Newtonsoft.Json.Serialization;

namespace NES.EventStore
{
    public class EventSerializationBinder : DefaultSerializationBinder
    {
        private readonly IEventMapper _eventMapper;

        public EventSerializationBinder(IEventMapper eventMapper)
        {
            _eventMapper = eventMapper;
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            var mappedType = _eventMapper.GetMappedTypeFor(serializedType);

            assemblyName = mappedType.Assembly.FullName;
            typeName = mappedType.FullName;
        }
    }
}