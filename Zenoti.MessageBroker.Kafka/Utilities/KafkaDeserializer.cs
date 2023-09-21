using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenoti.MessageBroker.Kafka.Utilities
{
    internal sealed class KafkaDeserializer<T> : IDeserializer<T>
    {
        /// <summary>
        /// Kafka Deserializer from JSON to Model
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isNull"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
            {
                if (data.Length > 0)
                    throw new ArgumentException("The data is null not null.");
                return default;
            }

            if (typeof(T) == typeof(Ignore))
                return default;

            var dataJson = Encoding.UTF8.GetString(data);

            return JsonConvert.DeserializeObject<T>(dataJson);
        }
    }
}
