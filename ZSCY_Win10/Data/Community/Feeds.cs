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
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserHead { get; set; }
        public string Time { get; set; }
        public string Content { get; set; }
        public Img[] Img { get; set; }
        public int LikeNum { get; set; }
        public string IsMyLike { get; set; }
        public int CommentNum { get; set; }
    }
}
