using Newtonsoft.Json.Linq;
using System;

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
        public string content_short { get; set; }

        public void GetAttributes(JObject feedsJObject)
        {
            id = feedsJObject["id"].ToString();
            articleid = feedsJObject["articleid"].ToString();
            head = feedsJObject["head"].ToString();
            date = feedsJObject["date"].ToString();
            read = feedsJObject["read"] != null ? feedsJObject["read"].ToString() : "0";
            title = feedsJObject["title"].ToString();
            content = feedsJObject["content"].ToString();
            try
            {
                while (content.IndexOf("<") != -1)
                {
                    content = content.Remove(content.IndexOf("<"), content.IndexOf(">") - content.IndexOf("<") + 1);
                }
            }
            catch (Exception) { }
            //content_short = feedsJObject["content"].ToString();
            content = content.Replace("\r", "");
            content = content.Replace("\t", "");
            content = content.Replace("\n", "");
            content = content.Replace("&nbsp;", "");
            content = content.Replace(" ", "");
            content = content.Replace("（见附件）", "见附件");
            content = content.Replace("MicrosoftInternetExplorer4", "");
            content = content.Replace("Normal07.8磅02falsefalsefalse", "");
        }
    }
}