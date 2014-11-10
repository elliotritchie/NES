namespace NES.Contracts
{
    public interface IEventSourceGeneric<TId, TMemento> : IEventSourceBase, IIdGeneric<TId>, ISnapshotGeneric<TId, TMemento> 
        where TMemento : IMementoGeneric<TId>
    {
    }
}
