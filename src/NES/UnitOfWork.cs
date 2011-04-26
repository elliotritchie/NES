using System;
using System.Collections.Generic;
using System.Linq;

namespace NES
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IEventSourceMapper _eventSourceMapper;
        private readonly HashSet<IEventSource> _eventSources = new HashSet<IEventSource>();

        public UnitOfWork(IEventSourceMapper eventSourceMapper)
        {
            _eventSourceMapper = eventSourceMapper;
        }

        public T Get<T>(Guid id) where T : class, IEventSource
        {
            var eventSource = _eventSources.OfType<T>().SingleOrDefault(s => s.Id == id) ?? _eventSourceMapper.Get<T>(id);

            Register(eventSource);

            return eventSource;
        }

        public void Register<T>(T eventSource) where T : class, IEventSource
        {
            _eventSources.Add(eventSource);
        }

        public void Commit()
        {
            foreach (var eventSource in _eventSources)
            {
                _eventSourceMapper.Set(eventSource);
            }
        }
    }
}