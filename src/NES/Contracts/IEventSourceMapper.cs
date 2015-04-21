namespace NES.Contracts
{
    public interface IEventSourceMapper
    {
        T Get<T, TId>(string bucketId, string id, int version) where T : class, IEventSource<TId>;
        void Set(CommandContext commandContext, IEventSourceBase eventSource);
    }
}