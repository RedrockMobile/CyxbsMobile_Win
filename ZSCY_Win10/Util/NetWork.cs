using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace ZSCY_Win10.Util
{
    class NetWork
    {
        public static async Task<string> getHttpWebRequest(string api, List<KeyValuePair<String, String>> paramList = null, int PostORGet = 0, bool fulluri = false)
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
                //if (content.IndexOf("{") != 0)
                //    return "";
                //else
                return content;

            });
        }

        public static async Task<string> headUpload(string stunum, string fileUri)
        {
            Windows.Web.Http.HttpClient _httpClient = new Windows.Web.Http.HttpClient();
            CancellationTokenSource _cts = new CancellationTokenSource();
            Windows.Web.Http.HttpStringContent stunumStringContent = new Windows.Web.Http.HttpStringContent(stunum);
            string head = "";
            //IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            IStorageFile saveFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(fileUri));
            try
            {
                // 构造需要上传的文件数据
                IRandomAccessStreamWithContentType stream1 = await saveFile.OpenReadAsync();
                Windows.Web.Http.HttpStreamContent streamContent = new Windows.Web.Http.HttpStreamContent(stream1);
                Windows.Web.Http.HttpMultipartFormDataContent fileContent = new Windows.Web.Http.HttpMultipartFormDataContent();

                fileContent.Add(streamContent, "fold", "head.png");
                fileContent.Add(stunumStringContent, "stunum");

                Windows.Web.Http.HttpResponseMessage response =
                    await
                        _httpClient.PostAsync(new Uri("http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/home/Photo/upload"), fileContent)
                            .AsTask(_cts.Token);
                head = Utils.ConvertUnicodeStringToChinese(await response.Content.ReadAsStringAsync().AsTask(_cts.Token));
                Debug.WriteLine(head);
                return head;
            }
            catch (Exception)
            {
                Debug.WriteLine("上传头像失败,编辑页面");
                return "";
            }


        }

    }
}
