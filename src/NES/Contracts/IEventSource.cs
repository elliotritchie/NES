using System;

namespace NES.Contracts
{
    public interface IEventSource : IEventSourceGeneric<Guid, IMemento>
    {
    }
}