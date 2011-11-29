using System;
using EventStore;

namespace NES.EventStore
{
    public class EventConverterPipelineHook : IPipelineHook
    {
        private readonly Func<IEventConversionRunner> _eventConversionRunnerFactory;

        public EventConverterPipelineHook(Func<IEventConversionRunner> eventConversionRunnerFactory)
        {
            _eventConversionRunnerFactory = eventConversionRunnerFactory;
        }

        public Commit Select(Commit committed)
        {
            var eventConversionRunner = _eventConversionRunnerFactory();

            foreach (var eventMessage in committed.Events)
            {
                eventMessage.Body = eventConversionRunner.Run(eventMessage.Body);
            }

            return committed;
        }

        public bool PreCommit(Commit attempt)
        {
            return true;
        }

        public void PostCommit(Commit committed)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}