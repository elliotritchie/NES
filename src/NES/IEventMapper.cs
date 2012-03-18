using System;

namespace NES
{
    public interface IEventMapper
    {
        Type GetMappedTypeFor(Type type);
    }
}