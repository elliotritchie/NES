using System;
using System.Collections.Generic;
using NES.Sample.Messages;
using NES.Sample.Model;

namespace NES.Sample.Tests.Builders
{
    public abstract class AggregateBuilder<T> where T : Aggregate
    {
        private readonly IEventSourceFactory _eventSourceFactory = new EventSourceFactory();
        private readonly IEventFactory _eventFactory = new EventFactory();
        private readonly List<IEvent> _events = new List<IEvent>();
        private IEventSource _source;
        private T _aggregate;

        protected void Apply<TEvent>(Action<TEvent> action) where TEvent : IEvent
        {
            _events.Add(_eventFactory.Create(action));
        }

        public T Build()
        {
            _source = _aggregate = _eventSourceFactory.Create<T>();

            _source.Hydrate(_events);
            _source.Flush();

            return _aggregate;
        }
    }
}