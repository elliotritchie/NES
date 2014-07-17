// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageMapperAdapter.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The message mapper adapter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NServiceBus
{
    using System;

    using global::NServiceBus.MessageInterfaces;

    /// <summary>
    ///     The message mapper adapter.
    /// </summary>
    public class MessageMapperAdapter : IEventMapper
    {
        #region Fields

        private readonly IMessageMapper _messageMapper;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageMapperAdapter"/> class.
        /// </summary>
        /// <param name="messageMapper">
        /// The message mapper.
        /// </param>
        public MessageMapperAdapter(IMessageMapper messageMapper)
        {
            this._messageMapper = messageMapper;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The get mapped type for.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/>.
        /// </returns>
        public Type GetMappedTypeFor(Type type)
        {
            return this._messageMapper.GetMappedTypeFor(type);
        }

        #endregion
    }
}