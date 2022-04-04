using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions
{
    public class IotDevice
    {
        private readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotDevice"));
        private Random rnd = new Random();


        [FunctionName("IotDevice")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            await SendMessageAsync();
            log.LogInformation($"Message sent to Azure IOT Hub at: {DateTime.Now}");
        }


        private async Task SendMessageAsync()
        {
            var temperature = rnd.Next(20, 30);
            var humidity = rnd.Next(30, 40);
            var temperatureAlert = false;

            if (temperature >= 25)
                temperatureAlert = true;

            var data = new { temperature = temperature, humiditiy = humidity };
            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));
            await deviceClient.SendEventAsync(message);

            TwinCollection reportedProperties = new TwinCollection();
            reportedProperties["sensorType"] = "Azure Function Device";
            reportedProperties["temperature"] = temperature;
            reportedProperties["humidity"] = humidity;
            reportedProperties["temperatureAlert"] = temperatureAlert;
            await deviceClient.UpdateReportedPropertiesAsync(reportedProperties);




        }
    }
}
