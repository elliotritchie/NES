using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES.Sample.Messages;
using NES.Sample.Model;

namespace NES.Sample.Tests.Model
{
    public static class UserTests
    {
        [TestClass]
        public class When_creating_a_new_user : Test
        {
            private IEventSource _source;
            private readonly Guid _userId = GuidComb.NewGuidComb();
            private const string _username = "Bob";

            protected override void Context()
            {
            }

            protected override void Event()
            {
                _source = new User(_userId, _username);
            }

            [TestMethod]
            public void Should_apply_created_user_event()
            {
                var @event = _source.Flush().OfType<CreatedUserEvent>().Single();

                Assert.AreEqual(_userId, @event.UserId);
                Assert.AreEqual(_username, @event.Username);
            }
        }
    }
}