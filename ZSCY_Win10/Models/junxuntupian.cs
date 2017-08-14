using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZSCY_Win10.Models
{
    [DataContract]
    internal class junxuntupian
    {
        [DataMember]
        public List<string> title { get; set; }

        [DataMember]
        public List<string> url { get; set; }
    }

    [DataContract]
    internal class root
    {
        [DataMember]
        public junxuntupian Data { get; set; }

        [DataMember]
        public int Status { get; set; }

        [DataMember]
        public string Info { get; set; }

        [DataMember]
        public string Version { get; set; }
    }
}