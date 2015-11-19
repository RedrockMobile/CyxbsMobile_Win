using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JWContentPage : Page
    {
        public JWContentPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var JWItem = (JWList)e.Parameter;

            if (JWItem.Content == "加载中...")
                getContent(JWItem.ID);

            TitleTextBlock.Text = JWItem.Title;
            ContentTextBlock.Text = JWItem.Content;
            DateReadTextBlock.Text = "发布时间:" + JWItem.Date + "阅读人数:" + JWItem.Read;
            UmengSDK.UmengAnalytics.TrackPageStart("JWContentPage");
        }


        private async void getContent(string ID)
        {
            List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
            contentparamList.Add(new KeyValuePair<string, string>("id", ID));
            string jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
            Debug.WriteLine("jwContent->" + jwContent);
            if (jwContent != "")
            {
                string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");
                JObject jwContentobj = JObject.Parse(JWContentText);
                if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                {
                    string JWitemContent = jwContentobj["data"]["content"].ToString();
                    while (JWitemContent.StartsWith("\r\n "))
                        JWitemContent = JWContentText.Substring(3);
                    while (JWitemContent.StartsWith("\r\n"))
                        JWitemContent = JWContentText.Substring(2);
                    while (JWitemContent.StartsWith("\n\t"))
                        JWitemContent = JWContentText.Substring(2);
                    while (JWitemContent.StartsWith("\n"))
                        JWitemContent = JWitemContent.Substring(1);
                }
                else
                    ContentTextBlock.Text = "加载失败";
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("JWContentPage");
        }
    }
}
