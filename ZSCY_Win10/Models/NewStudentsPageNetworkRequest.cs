using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY.Models
{
    /// <summary>
    /// 新生专题网 网络请求
    /// </summary>
    public class NewStudentsPageNetworkRequest
    {
        /// <summary>
        /// 网络请求接口
        /// </summary>
        /// <param name="uri">网址</param>
        /// <param name="paramList">键值对</param>
        /// <param name="PostOrGet">获取方式PostOrGet = 0 为Post；PostOrGet = 1 为Get</param>
        /// <returns></returns>
        public static async Task<string> NetworkRequest(string uri, List<KeyValuePair<String, String>> paramList = null, int PostOrGet = 0)
        {
            //返回内容
            string _content = "";
            return await Task.Run(() => {

                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                        System.Net.Http.HttpRequestMessage requst;
                        System.Net.Http.HttpResponseMessage response;

                        if (PostOrGet == 0)
                        {
                            requst = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, new Uri(uri));
                            response = httpClient.PostAsync(new Uri(uri), new System.Net.Http.FormUrlEncodedContent(paramList)).Result;
                            if (response.StatusCode == HttpStatusCode.OK)
                                _content = response.Content.ReadAsStringAsync().Result;
                        }
                        else if (PostOrGet == 1)
                        {
                            string key = "";
                            string value = "";
                            string newUri = "";
                            foreach (var item in paramList)
                            {
                                key = item.Key;
                                value = item.Value;
                            }
                            //http://yangruixin.com/test/apiForText.php?RequestType=organizations
                            newUri = uri + "?" + key + "=" + value;
                            requst = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, new Uri(uri));
                            response = httpClient.GetAsync(new Uri(newUri)).Result;

                            if (response.StatusCode == HttpStatusCode.OK)
                                _content = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "网络请求异常");
                    }
                }
                return _content;
            });
        }
    }
}
