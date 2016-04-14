using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Data.Community
{
    public class Feeds
    {
        public string Id { get; set; }
        public string User_id { get; set; }
        public string user_name { get; set; }
        public string user_head { get; set; }
        public string time { get; set; }
        public string content { get; set; }
        public Img[] img { get; set; }
        public int like_num { get; set; }
        public string is_my_like { get; set; }
        public int comment_num { get; set; }
    }
}
