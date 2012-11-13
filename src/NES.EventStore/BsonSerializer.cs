using System;
using System.Collections;
using System.IO;
using EventStore.Logging;
using Newtonsoft.Json.Bson;

namespace NES.EventStore
{
    /// <summary>
    /// Based on the JsonSerializer implementation in EventStore
    /// https://github.com/joliver/EventStore/blob/master/src/proj/EventStore.Serialization.Json/BsonSerializer.cs
    /// </summary>
    public class BsonSerializer : JsonSerializer
    {
        private static readonly ILog _logger = LogFactory.BuildLogger(typeof(BsonSerializer));

        public BsonSerializer(Func<IEventMapper> eventMapperFunc, Func<IEventFactory> eventFactoryFunc) 
            : base(eventMapperFunc, eventFactoryFunc)
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

            _logger.Verbose(string.Format("Objects of type '{0}' are considered to be an array: '{1}'.", type, array));

            return array;
        }
    }
}