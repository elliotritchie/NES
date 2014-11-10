using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NES.Contracts;

namespace NES.StringWay
{
    public interface IRepository : IRepositoryAdd
    {
        T Get<T>(string bucketId, string id) where T : class, IEventSource;
        T Get<T>(string id) where T : class, IEventSource;
    }
}
