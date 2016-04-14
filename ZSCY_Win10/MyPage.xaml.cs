using System;
using System.Collections.Generic;
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
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MyPage : Page
    {
        public MyPage()
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
                    if (MyFrame.Visibility == Visibility.Visible)
                    {
                        //JWBackAppBarButton.Visibility = Visibility.Visible;
                        SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
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
                if (e.NewSize.Width > 800)
                {
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    HubSectionKBTitle.Text = "个人中心";
                    //MyRefreshAppBarButton.Visibility = Visibility.Visible;
                    //ConmunityMyAppBarButton.Visibility = Visibility.Visible;
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
            ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (appSetting.Values["gender"].ToString().IndexOf("男") != (-1))
            {
                stuSexText.Text = "♂";
                stuSexText.Foreground = new SolidColorBrush(Color.FromArgb(255, 6, 140, 253));
            }
            else
                stuSexText.Text = "♀";
            stuNumText.Text = appSetting.Values["stuNum"].ToString();
            stuIntText.Text = "这是简介啦啦啦啦（凑字数用凑字数用凑字数用凑字数用凑字数用）";
            phoneNumText.Text = "13800000000";
            qqNumText.Text = "1000000000";
        }
        public Frame MyFrame { get { return this.frame; } }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            MyFrame.Visibility = Visibility.Collapsed;
            //MyRefreshAppBarButton.Visibility = Visibility.Visible;
            //ConmunityMyAppBarButton.Visibility = Visibility.Visible;
        }

    }
}
