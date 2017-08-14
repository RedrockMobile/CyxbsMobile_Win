using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{
    internal class Mark
    {
        public string stunum { get; set; }
        public string nickname { get; set; }
        public string username { get; set; }
        public string photo_src { get; set; }
        public string photo_thumbnail_src { get; set; }
        public string created_time { get; set; }
        public string content { get; set; }

        public void GetListAttribute(JObject jObject)
        {
            stunum = jObject["stunum"].ToString();
            nickname = jObject["nickname"].ToString();
            username = jObject["username"].ToString();
            photo_src = jObject["photo_src"].ToString();
            photo_thumbnail_src = jObject["photo_thumbnail_src"].ToString();
            created_time = jObject["created_time"].ToString();
            content = jObject["content"].ToString();
        }
    }
}