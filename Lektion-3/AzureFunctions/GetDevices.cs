using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using System.Collections.Generic;
using AzureFunctions.Models;

namespace AzureFunctions
{
    public static class GetDevices
    {
        private static readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("ServiceSDK"));

        [FunctionName("GetDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices")] HttpRequest req,
            ILogger log)
        {
            var devices = new List<DeviceModel>();
            
            var result = registryManager.CreateQuery("SELECT * FROM devices");
            if (result.HasMoreResults)
            {
                foreach(var twin in await result.GetNextAsTwinAsync())
                {
                    var device = new DeviceModel
                    {
                        DeviceId = twin.DeviceId,
                        ConnectionState = twin.ConnectionState.ToString(),
                        LastActivityTime = twin.LastActivityTime
                    };

                    try { device.Placement = twin.Properties.Desired["placement"]; }
                    catch { device.Placement = ""; }

                    try { device.SensorType = twin.Properties.Reported["sensorType"]; }
                    catch { device.SensorType = ""; }

                    try { device.TemperatureAlert = twin.Properties.Reported["temperatureAlert"]; }
                    catch { device.TemperatureAlert = false; }

                    try { device.Temperature = twin.Properties.Reported["temperature"]; }
                    catch { device.Temperature = 0; }

                    try { device.Humidity = twin.Properties.Reported["humidity"]; }
                    catch { device.Humidity = 0; }

                    devices.Add(device);
                }
            }

            return new OkObjectResult(devices);
        }
    }
}
