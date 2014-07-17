// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BsonSerializer.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The bson serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;
    using System.Collections;
    using System.IO;

    using global::NEventStore.Logging;

    using Newtonsoft.Json.Bson;

    /// <summary>
    ///     The bson serializer.
    /// </summary>
    public class BsonSerializer : JsonSerializer
    {
        #region Static Fields

        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(BsonSerializer));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BsonSerializer"/> class.
        /// </summary>
        /// <param name="eventMapperFunc">
        /// The event mapper func.
        /// </param>
        /// <param name="eventFactoryFunc">
        /// The event factory func.
        /// </param>
        public BsonSerializer(Func<IEventMapper> eventMapperFunc, Func<IEventFactory> eventFactoryFunc)
            : base(eventMapperFunc, eventFactoryFunc)
        {
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
        /// Type to deserialize
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public override T Deserialize<T>(Stream input)
        {
            return this.Deserialize<T>(new BsonReader(input, IsArray(typeof(T)), DateTimeKind.Utc));
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
        /// Type to serialize
        /// </typeparam>
        public override void Serialize<T>(Stream output, T graph)
        {
            this.Serialize(new BsonWriter(output) { DateTimeKindHandling = DateTimeKind.Utc }, graph);
        }

        #endregion

        #region Methods

        private static bool IsArray(Type type)
        {
            var array = typeof(IEnumerable).IsAssignableFrom(type) && !typeof(IDictionary).IsAssignableFrom(type);

            Logger.Verbose(string.Format("Objects of type '{0}' are considered to be an array: '{1}'.", type, array));

            return array;
        }

        #endregion
    }
}