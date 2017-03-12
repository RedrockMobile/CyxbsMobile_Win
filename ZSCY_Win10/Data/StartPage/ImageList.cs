using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.StartPage
{
    [DataContract]
    public class ImageList
    {

        [DataMember(Name = "status")]

        public int Status { get; set; }
        [DataMember(Name = "info")]

        public string Info { get; set; }
        [DataMember(Name = "data")]
        public List<DataModel> Data { get; set; }
        [DataContract]
        public class DataModel
        {
            [DataMember(Name = "target_url")]
            public string TargetUrl { get; set; }
            [DataMember(Name = "photo_src")]
            public string ImageUrl { get; set; }
            [DataMember(Name = "start")]
            public string StartTime { get; set; }
            [DataMember(Name = "id")]
            public string Id { get; set; }
            [DataMember(Name = "annotation")]
            public string Name { get; set; }
            [DataMember(Name = "column")]
            public string Column { get; set; }
        }

    }
}
