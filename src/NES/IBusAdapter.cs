namespace NES
{
    public interface IBusAdapter
    {
        void Publish(params object[] events);
    }
}