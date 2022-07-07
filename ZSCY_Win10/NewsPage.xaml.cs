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
            initNewsList();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            JWListView.ItemsSource = JWList;
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //App.JWListCache = JWList;
        }

        private async void initNewsList(int page = 1)
        {
            int[] temp = pagestatus;
            JWListFailedStackPanel.Visibility = Visibility.Collapsed;
            JWListProgressStackPanel.Visibility = Visibility.Visible;

            Dictionary<string, string> query = new Dictionary<string, string>();
            query.Add("page", page.ToString());
            JObject news = await Requests.Send("magipoke-jwzx/jwNews/list", query: query);
            Debug.WriteLine("news->" + news);

            JWListProgressStackPanel.Visibility = Visibility.Collapsed;

            if (news != null)
            {
                if (Int32.Parse(news["status"].ToString()) == 200)
                {
                    JArray NewsListArray = (JArray)news["data"];
                    //JWListView.ItemsSource = JWList;

                    for (int i = 0; i < NewsListArray.Count; i++)
                    {
                        int failednum = 0;
                        NewsList Newsitem = new NewsList();
                        Newsitem.GetListAttribute((JObject)NewsListArray[i]);
                        if (Newsitem.Title != "")
                        {
                            //请求正文
                            Dictionary<string, string> contentQuery = new Dictionary<string, string>();
                            contentQuery.Add("id", Newsitem.ID.ToString());
                            JObject newsContent = await Requests.Send("magipoke-jwzx/jwNews/content", query: contentQuery);
                            //Debug.WriteLine("newsContent->" + newsContent);
                            if (newsContent != null)
                            {
                                if (Int32.Parse(newsContent["status"].ToString()) == 200)
                                {
                                    string content = newsContent["data"]["content"].ToString();
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

                                    Debug.WriteLine("content->" + content);
                                    JWList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent.ToString(), ID = Newsitem.ID });
                                }
                            }
                        }
                    }
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
            continueJWGrid.Visibility = Visibility.Collapsed;
            initNewsList(page);
        }

        /// <summary>
        /// 加载失败重新加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            initNewsList();
        }

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewsRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            page = 0;
            JWList.Clear();
            continueJWGrid.Visibility = Visibility.Collapsed;
            initNewsList();
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
                if (Int32.Parse(newsContentobj["status"].ToString()) == 200)
                {
                    if (newsContentobj["data"]["files"].HasValues)
                    {
                        JArray AnnexListArray = (JArray)newsContentobj["data"]["files"];
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

                                annexList.Add(new NewsContentList.Annex { name = annex.name, fileId = annex.fileId, Anneximg = Anneximg });
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
                Dictionary<string, string> fileQuery = new Dictionary<string, string>();
                fileQuery.Add("id", ((NewsContentList.Annex)e.ClickedItem).fileId);
                string uri = (await Util.Requests.Send("magipoke-jwzx/jwNews/file", query: fileQuery)).ToString();
                Debug.WriteLine(uri);
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
            page = 1;
            JWList.Clear();
            continueJWGrid.Visibility = Visibility.Collapsed;
            initNewsList();
        }
    }
}