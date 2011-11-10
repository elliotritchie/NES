namespace NES
{
    public interface IEventConversionRunner
    {
        object Run(object @event);
    }
}