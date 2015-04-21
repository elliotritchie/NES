namespace NES.Contracts
{
    public interface IUnitOfWork
    {
        T Get<T,TId>(string bucketId, string id, int version) where T : class, IEventSource<TId>;
        void Register<T>(T eventSource) where T : class, IEventSourceBase;

        void Commit();
    }
}