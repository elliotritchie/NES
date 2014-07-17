// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AggregateBase.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The aggregate base.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     The aggregate base.
    /// </summary>
    public abstract class AggregateBase : IEventSource
    {
        #region Static Fields

        private static readonly IEventFactory _eventFactory = DI.Current.Resolve<IEventFactory>();

        private static readonly IEventHandlerFactory _eventHandlerFactory = DI.Current.Resolve<IEventHandlerFactory>();

        #endregion

        #region Fields

        private readonly List<object> _events = new List<object>();

        private int _version;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the bucket id.
        /// </summary>
        public string BucketId { get; protected set; }

        /// <summary>
        ///     Gets or sets the id.
        /// </summary>
        public Guid Id { get; protected set; }

        #endregion

        #region Explicit Interface Properties

        int IEventSource.Version
        {
            get
            {
                return this._version;
            }
        }

        #endregion

        #region Explicit Interface Methods

        IEnumerable<object> IEventSource.Flush()
        {
            var events = new List<object>(this._events);

            this._events.Clear();
            this._version = this._version + events.Count;

            return events;
        }

        void IEventSource.Hydrate(IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                this.Raise(@event);
                this._version++;
            }
        }

        void IEventSource.RestoreSnapshot(IMemento memento)
        {
            this.RestoreSnapshot(memento);

            this.Id = memento.Id;
            this.BucketId = memento.BucketId;
            this._version = memento.Version;
        }

        IMemento IEventSource.TakeSnapshot()
        {
            var snapshot = this.TakeSnapshot();

            snapshot.Id = this.Id;
            snapshot.Version = this._version;

            return snapshot;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The apply.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        /// <typeparam name="TEvent">
        /// The event action which will by executed on created event
        /// </typeparam>
        protected void Apply<TEvent>(Action<TEvent> action)
        {
            var @event = _eventFactory.Create(action);

            this.Raise(@event);

            this._events.Add(@event);
        }

        /// <summary>
        /// The restore snapshot.
        /// </summary>
        /// <param name="memento">
        /// The memento.
        /// </param>
        protected virtual void RestoreSnapshot(IMemento memento)
        {
        }

        /// <summary>
        ///     The take snapshot.
        /// </summary>
        /// <returns>
        ///     The <see cref="IMemento" />.
        /// </returns>
        protected virtual IMemento TakeSnapshot()
        {
            return null;
        }

        private void Raise(object @event)
        {
            _eventHandlerFactory.Get(this, @event.GetType())(@event);
        }

        #endregion
    }
}