using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NES.Contracts;

namespace NES.Tests
{
    [TestClass]
    public abstract class Test
    {
        [TestInitialize]
        public void TestInitialize()
        {
            LoggerFactory.Create = type => new Mock<ILogger>().Object;
            this.Context();
            this.Event();
        }

        protected abstract void Context();
        protected abstract void Event();
    }
}