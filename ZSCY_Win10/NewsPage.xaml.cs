using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewsPage : Page
    {
        private int page = 0;
        ObservableCollection<NewsList> JWList = new ObservableCollection<NewsList>();
        ObservableCollection<NewsList> XWList = new ObservableCollection<NewsList>();
        ObservableCollection<NewsList> CYList = new ObservableCollection<NewsList>();
        ObservableCollection<NewsList> XSList = new ObservableCollection<NewsList>();
        public NewsPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000 && e.NewSize.Width < 750)
                {
                    if (JWListView.SelectedIndex != -1)
                    {
                        //JWBackAppBarButton.Visibility = Visibility.Visible;
                        SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        NewsRefreshAppBarButton.Visibility = Visibility.Collapsed;
                    }
                    JWListView.Width = e.NewSize.Width;
                }
                if (e.NewSize.Width < (400 - 48))
                {
                    NewsTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    NewsTitleGrid.Margin = new Thickness(0);
                }
                if (e.NewSize.Width > 750)
                {
                    //JWBackAppBarButton.Visibility = Visibility.Collapsed;
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    NewsRefreshAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState750";
                    JWListView.Width = 400;
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
            Debug.WriteLine("Init");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            JWListView.ItemsSource = JWList;
            //if (App.JWListCache.Count == 0)
            initNewsList("jwzx");
            //else
            //{
            //    getJWCache();
            //}
            UmengSDK.UmengAnalytics.TrackPageStart("JWPage");
        }



        //private async void getJWCache()
        //{
        //    JWList = await getCache();
        //    JWListProgressStackPanel.Visibility = Visibility.Collapsed;
        //    continueJWGrid.Visibility = Visibility.Visible;
        //    JWListView.ItemsSource = JWList;
        //    if (Utils.getPhoneWidth() > 750)
        //        JWListView.Width = 400;
        //    //foreach (var JWListitem in App.JWListCache)
        //    //{
        //    //    JWList.Add(JWListitem);
        //    //}

        //}

        //public static async Task<ObservableCollection<JWList>> getCache()
        //{
        //    return await Task.Run(() =>
        //    {
        //        ObservableCollection<JWList> jw = App.JWListCache;
        //        return jw;
        //    });
        //}

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //App.JWListCache = JWList;
            UmengSDK.UmengAnalytics.TrackPageEnd("JWPage");
        }
        private async void initNewsList(string type, int page = 0)
        {
            JWListFailedStackPanel.Visibility = Visibility.Collapsed;
            JWListProgressStackPanel.Visibility = Visibility.Visible;
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("type", "jwzx"));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            string news = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchtitle", paramList);
            Debug.WriteLine("news->" + news);
            JWListProgressStackPanel.Visibility = Visibility.Collapsed;
            if (news != "")
            {
                JObject obj = JObject.Parse(news);
                if (Int32.Parse(obj["state"].ToString()) == 200)
                {
                    JArray NewsListArray = Utils.ReadJso(news);
                    JWListView.ItemsSource = JWList;

                    for (int i = 0; i < NewsListArray.Count; i++)
                    {
                        int failednum = 0;
                        NewsList Newsitem = new NewsList();
                        Newsitem.GetListAttribute((JObject)NewsListArray[i]);
                        if (Newsitem.Title != "")
                        {

                            //请求正文
                            List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
                            contentparamList.Add(new KeyValuePair<string, string>("type", "jwzx"));
                            contentparamList.Add(new KeyValuePair<string, string>("articleid", Newsitem.Articleid));
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

                                    //while (content.StartsWith("\r") || content.StartsWith("\n") || content.StartsWith("\t") || content.StartsWith(" ") || content.StartsWith("&nbsp;"))
                                    //    content = content.Substring(1);
                                    //while (content.StartsWith("&nbsp;"))
                                    //    content = content.Substring(6);
                                    content = content.Replace("\r", "");
                                    content = content.Replace("\t", "");
                                    content = content.Replace("\n", "");
                                    content = content.Replace("&nbsp;", "");
                                    content = content.Replace(" ", "");


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
                                    Debug.WriteLine("content->" + content);
                                    JWList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                }
                            }

                            //    string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");

                            //    JObject jwContentobj = JObject.Parse(JWContentText);
                            //    if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                            //    {
                            //        JWitem.Content = jwContentobj["data"]["content"].ToString();
                            //        while (JWitem.Content.StartsWith("\r\n "))
                            //            JWitem.Content = JWitem.Content.Substring(3);
                            //        while (JWitem.Content.StartsWith("\r\n"))
                            //            JWitem.Content = JWitem.Content.Substring(2);
                            //        while (JWitem.Content.StartsWith("\n\t"))
                            //            JWitem.Content = JWitem.Content.Substring(2);
                            //        while (JWitem.Content.StartsWith("\n"))
                            //            JWitem.Content = JWitem.Content.Substring(1);
                            //    }
                            //    else
                            //    {
                            //        JWitem.Content = "";
                            //        failednum++;
                            //    }
                            //}
                            //else
                            //{
                            //    failednum++;
                            //    if (failednum < 2)
                            //    {
                            //        jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
                            //        Debug.WriteLine("jwContent->" + jwContent);
                            //        if (jwContent != "")
                            //        {
                            //            string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");
                            //            JObject jwContentobj = JObject.Parse(JWContentText);
                            //            if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                            //            {
                            //                JWitem.Content = jwContentobj["data"]["content"].ToString();
                            //                while (JWitem.Content.StartsWith("\r\n "))
                            //                    JWitem.Content = JWitem.Content.Substring(3);
                            //                while (JWitem.Content.StartsWith("\r\n"))
                            //                    JWitem.Content = JWitem.Content.Substring(2);
                            //                while (JWitem.Content.StartsWith("\n\t"))
                            //                    JWitem.Content = JWitem.Content.Substring(2);
                            //                while (JWitem.Content.StartsWith("\n"))
                            //                    JWitem.Content = JWitem.Content.Substring(1);
                            //            }
                            //            else
                            //            {
                            //                JWitem.Content = "";
                            //                failednum++;
                            //            }
                            //        }
                            //    }
                            //}
                            //setOpacity();
                        }
                    }
                    JWListView.ItemsSource = JWList;
                    continueJWGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    JWListFailedStackPanel.Visibility = Visibility.Visible;
                    continueJWGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                JWListFailedStackPanel.Visibility = Visibility.Visible;
                continueJWGrid.Visibility = Visibility.Collapsed;
            }
        }

        public Frame JWFrame { get { return this.frame; } }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Page_Loaded;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
        }

        private void continueNewsGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            page++;
            initNewsList("jwzx", page);
            continueJWGrid.Visibility = Visibility.Collapsed;

        }

        private void NewsListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            initNewsList("jwzx");
        }

        private void NewsRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            page = 0;
            JWList.Clear();
            continueJWGrid.Visibility = Visibility.Collapsed;
            initNewsList("jwzx");
        }

        private void NewsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewsList newsItem = new NewsList(((NewsList)e.ClickedItem).ID, ((NewsList)e.ClickedItem).Articleid, ((NewsList)e.ClickedItem).Title, ((NewsList)e.ClickedItem).Head, ((NewsList)e.ClickedItem).Date, ((NewsList)e.ClickedItem).Read, ((NewsList)e.ClickedItem).Content == null ? "加载中..." : ((NewsList)e.ClickedItem).Content, ((NewsList)e.ClickedItem).Content_all == null ? "加载中..." : ((NewsList)e.ClickedItem).Content_all);

            Debug.WriteLine("NewsListgrid.Width" + NewsListgrid.Width);
            if (NewsListgrid.Width == 400)
            {
                //JWBackAppBarButton.Visibility = Visibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                NewsRefreshAppBarButton.Visibility = Visibility.Visible;
            }
            else
            {
                //JWBackAppBarButton.Visibility = Visibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                NewsRefreshAppBarButton.Visibility = Visibility.Collapsed;
            }
            JWFrame.Visibility = Visibility.Visible;
            if (((NewsList)e.ClickedItem).Content_all != "")
            {
                JObject newsContentobj = JObject.Parse(((NewsList)e.ClickedItem).Content_all);
                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                {
                    JArray AnnexListArray = Utils.ReadJso(newsContentobj["data"].ToString(), "annex");
                    if (AnnexListArray != null)
                    {
                        ObservableCollection<NewsContentList.Annex> annexList = new ObservableCollection<NewsContentList.Annex>();
                        for (int i = 0; i < AnnexListArray.Count; i++)
                        {
                            NewsContentList.Annex annex = new NewsContentList.Annex();
                            annex.GetAttribute((JObject)AnnexListArray[i]);
                            if (annex.name != "")
                            {
                                annexList.Add(new NewsContentList.Annex { name = annex.name, address = annex.address });
                                DownloadAppBarButton.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                DownloadAppBarButton.Visibility = Visibility.Collapsed;
                                break;
                            }
                        }
                        AnnexListView.ItemsSource = annexList;
                    }
                    else
                        DownloadAppBarButton.Visibility = Visibility.Collapsed;
                }
            }
            this.JWFrame.Navigate(typeof(NewsContentPage), newsItem);
        }

        private void JWBackAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //if (JWFrame == null)
            //    return;
            //if (JWFrame.CanGoBack)
            //{
            //    JWFrame.GoBack();
            //}
            //JWBackAppBarButton.Visibility = Visibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            JWFrame.Visibility = Visibility.Collapsed;
            NewsRefreshAppBarButton.Visibility = Visibility.Visible;
            JWListView.SelectedIndex = -1;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //if (JWFrame == null)
            //    return;
            //if (JWFrame.CanGoBack)
            //{
            //    JWFrame.GoBack();
            //}
            //JWBackAppBarButton.Visibility = Visibility.Collapsed;
            e.Handled = true;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            DownloadAppBarButton.Visibility = Visibility.Collapsed;
            JWFrame.Visibility = Visibility.Collapsed;
            NewsRefreshAppBarButton.Visibility = Visibility.Visible;
            JWListView.SelectedIndex = -1;
        }


        private void DownloadAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AnnexListView.SelectedIndex = -1;
            DownloadFlyout.ShowAt(newsGrid);
        }

        private async void AnnexListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            string uri = await Util.NetWork.getHttpWebRequest((((NewsContentList.Annex)e.ClickedItem).address.ToString()), PostORGet: 1, fulluri: true);
            Debug.WriteLine(uri);
            uri = uri.Replace("\"", "");
            uri = uri.Replace("\\", "");
            Debug.WriteLine(uri);
            //uri = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/jwzxnews/10947.xlsx";
            bool success = await Launcher.LaunchUriAsync(new Uri(uri));
        }

        private void NewsListpr_RefreshInvoked(DependencyObject sender, object args)
        {
            page = 0;
            JWList.Clear();
            continueJWGrid.Visibility = Visibility.Collapsed;
            initNewsList("jwzx");
        }

        private void NewsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = "";
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    break;
                case 1:
                    type = "xwgg";
                    break;
                case 2:
                    type = "cyxw ";
                    break;
                case 3:
                    type = "xsjz ";
                    break;
            }
            initNewsList(type);
        }
    }
}
