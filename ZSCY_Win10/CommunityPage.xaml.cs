using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Pages;
using ZSCY_Win10.Pages.CommunityPages;
using ZSCY_Win10.Service;
using ZSCY_Win10.Util;
using ZSCY_Win10.ViewModels.Community;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CommunityPage : Page
    {
        private int page = 0;
        int[] pagestatus = new int[] { 0, 0, 0, 0 };
        CommunityViewModel ViewModel { get; set; }
        public CommunityPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (CommunityFrame.Visibility == Visibility.Visible)
                {
                    //JWBackAppBarButton.Visibility = Visibility.Visible;
                    SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    CommunityRefreshAppBarButton.Visibility = Visibility.Collapsed;
                    //ConmunityMyAppBarButton.Visibility = Visibility.Collapsed;
                }
                if (e.NewSize.Width > 000 && e.NewSize.Width < 850)
                {
                    //if (CommunityListView.SelectedIndex != -1)
                    //{
                    //    SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                    //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    //    HubSectionKBTitle.Text = CommunityContentTitleTextBlock.Text;
                    //}

                }
                if (!App.showpane)
                {
                    CommunityTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    CommunityTitleGrid.Margin = new Thickness(0);
                }
                if (e.NewSize.Width > 800)
                {
                    //SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    HubSectionKBTitle.Text = "社区";
                    CommunityRefreshAppBarButton.Visibility = Visibility.Visible;
                    //ConmunityMyAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
            ViewModel = new CommunityViewModel();
            //CommunityFrame.Visibility = Visibility.Visible;
            //CommunityFrame.Navigate(typeof(CommunityContentPage));
        }

        public Frame CommunityFrame { get { return this.cframe; } }
        //public Frame MyFrame { get { return this.Myframe; } }


        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            CommunityFrame.Visibility = Visibility.Collapsed;
            CommunityRefreshAppBarButton.Visibility = Visibility.Visible;
            //ConmunityMyAppBarButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 下拉刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CommunityListpr_RefreshInvoked(DependencyObject sender, object args)
        {
            page = 0;
            string type = "";
            pagestatus[CommunityPivot.SelectedIndex]++;
            switch (CommunityPivot.SelectedIndex)
            {
                case 0:
                    type = "RMDT";
                    //RMDTList.Clear();
                    continueCommunityRMDTGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "BBDD";
                    //BBDDList.Clear();
                    continueCommunityBBDDGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "GFZX ";
                    //GFZXList.Clear();
                    continueCommunityGFZXGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            //init
        }

        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommunityRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            page = 0;
            string type = "";
            pagestatus[CommunityPivot.SelectedIndex]++;
            switch (CommunityPivot.SelectedIndex)
            {
                case 0:
                    type = "RMDT";
                    //RMDTList.Clear();
                    continueCommunityRMDTGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "BBDD";
                    //BBDDList.Clear();
                    continueCommunityBBDDGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "GFZX ";
                    //GFZXList.Clear();
                    continueCommunityGFZXGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            //init
        }

        /// <summary>
        /// 继续加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continueCommunityGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            page++;
            string type = "";
            switch (CommunityPivot.SelectedIndex)
            {
                case 0:
                    type = "RMDT";
                    continueCommunityRMDTGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "BBDD";
                    continueCommunityBBDDGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "GFZX ";
                    continueCommunityGFZXGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            //init
        }

        /// <summary>
        /// Pivot切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommunityPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = "";
            switch (CommunityPivot.SelectedIndex)
            {
                case 0:
                    type = "RMDT";
                    break;
                case 1:
                    type = "BBDD";
                    break;
                case 2:
                    type = "GFZX ";
                    break;
            }
            if (pagestatus[CommunityPivot.SelectedIndex] == 0)
            {
                pagestatus[CommunityPivot.SelectedIndex]++;
                //init
            }
        }

        /// <summary>
        /// 加载失败重新加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CommunityListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string type = "";
            switch (CommunityPivot.SelectedIndex)
            {
                case 0:
                    type = "RMDT";
                    break;
                case 1:
                    type = "BBDD";
                    break;
                case 2:
                    type = "GFZX ";
                    break;
            }
            //init
        }

        private void CommunityAddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            CommunityFrame.Visibility = Visibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            CommunityFrame.Navigate(typeof(CommunityAddPage));

        }

        private void BBDDListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.cframe.Navigate(typeof(CommunityContentPage), e.ClickedItem, new DrillInNavigationTransitionInfo());
            CommunityFrame.Visibility = Visibility.Visible;
        }

        private void RMDTListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.cframe.Navigate(typeof(CommunityContentPage), e.ClickedItem, new DrillInNavigationTransitionInfo());
            CommunityFrame.Visibility = Visibility.Visible;
        }

        private async void liskButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;
            string num_id = b.TabIndex.ToString();
            Debug.WriteLine(num_id);
            Debug.WriteLine("id " + num_id.Substring(2));

            string like_num = "";
            if (int.Parse(num_id[0].ToString()) < 5) //hot
            {
                HotFeed hotfeed = ViewModel.HotFeeds.First(p => p.article_id.Equals(num_id.Substring(2)));
                if (hotfeed.is_my_Like == "true" || hotfeed.is_my_Like == "True")
                {
                    like_num = await CommunityFeedsService.setPraise(hotfeed.type_id, num_id.Substring(2), false);
                    if (like_num != "")
                    {
                        hotfeed.like_num = like_num;
                        hotfeed.is_my_Like = "false";
                    }
                }
                else
                {
                    like_num = await CommunityFeedsService.setPraise(hotfeed.type_id, num_id.Substring(2), true);
                    if (like_num != "")
                    {
                        hotfeed.like_num = like_num;
                        hotfeed.is_my_Like = "true";
                    }
                }
            }
            else if (num_id[0] == '5') //bbdd
            {
                BBDDFeed bbddfeed = ViewModel.BBDD.First(p => p.id.Equals(num_id.Substring(2)));
                if (bbddfeed.is_my_like == "true" || bbddfeed.is_my_like == "True")
                {
                    like_num = await CommunityFeedsService.setPraise(bbddfeed.type_id, num_id.Substring(2), false);
                    if (like_num != "")
                    {
                        bbddfeed.like_num = like_num;
                        bbddfeed.like_num = "false";
                    }
                }
                else
                {
                    like_num = await CommunityFeedsService.setPraise(bbddfeed.type_id, num_id.Substring(2), true);
                    if (like_num != "")
                    {
                        bbddfeed.like_num = like_num;
                        bbddfeed.like_num = "true";
                        //OnPropertyChanged("Vis");
                    }
                }
            }
        }
        //private void ConmunityMyAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    Myframe.Visibility = Visibility.Visible;
        //    SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        //    Myframe.Navigate(typeof(CommunityMyPage));
        //}
    }
}
