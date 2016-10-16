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
        private static string resourceName = "ZSCY";

        public static async Task<List<MyNotification>> GetNotifications(int page = 0, int size = 15)
        {
            return await Task.Run(async () =>
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
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
                        if (page == 0) //将第一面的数据存入文件，用于后台任务
                        {
                            IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                            IStorageFile storageFileWR = await applicationFolder.CreateFileAsync("aboutme.txt", CreationCollisionOption.ReplaceExisting);
                            await FileIO.WriteTextAsync(storageFileWR, response);
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
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
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
                                f.GetAttributes((JObject)bbddarray[i],false);
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


        const string api = "cyxbsMobile/index.php/Home/Person/search";
        public static async Task<PeoInfo> GetPerson()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
            string response = await NetWork.getHttpWebRequest(api, paramList);
            try
            {
                if (response != "" && response != "[]")
                {
                    JObject bbddfeeds = JObject.Parse(response);
                    if (bbddfeeds["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)bbddfeeds["data"];
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
