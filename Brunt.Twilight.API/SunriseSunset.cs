using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.API
{
    [DataContract]
    public class SunriseSunset
    {
        [DataMember]
        public DateTime sunrise { get; set; }
        [DataMember]
        public DateTime sunset { get; set; }
        [DataMember]
        public DateTime solar_noon { get; set; }
        [DataMember]
        public string day_length { get; set; }
        [DataMember]
        public DateTime civil_twilight_begin { get; set; }
        [DataMember]
        public DateTime civil_twilight_end { get; set; }
        [DataMember]
        public DateTime nautical_twilight_begin { get; set; }
        [DataMember]
        public DateTime nautical_twilight_end { get; set; }
        [DataMember]
        public DateTime astronomical_twilight_begin { get; set; }
        [DataMember]
        public DateTime astronomical_twilight_end { get; set; }
    }
}
