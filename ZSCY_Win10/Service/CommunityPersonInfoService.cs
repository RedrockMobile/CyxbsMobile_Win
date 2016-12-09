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
    public class CommunityPersonInfoService
    {
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        const string api = "cyxbsMobile/index.php/Home/Person/search";
        private static string resourceName = "ZSCY";
        public static async Task<PeoInfo> GetPerson(string stunum_other)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
            paramList.Add(new KeyValuePair<string, string>("stunum_other", stunum_other));
            string response = await NetWork.getHttpWebRequest(api, paramList);
            try
            {
                if (response != "" && response != "[]")
                {
                    JObject bbddfeeds = JObject.Parse(response);
                    if (bbddfeeds["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)bbddfeeds["data"];
                        PeoInfo f = new PeoInfo();
                        f.GetAttributes(feed);
                        return f;
                    }
                }
            }
            catch (Exception) { }
            return null;
        }
        public static async Task<List<MyFeed>> GetMyFeeds(string stunum_other,int page = 0, int size = 15)
        {
            //TODO:未登陆时 不传入参数stuNum和idNum
            return await Task.Run(async () =>
            {
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                try
                {
                    var vault = new Windows.Security.Credentials.PasswordVault();
                    var credentialList = vault.FindAllByResource(resourceName);
                    credentialList[0].RetrievePassword();
                    if (credentialList.Count > 0)
                    {
                        //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                        //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                        paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                        paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                    }
                }
                catch { }
                paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
                paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
                paramList.Add(new KeyValuePair<string, string>("stunum_other", stunum_other.ToString()));
                //string response = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Article/searchtrends", paramList);
                string response = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/NewArticle/searchtrends", paramList);
                Debug.WriteLine(response);
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
