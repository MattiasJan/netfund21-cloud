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
        private static readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("RegistryManager"));


        [FunctionName("GetDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices")] HttpRequest req,
            ILogger log)
        {
            var devices = new List<DeviceModel>();
            var result = registryManager.CreateQuery("SELECT * From devices");

            if (result.HasMoreResults)
            {
                foreach (var twin in await result.GetNextAsTwinAsync())
                {
                    var device = new DeviceModel
                    {
                        Id = twin.DeviceId,
                        ConnectionState = twin.ConnectionState.ToString(),
                        LastUpdated = twin.LastActivityTime,
                    };

                    try { device.Placement = twin.Properties.Desired["placement"]; }
                    catch { }

                    try { device.SensorType = twin.Properties.Reported["sensorType"]; }
                    catch { }

                    try { device.AlertNotification = twin.Properties.Reported["alertNotification"]; }
                    catch { }

                    devices.Add(device);
                }
            }

            return new OkObjectResult(devices);
        }
    }
}
