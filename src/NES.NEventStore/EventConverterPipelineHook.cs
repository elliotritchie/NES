// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventConverterPipelineHook.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The event converter pipeline hook.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES.NEventStore
{
    using System;

    using global::NEventStore;

    /// <summary>
    ///     The event converter pipeline hook.
    /// </summary>
    public class EventConverterPipelineHook : IPipelineHook
    {
        #region Fields

        private readonly Func<IEventConversionRunner> _eventConversionRunnerFactory;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventConverterPipelineHook"/> class.
        /// </summary>
        /// <param name="eventConversionRunnerFactory">
        /// The event conversion runner factory.
        /// </param>
        public EventConverterPipelineHook(Func<IEventConversionRunner> eventConversionRunnerFactory)
        {
            this._eventConversionRunnerFactory = eventConversionRunnerFactory;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The on delete stream.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <param name="streamId">
        /// The stream id.
        /// </param>
        public void OnDeleteStream(string bucketId, string streamId)
        {
        }

        /// <summary>
        /// The on purge.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        public void OnPurge(string bucketId)
        {
        }

        /// <summary>
        /// The post commit.
        /// </summary>
        /// <param name="committed">
        /// The committed.
        /// </param>
        public void PostCommit(ICommit committed)
        {
        }

        /// <summary>
        /// The pre commit.
        /// </summary>
        /// <param name="attempt">
        /// The attempt.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool PreCommit(CommitAttempt attempt)
        {
            return true;
        }

        /// <summary>
        /// The select.
        /// </summary>
        /// <param name="committed">
        /// The committed.
        /// </param>
        /// <returns>
        /// The <see cref="ICommit"/>.
        /// </returns>
        public ICommit Select(ICommit committed)
        {
            var eventConversionRunner = this._eventConversionRunnerFactory();

            foreach (var eventMessage in committed.Events)
            {
                eventMessage.Body = eventConversionRunner.Run(eventMessage.Body);
            }

            return committed;
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