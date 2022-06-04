using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Core31.Shared.Services.ConfluentKafka.Deserializers
{
    public class JsonDeserializer<TResult> : IDeserializer<TResult>
        where TResult : class
    {
        public TResult Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            try
            {
                var json = Encoding.Default.GetString(data);
                return JsonConvert.DeserializeObject<TResult>(json);
            }
            catch
            {
                return default(TResult);
            }
        }
    }
}
