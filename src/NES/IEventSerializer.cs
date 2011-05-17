namespace NES
{
    public interface IEventSerializer
    {
        string Serialize(object @event);
        object Deserialize(string data);
    }
}