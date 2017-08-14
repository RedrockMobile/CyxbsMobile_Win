using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
        private ObservableCollection<NewsList> JWList = new ObservableCollection<NewsList>();
        private ObservableCollection<NewsList> XWList = new ObservableCollection<NewsList>();
        private ObservableCollection<NewsList> CYList = new ObservableCollection<NewsList>();
        private ObservableCollection<NewsList> XSList = new ObservableCollection<NewsList>();
        private int[] pagestatus = new int[] { 0, 0, 0, 0 };

        public NewsPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000 && e.NewSize.Width < 800)
                {
                    //if (JWListView.SelectedIndex != -1)
                    if (NewsFrame.Visibility == Visibility.Visible)
                    {
                        //JWBackAppBarButton.Visibility = Visibility.Visible;
                        SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        NewsRefreshAppBarButton.Visibility = Visibility.Collapsed;
                    }
                    JWListView.Width = e.NewSize.Width;
                }
                if (!App.showpane)
                {
                    NewsTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    NewsTitleGrid.Margin = new Thickness(0);
                }
                if (e.NewSize.Width > 800)
                {
                    //JWBackAppBarButton.Visibility = Visibility.Collapsed;
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    NewsRefreshAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState800";
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
            XWListView.ItemsSource = XWList;
            CYListView.ItemsSource = CYList;
            XSListView.ItemsSource = XSList;

            //if (App.JWListCache.Count == 0)
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
            int[] temp = pagestatus;
            switch (type)
            {
                case "jwzx":
                    JWListFailedStackPanel.Visibility = Visibility.Collapsed;
                    JWListProgressStackPanel.Visibility = Visibility.Visible;
                    break;

                case "xwgg":
                    XWListFailedStackPanel.Visibility = Visibility.Collapsed;
                    XWListProgressStackPanel.Visibility = Visibility.Visible;
                    break;

                case "cyxw ":
                    CYListFailedStackPanel.Visibility = Visibility.Collapsed;
                    CYListProgressStackPanel.Visibility = Visibility.Visible;
                    break;

                case "xsjz ":
                    XSListFailedStackPanel.Visibility = Visibility.Collapsed;
                    XSListProgressStackPanel.Visibility = Visibility.Visible;
                    break;
            }

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("type", type));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            string news = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchtitle", paramList);
            Debug.WriteLine("news->" + news);

            switch (type)
            {
                case "jwzx":
                    JWListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;

                case "xwgg":
                    XWListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;

                case "cyxw ":
                    CYListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;

                case "xsjz ":
                    XSListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;
            }

            if (news != "")
            {
                JObject obj = JObject.Parse(news);
                if (Int32.Parse(obj["state"].ToString()) == 200)
                {
                    JArray NewsListArray = Utils.ReadJso(news);
                    //JWListView.ItemsSource = JWList;

                    for (int i = 0; i < NewsListArray.Count; i++)
                    {
                        int failednum = 0;
                        NewsList Newsitem = new NewsList();
                        Newsitem.GetListAttribute((JObject)NewsListArray[i]);
                        if (Newsitem.Title != "")
                        {
                            //请求正文
                            List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
                            contentparamList.Add(new KeyValuePair<string, string>("type", type));
                            contentparamList.Add(new KeyValuePair<string, string>("articleid", Newsitem.Articleid));
                            string newsContent = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchcontent", contentparamList);
                            //Debug.WriteLine("newsContent->" + newsContent);
                            if (temp[NewsPivot.SelectedIndex] != pagestatus[NewsPivot.SelectedIndex])
                            {
                                Debug.WriteLine("newsContent->在此退出");
                                return;
                            }
                            if (newsContent != "")
                            {
                                JObject newsContentobj = JObject.Parse(newsContent);
                                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                                {
                                    string content = (JObject.Parse(newsContentobj["data"].ToString()))["content"].ToString();
                                    string content_all = content;
                                    Debug.WriteLine("content->" + content);
                                    try
                                    {
                                        while (content.IndexOf("<") != -1)
                                        {
                                            content = content.Remove(content.IndexOf("<"), content.IndexOf(">") - content.IndexOf("<") + 1);
                                        }
                                    }
                                    catch (Exception) { }

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
                                    content = content.Replace("（见附件）", "见附件");
                                    content = content.Replace("MicrosoftInternetExplorer4", "");
                                    content = content.Replace("Normal07.8磅02falsefalsefalse", "");

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
                                    switch (type)
                                    {
                                        case "jwzx":
                                            JWList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;

                                        case "xwgg":
                                            XWList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;

                                        case "cyxw ":
                                            CYList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;

                                        case "xsjz ":
                                            XSList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    //JWListView.ItemsSource = JWList;
                    switch (type)
                    {
                        case "jwzx":
                            continueJWGrid.Visibility = Visibility.Visible;
                            break;

                        case "xwgg":
                            continueXWGrid.Visibility = Visibility.Visible;
                            break;

                        case "cyxw ":
                            continueCYGrid.Visibility = Visibility.Visible;
                            break;

                        case "xsjz ":
                            continueXSGrid.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case "jwzx":
                            JWListFailedStackPanel.Visibility = Visibility.Visible;
                            continueJWGrid.Visibility = Visibility.Collapsed;
                            break;

                        case "xwgg":
                            XWListFailedStackPanel.Visibility = Visibility.Visible;
                            continueXWGrid.Visibility = Visibility.Collapsed;
                            break;

                        case "cyxw ":
                            CYListFailedStackPanel.Visibility = Visibility.Visible;
                            continueCYGrid.Visibility = Visibility.Collapsed;
                            break;

                        case "xsjz ":
                            XSListFailedStackPanel.Visibility = Visibility.Visible;
                            continueXSGrid.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
            else
            {
                switch (type)
                {
                    case "jwzx":
                        JWListFailedStackPanel.Visibility = Visibility.Visible;
                        continueJWGrid.Visibility = Visibility.Collapsed;
                        break;

                    case "xwgg":
                        XWListFailedStackPanel.Visibility = Visibility.Visible;
                        continueXWGrid.Visibility = Visibility.Collapsed;
                        break;

                    case "cyxw ":
                        CYListFailedStackPanel.Visibility = Visibility.Visible;
                        continueCYGrid.Visibility = Visibility.Collapsed;
                        break;

                    case "xsjz ":
                        XSListFailedStackPanel.Visibility = Visibility.Visible;
                        continueXSGrid.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        public Frame NewsFrame { get { return this.frame; } }

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

        /// <summary>
        /// 继续加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continueNewsGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            page++;
            string type = "";
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    continueJWGrid.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    type = "xwgg";
                    continueXWGrid.Visibility = Visibility.Collapsed;
                    break;

                case 2:
                    type = "cyxw ";
                    continueCYGrid.Visibility = Visibility.Collapsed;
                    break;

                case 3:
                    type = "xsjz ";
                    continueXSGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            initNewsList(type, page);
        }

        /// <summary>
        /// 加载失败重新加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
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

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            page = 0;
            string type = "";
            pagestatus[NewsPivot.SelectedIndex]++;
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    JWList.Clear();
                    continueJWGrid.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    type = "xwgg";
                    XWList.Clear();
                    continueXWGrid.Visibility = Visibility.Collapsed;
                    break;

                case 2:
                    type = "cyxw ";
                    CYList.Clear();
                    continueCYGrid.Visibility = Visibility.Collapsed;
                    break;

                case 3:
                    type = "xsjz ";
                    XSList.Clear();
                    continueXSGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            initNewsList(type);
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
            NewsFrame.Visibility = Visibility.Visible;
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
                                Uri Anneximg;
                                if (annex.name.IndexOf(".zip") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_zip.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".rar") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_rar.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".pdf") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_pdf.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".doc") != -1 || annex.name.IndexOf(".docx") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_doc.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".xls") != -1 || annex.name.IndexOf(".xlsx") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_xls.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".ppt") != -1 || annex.name.IndexOf(".pptx") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_ppt.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".jpg") != -1 || annex.name.IndexOf(".png") != -1 || annex.name.IndexOf(".gif") != -1 || annex.name.IndexOf(".bmp") != -1 || annex.name.IndexOf(".jpeg") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_image.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".mp4") != -1 || annex.name.IndexOf(".rmvb") != -1 || annex.name.IndexOf(".avi") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_video.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".mp3") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_music.png", UriKind.Absolute);
                                }
                                else if (annex.name.IndexOf(".apk") != -1)
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_apk.png", UriKind.Absolute);
                                }
                                else
                                {
                                    Anneximg = new Uri("ms-appx:///Assets/Annex_img/Annex_other.png", UriKind.Absolute);
                                }

                                annexList.Add(new NewsContentList.Annex { name = annex.name, address = annex.address, Anneximg = Anneximg });
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
            this.NewsFrame.Navigate(typeof(NewsContentPage), newsItem);
        }

        private void JWBackAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //if (NewsFrame == null)
            //    return;
            //if (NewsFrame.CanGoBack)
            //{
            //    NewsFrame.GoBack();
            //}
            //JWBackAppBarButton.Visibility = Visibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            NewsFrame.Visibility = Visibility.Collapsed;
            NewsRefreshAppBarButton.Visibility = Visibility.Visible;
            JWListView.SelectedIndex = -1;
            XWListView.SelectedIndex = -1;
            CYListView.SelectedIndex = -1;
            XSListView.SelectedIndex = -1;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //if (NewsFrame == null)
            //    return;
            //if (NewsFrame.CanGoBack)
            //{
            //    NewsFrame.GoBack();
            //}
            //JWBackAppBarButton.Visibility = Visibility.Collapsed;
            e.Handled = true;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            DownloadAppBarButton.Visibility = Visibility.Collapsed;
            NewsFrame.Visibility = Visibility.Collapsed;
            NewsRefreshAppBarButton.Visibility = Visibility.Visible;
            JWListView.SelectedIndex = -1;
            XWListView.SelectedIndex = -1;
            CYListView.SelectedIndex = -1;
            XSListView.SelectedIndex = -1;
        }

        private void DownloadAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            AnnexListView.SelectedIndex = -1;
            DownloadFlyout.ShowAt(newsGrid);
        }

        private async void AnnexListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                string uri = ((NewsContentList.Annex)e.ClickedItem).address;
                if (NewsPivot.SelectedIndex == 0)
                {
                    uri = await Util.NetWork.getHttpWebRequest((((NewsContentList.Annex)e.ClickedItem).address.ToString()), PostORGet: 1, fulluri: true);
                    Debug.WriteLine(uri);
                    uri = uri.Replace("\"", "");
                    uri = uri.Replace("\\", "");
                    Debug.WriteLine(uri);
                    //uri = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/jwzxnews/10947.xlsx";
                }
                await Launcher.LaunchUriAsync(new Uri(uri));
            }
            catch (Exception)
            {
                await new MessageDialog("附件地址异常").ShowAsync();
            }
        }

        /// <summary>
        /// 下拉刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NewsListpr_RefreshInvoked(DependencyObject sender, object args)
        {
            page = 0;
            string type = "";
            pagestatus[NewsPivot.SelectedIndex]++;
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    JWList.Clear();
                    continueJWGrid.Visibility = Visibility.Collapsed;
                    break;

                case 1:
                    type = "xwgg";
                    XWList.Clear();
                    continueXWGrid.Visibility = Visibility.Collapsed;
                    break;

                case 2:
                    type = "cyxw ";
                    CYList.Clear();
                    continueCYGrid.Visibility = Visibility.Collapsed;
                    break;

                case 3:
                    type = "xsjz ";
                    XSList.Clear();
                    continueXSGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            initNewsList(type);
        }

        /// <summary>
        /// Pivot切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (pagestatus[NewsPivot.SelectedIndex] == 0)
            {
                pagestatus[NewsPivot.SelectedIndex]++;
                initNewsList(type);
            }
        }
    }
}