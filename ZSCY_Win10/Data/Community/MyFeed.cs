using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.Community
{
    public class MyFeed
    {
        public string id { get; set; }
        public string photo_src { get; set; }
        public string thumbnail_src { get; set; }
        public string content { get; set; }
        public string type_id { get; set; }
        public string created_time { get; set; }
        public string updated_time { get; set; }
        public string like_num { get; set; }
        public string remark_num { get; set; }
    }
}
