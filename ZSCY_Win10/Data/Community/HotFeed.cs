using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.Community
{
    class HotFeed
    {
        public class Rootobject
        {
            public string id { get; set; }
            public string type { get; set; }
            public string type_id { get; set; }
            public string article_id { get; set; }
            public string user_id { get; set; }
            public string nick_name { get; set; }
            public string user_head { get; set; }
            public string time { get; set; }
            public string content { get; set; }
            public Img img { get; set; }
            public string like_num { get; set; }
            public string remark_num { get; set; }
            public bool is_my_Like { get; set; }
        }

        public class Img
        {
            public string img_small_src { get; set; }
            public string img_src { get; set; }
        }

    }
}
