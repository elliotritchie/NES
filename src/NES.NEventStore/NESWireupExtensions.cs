using NEventStore;

namespace NES.NEventStore
{
    public static class NESWireupExtensions
    {
        public static NESWireup NES(this Wireup wireup)
        {
            return new NESWireup(wireup);
        }
    }
}
