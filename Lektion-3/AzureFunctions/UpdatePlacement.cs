using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Models;
using Microsoft.Azure.Devices;

namespace AzureFunctions
{
    public static class UpdatePlacement
    {
        private static readonly RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("ServiceSDK"));

        [FunctionName("UpdatePlacement")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var data = JsonConvert.DeserializeObject<UpdatePlacementModel>(await new StreamReader(req.Body).ReadToEndAsync());
            if (!string.IsNullOrEmpty(data.Placement) && !string.IsNullOrEmpty(data.DeviceId))
            {
                var twin = await registryManager.GetTwinAsync(data.DeviceId);
                if(twin != null)
                {
                    twin.Properties.Desired["placement"] = data.Placement;
                    await registryManager.UpdateTwinAsync(twin.DeviceId, twin, twin.ETag);
                    return new OkResult();
                }
            }

            return new BadRequestResult();
        }
    }
}
