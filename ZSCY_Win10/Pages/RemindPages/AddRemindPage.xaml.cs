using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Connectivity;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Controls;
using ZSCY_Win10.Models.RemindModels;
using ZSCY_Win10.Util.Remind;
using static ZSCY_Win10.Util.Remind.RemindSystemUtil;


// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10.Pages.RemindPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AddRemindPage : Page
    {
        string pageType = "";
        RemindListModel editRemind;
        public AddRemindPage()
        {
            this.InitializeComponent();
            App.indexBefore = -1;
            //ClearAllData();
            this.DataContext = App.addRemindViewModel;
            var state = "VisualState000";
            this.SizeChanged += (s, e) =>
            {
                Frame2.Height = e.NewSize.Height;
                RemindGrid1.Width = 400;
                if (e.NewSize.Width > 000)
                {
                    RemindGrid1.Width = e.NewSize.Width;
                    Frame2.Width = e.NewSize.Width;
                    SplitLine1.Visibility = Visibility.Collapsed;
                    state = "VisualState000";

                }
                if (e.NewSize.Width > 800)
                {
                    RemindGrid1.Width = 400;
                    Frame2.Width = e.NewSize.Width - 400;
                    SplitLine1.Visibility = Visibility.Visible;
                    state = "VisualState800";

                }
                VisualStateManager.GoToState(this, state, true);

            };
            SelRemindListView.ItemsSource = App.addRemindViewModel.RemindModel.BeforeTime;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += AddRemindPage_BackRequested;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            pageType = e.Parameter.ToString();
            if (!pageType.Equals("add"))
            {
                editRemind = e.Parameter as RemindListModel;
                ReloadData();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            }
            else
            {
                ClearAllData();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            }
        }

        private void ReloadData()
        {
            App.addRemindViewModel.RemindModel.Title = editRemind.Remind.Title;
            App.addRemindViewModel.RemindModel.Content = editRemind.Remind.Content;
            App.SelCoursList.Clear();
            App.SelWeekList.Clear();
            foreach (var item in editRemind.Remind.DateItems)
            {
                App.SelCoursList.Add(new SelCourseModel(int.Parse(item.Day), int.Parse(item.Class)));
            }
            string[] weekArray = editRemind.Remind.DateItems[0].Week.Split(',');
            for (int i = 0; i < weekArray.Length; i++)
            {
                App.SelWeekList.Add(int.Parse(weekArray[i]) - 1);
            }
            App.indexBefore = reloadIndex(editRemind);
            WeekAndTime();
        }

        private static void WeekAndTime()
        {
            App.addRemindViewModel.RemindModel.DayAndClass = "";
            var list = App.SelCoursList.OrderBy(x => x.DayNum).ThenBy(y => y.ClassNum).ToList<SelCourseModel>();
            foreach (var item in list)
            {
                App.addRemindViewModel.RemindModel.DayAndClass += $"{item.CourseTime()}、";
            }
            App.addRemindViewModel.RemindModel.DayAndClass =
                 App.addRemindViewModel.RemindModel.DayAndClass.Remove(App.addRemindViewModel.RemindModel.DayAndClass.Length - 1);
            App.addRemindViewModel.RemindModel.WeekNum = "";
            var weekList = App.SelWeekList.OrderBy(x => x);
            App.addRemindViewModel.RemindModel.WeekNum = "";
            foreach (var item in weekList)
            {
                App.addRemindViewModel.RemindModel.WeekNum += $"{item + 1}、";
            }
        }

        private int reloadIndex(RemindListModel remind)
        {
            int index;
            int? time = remind.Remind.Time;
            if (time == null) index = 0;
            else if (time == 5) index = 1;
            else if (time == 10) index = 2;
            else if (time == 20) index = 3;
            else if (time == 60) index = 4;
            else index = 3;
            SelectedRemindTextBlock.Text = App.addRemindViewModel.RemindModel.BeforeTime[index].BeforeTimeString;
            return index;
        }
        private void AddRemindPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!pageType.Equals("add"))
                Frame.GoBack();
            else
            {
                Frame.GoBack();
                ClearAllData();
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= AddRemindPage_BackRequested;
        }

        private void SaveEditRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string content = ContentTextBox.Text;
            string title = TitleTextBox.Text;
            if (title == "")
                new NotifyPopup("标题不能为空").Show();
            else if (SelectedWeekNumTextBlock.Text == "")
                new NotifyPopup("请选择提醒周数").Show();
            else if (SelectedTimeTextBlock.Text == "")
                new NotifyPopup("请选择提醒时间").Show();
            else if (SelectedRemindTextBlock.Text == "")
                new NotifyPopup("请选择提前时间").Show();
            else
            {
                SaveMethod(content, title);

            }
        }

        private async void SaveMethod(string content, string title)
        {
            if (pageType.Equals("add"))
            {

                App.addRemindViewModel.AddRemind(content, title);
                ClearAllData();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                SaveEditRemind.IsEnabled = false;
                new Action(async () =>
                {
                    await Task.Delay(600);
                    SaveEditRemind.IsEnabled = true;
                })();//防止多次添加
            }
            else
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    SaveEditRemind.IsEnabled = false;
                    App.addRemindViewModel.EditRemind(content, title, editRemind);
                    ClearAllData();
                    new Action(async () =>
                    {
                        await Task.Delay(500);//保证数据库存储完成
                        Frame.Navigate(typeof(RemindListPage), true);
                    })();
                }
                else
                {
                    await new MessageDialog("请打开网络!!!").ShowAsync();
                }

            }

        }

        private void TimeGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame2.Navigate(typeof(CourseTablePage));
        }

        private void WeekNumGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame2.Navigate(typeof(WeeksPage));
        }

        private void RemindGridButon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }

        private void SelRemindBackgroupGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }
        private void SelRemindListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (App.indexBefore == -1)
            {
                App.indexBefore = SelRemindListView.SelectedIndex;
                App.addRemindViewModel.RemindModel.BeforeTime[App.indexBefore].IconVisibility = Visibility.Visible;
            }
            else
            {
                App.addRemindViewModel.RemindModel.BeforeTime[App.indexBefore].IconVisibility = Visibility.Collapsed;
                App.indexBefore = SelRemindListView.SelectedIndex;
                if (App.indexBefore != -1)
                    App.addRemindViewModel.RemindModel.BeforeTime[App.indexBefore].IconVisibility = Visibility.Visible;
            }
            if (App.indexBefore != -1)
                SelectedRemindTextBlock.Text = App.addRemindViewModel.RemindModel.BeforeTime[App.indexBefore].BeforeTimeString;
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }


        private void ClearAllData()
        {
            App.addRemindViewModel.RemindModel.Content = "";
            App.addRemindViewModel.RemindModel.Title = "";
            App.addRemindViewModel.RemindModel.WeekNum = "";
            App.addRemindViewModel.RemindModel.DayAndClass = "";
            App.SelCoursList.Clear();
            App.SelWeekList.Clear();
            SelectedRemindTextBlock.Text = "";
            SelRemindListView.SelectedIndex = -1;
        }
    }
}
