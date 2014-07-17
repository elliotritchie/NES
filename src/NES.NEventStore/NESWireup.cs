// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NESWireup.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The nes wireup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using global::NEventStore;

    using global::NEventStore.Dispatcher;

    using global::NEventStore.Logging;

    using global::NEventStore.Persistence;

    using global::NEventStore.Serialization;

    /// <summary>
    ///     The nes wireup.
    /// </summary>
    public class NESWireup : Wireup
    {
        #region Static Fields

        private static readonly ILog Logger = LogFactory.BuildLogger(typeof(NESWireup));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NESWireup"/> class.
        /// </summary>
        /// <param name="wireup">
        /// The wireup.
        /// </param>
        public NESWireup(Wireup wireup)
            : base(wireup)
        {
            var serializer = this.Container.Resolve<ISerialize>();

            if (serializer != null)
            {
                Logger.Debug("Configuring custom NES serializer to cope with payloads that contain messages as interfaces.");
                Logger.Debug(string.Format("Wrapping serializer of type '{0}' in '{1}'", serializer.GetType(), typeof(CompositeSerializer)));

                this.Container.Register<ISerialize>(new CompositeSerializer(serializer, () => DI.Current.Resolve<IEventSerializer>()));
            }

            Logger.Debug("Configuring the store to dispatch messages synchronously.");
            Logger.Debug(string.Format("Registering dispatcher of type '{0}'.", typeof(MessageDispatcher)));

            this.Container.Register<IDispatchCommits>(new MessageDispatcher(() => DI.Current.Resolve<IEventPublisher>()));
            this.Container.Register<IScheduleDispatches>(c =>
            {
                var dispatchScheduler = new SynchronousDispatchScheduler(
                    c.Resolve<IDispatchCommits>(),
                    c.Resolve<IPersistStreams>());
                if (c.Resolve<DispatcherSchedulerStartup>() == DispatcherSchedulerStartup.Auto)
                {
                    dispatchScheduler.Start();
                }

                return dispatchScheduler;
            });

            DI.Current.Register<IEventStore, IStoreEvents>(eventStore => new EventStoreAdapter(eventStore));
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
            Logger.Debug("Configuring the store to upconvert events when fetched.");

            var pipelineHooks = this.Container.Resolve<ICollection<IPipelineHook>>();
            var eventConverterPipelineHook = new EventConverterPipelineHook(() => DI.Current.Resolve<IEventConversionRunner>());

            if (pipelineHooks == null)
            {
                this.Container.Register(pipelineHooks = new Collection<IPipelineHook>());
            }

            pipelineHooks.Add(eventConverterPipelineHook);

            var store = base.Build();

            DI.Current.Register(() => store);

            return store;
        }

        /// <summary>
        ///     The using bson serialization.
        /// </summary>
        /// <returns>
        ///     The <see cref="SerializationWireup" />.
        /// </returns>
        public SerializationWireup UsingBsonSerialization()
        {
            Logger.Debug("Configuring custom NES Bson serializer to cope with payloads that contain messages as interfaces.");

            return new SerializationWireup(
                this, 
                new BsonSerializer(() => DI.Current.Resolve<IEventMapper>(), () => DI.Current.Resolve<IEventFactory>()));
        }

        /// <summary>
        ///     The using json serialization.
        /// </summary>
        /// <returns>
        ///     The <see cref="SerializationWireup" />.
        /// </returns>
        public SerializationWireup UsingJsonSerialization()
        {
            Logger.Debug("Configuring custom NES Json serializer to cope with payloads that contain messages as interfaces.");

            return new SerializationWireup(
                this, 
                new JsonSerializer(() => DI.Current.Resolve<IEventMapper>(), () => DI.Current.Resolve<IEventFactory>()));
        }

        #endregion
    }
}