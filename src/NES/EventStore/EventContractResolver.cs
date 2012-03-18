using System;
using Newtonsoft.Json.Serialization;

namespace NES.EventStore
{
    public class EventContractResolver : DefaultContractResolver
    {
        private readonly IEventMapper _eventMapper;
        private readonly IEventFactory _eventFactory;

        public EventContractResolver(IEventMapper eventMapper, IEventFactory eventFactory)
            : base(true)
        {
            _eventMapper = eventMapper;
            _eventFactory = eventFactory;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var mappedType = _eventMapper.GetMappedTypeFor(objectType);

            if (mappedType == null)
                return base.CreateObjectContract(objectType);

            var jsonObjectContract = base.CreateObjectContract(mappedType);

            jsonObjectContract.DefaultCreator = () => _eventFactory.Create(mappedType);

            return jsonObjectContract;
        }
    }
}