// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageCreatorAdapter.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The message creator adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using System;

    using global::NServiceBus;

    /// <summary>
    ///     The message creator adapter.
    /// </summary>
    public class MessageCreatorAdapter : IEventFactory
    {
        #region Fields

        private readonly IMessageCreator _messageCreator;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageCreatorAdapter"/> class.
        /// </summary>
        /// <param name="messageCreator">
        /// The message creator.
        /// </param>
        public MessageCreatorAdapter(IMessageCreator messageCreator)
        {
            this._messageCreator = messageCreator;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="T">
        /// Type of message
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Create<T>(Action<T> action)
        {
            return this._messageCreator.CreateInstance(action);
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object Create(Type type)
        {
            return this._messageCreator.CreateInstance(type);
        }

        #endregion
    }
}