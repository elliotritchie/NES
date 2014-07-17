// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestIntegration.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The test integration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore.Tests
{
    using System;
    using System.Collections.Generic;

    using Moq;

    using NES.NServiceBus;

    using global::NEventStore;

    using global::NServiceBus;

    using global::NServiceBus.Unicast;

    /// <summary>
    ///     The test integration.
    /// </summary>
    public abstract class TestIntegration
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="TestIntegration" /> class.
        /// </summary>
        protected TestIntegration()
        {
            LoggerFactory.Create = type => new Mock<ILogger>().Object;

            this.Repository = new Repository();

            DI.Current.Register<IRepository>(() => this.Repository);

            var busMock = new Mock<IBus>();
            busMock.Setup(b => b.CurrentMessageContext).Returns(new MessageContext(new TransportMessage()));
            busMock.Setup(b => b.OutgoingHeaders).Returns(new Dictionary<string, string>());
            global::NServiceBus.ExtensionMethods.CurrentMessageBeingHandled = new Mock<ICommand>().Object;

            DI.Current.Register(() => busMock.Object);

            var commandContextProvider = new CommandContextProvider(busMock.Object);

            StoreEvents =
                Wireup.Init().UsingInMemoryPersistence().InitializeStorageEngine().UsingSynchronousDispatchScheduler().NES().Build();

            DI.Current.Register<ICommandContextProvider>(() => commandContextProvider);
            DI.Current.Register<IEventPublisher, IBus>(bus => new BusAdapter(bus));
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the store events.
        /// </summary>
        protected static IStoreEvents StoreEvents { get; private set; }

        /// <summary>
        ///     Gets the bus scope.
        /// </summary>
        protected BusScope BusScope
        {
            get
            {
                return new BusScope();
            }
        }

        /// <summary>
        ///     Gets or sets the repository.
        /// </summary>
        protected IRepository Repository { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The repository add.
        /// </summary>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <typeparam name="T">
        /// Type of the eventsource
        /// </typeparam>
        protected void RepositoryAdd<T>(T eventSource) where T : class, IEventSource
        {
            using (this.BusScope)
            {
                this.Repository.Add(eventSource);
            }
        }

        #endregion
    }

    /// <summary>
    ///     The bus scope.
    /// </summary>
    public class BusScope : IDisposable
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BusScope" /> class.
        /// </summary>
        public BusScope()
        {
            UnitOfWorkFactory.Begin();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            UnitOfWorkFactory.Current.Commit();
            UnitOfWorkFactory.End();
        }

        #endregion
    }
}