using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.ViewModels.Remind;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10.Pages.RemindPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WeeksPage : Page
    {

        WeeksPageViewModel viewmodel = new WeeksPageViewModel();
        public WeeksPage()
        {
            this.InitializeComponent();
            WeekListGridView.ItemsSource = viewmodel.WeekNumList;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }



        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.SelWeekList.Count > 0)
            {
                for (int i = 0; i < App.SelWeekList.Count; i++)
                {
                    viewmodel.SelectItem(App.SelWeekList[i], true);
                }
            }
        }

        private void SaveSelected_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.addRemindViewModel.RemindModel.WeekNum = "";
            var list = App.SelWeekList.OrderBy(x => x);
            foreach (var item in list)
            {
                App.addRemindViewModel.RemindModel.WeekNum += $"{item + 1}、";
            }
            this.Visibility = Visibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

        }

        private void SelWeek_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int temp = (sender as GridView).SelectedIndex;
            if (temp > -1)
            {
                viewmodel.SelectItem(temp);
            }
        }
    }
}
