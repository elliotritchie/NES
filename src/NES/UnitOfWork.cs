using System;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ILogger Logger = LoggerFactory.Create(typeof(UnitOfWork));
        private readonly ICommandContextProvider _commandContextProvider;
        private readonly IEventSourceMapper _eventSourceMapper;
        private CommandContext _commandContext;
        private readonly List<IEventSource> _eventSources = new List<IEventSource>();

        public UnitOfWork(ICommandContextProvider commandContextProvider, IEventSourceMapper eventSourceMapper)
        {
            _commandContextProvider = commandContextProvider;
            _eventSourceMapper = eventSourceMapper;
        }

        public T Get<T>(string bucketId, Guid id) where T : class, IEventSource
        {
            var eventSource = _eventSources.OfType<T>().SingleOrDefault(s => s.Id == id && (s.BucketId == bucketId || string.IsNullOrEmpty(s.BucketId)));

            if (eventSource == null)
            {
                Logger.Debug("EventSource not found in mememory with id {0} and BucketId {1}. So read from event source.", id, bucketId);
                eventSource = _eventSourceMapper.Get<T>(bucketId, id);
            }

            this.Register(eventSource);

            return eventSource;
        }

        public void Register<T>(T eventSource) where T : class, IEventSource
        {
            if (eventSource != null && !_eventSources.Contains(eventSource))
            {
                Logger.Debug("Register event source Id '{0}', Version '{1}', Type '{2}'", eventSource.Id, eventSource.Version, eventSource.GetType().Name);

                _eventSources.Add(eventSource);
            }

            if (_commandContext == null)
            {
                _commandContext = _commandContextProvider.Get();
            }
        }

        public void Commit()
        {
            foreach (var eventSource in _eventSources)
            {
                _eventSourceMapper.Set(_commandContext, eventSource);
            }
        }
    }
}