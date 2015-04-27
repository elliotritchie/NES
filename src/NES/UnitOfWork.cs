using System;
using System.Collections.Generic;
using System.Linq;
using NES.Contracts;

namespace NES
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ILogger Logger = LoggerFactory.Create(typeof(UnitOfWork));
        private readonly ICommandContextProvider _commandContextProvider;
        private readonly IEventSourceMapper _eventSourceMapper;
        private CommandContext _commandContext;
        private readonly Dictionary<IEventSourceBase, bool> _eventSources = new Dictionary<IEventSourceBase, bool>();

        public UnitOfWork(ICommandContextProvider commandContextProvider, IEventSourceMapper eventSourceMapper)
        {
            _commandContextProvider = commandContextProvider;
            _eventSourceMapper = eventSourceMapper;
        }

        public T Get<T, TId>(string bucketId, string id, int version = int.MaxValue)
            where T : class, IEventSource<TId>
        {
            bool isMaxVersion = version == int.MaxValue;

            var eventSource = this.GetCachedEventSource<T, TId>(bucketId, id, version, isMaxVersion);

            if (eventSource == null)
            {
                Logger.Debug("EventSource not found in memory with id {0} and BucketId {1}. So read from event source.", id, bucketId);
                eventSource = _eventSourceMapper.Get<T, TId>(bucketId, id, version);
            }

            this.RegisterInternal(eventSource, isMaxVersion);

            return eventSource;
        }

        private T GetCachedEventSource<T, TId>(string bucketId, string id, int version, bool isMaxVersion) where T : class, IEventSource<TId>
        {
            var toSearchIn = isMaxVersion ? this._eventSources.Where(e => e.Value).Select(e => e.Key).ToList() : this._eventSources.Keys.ToList();

            Func<T, bool> searchFilter = s => s.StringId == id && (s.BucketId == bucketId || string.IsNullOrEmpty(s.BucketId));
            if (!isMaxVersion)
            {
                Func<T, bool> filter = searchFilter;
                searchFilter = s => filter(s) && (s.Version == version);
            }
            var eventSource = toSearchIn.OfType<T>().SingleOrDefault(searchFilter);
            return eventSource;
        }

        public void Register<T>(T eventSource) where T : class, IEventSourceBase
        {
            this.RegisterInternal(eventSource, true);
        }

        private void RegisterInternal<T>(T eventSource, bool isMaxVersion) where T : class, IEventSourceBase
        {
            if (eventSource != null && !_eventSources.ContainsKey(eventSource))
            {
                Logger.Debug("Register event source Id '{0}', Version '{1}', Type '{2}'", eventSource.StringId, eventSource.Version, eventSource.GetType().Name);

                _eventSources.Add(eventSource, isMaxVersion);
            }

            if (_commandContext == null)
            {
                _commandContext = _commandContextProvider.Get();
            }
        }

        public void Commit()
        {
            foreach (var eventSource in _eventSources.Keys)
            {
                _eventSourceMapper.Set(_commandContext, eventSource);
            }
        }
    }
}