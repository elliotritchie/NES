// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitOfWork.cs" company="Elliot Ritchie">
//   Copyright © Elliot Ritchie. All rights reserved.
// </copyright>
// <summary>
//   The unit of work.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace NES
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     The unit of work.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Static Fields

        private static readonly ILogger Logger = LoggerFactory.Create(typeof(UnitOfWork));

        #endregion

        #region Fields

        private readonly ICommandContextProvider _commandContextProvider;

        private readonly IEventSourceMapper _eventSourceMapper;

        private readonly List<IEventSource> _eventSources = new List<IEventSource>();

        private CommandContext _commandContext;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="commandContextProvider">
        /// The command context provider.
        /// </param>
        /// <param name="eventSourceMapper">
        /// The event source mapper.
        /// </param>
        public UnitOfWork(ICommandContextProvider commandContextProvider, IEventSourceMapper eventSourceMapper)
        {
            this._commandContextProvider = commandContextProvider;
            this._eventSourceMapper = eventSourceMapper;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The commit.
        /// </summary>
        public void Commit()
        {
            foreach (var eventSource in this._eventSources)
            {
                this._eventSourceMapper.Set(this._commandContext, eventSource);
            }
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="bucketId">
        /// The bucket id.
        /// </param>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <typeparam name="T">
        /// Type of event source
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource
        {
            var eventSource = this._eventSources.OfType<T>().SingleOrDefault(s => s.Id == id && (s.BucketId == bucketId || string.IsNullOrEmpty(s.BucketId)));

            if (eventSource == null)
            {
                Logger.Debug(string.Format("EventSource not found in mememory with id {0} and BucketId {1}. So read from event source.", id, bucketId));
                eventSource = this._eventSourceMapper.Get<T>(bucketId, id);
            }
                              
            this.Register(eventSource);

            return eventSource;
        }

        /// <summary>
        /// The register.
        /// </summary>
        /// <param name="eventSource">
        /// The event source.
        /// </param>
        /// <typeparam name="T">
        /// Type of event source
        /// </typeparam>
        public void Register<T>(T eventSource) where T : class, IEventSource
        {
            if (eventSource != null && !this._eventSources.Contains(eventSource))
            {
                Logger.Debug(
                    "Register event source Id '{0}', Version '{1}', Type '{2}'", 
                    eventSource.Id, 
                    eventSource.Version, 
                    eventSource.GetType().Name);

                this._eventSources.Add(eventSource);
            }

            if (this._commandContext == null)
            {
                this._commandContext = this._commandContextProvider.Get();
            }
        }

        #endregion
    }
}