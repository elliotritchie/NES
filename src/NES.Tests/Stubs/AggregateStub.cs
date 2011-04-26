using System;
using System.Collections.Generic;

namespace NES.Tests.Stubs
{
    public class AggregateStub : AggregateBase<IEvent>
    {
        public readonly List<IEvent> HandledEvents = new List<IEvent>();

        private string _something;

        public AggregateStub(Guid id)
        {
            Apply<CreatedAggregateStubEvent>(e =>
            {
                e.Id = id;
            });
        }

        public AggregateStub()
        {
        }

        public void DoSomething(string value)
        {
            Apply<SomethingHappenedEvent>(e =>
            {
                e.Something = value;
            });
        }

        private void Handle(CreatedAggregateStubEvent @event)
        {
            Id = @event.Id;

            HandledEvents.Add(@event);
        }

        private void Handle(SomethingHappenedEvent @event)
        {
            _something = @event.Something;

            HandledEvents.Add(@event);
        }
    }
}