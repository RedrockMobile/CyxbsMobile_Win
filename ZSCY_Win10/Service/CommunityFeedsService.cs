using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Newtonsoft.Json.Linq;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Util;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace ZSCY_Win10.Service
{
    public class CommunityFeedsService
    {

        const string hotFeeds = @"cyxbsMobile/index.php/Home/Article/searchHotArticle";
        const string bbddfeeds = @"cyxbsMobile/index.php/Home/Article/listArticle";
        public static string[] feedsapi = { hotFeeds, bbddfeeds };
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;

        /// <summary>
        /// 获取动态列表
        /// </summary>
        /// <param name="type">动态参数，重邮新闻cyxw=>1,教务咨询jwzx=>2,xsjz=>3,xwgg=>4,bbdd=>5</param>
        /// <returns>返回参数对应的列表数据</returns>
        public static async Task<List<BBDDFeed>> GetBBDD(int type = 1, int page = 1, int size = 1, int typeid = 5)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
            if (typeid != 0)
                paramList.Add(new KeyValuePair<string, string>("type_id", typeid.ToString()));
            string response = await NetWork.getHttpWebRequest(feedsapi[type], paramList);
            response = Utils.ConvertUnicodeStringToChinese(response);
            JObject bbddfeeds = JObject.Parse(response);
            if (bbddfeeds["status"].ToString() == "200")
            {
                List<BBDDFeed> feeds = new List<BBDDFeed>();
                JArray bbddarray = JArray.Parse(bbddfeeds["data"].ToString());
                for (int i = 0; i < bbddarray.Count; i++)
                {
                    BBDDFeed f = new BBDDFeed();
                    f.GetAttributes((JObject)bbddarray[i]);
                    feeds.Add(f);
                }
                return feeds;
            }
            /*    try
                {
                    JArray jsonstr = JArray.Parse(response);

                    List<BBDDFeed> feedslist = new List<BBDDFeed>();

                    for (int j = 0; j < jsonstr.Count; j++)
                    {
                        JObject jsondetail = (JObject)(jsonstr[j]);
                        if (jsondetail["status"].ToString() == "200")
                        {
                            JObject jo = (JObject)jsondetail["data"];
                            BBDDFeed f = new BBDDFeed();
                            f.GetAttributes(jo);
                            feedslist.Add(f);
                        }
                    }
                    return feedslist;

                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    List<BBDDFeed> feedslist = new List<BBDDFeed>();
                    JObject jsonobj = JObject.Parse(response);
                    if (jsonobj["status"].ToString() == "200")
                    {
                        string jsonstr = jsonobj["data"].ToString();
                        JArray feedsarray = Utils.ReadJso(response);
                        for (int i = 0; i < feedsarray.Count; i++)
                        {
                            JObject f = (JObject)feedsarray[i];
                            BBDDFeed fd = new BBDDFeed();
                            fd.GetAttributes(f);
                            feedslist.Add(fd);
                        }
                        return feedslist;
                    }
                }
                catch (InvalidCastException e)
                {
                    Debug.WriteLine(e.Message);
                }
                */
            return null;
        }

        public static async Task<List<HotFeed>> GetHot(int type = 0, int page = 1, int size = 1, int typeid = 5)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", "2013211429"));
            paramList.Add(new KeyValuePair<string, string>("idNum", "252617"));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
            if (typeid != 0)
                paramList.Add(new KeyValuePair<string, string>("type_id", typeid.ToString()));
            string response = await NetWork.getHttpWebRequest(feedsapi[type], paramList);
            response = Utils.ConvertUnicodeStringToChinese(response);
            List<HotFeed> feeds = new List<HotFeed>();
            JArray hotfeed = JArray.Parse(response);
            for (int i = 0; i < hotfeed.Count; i++)
            {
                JObject hot = (JObject) hotfeed[i];
                if (hot["status"].ToString() == "200")
                {
                    JObject data = (JObject) hot["data"];
                    HotFeed f = new HotFeed();
                    f.GetAttributes(data);
                    feeds.Add(f);
                }
            }
            return feeds;
          }
    }
}
