using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ImportKB2CalendarPage : Page
    {
        private ApplicationDataContainer appSetting;
        public ImportKB2CalendarPage()
        {
            this.InitializeComponent();
            appSetting = ApplicationData.Current.LocalSettings; //本地存储

        }
        private static string resourceName = "ZSCY";


        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            //URLTextBlock.Text = "http://hongyan.cqupt.edu.cn/api/kebiao_ics?xh=" + appSetting.Values["stuNum"].ToString();
            URLTextBlock.Text = "http://hongyan.cqupt.edu.cn/api/kebiao_ics?xh=" + credentialList[0].UserName;
            UmengSDK.UmengAnalytics.TrackPageStart("ImportKB2CalendarPage");
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("ImportKB2CalendarPage");
        }



        private async void ToCalendar_Click(object sender, RoutedEventArgs e)
        {
            bool success = await Launcher.LaunchUriAsync(new Uri("https://bay04.calendar.live.com/calendar/import.aspx?mkt=zh-CN#"));
        }

        private async void ToAccount_Click(object sender, RoutedEventArgs e)
        {
            bool success = await Launcher.LaunchUriAsync(new Uri("ms-settings-emailandaccounts:"));

        }

        private void URLTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName); 
            credentialList[0].RetrievePassword();
            //URLTextBlock.Text = "http://hongyan.cqupt.edu.cn/api/kebiao_ics?xh=" + appSetting.Values["stuNum"].ToString();
            URLTextBlock.Text = "http://hongyan.cqupt.edu.cn/api/kebiao_ics?xh=" + credentialList[0].UserName;
        }
    }
}
