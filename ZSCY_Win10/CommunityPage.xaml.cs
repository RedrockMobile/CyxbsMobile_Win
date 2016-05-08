using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Common;
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
        double hotOldScrollableHeight = 0;
        double BBDDOldScrollableHeight = 0;
        List<Img> clickImgList = new List<Img>();
        int clickImfIndex = 0;

        public CommunityPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (CommunityFrame.Visibility == Visibility.Visible && e.NewSize.Width <= 800)
                {
                    //JWBackAppBarButton.Visibility = Visibility.Visible;
                    SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    CommunityRefreshAppBarButton.Visibility = Visibility.Collapsed;
                    //CommunityMyAppBarButton.Visibility = Visibility.Collapsed;
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
                    //CommunityMyAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
            //CommunityFrame.Visibility = Visibility.Visible;
            //CommunityFrame.Navigate(typeof(CommunityContentPage));
            //RMDTListView.ContainerContentChanging += RMDTListView_ContainerContentChanging;
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.NavigationCacheMode = NavigationCacheMode.Required;
            if (e.NavigationMode == NavigationMode.New)
            {
                ViewModel = new CommunityViewModel();
            }
            else
            {
                ViewModel = App.ViewModel;
                CommunityPivot.SelectedIndex = App.CommunityPivotState;
                await Task.Delay(1);
                BBDDScrollViewer.ChangeView(0, App.CommunityScrollViewerOffset, 1);
            }
        }

        public Frame CommunityFrame { get { return this.cframe; } }
        //public Frame MyFrame { get { return this.Myframe; } }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                if (CommunityItemPhotoGrid.Visibility == Visibility.Visible)
                {
                    CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    e.Handled = true;
                }
                else
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    CommunityFrame.Visibility = Visibility.Collapsed;
                    CommunityRefreshAppBarButton.Visibility = Visibility.Visible;
                    //CommunityMyAppBarButton.Visibility = Visibility.Visible;
                }
            }
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
                    ViewModel.gethot(0, 15, 5, true);
                    //RMDTList.Clear();
                    //continueCommunityRMDTGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "BBDD";
                    ViewModel.getbbdd(1, 15, 5, true);
                    //BBDDList.Clear();
                    //continueCommunityBBDDGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "GFZX ";
                    //GFZXList.Clear();
                    //continueCommunityGFZXGrid.Visibility = Visibility.Collapsed;
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
                    ViewModel.gethot(0, 15, 5, true);
                    //RMDTList.Clear();
                    //continueCommunityRMDTGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "BBDD";
                    ViewModel.getbbdd(1, 15, 5, true);
                    //BBDDList.Clear();
                    //continueCommunityBBDDGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "GFZX ";
                    //GFZXList.Clear();
                    //continueCommunityGFZXGrid.Visibility = Visibility.Collapsed;
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
                    //continueCommunityRMDTGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "BBDD";
                    //continueCommunityBBDDGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "GFZX ";
                    //continueCommunityGFZXGrid.Visibility = Visibility.Collapsed;
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
            if (CommunityTitleGrid.Width != 400)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }

        private void RMDTListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.cframe.Navigate(typeof(CommunityContentPage), e.ClickedItem, new DrillInNavigationTransitionInfo());
            CommunityFrame.Visibility = Visibility.Visible;
            if (CommunityTitleGrid.Width != 400)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            }
        }

        private async void liskButton_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;

            string num_id = b.TabIndex.ToString();
            Debug.WriteLine(num_id);
            Debug.WriteLine("id " + num_id.Substring(2));
            string like_num = "";
            try
            {
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
                            if (ViewModel.BBDD.Count(p => p.id.Equals(num_id.Substring(2))) != 0)
                            {
                                BBDDFeed s = ViewModel.BBDD.First(p => p.id.Equals(num_id.Substring(2)));
                                if (s != null)
                                {
                                    s.like_num = like_num;
                                    s.is_my_like = "false";
                                }
                            }
                        }
                    }
                    else
                    {
                        like_num = await CommunityFeedsService.setPraise(hotfeed.type_id, num_id.Substring(2), true);
                        if (like_num != "")
                        {
                            hotfeed.like_num = like_num;
                            hotfeed.is_my_Like = "true";
                            if (ViewModel.BBDD.Count(p => p.id.Equals(num_id.Substring(2))) != 0)
                            {
                                BBDDFeed s = ViewModel.BBDD.First(p => p.id.Equals(num_id.Substring(2)));
                                if (s != null)
                                {
                                    s.like_num = like_num;
                                    s.is_my_like = "true";
                                }
                            }
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
                            bbddfeed.is_my_like = "false";
                            if (ViewModel.HotFeeds.Count(p => p.article_id.Equals(num_id.Substring(2))) != 0)
                            {
                                HotFeed h = ViewModel.HotFeeds.First(p => p.article_id.Equals(num_id.Substring(2)));
                                if (h != null)
                                {
                                    h.like_num = like_num;
                                    h.is_my_Like = "false";
                                }
                            }
                        }
                    }
                    else
                    {
                        like_num = await CommunityFeedsService.setPraise(bbddfeed.type_id, num_id.Substring(2), true);
                        if (like_num != "")
                        {
                            bbddfeed.like_num = like_num;
                            bbddfeed.is_my_like = "true";
                            if (ViewModel.HotFeeds.Count(p => p.article_id.Equals(num_id.Substring(2))) != 0)
                            {
                                HotFeed h = ViewModel.HotFeeds.First(p => p.article_id.Equals(num_id.Substring(2)));
                                if (h != null)
                                {
                                    h.like_num = like_num;
                                    h.is_my_Like = "true";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("点赞异常");
            }

        }

        private async void RMDTScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (RMDTScrollViewer.VerticalOffset > (RMDTScrollViewer.ScrollableHeight - 500) && RMDTScrollViewer.ScrollableHeight != hotOldScrollableHeight)
            {
                hotOldScrollableHeight = RMDTScrollViewer.ScrollableHeight;
                Debug.WriteLine("RMDT继续加载");
                ViewModel.gethot(0, 15, 5);
            }
            //Debug.WriteLine(RMDTScrollViewer.VerticalOffset);
        }

        private void BBDDScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (BBDDScrollViewer.VerticalOffset > (BBDDScrollViewer.ScrollableHeight - 500) && BBDDScrollViewer.ScrollableHeight != BBDDOldScrollableHeight)
            {
                BBDDOldScrollableHeight = BBDDScrollViewer.ScrollableHeight;
                Debug.WriteLine("BBDD继续加载");
                ViewModel.getbbdd(1, 15, 5);
            }
        }

        private void PhotoGrid_ItemClick(object sender, ItemClickEventArgs e)
        {
            Img img = e.ClickedItem as Img;
            GridView gridView = sender as GridView;
            clickImgList = ((Img[])gridView.ItemsSource).ToList();
            clickImfIndex = clickImgList.IndexOf(img);
            //if (clickImgList.Count > 0 && clickImfIndex < clickImgList.Count - 1)
            //{
            //    nextImgButton.Visibility = Visibility.Visible;
            //}
            //if (clickImgList.Count > 0 && clickImfIndex > 0)
            //{
            //    backImgButton.Visibility = Visibility.Visible;
            //}
            CommunityItemPhotoFlipView.ItemsSource = clickImgList;
            CommunityItemPhotoFlipView.SelectedIndex = clickImfIndex;
            //CommunityItemPhotoImage.Source = new BitmapImage(new Uri(img.ImgSrc));
            CommunityItemPhotoGrid.Visibility = Visibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void CommunityItemPhoto_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CommunityItemPhotoGrid.Visibility = Visibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //backImgButton.Visibility = Visibility.Collapsed;
            //nextImgButton.Visibility = Visibility.Collapsed;
        }



        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            BBDDFeed b = ((StackPanel)sender).DataContext as BBDDFeed;
            App.ViewModel = ViewModel;
            App.CommunityPivotState = CommunityPivot.SelectedIndex;
            App.CommunityScrollViewerOffset = BBDDScrollViewer.VerticalOffset;
            Frame.Navigate(typeof(CommunityPersonInfo), b.stunum);
        }
    }
}
