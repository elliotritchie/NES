using System;
using System.Collections.Generic;

namespace NES.Tests.Stubs
{
    public class AggregateStringStub : AggregateBase<string>
    {
        public readonly List<IEvent> HandledEvents = new List<IEvent>();

        private string _something;

        public AggregateStringStub(Guid id)
        {
            Apply<CreatedAggregateEvent>(e =>
            {
                e.Id = id;
            });
        }

        public AggregateStringStub()
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

            Id = @event.Id.ToString();
        }

        private void Handle(SomethingHappenedEvent @event)
        {
            HandledEvents.Add(@event);

            _something = @event.Something;
        }
    }
}