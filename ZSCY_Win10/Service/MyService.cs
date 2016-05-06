using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Util;

namespace ZSCY_Win10.Service
{
    public class MyService
    {
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;

        public static async Task<List<MyNotification>> GetNotifications(int page = 0, int size = 15)
        {
            return await Task.Run(async () =>
            {
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
                paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
                string response = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Article/aboutme", paramList);
                //response = Utils.ConvertUnicodeStringToChinese(response);
                Debug.WriteLine(response);
                List<MyNotification> feeds = new List<MyNotification>();
                try
                {
                    if (response != "" || response != "[]")
                    {
                        JObject bbddfeeds = JObject.Parse(response);
                        if (bbddfeeds["status"].ToString() == "200")
                        {
                            JArray bbddarray = JArray.Parse(bbddfeeds["data"].ToString());
                            for (int i = 0; i < bbddarray.Count; i++)
                            {
                                MyNotification f = new MyNotification();
                                f.GetAttributes((JObject)bbddarray[i]);
                                feeds.Add(f);
                            }
                        }
                    }
                }
                catch (Exception) { }
                return feeds;
            });

            return null;
        }
        public static async Task<List<MyFeed>> GetMyFeeds(int page = 0, int size = 15)
        {
            return await Task.Run(async () =>
            {
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
                paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
                string response = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Article/searchtrends", paramList);
                Debug.WriteLine(response);
                //response = Utils.ConvertUnicodeStringToChinese(response);
                List<MyFeed> feeds = new List<MyFeed>();
                try
                {
                    if (response != "" || response != "[]")
                    {
                        JObject bbddfeeds = JObject.Parse(response);
                        if (bbddfeeds["status"].ToString() == "200")
                        {
                            JArray bbddarray = JArray.Parse(bbddfeeds["data"].ToString());
                            for (int i = 0; i < bbddarray.Count; i++)
                            {
                                MyFeed f = new MyFeed();
                                f.GetAttributes((JObject)bbddarray[i]);
                                feeds.Add(f);
                            }
                        }
                    }
                }
                catch (Exception) { }
                return feeds;
            });

            return null;
        }
    }
}
