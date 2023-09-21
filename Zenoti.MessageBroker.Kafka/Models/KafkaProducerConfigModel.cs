using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenoti.MessageBroker.Kafka.Models
{
    /// <summary>
    /// Extending the producer config file
    /// </summary>
    public class KafkaProducerConfigModel : ProducerConfig
    {
        public string Topic { get; set; }
    }
}
