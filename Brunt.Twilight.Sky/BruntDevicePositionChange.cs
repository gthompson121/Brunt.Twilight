using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.Sky
{
    public class BruntDevicePositionChange
    {
        [JsonIgnore]
        public string DeviceName { get; set; }
        public string requestPosition { get; set; }
    }
}
