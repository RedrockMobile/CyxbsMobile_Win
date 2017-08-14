using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Models
{
    public class FailRatio
    {


        public class Rootobject
        {
            public int Status { get; set; }
            public string Info { get; set; }
            public string Version { get; set; }
            public Datum[] Data { get; set; }
        }

        public class Datum
        {
            public string college { get; set; }
            public Major[] major { get; set; }
        }

        public class Major
        {
            public string major { get; set; }
            public Course[] course { get; set; }
        }

        public class Course
        {
            public string course { get; set; }
            public string ratio { get; set; }
        }


    }
}
