using System;
using System.Collections.Generic;
using Moq;
using NES.Contracts;
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

            var busMock = new Mock<IManageMessageHeaders>().As<IBus>();

            busMock.As<IManageMessageHeaders>().SetupGet(b => b.SetHeaderAction).Returns((o, s, arg3) => { });
            busMock.Setup(b => b.CurrentMessageContext).Returns(new MessageContext(new TransportMessage()));
            busMock.Setup(b => b.OutgoingHeaders).Returns(new Dictionary<string, string>());
            global::NServiceBus.ExtensionMethods.CurrentMessageBeingHandled = new Mock<ICommand>().Object;

            DI.Current.Register(() => busMock.Object);

            var commandContextProvider = new CommandContextProvider(busMock.Object);

            StoreEvents =
                Wireup.Init().UsingInMemoryPersistence().InitializeStorageEngine().NES().Build();
            
            DI.Current.Register<ICommandContextProvider>(() => commandContextProvider);
            DI.Current.Register<IEventPublisher, IBus>(bus => new BusAdapter(bus));
        }

        protected static IStoreEvents StoreEvents { get; private set; }

        protected BusScope BusScope
        {
            get { return new BusScope(); }
        }

        protected IRepository Repository { get; set; }

        protected void RepositoryAdd<T>(T eventSource) where T : class, IEventSourceBase
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