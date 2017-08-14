using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CalendarPage : Page
    {
        public CalendarPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                CalendarImage.Width = e.NewSize.Width;
                CalendarImage.Height = e.NewSize.Height;
            };
            this.CalendarImage.ImageOpened += CalendarImage_ImageOpened;
            //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
        }

        private void CalendarImage_ImageOpened(object sender, RoutedEventArgs e)
        {
            this.progress.IsActive = false;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageStart("CalendarPage");
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("CalendarPage");
        }
    }
}