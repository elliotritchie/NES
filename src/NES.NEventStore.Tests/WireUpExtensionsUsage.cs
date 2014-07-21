using Microsoft.VisualStudio.TestTools.UnitTesting;
using NEventStore;

namespace NES.NEventStore.Tests
{
    [TestClass]
    public class WireUpExtensionsUsage
    {
        [TestMethod]
        public void TestUseNEvetStoreStartUpExtensionExplicit()
        {
            var store =
                Wireup.Init()
                    .UsingInMemoryPersistence()
                    .UsingSynchronousDispatchScheduler()
                    .Startup(DispatcherSchedulerStartup.Explicit)
                    .NES()
                    .Build();

            store.StartDispatchScheduler();
        }
    }
}