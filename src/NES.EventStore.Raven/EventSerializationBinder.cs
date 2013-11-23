using System;
using Raven.Imports.Newtonsoft.Json.Serialization;

namespace NES.EventStore.Raven
{
    public class SerializationBinder : DefaultSerializationBinder
    {
        private readonly IEventMapper _eventMapper;

        public SerializationBinder(IEventMapper eventMapper)
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