namespace NES.Tests.Stubs
{
    public interface SomethingHappenedEvent : IEvent
    {
        string Something { get; set; }
    }
}