namespace NES.Contracts
{
    public interface ISnapshotGeneric<TId> 
    {
        void RestoreSnapshot(Memento<TId> memento);
        Memento<TId> TakeSnapshot();
    }
}
