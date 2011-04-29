using System;
using System.Linq;
using EventStore;
using EventStore.Dispatcher;

namespace NES.EventStore
{
    public class MessagePublisher : IPublishMessages
    {
        private readonly Func<IBusAdapter> _factory;

        public MessagePublisher(Func<IBusAdapter> factory)
        {
            _factory = factory;
        }

        public virtual void Publish(Commit commit)
        {
            _factory().Publish(commit.Events.Select(e => e.Body));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}