// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomizedRavenSerializerWireup.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The customized raven serializer wireup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore.Raven
{
    using System.Reflection;

    using global::NEventStore;

    using global::NEventStore.Logging;

    using global::NEventStore.Persistence;

    using global::NEventStore.Persistence.RavenPersistence;

    using global::Raven.Client;

    /// <summary>
    ///     The customized raven serializer wireup.
    /// </summary>
    public class CustomizedRavenSerializerWireup : Wireup
    {
        #region Static Fields

        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(CustomizedRavenSerializerWireup));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizedRavenSerializerWireup"/> class.
        /// </summary>
        /// <param name="wireup">
        /// The wireup.
        /// </param>
        public CustomizedRavenSerializerWireup(NESWireup wireup)
            : base(wireup)
        {
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The build.
        /// </summary>
        /// <returns>
        ///     The <see cref="IStoreEvents" />.
        /// </returns>
        public override IStoreEvents Build()
        {
            Logger.Debug("Configuring customized Raven serializer to cope with payloads that contain messages as interfaces.");

            var engine = (RavenPersistenceEngine)this.Container.Resolve<IPersistStreams>();
            var store = (IDocumentStore)engine.GetType().GetField("store", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(engine);

            store.Conventions.CustomizeJsonSerializer = s =>
                {
                    s.Binder = new EventSerializationBinder(DI.Current.Resolve<IEventMapper>());
                    s.ContractResolver = new EventContractResolver(DI.Current.Resolve<IEventMapper>(), DI.Current.Resolve<IEventFactory>());
                };

            return base.Build();
        }

        #endregion
    }
}