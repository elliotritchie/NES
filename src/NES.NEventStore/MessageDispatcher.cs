// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessageDispatcher.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The message dispatcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;
    using System.Linq;

    using global::NEventStore;

    using global::NEventStore.Dispatcher;

    /// <summary>
    ///     The message dispatcher.
    /// </summary>
    public class MessageDispatcher : IDispatchCommits
    {
        #region Fields

        private readonly Func<IEventPublisher> _eventPublisherFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageDispatcher"/> class.
        /// </summary>
        /// <param name="eventPublisherFactory">
        /// The event publisher factory.
        /// </param>
        public MessageDispatcher(Func<IEventPublisher> eventPublisherFactory)
        {
            this._eventPublisherFactory = eventPublisherFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The dispatch.
        /// </summary>
        /// <param name="commit">
        /// The commit.
        /// </param>
        public virtual void Dispatch(ICommit commit)
        {
            this._eventPublisherFactory()
                .Publish(commit.Events.Select(e => e.Body), commit.Headers, commit.Events.ToDictionary(e => e.Body, e => e.Headers));
        }

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}