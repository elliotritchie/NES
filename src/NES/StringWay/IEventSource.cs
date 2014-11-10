using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NES.Contracts;

namespace NES.StringWay
{
    public interface IEventSource : IEventSourceGeneric<string, IMemento>
    {
    }
}
