using NEventStore;

namespace NES.EventStore
{
    using System;

    using NEventStore.Logging;

    public static class NESWireupExtensions
    {
        public static NESWireup NES(this Wireup wireup)
        {
            return new NESWireup(wireup);
        }
    }
}
