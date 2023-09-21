using Integration.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Zenoti.MessageBroker.Consumer;

namespace Integration.Consumer.ResponseHandler
{
    public class CodatResponseHandler : IHandler<string, CodatEvent>
    {
        private readonly HttpClient _httpclient;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public CodatResponseHandler(ILogger<CodatResponseHandler> logger, HttpClient httpclient, IConfiguration configuration)
        {
            this._httpclient = httpclient;
            this._logger = logger;
            this._configuration = configuration;
        }
        
        /// <summary>
        /// Handle Codat response async from the queue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="consumerEvent"></param>
        /// <returns></returns>
        public async Task HandleAsync(string key, CodatEvent consumerEvent)
        {
            try
            {
                _logger.LogInformation("Processing codat request from the kafka queue ", consumerEvent);
                _httpclient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                var serialized = new StringContent(JsonConvert.SerializeObject(consumerEvent.CodatBody),
                                                    Encoding.UTF8, "application/json");

                using HttpResponseMessage response = await _httpclient.
                                                     PostAsync(GenerateRequestUrl(consumerEvent),
                                                     serialized);

                // TODO: Handle the failure scenarios. We need to have a retry mechanism or push everything in a different topic?
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                { 
                    _logger.LogError("Failed to process the request for the following event ", consumerEvent);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured while processing Codat Consumer request {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Generate Request URL for navigating the codat request
        /// </summary>
        /// <param name="consumerEvent"></param>
        /// <returns></returns>
        private string GenerateRequestUrl(CodatEvent consumerEvent)
        {
            var builder = new UriBuilder(_configuration["Codat:CodatRouteUrl"]);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["account_name"] = consumerEvent.CodatRequestParameters.account_name;
            query["subscriber_id"] = consumerEvent.CodatRequestParameters.subscriber_id.ToString();
            query["event_type"] = consumerEvent.CodatRequestParameters.event_type;
            query["provider_id"] = consumerEvent.CodatRequestParameters.provider_id;
            builder.Query = query.ToString();
            return builder.ToString();
        }
    }
}
