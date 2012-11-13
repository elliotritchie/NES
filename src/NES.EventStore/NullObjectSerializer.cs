using System.IO;
using EventStore.Serialization;

namespace NES.EventStore
{
    public class NullObjectSerializer : ISerialize
    {
        public void Serialize<T>(Stream output, T graph)
        {
        }

        public T Deserialize<T>(Stream input)
        {
            return default(T);
        }
    }
}