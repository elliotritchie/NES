namespace NES.Contracts
{
    public class Memento<T>
    {
        public T Id { get; set; }
        public int Version { get; set; }
        public string BucketId { get; set; }

        public string State { get; set; }
    }
}
