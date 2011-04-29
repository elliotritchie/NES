using EventStore;

namespace NES.EventStore
{
    public static class WireupExtensions
    {
        public static NESWireup NES(this Wireup wireup)
        {
            return new NESWireup(wireup);
        }
    }
}