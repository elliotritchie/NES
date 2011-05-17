using System;
using System.Linq;
using EventStore;
using EventStore.Dispatcher;

namespace NES.EventStore
{
    public class MessagePublisher : IPublishMessages
    {
        private readonly Func<IEventPublisher> _eventPublisherFactory;

        public MessagePublisher(Func<IEventPublisher> eventPublisherFactory)
        {
            _eventPublisherFactory = eventPublisherFactory;
        }

        public virtual void Publish(Commit commit)
        {
            _eventPublisherFactory().Publish(commit.Events.Select(e => e.Body));
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