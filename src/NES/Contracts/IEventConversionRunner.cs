namespace NES.Contracts
{
    public interface IEventConversionRunner
    {
        object Run(object @event);
    }
}