namespace NES.Contracts
{
    public interface ISnapshot
    {
        void RestoreSnapshot(IMemento memento);
        IMemento TakeSnapshot();
    }
}