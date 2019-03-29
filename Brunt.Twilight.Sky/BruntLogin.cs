using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.Sky
{
    [DataContract]
    public class BruntLogin
    {
        [DataMember]
        public long latestLogin { get; set; }
        [DataMember]
        public string primaryId { get; set; }
        [DataMember]
        public string ROLE { get; set; }
        [DataMember]
        public string nickname { get; set; }
        [DataMember]
        public long createdTime { get; set; }
        [DataMember]
        public string company { get; set; }
        [DataMember]
        public string newEmail { get; set; }
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string lang { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string bruntLoginTime { get; set; }
        [DataMember]
        public string sessionId { get; set; }
    }
}
