using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using ZSCY_Win10.ViewModels;

namespace ZSCY_Win10.Models
{
    class LostAndFoundPageModel
    {
        string[] catalog = { "", "一卡通", "钱包", "钥匙", "电子产品", "雨伞", "衣物", "其他" };
        public async Task<LostAndFoundPageViewModel> LoadItems(string uri, int cat = 0)
        {
            LostAndFoundPageViewModel LFPVM = new LostAndFoundPageViewModel();
            string content;
            content = await GetJson(uri + catalog[cat]);
            if (content != "NetworkError")
            {
                if (content == "")
                    return LFPVM;
                LFPVM = JsonToObject(content);
                foreach (LFItem i in LFPVM.data)
                {
                    if (i.wx_avatar == "")
                        continue;
                    else if (i.wx_avatar[0] == '/')
                        i.wx_avatar = "http://hongyan.cqupt.edu.cn" + i.wx_avatar;
                    i.HeadImg = new BitmapImage(new Uri(i.wx_avatar));
                }
            }
            return LFPVM;
        }
        
        protected async Task<string> GetJson(string Uri)
        {
            var uri = new Uri(Uri);
            string content = "";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await httpClient.GetAsync(Uri);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    content = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                content = "NetworkError";
                await new MessageDialog("网络错误\n" + e.Message).ShowAsync();
            }
            return content;
        }
        protected LostAndFoundPageViewModel JsonToObject(string Json)
        {
            //var lostAndFoundPageViewMode = new LostAndFoundPageViewMode();
            var lostAndFoundPageViewMode = JsonConvert.DeserializeObject<LostAndFoundPageViewModel>(Json);
            return lostAndFoundPageViewMode;
        }
    }
}
