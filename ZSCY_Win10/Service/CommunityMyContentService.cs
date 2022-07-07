using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Util;

namespace ZSCY_Win10.Service
{
    public class CommunityMyContentService
    {
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;

        //const string api = "cyxbsMobile/index.php/Home/Article/searchContent";
        private const string api = "cyxbsMobile/index.php/Home/NewArticle/searchContent";

        private static string resourceName = "ZSCY";

        public static async Task<MyFeed> GetFeed(int type_id, string article_id)
        {
            //TODO:未登陆时 不添加参数stuNum和idNum
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id.ToString()));
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id.ToString()));
            JObject response = await Requests.Send(api);
            Debug.WriteLine(response);
            try
            {
                if (response != null)
                {
                    if (response["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)response["data"][0];
                        MyFeed f = new MyFeed();
                        f.GetAttributes(feed, true);
                        return f;
                    }
                }
            }
            catch (Exception) { }
            return null;
        }

        public static async Task<HotFeed> GetHotFeed(int type_id, string article_id)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id.ToString()));
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id.ToString()));
            JObject response = await Requests.Send(api);
            Debug.WriteLine(response);
            try
            {
                if (response != null)
                {
                    if (response["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)response["data"][0];
                        HotFeed f = new HotFeed();
                        f.GetAttributes(feed);
                        return f;
                    }
                }
            }
            catch (Exception) { }
            return null;
        }
    }
}