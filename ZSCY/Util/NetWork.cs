﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;


namespace ZSCY.Util
{
    class NetWork
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="api"></param>
        /// <param name="paramList"></param>
        /// <param name="PostORGet">0：POST，1：GET</param>
        /// <returns></returns>
        public static async Task<string> getHttpWebRequest(string api, List<KeyValuePair<String, String>> paramList = null, int PostORGet = 0, bool fulluri = false)
        {
            string content = "";
            return await Task.Run(() =>
           {
               if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
               {
                   try
                   {
                       HttpClient httpClient = new HttpClient();
                       string uri;
                       if (!fulluri)
                           uri = "http://hongyan.cqupt.edu.cn/" + api;
                       else
                           uri = api;
                       httpClient.DefaultRequestHeaders.Add("API_APP", "winphone");
                       httpClient.DefaultRequestHeaders.Add("API_TOKEN", "0zLUZA0j+OL77OsjXC0ulOz50KaI6yANZtkOk2vQIDg=");
                       HttpRequestMessage requst;
                       System.Net.Http.HttpResponseMessage response;
                       if (PostORGet == 0)
                       {
                           requst = new HttpRequestMessage(HttpMethod.Post, new Uri(uri));
                           response = httpClient.PostAsync(new Uri(uri), new FormUrlEncodedContent(paramList)).Result;
                       }
                       else
                       {
                           requst = new HttpRequestMessage(HttpMethod.Get, new Uri(uri));
                           response = httpClient.GetAsync(new Uri(uri)).Result;
                       }
                       if (response.StatusCode == HttpStatusCode.OK)
                           content = response.Content.ReadAsStringAsync().Result;
                       else if (response.StatusCode == HttpStatusCode.NotFound)
                           Utils.Message("Oh...服务器又跪了，给我们点时间修好它");

                   }
                   catch (Exception e)
                   {
                       Debug.WriteLine(e.Message + "网络请求异常");
                   }
               }
               else
               {
               }
               //if (content.IndexOf("{") != 0)
               //    return "";
               //else
                   return content;

           });
        }
    }



}
