namespace NES.Contracts
{
    public interface IMementoBase : IStringId
    {
        string BucketId { get; set; }

        int Version { get; set; }
    }
}