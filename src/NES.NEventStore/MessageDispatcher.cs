using System;
using System.Linq;
using NEventStore;
using NEventStore.Dispatcher;

namespace NES.EventStore
{
    public class MessageDispatcher : IDispatchCommits
    {
        private readonly Func<IEventPublisher> _eventPublisherFactory;

        public MessageDispatcher(Func<IEventPublisher> eventPublisherFactory)
        {
            _eventPublisherFactory = eventPublisherFactory;
        }

        public virtual void Dispatch(Commit commit)
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