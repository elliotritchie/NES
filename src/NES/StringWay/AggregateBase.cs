using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NES.StringWay
{
    public abstract class AggregateBase : AggregateBaseGeneric<string, IEventSource, IMemento>, IEventSource
    {

    }
}
