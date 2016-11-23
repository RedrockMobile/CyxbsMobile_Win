using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using ZSCY_Win10.Models.RemindPage;

namespace RemindBackgroundTask3
{
    class NetWork
    {
      
      
 
        public static async Task<string> httpRequest(string api, List<KeyValuePair<string, string>> paramList)
        {

            string content = "";
            await Task.Run(() =>
            {
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = httpClient.PostAsync(new Uri(api), new FormUrlEncodedContent(paramList)).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    content = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(content);
                }
            });
            return content;
            //return await Task.Run(async () =>
            //{
            //HttpClient httpClient = new HttpClient();

            //    HttpResponseMessage response = await httpClient.PostAsync(new Uri(api), new FormUrlEncodedContent(paramList))).Result;
            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //        content = response.Content.ReadAsStringAsync().Result;

            //    }
            //});
            //            return content;

        }
    }
}
