using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
                                Windows.Storage.ApplicationData.Current.LocalSettings.Values["isLogin"] = false;
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
    }
}