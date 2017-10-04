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
            public int status { get; set; }
            public string info { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public string uid { get; set; }
            public string hours { get; set; }
            public Record[] record { get; set; }
        }

        public class Record
        {
            public string title { get; set; }
            public string content { get; set; }
            public string address { get; set; }
            public string start_time { get; set; }
            public string hours { get; set; }
            public int score { get; set; }
        }

    }


    

}
