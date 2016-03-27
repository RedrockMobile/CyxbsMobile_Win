using System;
using System.Collections.Generic;
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
        public CommunityPage()
        {
            this.InitializeComponent();
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
                    if (CommunityFrame.Visibility == Visibility.Visible)
                    {
                        //JWBackAppBarButton.Visibility = Visibility.Visible;
                        SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        CommunityRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        ConmunityMyAppBarButton.Visibility = Visibility.Collapsed;
                    }
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
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    HubSectionKBTitle.Text = "社区";
                    CommunityRefreshAppBarButton.Visibility = Visibility.Visible;

                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
        }

        public Frame CommunityFrame { get { return this.frame; } }


        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            CommunityFrame.Visibility = Visibility.Collapsed;
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


    }
}
