using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ZSCY_Win10.Data.Community
{


    public class JWZXFeeds : HotFeedsContentBase, IFeeds
    {
        public string id { get; set; }
        public string articleid { get; set; }
        public string head { get; set; }
        public string date { get; set; }
        public string read { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public void GetAttributes(JObject feedsJObject)
        {
            id = feedsJObject["id"].ToString();
            articleid = feedsJObject["articleid"].ToString();
            head = feedsJObject["head"].ToString();
            date = feedsJObject["date"].ToString();
            read = feedsJObject["read"].ToString();
            title = feedsJObject["title"].ToString();
            content = feedsJObject["content"].ToString();
        }
    }

}
