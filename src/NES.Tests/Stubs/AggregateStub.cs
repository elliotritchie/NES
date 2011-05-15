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
            Apply<CreatedAggregateEvent>(e =>
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

        private void Handle(CreatedAggregateEvent @event)
        {
            HandledEvents.Add(@event);

            Id = @event.Id;
        }

        private void Handle(SomethingHappenedEvent @event)
        {
            HandledEvents.Add(@event);

            _something = @event.Something;
        }
    }
}