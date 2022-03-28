using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    internal class DeviceItem
    {
        public string DeviceId { get; set; }
        public string Status { get; set; }
        public string Placement { get; set; }
        public string SensorType { get; set; }
        public string LastUpdated { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Alert { get; set; }
    }
}
