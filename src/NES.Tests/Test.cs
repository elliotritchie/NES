using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NES.Tests
{
    [TestClass]
    public abstract class Test
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Context();
            Event();
        }

        protected abstract void Context();
        protected abstract void Event();
    }
}