using Confluent.Kafka;
using Newtonsoft.Json;
using System.Text;

namespace Core31.Shared.Services.ConfluentKafka.Serializers
{
    public class JsonSerializer<TData> : ISerializer<TData>
        where TData : class
    {
        public byte[] Serialize(TData data, SerializationContext context)
        {
            if (data == null)
            {
                return default(byte[]);
            }

            var json = JsonConvert.SerializeObject(data);
            return Encoding.Default.GetBytes(json);
        }
    }
}
