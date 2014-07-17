// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Test.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The test.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    ///     The test.
    /// </summary>
    [TestClass]
    public abstract class Test
    {
        #region Public Methods and Operators

        /// <summary>
        ///     The test initialize.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            LoggerFactory.Create = type => new Mock<ILogger>().Object;
            this.Context();
            this.Event();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The context.
        /// </summary>
        protected abstract void Context();

        /// <summary>
        ///     The event.
        /// </summary>
        protected abstract void Event();

        #endregion
    }
}