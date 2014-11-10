namespace NES.Contracts
{
    public interface IEventSourceMapper
    {
        T Get<T, TId, TMemento>(string bucketId, string id)
            where T : class, IEventSourceGeneric<TId, TMemento>
            where TMemento : class, IMementoGeneric<TId>;

        void Set(CommandContext commandContext, IEventSourceBase eventSource);
    }
}