using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    public class BBDDFeed : IFeeds
    {
        public string title { get; set; }
        public string id { get; set; }
        public string num_id { get; set; }
        public Img[] article_photo_src { get; set; }//动态配图
        public string article_thumbnail_src { get; set; }
        public string type_id { get; set; }
        public string content { get; set; }
        public string updated_time { get; set; }
        public string created_time { get; set; }
        public string like_num { get; set; }
        public string remark_num { get; set; }
        public string nickname { get; set; }
        public string photo_src { get; set; }//头像
        public string photo_thumbnail_src { get; set; }
        public string is_my_like { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            title = feedsJObject["title"].ToString();
            id = feedsJObject["id"].ToString();
            num_id ="5"+ feedsJObject["type_id"].ToString() + feedsJObject["id"].ToString();
            type_id = feedsJObject["type_id"].ToString();
            content = feedsJObject["content"].ToString();
            updated_time = feedsJObject["updated_time"].ToString();
            created_time = feedsJObject["created_time"].ToString();
            like_num = feedsJObject["like_num"].ToString();
            remark_num = feedsJObject["remark_num"].ToString();
            nickname = feedsJObject["nickname"].ToString();
            photo_src = feedsJObject["photo_src"].ToString() == "" ? "ms-appx:///Assets/Boy-100.png" : feedsJObject["photo_src"].ToString();
            photo_thumbnail_src = feedsJObject["photo_thumbnail_src"].ToString();
            is_my_like = feedsJObject["is_my_like"].ToString();
            string articlephotos = feedsJObject["article_photo_src"].ToString();
            string picstart = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/";
            if (articlephotos != "")
            {
                try
                {
                    string[] i = articlephotos.Split(new char[] { ',' }, 9);
                    if (i.Length > 1)
                        article_photo_src = new Img[i.Length - 1];
                    else
                        article_photo_src = new Img[i.Length];
                    for (int j = 0; j < article_photo_src.Length; j++)
                    {
                        if (i[j] != "")
                        {
                            if (!i[j].StartsWith(picstart))
                                i[j] = picstart + i[j];
                            article_photo_src[j] = new Img();
                            article_photo_src[j].ImgSrc = i[j];
                            article_photo_src[j].ImgSmallSrc = i[j];
                        }
                        else
                        {
                            article_photo_src[j] = new Img();
                            article_photo_src[j].ImgSrc = article_photo_src[j].ImgSmallSrc = "";

                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                }
            }
            //if (UserHead == "")
            //{
            //    UserHead = "ms-appx:///Assets/Boy-100.png";
            //    Debug.WriteLine("---没有头像---");
            //}
            //Time = feedsJObject["time"].ToString();
            //Content = feedsJObject["content"].ToString();
            //LikeNum = int.Parse(feedsJObject["like_num"].ToString());
            //IsMyLike = feedsJObject["is_my_Like"].ToString();
            //CommentNum = int.Parse(feedsJObject["remark_num"].ToString());
            //try
            //{
            //    JArray imgs = (JArray)feedsJObject["img"];
            //    Imgs = new Img[imgs.Count];
            //    for (int i = 0; i < imgs.Count; i++)
            //    {
            //        Imgs[i] = new Img();
            //        Imgs[i].GetAttributes((JObject)imgs[i]);
            //    }
            //}
            //catch (InvalidCastException)
            //{
            //    JObject img = (JObject)feedsJObject["img"];
            //    Imgs = new Img[1];
            //    Imgs[0] = new Img();
            //    Imgs[0].GetAttributes(img);
            //}
        }
    }
}
