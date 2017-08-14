using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsContentPage : Page
    {
        public NewsContentPage()
        {
            this.InitializeComponent();
            //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //e.Handled = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var NewsItem = (NewsList)e.Parameter;

            if (NewsItem.Content == "加载中...")
                getContent(NewsItem.Articleid);

            TitleTextBlock.Text = NewsItem.Title;
            //ContentTextBlock.Text = NewsItem.Content_all;

            if (NewsItem.Content_all != "")
            {
                JObject newsContentobj = JObject.Parse(NewsItem.Content_all);
                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                {
                    ContentWebView.NavigateToString((JObject.Parse(newsContentobj["data"].ToString()))["content"].ToString());
                }
            }
            if (NewsItem.Read != "")
            {
                DateReadTextBlock.Text = "发布时间:" + NewsItem.Date + " 阅读人数:" + NewsItem.Read;
            }
            else
            {
                DateReadTextBlock.Text = "发布时间:" + NewsItem.Date;
            }
            UmengSDK.UmengAnalytics.TrackPageStart("NewsContentPage");
        }

        private async void getContent(string Articleid)
        {
            //List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
            //contentparamList.Add(new KeyValuePair<string, string>("id", ID));
            //string jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
            //Debug.WriteLine("jwContent->" + jwContent);
            //if (jwContent != "")
            //{
            //    string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");
            //    JObject jwContentobj = JObject.Parse(JWContentText);
            //    if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
            //    {
            //        string JWitemContent = jwContentobj["data"]["content"].ToString();
            //        while (JWitemContent.StartsWith("\r\n "))
            //            JWitemContent = JWContentText.Substring(3);
            //        while (JWitemContent.StartsWith("\r\n"))
            //            JWitemContent = JWContentText.Substring(2);
            //        while (JWitemContent.StartsWith("\n\t"))
            //            JWitemContent = JWContentText.Substring(2);
            //        while (JWitemContent.StartsWith("\n"))
            //            JWitemContent = JWitemContent.Substring(1);
            //    }
            //    else
            //        ContentTextBlock.Text = "加载失败";
            //}

            List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
            contentparamList.Add(new KeyValuePair<string, string>("type", "jwzx"));
            contentparamList.Add(new KeyValuePair<string, string>("articleid", Articleid));
            string newsContent = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchcontent", contentparamList);
            //Debug.WriteLine("newsContent->" + newsContent);
            if (newsContent != "")
            {
                JObject newsContentobj = JObject.Parse(newsContent);
                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                {
                    string content = (JObject.Parse(newsContentobj["data"].ToString()))["content"].ToString();
                    ContentWebView.NavigateToString(content);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("NewsContentPage");
        }
    }
}