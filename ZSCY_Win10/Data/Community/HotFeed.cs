using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using ZSCY_Win10.Common;

namespace ZSCY_Win10.Data.Community
{
    public class HotFeed : ViewModelBase, IFeeds
    {
        private string is_my_like;
        private string like_nums;
        private string remarknum;
        public string id { get; set; }
        public string type { get; set; }
        public string type_id { get; set; }
        public string article_id { get; set; }
        public string num_id { get; set; }
        public string user_id { get; set; }
        public string nick_name { get; set; }
        public string user_head { get; set; }
        public string time { get; set; }
        public HotFeedsContentBase content { get; set; }
        public Img[] img { get; set; }

        public string like_num
        {
            get
            {
                return like_nums;
            }
            set
            {
                like_nums = value;
                OnPropertyChanged(nameof(like_num));
            }
        }

        public string remark_num
        {
            get
            {
                return remarknum;
            }
            set
            {
                remarknum = value;
                OnPropertyChanged(nameof(remark_num));
            }
        }

        public string is_my_Like
        {
            get
            {
                return is_my_like;
            }
            set
            {
                is_my_like = value;
                OnPropertyChanged(nameof(is_my_Like));
            }
        }

        public void GetAttributes(JObject feedsJObject)
        {
            id = feedsJObject["id"].ToString();
            type = feedsJObject["type"].ToString();
            if (type != "bbdd" && type != "notice")
            {
                nick_name = feedsJObject["user_name"].ToString();
            }
            else
            {
                nick_name = feedsJObject["nick_name"].ToString();
            }
            type_id = feedsJObject["type_id"].ToString();
            article_id = feedsJObject["article_id"].ToString();
            num_id = "1" + feedsJObject["type_id"].ToString() + feedsJObject["article_id"].ToString();
            user_id = feedsJObject["user_id"].ToString();
            if (type == "jwzx")
            {
                user_head = "ms-appx:///Assets/Community_JWZX.png";
            }
            else if (type == "cyxw")
            {
                user_head = "ms-appx:///Assets/Community_CYXW.png";
            }
            else if (type == "xsjz")
            {
                user_head = "ms-appx:///Assets/Community_XSJZ.png";
            }
            else if (type == "xwgg")
            {
                user_head = "ms-appx:///Assets/Community_XWGG.png";
            }
            else
            {
                user_head = feedsJObject["user_head"].ToString() == "" ? "ms-appx:///Assets/Community_nohead.png" : feedsJObject["user_head"].ToString();
            }
            time = feedsJObject["time"].ToString();
            like_num = feedsJObject["like_num"].ToString();
            remark_num = feedsJObject["remark_num"].ToString();
            is_my_Like = feedsJObject["is_my_Like"].ToString();
            HotFeedsContentBase h = new HotFeedsContentBase();
            h.GetAttributes((JObject)feedsJObject["content"]);
            content = h;
            JObject imgs = (JObject)feedsJObject["img"];
            string imgsurls = imgs["img_src"].ToString();
            if (imgsurls != "")
            {
                try
                {
                    string[] i = imgsurls.Split(new char[] { ',' }, 9);
                    if (imgsurls.EndsWith(","))
                        img = new Img[i.Length - 1];
                    else
                        img = new Img[i.Length];
                    for (int j = 0; j < img.Length; j++)
                    {
                        if (i[j] != "")
                        {
                            if (!i[j].StartsWith(App.picstart))
                            {
                                img[j] = new Img();
                                img[j].ImgSrc = App.picstart + i[j];
                                img[j].ImgSmallSrc = App.picstartsmall + i[j];
                            }
                            else
                            {
                                img[j] = new Img();
                                img[j].ImgSrc = i[j];
                                img[j].ImgSmallSrc = i[j];
                            }
                        }
                        else
                        {
                            img[j] = new Img();
                            img[j].ImgSrc = img[j].ImgSmallSrc = "";
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