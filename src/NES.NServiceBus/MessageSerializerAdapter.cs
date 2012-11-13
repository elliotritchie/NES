using System.IO;
using NServiceBus.Serialization;

namespace NES.NServiceBus
{
    public class MessageSerializerAdapter : IEventSerializer
    {
        private readonly IMessageSerializer _messageSerializer;

        public MessageSerializerAdapter(IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;
        }

        public string Serialize(object @event)
        {
            using (var stream = new MemoryStream())
            {
                var reader = new StreamReader(stream);

                _messageSerializer.Serialize(new[] { @event }, stream);
                stream.Position = 0;

                return reader.ReadToEnd();
            }
        }

        public object Deserialize(string data)
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);

                writer.Write(data);
                writer.Flush();
                stream.Position = 0;

                return _messageSerializer.Deserialize(stream)[0];
            }
        }
    }
}