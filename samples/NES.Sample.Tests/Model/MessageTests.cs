using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NES.Sample.Messages;
using NES.Sample.Model;
using NES.Sample.Tests.Builders;

namespace NES.Sample.Tests.Model
{
    using NES.Contracts;

    public static class MessageTests
    {
        [TestClass]
        public class When_sending_a_message : Test
        {
            private User _user;
            private IEventSource<Guid> _source;
            private readonly Guid _userId = GuidComb.NewGuidComb();
            private readonly Guid _messageId = GuidComb.NewGuidComb();
            private const string _message = "Hello!";
            private readonly DateTime _now = DateTime.UtcNow;

            protected override void Context()
            {
                _user = new UserBuilder().CreatedUser(_userId).Build();
            }

            protected override void Event()
            {
                _source = new Message(_user, _messageId, _message);
            }

            [TestMethod]
            public void Should_apply_sent_message_event()
            {
                var @event = _source.Flush().OfType<ISentMessageEvent>().Single();

                Assert.AreEqual(_userId, @event.UserId);
                Assert.AreEqual(_messageId, @event.MessageId);
                Assert.AreEqual(_message, @event.Message);
                Assert.IsTrue(@event.Sent > _now);
            }
        }
    }
}