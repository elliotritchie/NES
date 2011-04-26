using System;
using System.Collections.Generic;

namespace NES.Tests.Stubs
{
    public class AggregateStub : AggregateBase<IEvent>
    {
        public readonly List<IEvent> Events = new List<IEvent>();

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
        }

        private void Handle(SomethingHappenedEvent @event)
        {
            Events.Add(@event);
        }
    }
}