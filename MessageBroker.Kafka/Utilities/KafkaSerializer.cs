using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenoti.MessageBroker.Kafka.Utilities
{
    /// <summary>
    /// Kafka Serializer from JSON to Model
    /// </summary>
    /// <param name="data"></param>
    /// <param name="isNull"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    internal sealed class KafkaSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
                return new byte[] { };

            if (typeof(T) == typeof(Ignore))
                throw new NotSupportedException("Not Supported.");

            var json = JsonConvert.SerializeObject(data);

            return Encoding.UTF8.GetBytes(json);
        }
    }
}
