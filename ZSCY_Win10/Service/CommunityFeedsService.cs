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

namespace ZSCY_Win10.Service
{
    public class CommunityFeedsService
    {

        const string hotFeeds = @"cyxbsMobile/index.php/Home/Article/searchHotArticle";
        const string bbddfeeds = @"cyxbsMobile/index.php/Home/Article/listArticle";
        public static string[] feedsapi = { hotFeeds, bbddfeeds };
        /// <summary>
        /// 获取动态列表
        /// </summary>
        /// <param name="type">动态参数，重邮新闻cyxw=>1,教务咨询jwzx=>2,xsjz=>3,xwgg=>4,bbdd=>5</param>
        /// <returns>返回参数对应的列表数据</returns>
        public static async Task<List<Feeds>> GetDatas(int type = 0, int page = 1, int size = 1, int typeid = 5)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", "2013211429"));
            paramList.Add(new KeyValuePair<string, string>("idNum", "252617"));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            paramList.Add(new KeyValuePair<string, string>("size", size.ToString()));
            if (typeid != 0)
                paramList.Add(new KeyValuePair<string, string>("type_id", typeid.ToString()));
            string response = await NetWork.getHttpWebRequest(feedsapi[type], paramList);
            try
            {
                JArray jsonstr = JArray.Parse(response);

                List<Feeds> feedslist = new List<Feeds>();

                for (int j = 0; j < jsonstr.Count; j++)
                {
                    JObject jsondetail = (JObject)(jsonstr[j]);
                    if (jsondetail["status"].ToString() == "200")
                    {
                        JObject jo = (JObject)jsondetail["data"];
                        Feeds f = new Feeds();
                        f.GetAttributes(jo);
                        feedslist.Add(f);
                    }
                }
                return feedslist;

            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                List<Feeds> feedslist = new List<Feeds>();
                JObject jsonobj = JObject.Parse(response);
                if (jsonobj["status"].ToString() == "200")
                {
                    string jsonstr = jsonobj["data"].ToString();
                    JArray feedsarray = Utils.ReadJso(response);
                    for (int i = 0; i < feedsarray.Count; i++)
                    {
                        JObject f = (JObject)feedsarray[i];
                        Feeds fd = new Feeds();
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
            return null;
        }
    }
}
