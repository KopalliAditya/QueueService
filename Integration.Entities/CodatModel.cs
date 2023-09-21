using Newtonsoft.Json;
using System;

namespace Integration.Models
{
    public class CodatQueryParameters
    {
        /// <summary>
        ///  It indicates the account name in Zenoti.
        /// </summary>
        public string account_name { get; set; }

        /// <summary>
        /// It indicates the id of subscriber in Zenoti. It can be org_id / center_id.
        /// </summary>
        public Guid subscriber_id { get; set; }

        /// <summary>
        /// It indicates the type of event.
        /// </summary>
        public string event_type { get; set; }

        /// <summary>
        /// provider_id is a string that is used to identify the provider of the given integration type.
        /// </summary>
        public string provider_id { get; set; }
    }

    public class CodatBody
    {
        /// <summary>
        /// It indicates the id of company in Codat.
        /// </summary>
        [JsonProperty("CompanyId")]
        public Guid CompanyId { get; set; }

        /// <summary>
        /// It indicates the id of rule in Codat.
        /// </summary>
        [JsonProperty("RuleId")]
        public Guid RuleId { get; set; }

        /// <summary>
        /// It indicates the type of rule configured in the Codat Portal to alert you to events affecting your companies and data.
        /// </summary>
        [JsonProperty("RuleType")]
        public string RuleType { get; set; }

        /// <summary>
        /// It indicates the id of the alert.
        /// </summary>
        [JsonProperty("AlertId")]
        public Guid AlertId { get; set; }

        /// <summary>
        /// It indicates the message for the alert from Codat.
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }

        /// <summary>
        /// The properties in data vary depending upon event type.
        /// </summary>
        [JsonProperty("Data")]
        public CodatData Data { get; set; }
    }

    public class CodatData
    {
        /// <summary>
        /// It represents the dataconnection id in Codat. A data connection represents a company's connection to a data source and allows you to exchange data (pull and push) with that source.
        /// </summary>
        [JsonProperty("dataConnectionId")]
        public Guid DataConnectionId { get; set; }

        /// <summary>
        /// It represents the id of the dataset in Codat.
        /// </summary>
        [JsonProperty("datasetId")]
        public Guid DataSetId { get; set; }

        /// <summary>
        /// It represents the type of data.
        /// </summary>
        [JsonProperty("dataType")]
        public string DataType { get; set; }

        /// <summary>
        /// It represents the key of platform in Codat.
        /// </summary>
        [JsonProperty("platformKey")]
        public string PlatformKey { get; set; }

        /// <summary>
        /// It represents the current status.
        /// </summary>
        [JsonProperty("newStatus")]
        public string NewStatus { get; set; }

        /// <summary>
        /// It represents the previous status.
        /// </summary>
        [JsonProperty("oldStatus")]
        public string OldStatus { get; set; }

        /// <summary>
        /// It represents the status of the dataset.
        /// </summary>
        [JsonProperty("datasetStatus")]
        public string DatasetStatus { get; set; }

        /// <summary>
        /// It represents the push operation key. 
        /// </summary>
        [JsonProperty("pushOperationKey")]
        public Guid PushOperationKey { get; set; }

        /// <summary>
        /// It represents the status.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// It represents the push operation guid.
        /// </summary>
        [JsonProperty("pushOperationGuid")]
        public Guid PushOperationGuid { get; set; }
    }

    public class CodatEvent
    {
        /// <summary>
        /// It represents the codat request parameters.
        /// </summary>
        [JsonProperty("codatRequest")]
        public CodatQueryParameters CodatRequestParameters { get; set; }

        /// <summary>
        /// It represents the codat request parameters.
        /// </summary>
        [JsonProperty("codatBody")]
        public CodatBody CodatBody { get; set; }
    }
}
