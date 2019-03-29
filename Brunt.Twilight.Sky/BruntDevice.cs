using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.Sky
{
    [DataContract]
    public class BruntDevice
    {
        [DataMember]
        public string TIMESTAMP { get; set; }
        [DataMember]
        public string NAME { get; set; }
        [DataMember]
        public string SERIAL { get; set; }
        [DataMember]
        public string MODEL { get; set; }
        [DataMember]
        public string requestPosition { get; set; }
        [DataMember]
        public string currentPosition { get; set; }
        [DataMember]
        public string moveState { get; set; }
        [DataMember]
        public string setLoad { get; set; }
        [DataMember]
        public string currentLoad { get; set; }
        [DataMember]
        public string FW_VERSION { get; set; }
        [DataMember]
        public string overStatus { get; set; }
        [DataMember]
        public string ICON { get; set; }
        [DataMember]
        public string thingUri { get; set; }
        [DataMember]
        public string PERMISSION_TYPE { get; set; }
        [DataMember]
        public int delay { get; set; }
    }
}
