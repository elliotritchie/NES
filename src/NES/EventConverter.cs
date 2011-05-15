namespace NES
{
    public abstract class EventConverter<TFrom, TTo>
    {
        public IEventFactory EventFactory { get; set; }
        public abstract TTo Convert(TFrom @event);
    }
}