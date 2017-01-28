using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindModels
{
    [DataContract]
    public class AddRemindBackModel
    {

        [DataMember(Name = "status")]
        public int Status { get; set; }
        [DataMember(Name = "info")]
        public string Info { get; set; }
        [DataMember(Name = "id")]
        public long Id { get; set; }


    }
}
