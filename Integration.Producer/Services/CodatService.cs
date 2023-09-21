using Confluent.Kafka;
using Integration.Models;
using Integration.Producer.Services.IServices;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Zenoti.MessageBroker.Producer;

namespace Integration.Producer.Services
{
    public class CodatService : ICodatService
    {
        readonly IZenotiProducer<string, CodatEvent> _zenotiProducer;
        private readonly ILogger _logger;
        public CodatService(IZenotiProducer<string, CodatEvent> zenotiProducer, ILogger<CodatService> logger)
        {
            _zenotiProducer = zenotiProducer;
            _logger = logger;
        }

        /// <summary>
        /// Implement PerformAction by pushing the data into Kafka.
        /// </summary>
        /// <param name="codatEvent"></param>
        /// <returns></returns>
        public bool PerformAction(CodatEvent codatEvent)
        {
            try
            {
                _logger.LogInformation($"Message received to producer API. {codatEvent}");
                var deliveryReport = _zenotiProducer.ProduceAsync(string.Empty, codatEvent);
                _logger.LogInformation($"Message sent (value: '{codatEvent}'). Delivery status: {deliveryReport.Status}");
                return true;
            }
            catch (ProduceException<long, string> e)
            {
                _logger.LogInformation($"Error: {e.Message} for message (value: '{e.DeliveryResult.Value}')");
                return false;
            }
        }
    }
}
