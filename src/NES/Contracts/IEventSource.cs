namespace NES.Contracts
{
    public interface IEventSource<TId> : IEventSourceBase, IIdGeneric<TId>, ISnapshotGeneric<TId> 
    {
    }
}
