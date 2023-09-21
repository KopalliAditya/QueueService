using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zenoti.MessageBroker.Consumer;
using Zenoti.MessageBroker.Kafka.Models;
using Zenoti.MessageBroker.Kafka.Utilities;

namespace Zenoti.MessageBroker.Kafka.Consumer
{
    public class KafkaBackgroundConsumer<Key, Value> : BackgroundService, IConsumer
    {
        private readonly IOptionsMonitor<KafkaConsumerConfigModel> _configOptions;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public KafkaBackgroundConsumer(IOptionsMonitor<KafkaConsumerConfigModel> config,
            IServiceScopeFactory serviceScopeFactory, ILogger<KafkaBackgroundConsumer<Key, Value>> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _configOptions = config;
            this._logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Run(() => this.ConsumeAsync(stoppingToken), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogError("Closing consumer.");
            }
        }

        /// <summary>
        /// Consume the events from the queue.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task ConsumeAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var _handler = scope.ServiceProvider.GetRequiredService<IHandler<Key, Value>>();

            var _config = _configOptions.Get(_handler.GetType().FullName);

            var builder = new ConsumerBuilder<Key, Value>(_config).SetValueDeserializer(new KafkaDeserializer<Value>());

            using IConsumer<Key, Value> consumer = builder.Build();
            consumer.Subscribe(_config.Topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                var consumerResult = consumer.Consume(_config.TimeOut);
                
                if (consumerResult != null)
                {
                    await _handler.HandleAsync(consumerResult.Message.Key, consumerResult.Message.Value);

                    // The Commit method sends a "commit offsets" request to the Kafka
                    // cluster and synchronously waits for the response. This is very
                    // slow compared to the rate at which the consumer is capable of
                    // consuming messages. A high performance application will typically
                    // commit offsets relatively infrequently and be designed handle
                    // duplicate messages in the event of failure.
                    // TODO: Handle this differently.
                    try
                    {
                        consumer.Commit(consumerResult);
                    }
                    catch (KafkaException e)
                    {
                        _logger.LogError($"Commit error: {e.Error.Reason}");
                        throw;
                    }
                    consumer.StoreOffset(consumerResult);
                }
            }
        }
    }
}
