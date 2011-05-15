using NES.Tests.Stubs;

namespace NES.Tests.Mocks
{
    public class EventConverterMock : EventConverter<SomethingHappenedEvent, SomethingElseHappenedEvent>
    {
        public override SomethingElseHappenedEvent Convert(SomethingHappenedEvent @event)
        {
            return null;
        }
    }
}