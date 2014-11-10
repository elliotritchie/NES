namespace NES.Contracts
{
    public interface ICommandContextProvider
    {
        CommandContext Get();
    }
}