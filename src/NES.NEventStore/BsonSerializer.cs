using System;
using System.Collections;
using System.IO;
using NES.Contracts;
using NEventStore.Logging;
using Newtonsoft.Json.Bson;

namespace NES.NEventStore
{
    /// <summary>
    /// Based on the BsonSerializer implementations in EventStore & NServiceBus
    /// https://github.com/joliver/EventStore/blob/master/src/proj/EventStore.Serialization.Json/BsonSerializer.cs
    /// https://github.com/NServiceBus/NServiceBus/blob/master/src/impl/Serializers/NServiceBus.Serializers.Json/BsonMessageSerializer.cs
    /// </summary>
    public class BsonSerializer : JsonSerializer
    {
        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(BsonSerializer));

        public BsonSerializer(Func<IEventMapper> eventMapperFunc, Func<IEventFactory> eventFactoryFunc) : base(eventMapperFunc, eventFactoryFunc)
        {
        }

        public override void Serialize<T>(Stream output, T graph)
        {
            Serialize(new BsonWriter(output) { DateTimeKindHandling = DateTimeKind.Utc }, graph);
        }
        
        public override T Deserialize<T>(Stream input)
        {
            return Deserialize<T>(new BsonReader(input, IsArray(typeof(T)), DateTimeKind.Utc));
        }

        private static bool IsArray(Type type)
        {
            var array = typeof(IEnumerable).IsAssignableFrom(type) && !typeof(IDictionary).IsAssignableFrom(type);

            Logger.Verbose(string.Format("Objects of type '{0}' are considered to be an array: '{1}'.", type, array));

            return array;
        }
    }
}