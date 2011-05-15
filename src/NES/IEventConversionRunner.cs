using System.Collections.Generic;

namespace NES
{
    public interface IEventConversionRunner
    {
        IEnumerable<object> Run(IEnumerable<object> events);
    }
}