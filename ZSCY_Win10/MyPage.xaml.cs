using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Pages.CommunityPages;
using ZSCY_Win10.Util;
using ZSCY_Win10.ViewModels.Community;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyPage : Page
    {
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        MyViewModel ViewModel;
        List<Img> clickImgList = new List<Img>();
        double myTidingsOldScrollableHeight = 0;
        double aboutMeOldScrollableHeight = 0;
        int clickImfIndex = 0;
        public MyPage()
        {
            this.InitializeComponent();
            ViewModel = new MyViewModel();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000 && e.NewSize.Width < 850)
                {
                    //if (CommunityListView.SelectedIndex != -1)
                    //{
                    //    SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                    //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    //    HubSectionKBTitle.Text = CommunityContentTitleTextBlock.Text;
                    //}
                    if (MyFrame.Visibility == Visibility.Visible || aboutMeGrid.Visibility == Visibility.Visible || myTidingsGrid.Visibility == Visibility.Visible)
                    {
                        //JWBackAppBarButton.Visibility = Visibility.Visible;
                        //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        //MyRefreshAppBarButton.Visibility = Visibility.Collapsed;
                    }
                }
                if (!App.showpane)
                {
                    MyTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    MyTitleGrid.Margin = new Thickness(0);
                }
                if (e.NewSize.Width > 800 && aboutMeGrid.Visibility == Visibility.Collapsed && myTidingsGrid.Visibility == Visibility.Collapsed)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    HubSectionKBTitle.Text = "个人中心";
                    //MyRefreshAppBarButton.Visibility = Visibility.Visible;
                    //CommunityMyAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState800";
                }
                else if (e.NewSize.Width > 800)
                {
                    HubSectionKBTitle.Text = "个人中心";
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };

            initInformation();

            if (appSetting.Values["gender"].ToString().IndexOf("男") != (-1))
            {
                stuSexText.Text = "♂";
                stuSexText.Foreground = new SolidColorBrush(Color.FromArgb(255, 6, 140, 253));
            }
            else
                stuSexText.Text = "♀";
        }

        private async void initInformation()
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("stunum", appSetting.Values["stuNum"].ToString()));
            string info = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Person/search", paramList);
            if (info != "")
            {
                JObject obj = JObject.Parse(info);
                if (Int32.Parse(obj["status"].ToString()) == 200)
                {
                    JObject objdata = JObject.Parse(obj["data"].ToString());
                    appSetting.Values["Community_people_id"] = objdata["id"].ToString();
                    stuNumText.Text = appSetting.Values["stuNum"].ToString();
                    nickNameText.Text = objdata["nickname"].ToString();
                    stuIntText.Text = objdata["introduction"].ToString();
                    phoneNumText.Text = objdata["phone"].ToString();
                    qqNumText.Text = objdata["qq"].ToString();
                    appSetting.Values["Community_nickname"] = objdata["nickname"].ToString();
                    if (objdata["photo_src"].ToString() == "")
                    {
                        mpgImageBrush.ImageSource = new BitmapImage(new Uri("ms-appdata:///Local/headimg.png"));
                    }
                    else
                    {
                        mpgImageBrush.ImageSource = new BitmapImage(new Uri(objdata["photo_src"].ToString()));
                        appSetting.Values["Community_headimg_src"] = objdata["photo_src"].ToString();
                    }
                }
            }
        }

        public Frame MyFrame { get { return this.frame; } }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (MyFrame.Visibility == Visibility.Visible)
            {
                MyFrame.Visibility = Visibility.Collapsed;
                //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            else if (CommunityItemPhotoGrid.Visibility == Visibility.Visible)
            {
                CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
                //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            else if (myTidingsGrid.Visibility == Visibility.Visible)
            {
                myTidingsGrid.Visibility = Visibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            else if (aboutMeGrid.Visibility == Visibility.Visible)
            {
                aboutMeGrid.Visibility = Visibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            //else
            //{
            //    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //    MyFrame.Visibility = Visibility.Collapsed;
            //    //MyRefreshAppBarButton.Visibility = Visibility.Visible;
            //    //CommunityMyAppBarButton.Visibility = Visibility.Visible;
            //}
        }



        private void aboutMe_Click(object sender, RoutedEventArgs e)
        {
            aboutMeGrid.Visibility = Visibility.Visible;
            ViewModel.notificationspage = 0;
            ViewModel.getNotifications();
            ViewModel.MyNotify.Clear();
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void myTidings_Click(object sender, RoutedEventArgs e)
        {
            myTidingsGrid.Visibility = Visibility.Visible;
            ViewModel.feedspage = 0;
            ViewModel.getFeeds();
            ViewModel.MyFeedlist.Clear();
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }


        private void EditAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            PeoInfo peoinfo = new PeoInfo(nickNameText.Text, stuIntText.Text, phoneNumText.Text, qqNumText.Text);
            Frame.Navigate(typeof(SetPersonInfoPage), peoinfo);
        }

        private void PhotoGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Img img = e.ClickedItem as Img;
            GridView gridView = sender as GridView;
            clickImgList = ((Img[])gridView.ItemsSource).ToList();
            clickImfIndex = clickImgList.IndexOf(img);
            CommunityItemPhotoFlipView.ItemsSource = clickImgList;
            CommunityItemPhotoFlipView.SelectedIndex = clickImfIndex;
            CommunityItemPhotoGrid.Visibility = Visibility.Visible;
            //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CommunityItemPhoto_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
            //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //backImgButton.Visibility = Visibility.Collapsed;
            //nextImgButton.Visibility = Visibility.Collapsed;
        }

        private void myTidingsScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (myTidingsScrollViewer.VerticalOffset > (myTidingsScrollViewer.ScrollableHeight - 500) && myTidingsScrollViewer.ScrollableHeight != myTidingsOldScrollableHeight)
            {
                myTidingsOldScrollableHeight = myTidingsScrollViewer.ScrollableHeight;
                Debug.WriteLine("Feeds继续加载");
                ViewModel.getFeeds();
            }
        }

        private void aboutMeScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (aboutMeScrollViewer.VerticalOffset > (aboutMeScrollViewer.ScrollableHeight - 500) && aboutMeScrollViewer.ScrollableHeight != aboutMeOldScrollableHeight)
            {
                aboutMeOldScrollableHeight = aboutMeScrollViewer.ScrollableHeight;
                Debug.WriteLine("Notifications继续加载");
                ViewModel.getNotifications();
            }
        }

        private void Aboutme_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyFrame.Visibility = Visibility.Visible;
            MyFrame.Navigate(typeof(CommunityMyContentPage), e.ClickedItem);
        }

        private void Myfeed_ItemClick(object sender, ItemClickEventArgs e)
        {
            MyFrame.Visibility = Visibility.Visible;
            MyFrame.Navigate(typeof(CommunityMyContentPage), e.ClickedItem);
        }
    }


}
