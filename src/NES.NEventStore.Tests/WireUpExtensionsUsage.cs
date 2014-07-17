// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WireUpExtensionsUsage.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   WireUpExtensionsUsage.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using global::NEventStore;

    /// <summary>
    ///     The wire up extensions usage.
    /// </summary>
    [TestClass]
    public class WireUpExtensionsUsage
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The test use n evet store start up extension explicit.
        /// </summary>
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

        #endregion
    }
}