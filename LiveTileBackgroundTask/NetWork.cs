using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace LiveTileBackgroundTask
{
    class NetWork
    {
        public static async Task<string> GetCourseSchedule(string api, List<KeyValuePair<String, String>> paramList)
        {
            string content = "";
            return await Task.Run(() =>
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        string uri = api;
                        httpClient.DefaultRequestHeaders.Add("API_APP", "winphone");
                        httpClient.DefaultRequestHeaders.Add("API_TOKEN", "0zLUZA0j+OL77OsjXC0ulOz50KaI6yANZtkOk2vQIDg=");
                        HttpResponseMessage response = httpClient.PostAsync(uri, new FormUrlEncodedContent(paramList)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            content = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "网络请求异常");
                    }
                }
                return content;
            });
        }

        public static async Task<string> GetTransaction(string api,List<KeyValuePair<String,String>> paramList)
        {
            string content = "";
            return await Task.Run(() => 
            {
                if(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        string uri = api;
                        HttpResponseMessage response = httpClient.PostAsync(new Uri(api), new FormUrlEncodedContent(paramList)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            content = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.Message + "网络请求异常");
                    }
                }
                return content;
            });
        }
    }
}
