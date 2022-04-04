using IoTHubTrigger = Microsoft.Azure.WebJobs.EventHubTriggerAttribute;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Models;
using Azure.Messaging.EventHubs;

namespace AzureFunctions
{
    public class SaveData
    {
        private static HttpClient client = new HttpClient();
        
        [FunctionName("SaveData")]
        public void Run(
            [IoTHubTrigger("messages/events", Connection = "IotHubEndpoint", ConsumerGroup = "azurefunctions")]EventData message,
            [CosmosDB(databaseName: "NETFUND21", collectionName: "IotMessages", CreateIfNotExists = true, ConnectionStringSetting = "CosmosDB")] out dynamic cosmos,       
            ILogger log)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<DeviceMessageModel>(Encoding.UTF8.GetString(message.EventBody));
                data.DeviceId = message.SystemProperties["iothub-connection-device-id"].ToString();
                cosmos = data;
            }
            catch
            {
                cosmos = null;
            }
        }
    }
}