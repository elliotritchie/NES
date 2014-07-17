// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventContractResolver.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event contract resolver.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore.Raven
{
    using System;

    using global::Raven.Imports.Newtonsoft.Json.Serialization;

    /// <summary>
    ///     The event contract resolver.
    /// </summary>
    public class EventContractResolver : DefaultContractResolver
    {
        #region Fields

        private readonly IEventFactory _eventFactory;

        private readonly IEventMapper _eventMapper;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventContractResolver"/> class.
        /// </summary>
        /// <param name="eventMapper">
        /// The event mapper.
        /// </param>
        /// <param name="eventFactory">
        /// The event factory.
        /// </param>
        public EventContractResolver(IEventMapper eventMapper, IEventFactory eventFactory)
            : base(true)
        {
            this._eventMapper = eventMapper;
            this._eventFactory = eventFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The create object contract.
        /// </summary>
        /// <param name="objectType">
        /// The object type.
        /// </param>
        /// <returns>
        /// The <see cref="JsonObjectContract"/>.
        /// </returns>
        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            if (objectType.IsInterface)
            {
                var mappedType = this._eventMapper.GetMappedTypeFor(objectType);
                var objectContract = base.CreateObjectContract(mappedType);

                objectContract.DefaultCreator = () => this._eventFactory.Create(mappedType);

                return objectContract;
            }

            return base.CreateObjectContract(objectType);
        }

        #endregion
    }
}