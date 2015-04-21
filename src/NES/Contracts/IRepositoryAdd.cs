namespace NES.Contracts
{
    public interface IRepositoryAdd
    {
        void Add<T>(T aggregate) where T : class, IEventSourceBase;
    }
}