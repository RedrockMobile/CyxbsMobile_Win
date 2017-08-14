using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models
{
    [DataContract]
    class junxuntupian
    {
        [DataMember]
        public List<string> title { get; set; }
        [DataMember]
        public List<string> url { get; set; }
    }
    [DataContract]
    class root
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
