using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public void GetAttributes(JObject feedsJObject)
        {
            Id = feedsJObject["id"].ToString();
            UserId = feedsJObject["user_id"].ToString();
            UserName = feedsJObject["nick_name"].ToString();
            UserHead = feedsJObject["user_head"].ToString();
            if (UserHead == "")
            {
                UserHead = "ms-appx:///Assets/Boy-100.png";
                Debug.WriteLine("---没有头像---");
            }
            Time = feedsJObject["time"].ToString();
            Content = feedsJObject["content"].ToString();
            LikeNum = int.Parse(feedsJObject["like_num"].ToString());
            IsMyLike = feedsJObject["is_my_Like"].ToString();
            CommentNum = int.Parse(feedsJObject["remark_num"].ToString());
            try
            {
                JArray imgs = (JArray)feedsJObject["img"];
                Imgs = new Img[imgs.Count];
                for (int i = 0; i < imgs.Count; i++)
                {
                    Imgs[i] = new Img();
                    Imgs[i].GetAttributes((JObject)imgs[i]);
                }
            }
            catch (InvalidCastException)
            {
                JObject img = (JObject)feedsJObject["img"];
                Imgs = new Img[1];
                Imgs[0] = new Img();
                Imgs[0].GetAttributes(img);
            }
            return;

        }
    }
}
