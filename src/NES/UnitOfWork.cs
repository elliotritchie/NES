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

        public T Get<T>(Guid id, Func<IEnumerable<object>, IEnumerable<object>> sortFilterAction) where T : class, IEventSource
        {
            var eventSource = _eventSources.OfType<T>().SingleOrDefault(s => s.Id == id) ?? _eventSourceMapper.Get<T>(id, sortFilterAction);
            
            Register(eventSource);

            return eventSource;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            return this.Get<T>(id, null);
        }

        public IEnumerable<object> GetEvents(Guid id)
        {
            return _eventSourceMapper.GetEvents(id);
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