using System;

namespace NES.Contracts
{
    public interface IEventMapper
    {
        Type GetMappedTypeFor(Type type);
    }
}