using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using Windows.Storage;

namespace ZSCY_Win10.Data.Community
{
    public class MyFeed:IFeeds
    {
        public string id { get; set; }
        public Img[] photo_src { get; set; }
        public Img[] thumbnail_src { get; set; }
        public string content { get; set; }
        public string type_id { get; set; }
        public string created_time { get; set; }
        public string updated_time { get; set; }
        public string like_num { get; set; }
        public string remark_num { get; set; }
        public string nickname { get; set; } = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Conmunity_nickname"].ToString();
        public string headimg { get; set; } = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Community_headimg_src"].ToString();

        public void GetAttributes(JObject feedsJObject)
        {
            id = feedsJObject["id"].ToString();
            content = feedsJObject["content"].ToString();
            type_id = feedsJObject["type_id"].ToString();
            created_time = feedsJObject["created_time"].ToString();
            updated_time = feedsJObject["updated_time"].ToString();
            like_num = feedsJObject["like_num"].ToString();
            remark_num = feedsJObject["remark_num"].ToString();
            string articlephotos = feedsJObject["article_photo_src"].ToString();
            string smallphotos = feedsJObject["thumbnail_src"].ToString();
            string picstart = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/";
            if (articlephotos != "")
            {
                try
                {
                    string[] i = articlephotos.Split(new char[] { ',' }, 9);
                    string[] k = articlephotos.Split(new char[] { ',' }, 9);
                    if (articlephotos.EndsWith(","))
                    {
                        photo_src = new Img[i.Length - 1];
                        thumbnail_src = new Img[i.Length - 1];
                    }
                    else
                    {
                        photo_src = new Img[i.Length];
                        thumbnail_src = new Img[i.Length];
                    }
                    for (int j = 0; j < photo_src.Length; j++)
                    {
                        if (i[j] != "")
                        {
                            if (!i[j].StartsWith(picstart))
                            {
                                i[j] = picstart + i[j];

                            }
                            photo_src[j] = new Img();
                            photo_src[j].ImgSrc = i[j];
                            thumbnail_src[j] = new Img();
                            thumbnail_src[j].ImgSrc = k[j];

                            //photo_src[j].ImgSmallSrc = i[j];
                        }
                        else
                        {
                            photo_src[j] = new Img();
                            photo_src[j].ImgSrc = photo_src[j].ImgSmallSrc = "";

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
