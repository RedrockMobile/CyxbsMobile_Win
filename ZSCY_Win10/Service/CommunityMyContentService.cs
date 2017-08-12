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
    public class CommunityMyContentService
    {
        public static ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        //const string api = "cyxbsMobile/index.php/Home/Article/searchContent";
        const string api = "cyxbsMobile/index.php/Home/NewArticle/searchContent";
        private static string resourceName = "ZSCY";

        public static async Task<MyFeed> GetFeed(int type_id, string article_id)
        {
            //TODO:未登陆时 不添加参数stuNum和idNum
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
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id.ToString()));
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id.ToString()));
            string response = await NetWork.getHttpWebRequest(api, paramList);
            Debug.WriteLine(response);
            try
            {
                if (response != "" || response != "[]")
                {
                    JObject bbddfeeds = JObject.Parse(response);
                    if (bbddfeeds["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)bbddfeeds["data"][0];
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
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password.ToString()));
            paramList.Add(new KeyValuePair<string, string>("type_id", type_id.ToString()));
            paramList.Add(new KeyValuePair<string, string>("article_id", article_id.ToString()));
            string response = await NetWork.getHttpWebRequest(api, paramList);
            Debug.WriteLine(response);
            try
            {
                if (response != "" || response != "[]")
                {
                    JObject bbddfeeds = JObject.Parse(response);
                    if (bbddfeeds["status"].ToString() == "200")
                    {
                        JObject feed = (JObject)bbddfeeds["data"][0];
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
