using System;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public class UnitOfWork : IUnitOfWork
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(UnitOfWork));
        private readonly ICommandContextProvider _commandContextProvider;
        private readonly IEventSourceMapper _eventSourceMapper;
        private CommandContext _commandContext;
        private readonly HashSet<IEventSource> _eventSources = new HashSet<IEventSource>();

        public UnitOfWork(ICommandContextProvider commandContextProvider, IEventSourceMapper eventSourceMapper)
        {
            _commandContextProvider = commandContextProvider;
            _eventSourceMapper = eventSourceMapper;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            

            var eventSource = _eventSources.OfType<T>().SingleOrDefault(s => s.Id == id) ?? _eventSourceMapper.Get<T>(id);
            
            Logger.Debug("Get id {0} eventSource found {1}", id, eventSource != null);

            Register(eventSource);

            return eventSource;
        }

        public void Register<T>(T eventSource) where T : class, IEventSource
        {
            if (_commandContext == null)
            {
                Logger.Debug("commandcontext was null so get the context using commandcontextprovider");
                _commandContext = _commandContextProvider.Get();
            }

            if (eventSource != null)
            {
                Logger.Debug("add eventsource to the list {0} Id {1} Version {2}", eventSource.GetType().FullName, eventSource.Id, eventSource.Version);
                _eventSources.Add(eventSource);
            }
        }

        public void Commit()
        {
            Logger.Debug("Commit doing");
            foreach (var eventSource in _eventSources)
            {
                _eventSourceMapper.Set(_commandContext, eventSource);
            }
            Logger.Debug("Commit done");
        }
    }
}