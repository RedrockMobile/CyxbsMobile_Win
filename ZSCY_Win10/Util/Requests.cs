using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;

namespace ZSCY_Win10.Util
{
    internal class Requests
    {
        public static string baseUrl = "https://be-prod.redrock.cqupt.edu.cn/";
        private static string resourceName = "ZSCY";
        /// <summary>
        ///
        /// </summary>
        /// <param name="api"></param>
        /// <param name="query"></param>
        /// <param name="param"></param>
        /// <param name="method">默认为get请求</param>
        /// <returns></returns>
        public static async Task<JObject> Send(string api, Dictionary<string, string> query = null, Dictionary<string, string> param = null, bool json = true, string method = "get", bool token = false, bool check = true)
        {
            JObject content = null;
            return await Task.Run(async () =>
            {
                if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                {
                    try
                    {
                        System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                        string uri = baseUrl + api;
                        if (query != null) uri = QueryHelpers.AddQueryString(uri, query);
                        System.Net.Http.HttpMethod requestMethod;
                        if (method == "post")
                            requestMethod = System.Net.Http.HttpMethod.Post;
                        else if (method == "get")
                            requestMethod = System.Net.Http.HttpMethod.Get;
                        else if (method == "delete")
                            requestMethod = System.Net.Http.HttpMethod.Delete;
                        else if (method == "put")
                            requestMethod = System.Net.Http.HttpMethod.Put;
                        else
                            throw new Exception("无效的请求");
                        var request = new HttpRequestMessage(requestMethod, new Uri(uri));
                        if (param != null)
                            if (json) request.Content = new StringContent(JsonConvert.SerializeObject(param), Encoding.UTF8, "application/json");
                            else request.Content = new FormUrlEncodedContent(param);
                        if (token)
                        {
                            var vault = new Windows.Security.Credentials.PasswordVault();
                            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", vault.Retrieve(resourceName, "token").Password.ToString());
                        }
                        var response = httpClient.SendAsync(request).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                            content = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                        if (check && content != null && content["status"].ToString() == "20003" && token)
                        {
                            var vault = new Windows.Security.Credentials.PasswordVault();
                            var tokenForm = new Dictionary<string, string>();
                            tokenForm.Add("refreshToken", vault.Retrieve(resourceName, "refreshToken").Password.ToString());
                            JObject newObj = await Requests.Send("magipoke/token/refresh", param: tokenForm, method: "post");
                            if (newObj != null && newObj["status"].ToString() == "10000")
                            {
                                vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, "refreshToken", newObj["data"]["refreshToken"].ToString()));
                                vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, "token", newObj["data"]["token"].ToString()));
                                return await Send(api, query, param, json, method, token);
                            }
                            else
                            {
                                vault.Remove(vault.Retrieve(resourceName, "token"));
                                vault.Remove(vault.Retrieve(resourceName, "refreshToken"));
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message + "网络请求异常");
                    }
                }
                else
                {
                    Utils.Message("无网络连接，请先检查本机网络哦");
                }
                return content;
            });
        }

        public static async Task<string> headUpload(string stunum, string fileUri, string uri = "http://hongyan.cqupt.edu.cn/cyxbsMobile/index.php/home/Photo/upload", bool isPath = false)
        {
            Windows.Web.Http.HttpClient _httpClient = new Windows.Web.Http.HttpClient();
            CancellationTokenSource _cts = new CancellationTokenSource();
            Windows.Web.Http.HttpStringContent stunumStringContent = new Windows.Web.Http.HttpStringContent(stunum);
            string head = "";
            //IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
            IStorageFile saveFile;
            if (isPath)
                saveFile = await StorageFile.GetFileFromPathAsync(fileUri);
            else
                saveFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(fileUri));

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
                        _httpClient.PostAsync(new Uri(uri), fileContent)
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

        public static async Task<bool> downloadFile(string uri, string saveUri, string filename)
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response = new System.Net.Http.HttpResponseMessage();
            return await Task.Run(async () =>
            {
                try
                {
                    if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
                    {
                        response = httpClient.GetAsync(new Uri(uri)).Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Stream stream = response.Content.ReadAsStreamAsync().Result;
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            StorageFolder downFolder = null;
                            if (saveUri == "picture")
                            {
                                downFolder = KnownFolders.SavedPictures;
                            }
                            else
                            {
                                downFolder = await StorageFolder.GetFolderFromPathAsync(saveUri);
                            }
                            var saveFile = await downFolder.CreateFileAsync(filename, CreationCollisionOption.GenerateUniqueName);
                            using (Stream streamSave = await saveFile.OpenStreamForWriteAsync())
                            {
                                await streamSave.WriteAsync(bytes, 0, bytes.Length);
                            }
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            });
        }
    }
}