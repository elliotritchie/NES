using System;
using System.Collections.Generic;

namespace NES
{
    public abstract class AggregateBase : IEventSource
    {
        public Guid Id { get; protected set; }

        int IEventSource.Version
        {
            get { return _version; }
        }

        private int _version;
        private readonly List<object> _events = new List<object>();
        private static readonly IEventFactory _eventFactory = DI.Current.Resolve<IEventFactory>();
        private static readonly IEventHandlerFactory _eventHandlerFactory = DI.Current.Resolve<IEventHandlerFactory>();

        void IEventSource.RestoreSnapshot(IMemento memento)
        {
            RestoreSnapshot(memento);

            Id = memento.Id;
            _version = memento.Version;
        }

        IMemento IEventSource.TakeSnapshot()
        {
            var snapshot = TakeSnapshot();

            snapshot.Id = Id;
            snapshot.Version = _version;

            return snapshot;
        }

        void IEventSource.Hydrate(IEnumerable<object> events)
        {
            events = this.ReoderEventsBeforeHydrate(events);

            foreach (var @event in events)
            {
                Raise(@event);
                _version++;
            }
        }

        IEnumerable<object> IEventSource.Flush()
        {
            var events = new List<object>(_events);

            _events.Clear();
            _version = _version + events.Count;

            return events;
        }

        protected void Apply<TEvent>(Action<TEvent> action)
        {
            var @event = _eventFactory.Create(action);

            Raise(@event);

            _events.Add(@event);
        }

        protected virtual void RestoreSnapshot(IMemento memento)
        {
        }

        protected virtual IMemento TakeSnapshot()
        {
            return null;
        }

        private void Raise(object @event)
        {
            _eventHandlerFactory.Get(this, @event.GetType())(@event);
        }

        protected virtual IEnumerable<object> ReoderEventsBeforeHydrate(IEnumerable<object> events)
        {
            return events;
        }
    }
}