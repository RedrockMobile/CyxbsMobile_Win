using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LiveTileBackgroundTask
{
    class NetWork
    {
        public static async Task<string> getCurriculum(string api, List<KeyValuePair<String, String>> paramList = null, int PostORGet = 0, bool fulluri = false)
        {
            string content = "";
            return await Task.Run(() =>
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                        string uri;
                        if (!fulluri)
                            uri = "http://hongyan.cqupt.edu.cn/" + api;
                        else
                            uri = api;
                        httpClient.DefaultRequestHeaders.Add("API_APP", "winphone");
                        httpClient.DefaultRequestHeaders.Add("API_TOKEN", "0zLUZA0j+OL77OsjXC0ulOz50KaI6yANZtkOk2vQIDg=");
                        System.Net.Http.HttpRequestMessage requst;
                        System.Net.Http.HttpResponseMessage response;
                        if (PostORGet == 0)
                        {
                            requst = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, new Uri(uri));
                            response = httpClient.PostAsync(new Uri(uri), new System.Net.Http.FormUrlEncodedContent(paramList)).Result;
                        }
                        else
                        {
                            requst = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, new Uri(uri));
                            response = httpClient.GetAsync(new Uri(uri)).Result;
                        }
                        if (response.StatusCode == HttpStatusCode.OK)
                            content = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "网络请求异常");
                    }
                }
                return content;
            });
        }

        public static async Task<string> getTransaction(string api, List<KeyValuePair<string, string>> paramList)
        {
            return await Task.Run(() =>
            {
                string content = "";
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage response = httpClient.PostAsync(new Uri(api), new System.Net.Http.FormUrlEncodedContent(paramList)).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    content = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(content);
                }
                return content;
            });
        }
    }
}
