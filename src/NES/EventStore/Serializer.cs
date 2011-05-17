using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EventStore;
using EventStore.Serialization;

namespace NES.EventStore
{
    public class Serializer : ISerialize
    {
        private readonly ISerialize _inner;
        private readonly Func<IEventSerializer> _eventSerializerFactory;

        public Serializer(ISerialize inner, Func<IEventSerializer> eventSerializerFactory)
        {
            _inner = inner;
            _eventSerializerFactory = eventSerializerFactory;
        }

        public void Serialize<T>(Stream output, T graph)
        {
            var eventMessages = graph as List<EventMessage>;

            if (eventMessages != null)
            {
                var cache = eventMessages.ToDictionary(m => m, m => m.Body);

                foreach (var eventMessage in eventMessages)
                {
                    eventMessage.Body = _eventSerializerFactory().Serialize(eventMessage.Body);
                }

                _inner.Serialize(output, graph);

                foreach (var eventMessage in eventMessages)
                {
                    eventMessage.Body = cache[eventMessage];
                }

                return;
            }

            _inner.Serialize(output, graph);
        }

        public T Deserialize<T>(Stream input)
        {
            var graph = _inner.Deserialize<T>(input);
            var eventMessages = graph as List<EventMessage>;

            if (eventMessages != null)
            {
                foreach (var eventMessage in eventMessages)
                {
                    eventMessage.Body = _eventSerializerFactory().Deserialize((string)eventMessage.Body);
                }
            }

            return graph;
        }
    }
}