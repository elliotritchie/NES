// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageSerializerAdapter.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The message serializer adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using System.IO;

    using global::NServiceBus.Serialization;

    /// <summary>
    ///     The message serializer adapter.
    /// </summary>
    public class MessageSerializerAdapter : IEventSerializer
    {
        #region Fields

        private readonly IMessageSerializer _messageSerializer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSerializerAdapter"/> class.
        /// </summary>
        /// <param name="messageSerializer">
        /// The message serializer.
        /// </param>
        public MessageSerializerAdapter(IMessageSerializer messageSerializer)
        {
            this._messageSerializer = messageSerializer;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Deserialize(string data)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);

                writer.Write(data);
                writer.Flush();
                stream.Position = 0;

                return this._messageSerializer.Deserialize(stream)[0];
            }
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="event">
        /// The event.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string Serialize(object @event)
        {
            using (var stream = new MemoryStream())
            {
                var reader = new StreamReader(stream);

                this._messageSerializer.Serialize(new[] { @event }, stream);
                stream.Position = 0;

                return reader.ReadToEnd();
            }
        }

        #endregion
    }
}