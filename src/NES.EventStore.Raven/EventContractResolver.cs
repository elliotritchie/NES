namespace NES.EventStore.Raven
{
    using System;

    using global::Raven.Imports.Newtonsoft.Json.Serialization;

    public class EventContractResolver : DefaultContractResolver
    {
        private readonly IEventMapper _eventMapper;
        private readonly IEventFactory _eventFactory;

        public EventContractResolver(IEventMapper eventMapper, IEventFactory eventFactory)
            : base(true)
        {
            this._eventMapper = eventMapper;
            this._eventFactory = eventFactory;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            if (objectType.IsInterface)
            {
                var mappedType = this._eventMapper.GetMappedTypeFor(objectType);
                var objectContract = base.CreateObjectContract(mappedType);

                objectContract.DefaultCreator = () => this._eventFactory.Create(mappedType);

                return objectContract;
            }

            return base.CreateObjectContract(objectType);
        }
    }
}