using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
            ContentWebView.NavigateToString(NewsItem.Content_all);
            DateReadTextBlock.Text = "发布时间:" + NewsItem.Date + "阅读人数:" + NewsItem.Read;
            UmengSDK.UmengAnalytics.TrackPageStart("JWContentPage");
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
                    string content_all = content;
                    Debug.WriteLine("content->" + content);
                    while (content.IndexOf("<") != -1)
                    {
                        content = content.Remove(content.IndexOf("<"), content.IndexOf(">") - content.IndexOf("<") + 1);
                    }
                    //content.Replace("&nbsp;", "");

                    while (content.StartsWith("\r") || content.StartsWith("\n") || content.StartsWith("\t") || content.StartsWith(" ") || content.StartsWith("&nbsp;"))
                        content = content.Substring(1);
                    while (content.StartsWith("&nbsp;"))
                        content = content.Substring(6);
                    //while (content.StartsWith("\r\n "))
                    //    content = content.Substring(3);
                    //while (content.StartsWith("\r\n"))
                    //    content = content.Substring(2);
                    //while (content.StartsWith("\n\t"))
                    //    content = content.Substring(2);
                    //while (content.StartsWith("\n"))
                    //    content = content.Substring(1);
                    //while (content.StartsWith("\r"))
                    //    content = content.Substring(1);
                    //while (content.StartsWith("\t"))
                    //    content = content.Substring(1);
                    //while (content.StartsWith("\\"))
                    //    content = content.Substring(1);
                    //content.Replace('\r', '\a');
                    //content.Replace('\n', '\a');
                    //content.Replace(" ", "");
                    ContentWebView.NavigateToString(content);
                    Debug.WriteLine("content->" + content);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("JWContentPage");
        }
    }
}
