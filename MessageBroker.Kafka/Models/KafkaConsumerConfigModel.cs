using Confluent.Kafka;
using System.Collections.Generic;

namespace Zenoti.MessageBroker.Kafka.Models
{
    /// <summary>
    /// Extending the consumer config file
    /// </summary>
    public class KafkaConsumerConfigModel : ConsumerConfig
    {
        public IEnumerable<string> Topic { get; set; }
        public int TimeOut { get; set; }

        public KafkaConsumerConfigModel()
        {
            AutoOffsetReset = Confluent.Kafka.AutoOffsetReset.Latest;
            EnableAutoOffsetStore = false;
            TimeOut = 1000;
        }
    }
}
