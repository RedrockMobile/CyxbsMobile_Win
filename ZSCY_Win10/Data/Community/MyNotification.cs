using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace ZSCY_Win10.Data.Community
{
    public class MyNotification : IFeeds
    {
        public string type { get; set; }
        public string content { get; set; }//回复内容
        public string article_content { get; set; }//原文
        public Img[] article_photo_src { get; set; }
        public string created_time { get; set; }
        public string article_id { get; set; }
        public string stunum { get; set; }
        public string nickname { get; set; }
        public string photo_src { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            type = feedsJObject["type"].ToString();
            content = feedsJObject["content"].ToString();
            article_content = feedsJObject["article_content"].ToString();
            created_time = feedsJObject["created_time"].ToString();
            article_id = feedsJObject["article_id"].ToString();
            stunum = feedsJObject["stunum"].ToString();
            nickname = feedsJObject["nickname"].ToString();
            photo_src = feedsJObject["photo_src"].ToString() == "" ? "ms-appx:///Assets/Boy-100.png" : feedsJObject["photo_src"].ToString();
            string articlephotos = feedsJObject["article_photo_src"].ToString();
            //string picstart = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/";
            if (articlephotos != "")
            {
                try
                {
                    string[] i = articlephotos.Split(new char[] { ',' }, 9);
                    if (articlephotos.EndsWith(","))
                        article_photo_src = new Img[i.Length - 1];
                    else
                        article_photo_src = new Img[i.Length];
                    for (int j = 0; j < article_photo_src.Length; j++)
                    {
                        if (i[j] != "")
                        {
                            if (!i[j].StartsWith(App.picstart))
                            {
                                article_photo_src[j] = new Img();
                                article_photo_src[j].ImgSrc = App.picstart + i[j];
                                article_photo_src[j].ImgSmallSrc = App.picstartsmall + i[j];
                            }
                            else
                            {
                                article_photo_src[j] = new Img();
                                article_photo_src[j].ImgSrc = i[j];
                                article_photo_src[j].ImgSmallSrc = i[j];
                            }
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
        }
    }
}