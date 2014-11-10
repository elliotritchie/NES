namespace NES.Contracts
{
    public interface IUnitOfWork
    {
        T Get<T,TId, TMemento>(string bucketId, string id) where T : class, IEventSourceGeneric<TId,TMemento> where TMemento : class, IMementoGeneric<TId>;
        void Register<T>(T eventSource) where T : class, IEventSourceBase;

        void Commit();
    }
}