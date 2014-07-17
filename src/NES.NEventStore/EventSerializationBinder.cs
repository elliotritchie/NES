// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventSerializationBinder.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event serialization binder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;

    using Newtonsoft.Json.Serialization;

    /// <summary>
    ///     The event serialization binder.
    /// </summary>
    public class EventSerializationBinder : DefaultSerializationBinder
    {
        #region Fields

        private readonly IEventMapper _eventMapper;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventSerializationBinder"/> class.
        /// </summary>
        /// <param name="eventMapper">
        /// The event mapper.
        /// </param>
        public EventSerializationBinder(IEventMapper eventMapper)
        {
            this._eventMapper = eventMapper;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The bind to name.
        /// </summary>
        /// <param name="serializedType">
        /// The serialized type.
        /// </param>
        /// <param name="assemblyName">
        /// The assembly name.
        /// </param>
        /// <param name="typeName">
        /// The type name.
        /// </param>
        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            var mappedType = this._eventMapper.GetMappedTypeFor(serializedType);

            assemblyName = mappedType.Assembly.FullName;
            typeName = mappedType.FullName;
        }

        #endregion
    }
}