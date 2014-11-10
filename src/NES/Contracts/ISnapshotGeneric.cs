namespace NES.Contracts
{
    public interface ISnapshotGeneric<TId,TMemento> 
        where TMemento : IMementoGeneric<TId>
    {
        void RestoreSnapshot(TMemento memento);
        TMemento TakeSnapshot();
    }
}
