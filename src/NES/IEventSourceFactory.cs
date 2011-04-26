namespace NES
{
    public interface IEventSourceFactory
    {
        T Create<T>() where T : IEventSource;
    }
}