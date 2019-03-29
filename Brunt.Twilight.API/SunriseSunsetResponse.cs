using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.API
{
    [DataContract]
    public class SunriseSunsetResponse
    {
        [DataMember]
        public SunriseSunset results { get; set; }
        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public StatusCode status { get; set; }
    }
}
