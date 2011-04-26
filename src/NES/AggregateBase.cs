using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public abstract class AggregateBase<T> : IEventSource
    {
        public Guid Id { get; protected set; }

        int IEventSource.Version
        {
            get { return _version; }
        }

        private int _version;
        private readonly List<T> _events = new List<T>();
        private static readonly EventFactory<T> _eventFactory = new EventFactory<T>();
        private static readonly EventHandlerFactory<T> _eventHandlerFactory = new EventHandlerFactory<T>();

        void IEventSource.Hydrate(IEnumerable events)
        {
            foreach (var @event in events.Cast<T>())
            {
                Raise(@event);
                _version++;
            }
        }

        IEnumerable IEventSource.Flush()
        {
            var events = new List<T>(_events);

            _events.Clear();
            _version = _version + events.Count;

            return events;
        }

        protected void Apply<TEvent>(Action<TEvent> action) where TEvent : T
        {
            var @event = _eventFactory.Create(action);

            Raise(@event);

            _events.Add(@event);
        }

        private void Raise(T @event)
        {
            _eventHandlerFactory.Get(GetType(), @event.GetType())(this, @event);
        }
    }
}