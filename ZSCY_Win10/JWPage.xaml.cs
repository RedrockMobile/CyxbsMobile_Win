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
    public sealed partial class JWPage : Page
    {
        private int page = 1;
        ObservableCollection<JWList> JWList = new ObservableCollection<JWList>();
        public JWPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000 && e.NewSize.Width < 750)
                {
                    if (JWListView.SelectedIndex != -1)
                    {
                        JWBackAppBarButton.Visibility = Visibility.Visible;
                        JWRefreshAppBarButton.Visibility = Visibility.Collapsed;
                    }
                    JWListView.Width = e.NewSize.Width;
                }
                if (e.NewSize.Width > 750)
                {
                    JWBackAppBarButton.Visibility = Visibility.Collapsed;
                    JWRefreshAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState750";
                    JWListView.Width = 400;
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
            Debug.WriteLine("Init");
        }

        protected  override void OnNavigatedTo(NavigationEventArgs e)
        {
            JWListView.ItemsSource = JWList;
            //if (App.JWListCache.Count == 0)
            initJWList();
            //else
            //{
            //    getJWCache();
            //}
            UmengSDK.UmengAnalytics.TrackPageStart("JWPage");
        }

        private async void getJWCache()
        {
            JWList = await getCache();
            JWListProgressStackPanel.Visibility = Visibility.Collapsed;
            continueJWGrid.Visibility = Visibility.Visible;
            JWListView.ItemsSource = JWList;
            if (Utils.getPhoneWidth() > 750)
                JWListView.Width = 400;
            //foreach (var JWListitem in App.JWListCache)
            //{
            //    JWList.Add(JWListitem);
            //}

        }

        public static async Task<ObservableCollection<JWList>> getCache()
        {
            return await Task.Run(() =>
            {
                ObservableCollection<JWList> jw = App.JWListCache;
                return jw;
            });
        }






        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.JWListCache = JWList;
            UmengSDK.UmengAnalytics.TrackPageEnd("JWPage");
        }
        private async void initJWList(int page = 1)
        {
            JWListFailedStackPanel.Visibility = Visibility.Collapsed;
            JWListProgressStackPanel.Visibility = Visibility.Visible;
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            string jw = await NetWork.getHttpWebRequest("api/jwNewsList", paramList);
            Debug.WriteLine("jw->" + jw);
            JWListProgressStackPanel.Visibility = Visibility.Collapsed;
            if (jw != "")
            {
                JObject obj = JObject.Parse(jw);
                if (Int32.Parse(obj["status"].ToString()) == 200)
                {
                    JArray JWListArray = Utils.ReadJso(jw);
                    JWListView.ItemsSource = JWList;

                    for (int i = 0; i < JWListArray.Count; i++)
                    {
                        int failednum = 0;
                        JWList JWitem = new JWList();
                        JWitem.GetListAttribute((JObject)JWListArray[i]);
                        List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
                        contentparamList.Add(new KeyValuePair<string, string>("id", JWitem.ID));
                        string jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
                        Debug.WriteLine("jwContent->" + jwContent);
                        if (jwContent != "")
                        {
                            string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");

                            JObject jwContentobj = JObject.Parse(JWContentText);
                            if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                            {
                                JWitem.Content = jwContentobj["data"]["content"].ToString();
                                while (JWitem.Content.StartsWith("\r\n "))
                                    JWitem.Content = JWitem.Content.Substring(3);
                                while (JWitem.Content.StartsWith("\r\n"))
                                    JWitem.Content = JWitem.Content.Substring(2);
                                while (JWitem.Content.StartsWith("\n\t"))
                                    JWitem.Content = JWitem.Content.Substring(2);
                                while (JWitem.Content.StartsWith("\n"))
                                    JWitem.Content = JWitem.Content.Substring(1);
                            }
                            else
                            {
                                JWitem.Content = "";
                                failednum++;
                            }
                        }
                        else
                        {
                            failednum++;
                            if (failednum < 2)
                            {
                                jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
                                Debug.WriteLine("jwContent->" + jwContent);
                                if (jwContent != "")
                                {
                                    string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");
                                    JObject jwContentobj = JObject.Parse(JWContentText);
                                    if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                                    {
                                        JWitem.Content = jwContentobj["data"]["content"].ToString();
                                        while (JWitem.Content.StartsWith("\r\n "))
                                            JWitem.Content = JWitem.Content.Substring(3);
                                        while (JWitem.Content.StartsWith("\r\n"))
                                            JWitem.Content = JWitem.Content.Substring(2);
                                        while (JWitem.Content.StartsWith("\n\t"))
                                            JWitem.Content = JWitem.Content.Substring(2);
                                        while (JWitem.Content.StartsWith("\n"))
                                            JWitem.Content = JWitem.Content.Substring(1);
                                    }
                                    else
                                    {
                                        JWitem.Content = "";
                                        failednum++;
                                    }
                                }
                            }
                        }
                        JWList.Add(new JWList { Title = JWitem.Title, Date = JWitem.Date, Read = JWitem.Read, Content = JWitem.Content, ID = JWitem.ID });
                        //setOpacity();
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

        private void continueJWGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            page++;
            initJWList(page);
            continueJWGrid.Visibility = Visibility.Collapsed;
        }

        private void JWListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            initJWList();
        }

        private void JWRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            page = 1;
            JWList.Clear();
            continueJWGrid.Visibility = Visibility.Collapsed;
            initJWList();
        }

        private void JWListView_ItemClick(object sender, ItemClickEventArgs e)
        {


            JWList JWItem = new JWList(((JWList)e.ClickedItem).ID, ((JWList)e.ClickedItem).Title, ((JWList)e.ClickedItem).Date, ((JWList)e.ClickedItem).Read, ((JWList)e.ClickedItem).Content == null ? "加载中..." : ((JWList)e.ClickedItem).Content);

            Debug.WriteLine("JWListgrid.Width" + JWListgrid.Width);
            if (JWListgrid.Width != null && JWListgrid.Width == 400)
            {
                JWBackAppBarButton.Visibility = Visibility.Collapsed;
                JWRefreshAppBarButton.Visibility = Visibility.Visible;
            }
            else
            {
                JWBackAppBarButton.Visibility = Visibility.Visible;
                JWRefreshAppBarButton.Visibility = Visibility.Collapsed;
            }
            JWFrame.Visibility = Visibility.Visible;
            this.JWFrame.Navigate(typeof(JWContentPage), JWItem);
        }

        private void JWBackAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //if (JWFrame == null)
            //    return;
            //if (JWFrame.CanGoBack)
            //{
            //    JWFrame.GoBack();
            //}
            JWBackAppBarButton.Visibility = Visibility.Collapsed;
            JWFrame.Visibility = Visibility.Collapsed;
            JWRefreshAppBarButton.Visibility = Visibility.Visible;
            JWListView.SelectedIndex = -1;
        }
    }
}
