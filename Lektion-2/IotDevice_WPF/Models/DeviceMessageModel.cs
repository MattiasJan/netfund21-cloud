using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IotDevice_WPF.Models
{
    internal class DeviceMessageModel
    {

        [JsonPropertyName("temperature")]
        public decimal Temperature { get; set; }

        [JsonPropertyName("humidity")]
        public decimal Humidity { get; set; }
    }
}
