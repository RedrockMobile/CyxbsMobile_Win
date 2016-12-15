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

namespace SycnRemindBackgroundTask
{
    internal sealed class NetWork
    {
        public static async Task<string> httpRequest(string api, List<KeyValuePair<string, string>> paramList)
        {
            //string content = "";
            //HttpClient httpClient = new HttpClient();
            //await Task.Run(() =>
            // {
            //     try
            //     {

            //         HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(api));
            //         HttpResponseMessage response = httpClient.PostAsync(new Uri(api), new FormUrlEncodedContent(paramList)).Result;
            //         if (response.StatusCode == HttpStatusCode.OK)
            //         {
            //             content = response.Content.ReadAsStringAsync().Result;
            //             Debug.WriteLine(content);
            //         }
            //     }
            //     catch (Exception x)
            //     {
            //         Debug.Write(x);
            //     }

            // });


            //return content;
            string content = "";
            try
            {
               
                    HttpClient httpClient = new HttpClient();
                    HttpRequestMessage requst = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, new Uri(api));

                    HttpResponseMessage response = httpClient.PostAsync(new Uri(api), new FormUrlEncodedContent(paramList)).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        content = response.Content.ReadAsStringAsync().Result;
                        Debug.WriteLine(content);
                    }
             
            }
            catch (Exception f)
            {
                Debug.WriteLine(f);
            }

            return content;
        }
        public static List<KeyValuePair<string, string>> addRemind(MyRemind myRemind)
        {
            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", myRemind.StuNum));
            paramList.Add(new KeyValuePair<string, string>("idNum", myRemind.IdNum));
            string date = "[";

            for (int i = 0; i < myRemind.DateItems.Count; i++)
            {
                string dateJson = JsonConvert.SerializeObject(myRemind.DateItems[i]);
                date += $"{dateJson},";
            }
            date = date.Remove(date.Length - 1) + "]";

            paramList.Add(new KeyValuePair<string, string>("date", date));
            paramList.Add(new KeyValuePair<string, string>("title", myRemind.Title));
            paramList.Add(new KeyValuePair<string, string>("time", myRemind.Time));
            paramList.Add(new KeyValuePair<string, string>("content", myRemind.Content));
            return paramList;
        }
    }
}
