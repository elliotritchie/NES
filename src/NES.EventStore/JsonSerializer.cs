﻿using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;
using EventStore.Logging;
using EventStore.Serialization;
using Newtonsoft.Json;
using JsonNetSerializer = Newtonsoft.Json.JsonSerializer;

namespace NES.EventStore
{
    /// <summary>
    /// Based on the JsonSerializer implementation in EventStore
    /// https://github.com/joliver/EventStore/blob/master/src/proj/EventStore.Serialization.Json/JsonSerializer.cs
    /// </summary>
    public class JsonSerializer : ISerialize
    {
        private static readonly ILog _logger = LogFactory.BuildLogger(typeof(JsonSerializer));

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeNameHandling = TypeNameHandling.Auto,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };
        
        private readonly Func<IEventMapper> _eventMapperFunc;
        private readonly Func<IEventFactory> _eventFactoryFunc;

        public JsonSerializer(Func<IEventMapper> eventMapperFunc, Func<IEventFactory> eventFactoryFunc)
        {
            _eventMapperFunc = eventMapperFunc;
            _eventFactoryFunc = eventFactoryFunc;
        }

        public virtual void Serialize<T>(Stream output, T graph)
        {
            _logger.Verbose("Serializing object graph of type '" + typeof(T) + "'.");

            using (var streamWriter = new StreamWriter(output, Encoding.UTF8))
            {
                Serialize(new JsonTextWriter(streamWriter), graph);
            }
        }

        protected virtual void Serialize(JsonWriter writer, object graph)
        {
            using (writer)
            {
                var serializer = JsonNetSerializer.Create(_settings);

                serializer.Binder = new EventSerializationBinder(_eventMapperFunc());
                serializer.Serialize(writer, graph);
            }
        }

        public virtual T Deserialize<T>(Stream input)
        {
            _logger.Verbose("Deserializing stream to object of type '" + typeof(T)  + "'.");

            using (var streamReader = new StreamReader(input, Encoding.UTF8))
            {
                return Deserialize<T>(new JsonTextReader(streamReader));
            }
        }

        protected virtual T Deserialize<T>(JsonReader reader)
        {
            using (reader)
            {
                var serializer = JsonNetSerializer.Create(_settings);

                serializer.ContractResolver = new EventContractResolver(_eventMapperFunc(), _eventFactoryFunc());

                return (T)serializer.Deserialize(reader, typeof(T));
            }
        }
    }
}