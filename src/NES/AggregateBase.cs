using System;
using System.Collections.Generic;
using System.ComponentModel;
using NES.Contracts;

namespace NES
{
    public abstract class AggregateBase<TId> : IEventSource<TId>
    {
        public TId Id { get; protected set; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        string IStringId.StringId
        {
            get { return this.ConvertToStringId(this.Id); }
        }

        public string BucketId { get; protected set; }

        int IEventSourceBase.Version
        {
            get { return _version; }
        }

        private int _version;
        private readonly List<object> _events = new List<object>();
        private static readonly IEventFactory _eventFactory = DI.Current.Resolve<IEventFactory>();
        private static readonly IEventHandlerFactory _eventHandlerFactory = DI.Current.Resolve<IEventHandlerFactory>();

        protected virtual string ConvertToStringId(TId id)
        {
            return object.ReferenceEquals(id, null) ? string.Empty : id.ToString();
        }

        void ISnapshotGeneric<TId>.RestoreSnapshot(Memento<TId> memento)
        {
            RestoreSnapshot(memento);

            Id = memento.Id;
            _version = memento.Version;
            BucketId = memento.BucketId;
        }

        Memento<TId> ISnapshotGeneric<TId>.TakeSnapshot()
        {
            var snapshot = TakeSnapshot();

            snapshot.Id = Id;
            snapshot.Version = _version;
            snapshot.BucketId = BucketId;

            return snapshot;
        }

        void IEventSourceBase.Hydrate(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                Raise(@event);
                _version++;
            }
        }

        IEnumerable<object> IEventSourceBase.Flush()
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

        protected virtual void RestoreSnapshot(Memento<TId> memento)
        {
        }

        protected virtual Memento<TId> TakeSnapshot()
        {
            return null;
        }

        private void Raise(object @event)
        {
            _eventHandlerFactory.Get(this, @event.GetType())(@event);
        }
    }
}