using Zenoti.MessageBroker.Kafka.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Zenoti.MessageBroker.Consumer;
using Microsoft.Extensions.Options;
using Confluent.Kafka;
using Zenoti.MessageBroker.Kafka.Producer;
using Zenoti.MessageBroker.Kafka.Consumer;
using Zenoti.MessageBroker.Kafka.Utilities;
using Zenoti.MessageBroker.Producer;

namespace Zenoti.MessageBroker.Kafka.Extensions
{
    public static class StartupServiceExtensions
    {

        /// <summary>
        /// Add Kafka Messaging singleton for the producer
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaServiceProducer(this IServiceCollection serviceCollection)
           => serviceCollection.AddSingleton(typeof(IZenotiProducer<,>), typeof(KafkaProducer<,>));

        /// <summary>
        /// Services for the producer
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services,
           Action<KafkaProducerConfigModel> configAction)
        {
            services.AddConfluentKafkaProducer<TKey, TValue>();

            services.AddSingleton<KafkaProducer<TKey, TValue>>();

            services.Configure(configAction);

            return services;
        }

        /// <summary>
        /// Services for the consumer
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="Consumer"></typeparam>
        /// <param name="services"></param>
        /// <param name="configAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddKafkaConsumer<TKey, TValue, Consumer>(this IServiceCollection services,
           Action<KafkaConsumerConfigModel> configAction) where Consumer : class, IHandler<TKey, TValue>
        {
            services.AddScoped<IHandler<TKey, TValue>, Consumer>();

            services.AddHostedService<KafkaBackgroundConsumer<TKey, TValue>>();

            services.Configure(typeof(Consumer).FullName , configAction);

            return services;
        }

        /// <summary>
        /// Add Confluent kafka producer
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        private static IServiceCollection AddConfluentKafkaProducer<TKey, TValue>(this IServiceCollection services)
        {
            services.AddSingleton(
                sp =>
                {
                    var config = sp.GetRequiredService<IOptions<KafkaProducerConfigModel>>();

                    var builder = new ProducerBuilder<TKey, TValue>(config.Value)
                    .SetValueSerializer(new KafkaSerializer<TValue>());

                    return builder.Build();
                });

            return services;
        }
    }
}
