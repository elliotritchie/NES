namespace NES
{
    public interface IUnitOfWorkFactory
    {
        void Begin();
        void End();
        IUnitOfWork GetUnitOfWork();
    }
}