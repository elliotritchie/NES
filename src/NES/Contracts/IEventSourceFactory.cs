namespace NES.Contracts
{
    public interface IEventSourceFactory
    {
        T Create<T>() where T : IEventSourceBase;
    }
}