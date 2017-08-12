using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Models;

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    public class NetWork
    {
        public async Task<string> GetElectricityByStuNum(string uri, List<KeyValuePair<string, string>> paramList)
        {
            string content = "";
            return await Task.Run(() =>
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        HttpRequestMessage requst;
                        HttpResponseMessage response;
                        requst = new HttpRequestMessage(HttpMethod.Post, new Uri(uri));
                        response = httpClient.PostAsync(new Uri(uri), new FormUrlEncodedContent(paramList)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            content = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "电费网络请求异常");
                    }
                }
                return content;
            });
        }

        public ElectricityByStuNum ByStuNumStringConvertToModel(string str)
        {
            ElectricityByStuNum electricityRootobject = JsonConvert.DeserializeObject<ElectricityByStuNum>(str);
            return electricityRootobject;
        }

        public async Task<string> GetElectricityByRoomNum(string uri, List<KeyValuePair<string, string>> paramList)
        {
            string content = "";
            return await Task.Run(() =>
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        HttpRequestMessage requst;
                        HttpResponseMessage response;
                        requst = new HttpRequestMessage(HttpMethod.Post, new Uri(uri));
                        response = httpClient.PostAsync(new Uri(uri), new FormUrlEncodedContent(paramList)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            content = response.Content.ReadAsStringAsync().Result;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "电费网络请求异常");
                    }
                }
                return content;
            });
        }

        public ElectricityByRoomNum ByRoomNumStringConvertToModel(string str)
        {
            ElectricityByRoomNum electricityRootobject = JsonConvert.DeserializeObject<ElectricityByRoomNum>(str);
            return electricityRootobject;
        }
    }
}
