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
    public class MyService
    {
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        private static string resourceName = "ZSCY";

        public static async Task<List<MyNotification>> GetNotifications(int page = 0, int size = 15)
        {
            return await Task.Run(async () =>
            {
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
                paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
                JObject response = await Requests.Send("cyxbsMobile/index.php/Home/Article/aboutme");
                //response = Utils.ConvertUnicodeStringToChinese(response);
                Debug.WriteLine(response);
                List<MyNotification> feeds = new List<MyNotification>();
                try
                {
                    if (response != null)
                    {
                        if (response["status"].ToString() == "200")
                        {
                            JArray bbddarray = JArray.Parse(response["data"].ToString());
                            for (int i = 0; i < bbddarray.Count; i++)
                            {
                                MyNotification f = new MyNotification();
                                f.GetAttributes((JObject)bbddarray[i]);
                                feeds.Add(f);
                            }
                        }
                        if (page == 0) //将第一面的数据存入文件，用于后台任务
                        {
                            IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                            IStorageFile storageFileWR = await applicationFolder.CreateFileAsync("aboutme.txt", CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteTextAsync(storageFileWR, response.ToString());
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
                paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
                paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));

                JObject response = await Requests.Send("cyxbsMobile/index.php/Home/Article/searchtrends");
                Debug.WriteLine(response);
                //response = Utils.ConvertUnicodeStringToChinese(response);
                List<MyFeed> feeds = new List<MyFeed>();
                try
                {
                    if (response != null)
                    {
                        if (response["status"].ToString() == "200")
                        {
                            JArray bbddarray = JArray.Parse(response["data"].ToString());
                            for (int i = 0; i < bbddarray.Count; i++)
                            {
                                MyFeed f = new MyFeed();
                                f.GetAttributes((JObject)bbddarray[i], false);
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

        private const string api = "cyxbsMobile/index.php/Home/Person/search";

        public static async Task<PeoInfo> GetPerson()
        {
            JObject response = await Requests.Send(api);
            try
            {
                if (response != null)
                {
                    if (response["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)response["data"];
                        appSetting.Values["Community_people_id"] = feed["id"].ToString();
                        appSetting.Values["Community_nickname"] = feed["nickname"].ToString();
                        appSetting.Values["Community_headimg_src"] = feed["photo_src"].ToString();
                        appSetting.Values["Community_introduction"] = feed["introduction"].ToString();
                        appSetting.Values["Community_phone"] = feed["phone"].ToString();
                        appSetting.Values["Community_qq"] = feed["qq"].ToString();
                        PeoInfo f = new PeoInfo();
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