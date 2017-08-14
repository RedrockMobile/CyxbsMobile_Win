using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Pages;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FirstPage : Page
    {
        private int click_index;

        private ZSCY_Win10.ViewModels.FirstPageViewModel viewmodel = new ZSCY_Win10.ViewModels.FirstPageViewModel();

        public static FirstPage firstpage;

        public FirstPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = viewmodel;
            firstpage = this;
            go_back_sb.Completed += Go_back_sb_Completed;
            this.SizeChanged += FirstPage_SizeChanged;
            Window.Current.SizeChanged += Current_SizeChanged;
            second_frame.Navigated += Second_frame_Navigated;
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (second_frame.Content == null)
            {
                if (e.Size.Width != second_frame.Width)
                {
                    second_frame_trans.X = (e.Size.Width - 30) / 2;
                }
                else
                {
                    second_frame_trans.X = this.Width;
                }
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.NavigationMode == NavigationMode.New)
            {
                viewmodel.Page_Height = this.Height;
                viewmodel.Page_Width = this.ActualWidth;
            }

        }

        public void Second_Page_Forwoard() //页面前进方法
        {
            (go_forward_sb.Children[0] as DoubleAnimation).From = second_frame_trans.X;
            go_forward_sb.Begin();
        }

        public void Second_Page_Back() //页面后退方法
        {
            (go_back_sb.Children[0] as DoubleAnimation).To = second_frame.ActualWidth;
            go_back_sb.Begin();
        }

        private void Go_back_sb_Completed(object sender, object e)
        {
            second_frame.Content = null;
        }

        private void FirstPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
                //viewmodel.Page_Height = e.NewSize.Height;
                viewmodel.Page_Width = 375;//375是个人认为比较合适的宽度
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ((sender as Button).Parent as Grid).Children.Count; i++)
            {
                if (((sender as Button).Parent as Grid).Children[i] == sender as Button)
                {
                    click_index = i;
                }
            }
            switch (click_index)
            {
                case 0:
                    {
                        second_frame.Navigate(typeof(StrategyPage));
                    }; break;
                case 1:
                    {
                        second_frame.Navigate(typeof(FengCaiPage));
                    }; break;
                case 2:
                    {
                        second_frame.Navigate(typeof(BigDataPage));
                    }; break;
                case 3:
                    {
                        second_frame.Navigate(typeof(JunxunPage));
                    }; break;
            }
        }

        private async void Second_frame_Navigated(object sender, NavigationEventArgs e)
        {
            await Task.Delay(200);
            Second_Page_Forwoard();
        }
    }
}
