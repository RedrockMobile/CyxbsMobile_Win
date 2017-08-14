using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Models
{
    //最难科目
    public class CourseRootobject
    {
        public int Status { get; set; }
        public string Info { get; set; }
        public string Version { get; set; }
        public CourseDatum[] Data { get; set; }
    }

    public class CourseDatum
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
