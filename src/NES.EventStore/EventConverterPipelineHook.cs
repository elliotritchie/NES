using System;
using NEventStore;

namespace NES.EventStore
{
    public class EventConverterPipelineHook : IPipelineHook
    {
        private static readonly ILogger Logger = LoggingFactory.BuildLogger(typeof(EventConverterPipelineHook));
        private readonly Func<IEventConversionRunner> _eventConversionRunnerFactory;

        public EventConverterPipelineHook(Func<IEventConversionRunner> eventConversionRunnerFactory)
        {
            _eventConversionRunnerFactory = eventConversionRunnerFactory;
        }

        public Commit Select(Commit committed)
        {
            Logger.Debug(string.Format("Select commitId{0}", committed.CommitId));

            var eventConversionRunner = _eventConversionRunnerFactory();

            foreach (var eventMessage in committed.Events)
            {
                eventMessage.Body = eventConversionRunner.Run(eventMessage.Body);
            }

            return committed;
        }

        public bool PreCommit(Commit attempt)
        {
            Logger.Debug("PreComit");
            return true;
        }

        public void PostCommit(Commit committed)
        {
            Logger.Debug("PostCommit");
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