using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using ZSCY_Win10.ViewModels;

namespace ZSCY_Win10.Models
{
    class LFDetailPageModel
    {
        public async Task<LFDetailPageViewModel> GetDetail(string id)
        {
            LFDetailPageViewModel LFDPVM = new LFDetailPageViewModel();
            string content = await GetJson("http://hongyan.cqupt.edu.cn/laf/api/detail/" + id);
            if (content != "NetworkError")
            {
                LFDPVM = JsonToObject(content);
                if (LFDPVM.wx_avatar != "")
                    if (LFDPVM.wx_avatar[0] == '/')
                        LFDPVM.HeadImg.UriSource = new Uri("http://hongyan.cqupt.edu.cn" + LFDPVM.wx_avatar);
                    else
                        LFDPVM.HeadImg.UriSource = new Uri(LFDPVM.wx_avatar);
            }
            return LFDPVM;
        }
        protected async Task<string> GetJson(string Uri)
        {
            var uri = new Uri(Uri);
            string content = "";
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = httpClient.GetAsync(uri).Result;
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
        protected LFDetailPageViewModel JsonToObject(string Json)
        {
            var LFDetailViewMode = new LFDetailPageViewModel();
            LFDetailViewMode = JsonConvert.DeserializeObject<LFDetailPageViewModel>(Json);
            return LFDetailViewMode;
        }
    }
}
