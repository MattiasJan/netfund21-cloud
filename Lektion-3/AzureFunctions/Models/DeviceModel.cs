using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    internal class DeviceModel
    {
        public string DeviceId { get; set; }
        public string SensorType { get; set; }
        public string Placement { get; set; }
        public string ConnectionState { get; set; }
        public DateTime? LastActivityTime { get; set; }
        public bool TemperatureAlert { get; set; }
        public int Temperature { get; set; }
        public int Humidity { get; set; }
    }
}
