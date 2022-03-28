using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using AzureFunctions.Models;

namespace AzureFunctions
{
    public static class GetDevices
    {
        [FunctionName("GetDevices")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var list = new List<DeviceItem>()
            {
                new DeviceItem
                {
                    DeviceId = Guid.NewGuid().ToString(),
                    Status = "connected",
                    Placement = "Conference Room",
                    SensorType = "Temperature Sensor",
                    LastUpdated = DateTime.Now.ToString(),
                    Temperature = "22",
                    Humidity = "44",
                    Alert = "false"
                },
                new DeviceItem
                {
                    DeviceId = Guid.NewGuid().ToString(),
                    Status = "connected",
                    Placement = "Conference Room",
                    SensorType = "Temperature Sensor",
                    LastUpdated = DateTime.Now.ToString(),
                    Temperature = "22",
                    Humidity = "44",
                    Alert = "false"
                }
            };

            return new OkObjectResult(list);
        }
    }
}
