// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompositeSerializer.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The composite serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using global::NEventStore;

    using global::NEventStore.Serialization;

    /// <summary>
    ///     The composite serializer.
    /// </summary>
    public class CompositeSerializer : ISerialize
    {
        #region Fields

        private readonly Func<IEventSerializer> _eventSerializerFunc;

        private readonly ISerialize _inner;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeSerializer"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner.
        /// </param>
        /// <param name="eventSerializerFunc">
        /// The event serializer func.
        /// </param>
        public CompositeSerializer(ISerialize inner, Func<IEventSerializer> eventSerializerFunc)
        {
            this._inner = inner;
            this._eventSerializerFunc = eventSerializerFunc;
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
        public T Deserialize<T>(Stream input)
        {
            var graph = this._inner.Deserialize<T>(input);
            var eventMessages = graph as List<EventMessage>;

            if (eventMessages != null)
            {
                foreach (var eventMessage in eventMessages)
                {
                    eventMessage.Body = this._eventSerializerFunc().Deserialize((string)eventMessage.Body);
                }
            }

            return graph;
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
        public void Serialize<T>(Stream output, T graph)
        {
            var eventMessages = graph as List<EventMessage>;

            if (eventMessages != null)
            {
                var cache = eventMessages.ToDictionary(m => m, m => m.Body);

                foreach (var eventMessage in eventMessages)
                {
                    eventMessage.Body = this._eventSerializerFunc().Serialize(eventMessage.Body);
                }

                this._inner.Serialize(output, graph);

                foreach (var eventMessage in eventMessages)
                {
                    eventMessage.Body = cache[eventMessage];
                }

                return;
            }

            this._inner.Serialize(output, graph);
        }

        #endregion
    }
}