using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    internal class UpdatePlacementModel
    {
        public string DeviceId { get; set; }
        public string Placement { get; set; }
    }
}
