using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    internal class DeviceModel
    {
        public string Id { get; set; } = "";
        public string SensorType { get; set; } = "";
        public string Placement { get; set; } = "";
        public DateTime? LastUpdated { get; set; }
        public string ConnectionState { get; set; } = "";
        public bool AlertNotification { get; set; } = false;
    }
}
