using System;
using NEventStore;

namespace NES.NEventStore
{
    public class EventConverterPipelineHook : IPipelineHook
    {
        private readonly Func<IEventConversionRunner> _eventConversionRunnerFactory;

        public EventConverterPipelineHook(Func<IEventConversionRunner> eventConversionRunnerFactory)
        {
            _eventConversionRunnerFactory = eventConversionRunnerFactory;
        }

        public ICommit Select(ICommit committed)
        {
            var eventConversionRunner = _eventConversionRunnerFactory();

            foreach (var eventMessage in committed.Events)
            {
                eventMessage.Body = eventConversionRunner.Run(eventMessage.Body);
            }

            return committed;
        }

        public bool PreCommit(CommitAttempt attempt)
        {
            return true;
        }

        public void PostCommit(ICommit committed)
        {
        }

        public void OnDeleteStream(string bucketId, string streamId)
        {
        }

        public void OnPurge(string bucketId)
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