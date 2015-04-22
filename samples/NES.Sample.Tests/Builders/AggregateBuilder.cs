using System;
using System.Collections.Generic;

namespace NES.Sample.Tests.Builders
{
    using NES.Contracts;

    public abstract class AggregateBuilder<T> where T : AggregateBase<Guid>
    {
        private readonly IEventSourceFactory _eventSourceFactory = new EventSourceFactory();
        private readonly IEventFactory _eventFactory = new EventFactory();
        private readonly List<object> _events = new List<object>();
        private IEventSource<Guid> _source;
        private T _aggregate;

        protected void Apply<TEvent>(Action<TEvent> action)
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