using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        private ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        private MyViewModel ViewModel;
        private List<Img> clickImgList = new List<Img>();
        private double myTidingsOldScrollableHeight = 0;
        private double aboutMeOldScrollableHeight = 0;
        private int clickImfIndex = 0;

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
        }

        public Frame MyFrame { get { return this.frame; } }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (MyFrame.Visibility == Visibility.Visible)
            {
                if (!App.isPerInfoContentImgShow)
                {
                    MyFrame.Visibility = Visibility.Collapsed;
                    //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
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
            e.Handled = true;
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
            PeoInfo peoinfo = new PeoInfo(ViewModel.Info.nickname, ViewModel.Info.introduction, ViewModel.Info.phone, ViewModel.Info.qq);
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

        private void CommunityItemPhotoImage_Holding(object sender, HoldingRoutedEventArgs e)
        {
            //Debug.WriteLine("Holding");
            //savePic();
        }

        private void CommunityItemPhotoImage_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Debug.WriteLine("RightTapped");
            savePic();
        }

        private async void savePic()
        {
            var dig = new MessageDialog("是否保存此图片");
            var btnOk = new UICommand("是");
            dig.Commands.Add(btnOk);
            var btnCancel = new UICommand("否");
            dig.Commands.Add(btnCancel);
            var result = await dig.ShowAsync();
            if (null != result && result.Label == "是")
            {
                Debug.WriteLine("保存图片");
                bool saveImg = await NetWork.downloadFile(((Img)CommunityItemPhotoFlipView.SelectedItem).ImgSrc, "picture", ((Img)CommunityItemPhotoFlipView.SelectedItem).ImgSrc.Replace("http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/", ""));
                if (saveImg)
                {
                    Utils.Toast("图片已保存到 \"保存的图片\"", "SavedPictures");
                }
                else
                {
                    Utils.Toast("图片保存遇到了麻烦");
                }
            }
            else if (null != result && result.Label == "否")
            {
            }
        }

        private void CommunityItemPhotoImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            DependencyObject x = VisualTreeHelper.GetParent(sender as Image);
            Grid g = x as Grid;
            var z = g.Children[0];
            ProgressRing p = z as ProgressRing;
            p.IsActive = false;
        }
    }
}