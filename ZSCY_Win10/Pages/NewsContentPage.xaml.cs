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
                getContent(NewsItem.ID);

            TitleTextBlock.Text = NewsItem.Title;
            //ContentTextBlock.Text = NewsItem.Content_all;

            if (NewsItem.Content_all != "")
            {
                JObject newsContentobj = JObject.Parse(NewsItem.Content_all);
                if (Int32.Parse(newsContentobj["status"].ToString()) == 200)
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
        }

        private async void getContent(string id)
        {
            Dictionary<string, string> contentQuery = new Dictionary<string, string>();
            contentQuery.Add("id", id);
            JObject newsContent = await Requests.Send("magipoke-jwzx/jwNews/content", query: contentQuery);
            //Debug.WriteLine("newsContent->" + newsContent);
            if (newsContent != null)
            {
                if (Int32.Parse(newsContent["status"].ToString()) == 200)
                {
                    string content = newsContent["data"]["content"].ToString();
                    ContentWebView.NavigateToString(content);
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        { }
    }
}