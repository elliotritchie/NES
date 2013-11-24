﻿using System;
using Newtonsoft.Json.Serialization;

namespace NES.EventStore
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
            if (objectType.IsInterface  && _eventMapper.GetMappedTypeFor(objectType) != null)
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
