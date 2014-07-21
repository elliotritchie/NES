﻿using System;
using System.Collections.Generic;
using Moq;
using NES.NServiceBus;
using NEventStore;
using NServiceBus;
using NServiceBus.Unicast;

namespace NES.NEventStore.Tests
{
    public abstract class TestIntegration
    {
        protected TestIntegration()
        {
            LoggerFactory.Create = type => new Mock<ILogger>().Object;

            this.Repository = new Repository();

            DI.Current.Register(() => this.Repository);

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

        protected static IStoreEvents StoreEvents { get; private set; }

        protected BusScope BusScope
        {
            get { return new BusScope(); }
        }

        protected IRepository Repository { get; set; }

        protected void RepositoryAdd<T>(T eventSource) where T : class, IEventSource
        {
            using (this.BusScope)
            {
                this.Repository.Add(eventSource);
            }
        }
    }

    public class BusScope : IDisposable
    {
        public BusScope()
        {
            UnitOfWorkFactory.Begin();
        }

        public void Dispose()
        {
            UnitOfWorkFactory.Current.Commit();
            UnitOfWorkFactory.End();
        }
    }
}