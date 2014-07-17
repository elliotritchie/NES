// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonSerializer.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The json serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters;
    using System.Text;

    using global::NEventStore.Logging;

    using global::NEventStore.Serialization;

    using Newtonsoft.Json;

    /// <summary>
    ///     The json serializer.
    /// </summary>
    public class JsonSerializer : ISerialize
    {
        #region Static Fields

        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(JsonSerializer));

        #endregion

        #region Fields

        private readonly Func<IEventFactory> _eventFactoryFunc;

        private readonly Func<IEventMapper> _eventMapperFunc;

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
                                                                {
                                                                    TypeNameAssemblyFormat =
                                                                        FormatterAssemblyStyle.Simple, 
                                                                    TypeNameHandling = TypeNameHandling.Auto, 
                                                                    DefaultValueHandling =
                                                                        DefaultValueHandling.Ignore, 
                                                                    NullValueHandling = NullValueHandling.Ignore
                                                                };

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonSerializer"/> class.
        /// </summary>
        /// <param name="eventMapperFunc">
        /// The event mapper func.
        /// </param>
        /// <param name="eventFactoryFunc">
        /// The event factory func.
        /// </param>
        public JsonSerializer(Func<IEventMapper> eventMapperFunc, Func<IEventFactory> eventFactoryFunc)
        {
            this._eventMapperFunc = eventMapperFunc;
            this._eventFactoryFunc = eventFactoryFunc;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <typeparam name="T">
        /// The type to deserialize
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public virtual T Deserialize<T>(Stream input)
        {
            Logger.Verbose(string.Format("Deserializing stream to object of type '{0}'.", typeof(T)));

            using (var streamReader = new StreamReader(input, Encoding.UTF8))
            {
                return this.Deserialize<T>(new JsonTextReader(streamReader));
            }
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="graph">
        /// The graph.
        /// </param>
        /// <typeparam name="T">
        /// The type to serialize
        /// </typeparam>
        public virtual void Serialize<T>(Stream output, T graph)
        {
            Logger.Verbose(string.Format("Serializing object graph of type '{0}'.", typeof(T)));

            using (var streamWriter = new StreamWriter(output, Encoding.UTF8))
            {
                this.Serialize(new JsonTextWriter(streamWriter), graph);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="reader">
        /// The reader.
        /// </param>
        /// <typeparam name="T">
        /// The type to deserialize
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        protected virtual T Deserialize<T>(JsonReader reader)
        {
            using (reader)
            {
                var serializer = Newtonsoft.Json.JsonSerializer.Create(this._settings);

                serializer.ContractResolver = new EventContractResolver(this._eventMapperFunc(), this._eventFactoryFunc());

                return (T)serializer.Deserialize(reader, typeof(T));
            }
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        /// <param name="graph">
        /// The graph.
        /// </param>
        protected virtual void Serialize(JsonWriter writer, object graph)
        {
            using (writer)
            {
                var serializer = Newtonsoft.Json.JsonSerializer.Create(this._settings);

                serializer.Binder = new EventSerializationBinder(this._eventMapperFunc());
                serializer.Serialize(writer, graph);
            }
        }

        #endregion
    }
}