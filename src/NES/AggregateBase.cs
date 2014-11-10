using System;
using System.Collections.Generic;
using NES.Contracts;

namespace NES
{
    public abstract class AggregateBase : AggregateBaseGeneric<Guid, IEventSource, IMemento>, IEventSource
    {

    }
}