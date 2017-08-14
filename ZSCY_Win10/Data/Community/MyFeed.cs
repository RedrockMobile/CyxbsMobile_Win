using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using Windows.Storage;
using ZSCY_Win10.Common;

namespace ZSCY_Win10.Data.Community
{
    public class MyFeed : ViewModelBase
    {
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;

        private string remarknum { get; set; }
        private string nick_name;
        private string head_img = "ms-appx:///Community_nohead.png";
        public string id { get; set; }
        public Img[] photo_src { get; set; }
        public Img[] thumbnail_src { get; set; }
        public string content { get; set; }
        public string type_id { get; set; }
        public string created_time { get; set; }
        public string updated_time { get; set; }
        public string like_num { get; set; }

        public string remark_num
        {
            get
            {
                return remarknum;
            }
            set
            {
                this.remarknum = value;
                OnPropertyChanged(nameof(remark_num));
            }
        }

        public string nickname
        {
            get
            {
                return nick_name;
            }
            set
            {
                this.nick_name = value;
                OnPropertyChanged();
            }
        }

        public string headimg
        {
            get
            {
                return head_img;
            }
            set
            {
                this.head_img = value;
                OnPropertyChanged();
            }
        }

        public void GetAttributes(JObject feedsJObject, bool myfeed = false)
        {
            id = feedsJObject["id"].ToString();
            content = feedsJObject["content"].ToString();
            type_id = feedsJObject["type_id"].ToString();
            created_time = feedsJObject["created_time"].ToString();
            updated_time = feedsJObject["updated_time"].ToString();
            like_num = feedsJObject["like_num"].ToString();
            remark_num = feedsJObject["remark_num"].ToString();
            string articlephotos = feedsJObject["photo_src"].ToString();
            string smallphotos = feedsJObject["thumbnail_src"].ToString();
            if (myfeed)
            {
                nickname = feedsJObject["nickname"].ToString();
                headimg = feedsJObject["user_photo"].ToString();
            }
            else
            {
                nickname = appSetting.Values["Community_nickname"].ToString();
                headimg = appSetting.Values["Community_headimg_src"].ToString();
            }

            if (articlephotos != "")
            {
                try
                {
                    string[] i = articlephotos.Split(new char[] { ',' }, 9);
                    if (articlephotos.EndsWith(","))
                    {
                        photo_src = new Img[i.Length - 1];
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
                            if (!i[j].StartsWith(App.picstart))
                            {
                                photo_src[j] = new Img();
                                photo_src[j].ImgSrc = App.picstart + i[j];
                                photo_src[j].ImgSmallSrc = App.picstartsmall + i[j];
                            }
                            else
                            {
                                photo_src[j] = new Img();
                                photo_src[j].ImgSrc = i[j];
                                photo_src[j].ImgSmallSrc = i[j];
                            }
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