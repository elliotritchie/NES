using System;
using NES.Contracts;
using Raven.Imports.Newtonsoft.Json.Serialization;

namespace NES.NEventStore.Raven
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