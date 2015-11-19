using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Util
{
    class NetWork
    {
        public static async Task<string> getHttpWebRequest(string api, List<KeyValuePair<String, String>> paramList = null)
        {
            string content = "";
            return await Task.Run(() =>
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        string uri = "http://hongyan.cqupt.edu.cn/" + api;
                        httpClient.DefaultRequestHeaders.Add("API_APP", "winphone");
                        httpClient.DefaultRequestHeaders.Add("API_TOKEN", "0zLUZA0j+OL77OsjXC0ulOz50KaI6yANZtkOk2vQIDg=");
                        HttpResponseMessage response = httpClient.PostAsync(new Uri(uri), new FormUrlEncodedContent(paramList)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                            content = response.Content.ReadAsStringAsync().Result;
                        //else if (response.StatusCode == HttpStatusCode.NotFound)
                        //    Utils.Message("Oh...服务器又跪了，给我们点时间修好它");

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "网络请求异常");
                    }
                }
                else
                {
                }
                if (content.IndexOf("{") != 0)
                    return "";
                else
                    return content;

            });
        }
    }
}
