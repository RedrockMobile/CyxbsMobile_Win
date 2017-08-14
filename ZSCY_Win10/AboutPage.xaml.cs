using System.Diagnostics;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                Debug.WriteLine(e.NewSize.Height);
                var state = "VisualState_W0H0";
                if (e.NewSize.Height > 280)
                    state = "VisualState_W0H1";
                if (e.NewSize.Height > 400)
                    state = "VisualState_W0H2";
                VisualStateManager.GoToState(this, state, true);
            };
            SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        { }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            UmengSDK.UmengAnalytics.TrackPageStart("AboutPage");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
            UmengSDK.UmengAnalytics.TrackPageEnd("AboutPage");
        }
    }
}