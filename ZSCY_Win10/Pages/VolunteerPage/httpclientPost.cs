using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace ZSCY_Win10.Pages
{
    class httpclientPost
    {
        List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
        public async Task<string> PostHttpClient(string account, string password)
        {
            string content = "";
           
                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        list.Add(new KeyValuePair<string, string>("account", account));
                        list.Add(new KeyValuePair<string, string>("password", password));
                        HttpResponseMessage responseMessage = httpClient.PostAsync(new Uri("https://redrock.team/servicerecord/login"), new FormUrlEncodedContent(list)).Result;
                        if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            content = responseMessage.Content.ReadAsStringAsync().Result;

                        }
                    }

                    catch(Exception)
                    {
                        var dialog = new ContentDialog();
                        dialog.Title = "error";
                        dialog.Content = "好像没网络哦~~";
                        dialog.PrimaryButtonText = "确定";
                        dialog.PrimaryButtonClick += (_s, _e) =>
                        {

                        };
                        await dialog.ShowAsync();
                    }
                    
           
            return content;
        }
        }
    }
