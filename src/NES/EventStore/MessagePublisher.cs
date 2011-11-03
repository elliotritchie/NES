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
            _eventPublisherFactory().Publish(commit.Events.Select(e => e.Body), commit.Headers, commit.Events.ToDictionary(e => e.Body, e => e.Headers));
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