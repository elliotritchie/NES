using EventStore;

namespace NES.EventStore
{
    public static class NESWireupExtensions
    {
        public static NESWireup NES(this Wireup wireup)
        {
            return new NESWireup(wireup);
        }
    }
}