using System;
using NES.Contracts;
using Raven.Imports.Newtonsoft.Json.Serialization;

namespace NES.NEventStore.Raven
{
    public class EventContractResolver : DefaultContractResolver
    {
        private readonly IEventMapper _eventMapper;
        private readonly IEventFactory _eventFactory;

        public EventContractResolver(IEventMapper eventMapper, IEventFactory eventFactory) : base(true)
        {
            _eventMapper = eventMapper;
            _eventFactory = eventFactory;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            if (objectType.IsInterface)
            {
                var mappedType = _eventMapper.GetMappedTypeFor(objectType);
                var objectContract = base.CreateObjectContract(mappedType);

                objectContract.DefaultCreator = () => _eventFactory.Create(mappedType);

                return objectContract;
            }

            return base.CreateObjectContract(objectType);
        }
    }
}