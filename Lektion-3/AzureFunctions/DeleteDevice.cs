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

namespace AzureFunctions
{
    public static class DeleteDevice
    {
        private static readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("ServiceSDK"));

        [FunctionName("DeleteDevice")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "devices")] HttpRequest req,
            ILogger log)
        {

            string id = req.Query["id"];
            if (!string.IsNullOrEmpty(id))
            {
                var device = await registryManager.GetDeviceAsync(id);
                if(device != null)
                {
                    await registryManager.RemoveDeviceAsync(device);
                    return new OkResult();
                }
            }

            return new BadRequestResult();
        }
    }
}
