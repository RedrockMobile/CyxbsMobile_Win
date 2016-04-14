using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
        public Img[] Imgs { get; set; }
        public int LikeNum { get; set; }
        public string IsMyLike { get; set; }
        public int CommentNum { get; set; }

        public Feeds GetAttributes(JObject feedsJObject)
        {
            Id = feedsJObject["id"].ToString();
            UserId = feedsJObject["user_id"].ToString();
            UserName = feedsJObject["user_name"].ToString();
            UserHead = feedsJObject["user_head"].ToString();
            Time = feedsJObject["time"].ToString();
            Content = feedsJObject["content"].ToString();
            LikeNum = int.Parse(feedsJObject["like_num"].ToString());
            IsMyLike = feedsJObject["is_my_like"].ToString();
            CommentNum =int.Parse(feedsJObject["comment_num"].ToString());
            JArray imgs = (JArray) feedsJObject["img"];
            Imgs = new Img[imgs.Count];
            for (int i = 0; i < imgs.Count; i++)
            {
                Imgs[i] = new Img().GetAttributes((JObject)imgs[i]);
            }
            return this;
        } 
    }
}
