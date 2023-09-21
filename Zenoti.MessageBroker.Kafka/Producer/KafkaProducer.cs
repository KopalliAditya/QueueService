using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenoti.MessageBroker.Kafka.Models;
using Zenoti.MessageBroker.Producer;

namespace Zenoti.MessageBroker.Kafka.Producer
{
    /// <summary>
    /// Kafka Producer
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    public class KafkaProducer<Key, Value> : IZenotiProducer<Key, Value>
    {
        private readonly IProducer<Key, Value> _producer;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaProducerConfigModel> topicOptions, IProducer<Key, Value> producer)
        {
            _topic = topicOptions.Value.Topic;
            _producer = producer;
        }

        /// <summary>
        /// Push the messages to the kafka topics
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task ProduceAsync(Key key, Value value)
        {
            await _producer.ProduceAsync(_topic, new Message<Key, Value> { Key = key, Value = value });
        }

        public void Dispose() => _producer?.Dispose();
        public int Flush(TimeSpan? timeout) => timeout is null
        ? _producer.Flush(TimeSpan.MaxValue)
        : _producer.Flush(timeout.Value);
    }
}
