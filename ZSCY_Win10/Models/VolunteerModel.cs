using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models
{
    class VolunteerModel
    {

        public class Rootobject
        {
            public int code { get; set; }
            public float hours { get; set; }
            public Record[] record { get; set; }
        }

        public class Record
        {
            public string title { get; set; }
            public string content { get; set; }
            public string addWay { get; set; }
            public string start_time { get; set; }
            public string hours { get; set; }
            public string server_group { get; set; }
            public string orgId { get; set; }
        }

    }


    

}
