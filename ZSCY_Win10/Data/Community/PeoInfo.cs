using Newtonsoft.Json.Linq;
using Windows.Storage;

namespace ZSCY_Win10.Data.Community
{
    public class PeoInfo : IFeeds
    {
        private ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;

        public PeoInfo()
        {
        }

        public PeoInfo(string nickname, string introduction, string phone, string qq)
        {
            this.nickname = nickname;
            this.introduction = introduction;
            this.phone = phone;
            this.qq = qq;
        }

        public PeoInfo(string nickname, string introduction, string gender, string phone, string qq)
        {
            this.nickname = nickname;
            this.introduction = introduction;
            this.gender = gender;
            this.phone = phone;
            this.qq = qq;
        }

        public string id { get; set; }
        public string stunum { get; set; }
        public string introduction { get; set; }
        public string username { get; set; }
        public string nickname { get; set; }
        public string gender { get; set; }
        public string photo_thumbnail_src { get; set; }
        public string photo_src { get; set; }
        public string updated_time { get; set; }
        public string phone { get; set; }
        public string qq { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            id = feedsJObject["id"].ToString();
            stunum = feedsJObject["stunum"].ToString();
            introduction = feedsJObject["introduction"].ToString();
            username = feedsJObject["username"].ToString();
            nickname = feedsJObject["nickname"].ToString();
            gender = feedsJObject["gender"].ToString();
            photo_thumbnail_src = feedsJObject["photo_thumbnail_src"].ToString() == "" ? "ms-appx:///Assets/Community_nohead.png" : feedsJObject["photo_thumbnail_src"].ToString();
            photo_src = feedsJObject["photo_src"].ToString() == "" ? "ms-appx:///Assets/Community_nohead.png" : feedsJObject["photo_src"].ToString();
            updated_time = feedsJObject["updated_time"].ToString();
            phone = feedsJObject["phone"].ToString();
            qq = feedsJObject["qq"].ToString();
        }
    }
}