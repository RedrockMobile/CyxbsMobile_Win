using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZSCY_Win10.Controls.RemindPage;
using ZSCY_Win10.Models.RemindPage;
using ZSCY_Win10.Util;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class EditRemindPage : Page
    {
        private static SolidColorBrush UnselectedFontColor = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
        private static SolidColorBrush SelectedFontColor = new SolidColorBrush(Color.FromArgb(255, 233, 243, 253));
        ObservableCollection<BeforeTimeSel> beforeTime = new ObservableCollection<BeforeTimeSel>();
        WeekList[] weekList = new WeekList[20];
        string tempID = "";
        public EditRemindPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            SelectedTimeTextBlock.DataContext = App.SelectedTime;
            SelectedWeekNumTextBlock.DataContext = App.selectedWeek;
            //selCourseList.ItemsSource = App.courseList;
            SelRemindListView.ItemsSource = beforeTime;
            beforeTime.Add(new BeforeTimeSel { BeforeString = "不提醒", isRemind = false, IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前五分钟", isRemind = true, BeforeTime = new TimeSpan(0, 5, 0), IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前十分钟", isRemind = true, BeforeTime = new TimeSpan(0, 10, 0), IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前二十分钟", isRemind = true, BeforeTime = new TimeSpan(0, 20, 0), IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前一个小时", isRemind = true, BeforeTime = new TimeSpan(1, 0, 0), IconVisibility = Visibility.Collapsed });
            //App.courseList.Clear();
            //CreateCourseWeek();
            this.SizeChanged += (s, e) =>
            {
                SplitLine.Y2 = e.NewSize.Height - 48;
                EditRemindScrollViewer.Height = e.NewSize.Height - 48;
            };
            //this.Initial();
            //SystemNavigationManager.GetForCurrentView().BackRequested += EditRemindPage_BackRequested ;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        //private void EditRemindPage_BackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    if (SelRemindGrid.Visibility == Visibility.Visible)
        //    {
        //        SelRemindGrid.Visibility = Visibility.Visible;
        //        SystemNavigationManager.GetForCurrentView().BackRequested -= EditRemindPage_BackRequested ;
        //        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //    }
        //    else
        //    {
        //        this.NavigationCacheMode = NavigationCacheMode.Disabled;
        //        this.Visibility = Visibility.Collapsed;
        //        SystemNavigationManager.GetForCurrentView().BackRequested -= EditRemindPage_BackRequested; 
        //        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //    }
        //}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && !App.isLoad)
            {
                App.isLoad = true;
                App.SelectedTime.SelTimeString = "";
                App.selectedWeek.WeekNumString = "";

                MyRemind temp = new MyRemind();
                temp = e.Parameter as MyRemind;
                tempID = temp.Id;
                int x = 0;
                for (int i = 0; i < beforeTime.Count; i++)
                {
                    if (temp.Time != null)
                    {

                        if (beforeTime[i].getBeforeTime() == int.Parse(temp.Time))
                        {
                            x = i;
                            break;
                        }
                    }
                    else
                        x = 0;
                }
                SelRemindListView.SelectedIndex = indexBefore = x;
                TitleTextBox.Text = temp.Title;

                ContentTextBox.Text = temp.Content;
                string s = temp.DateItems[0].Week;
                App.selectedWeek.WeekNumString = s + ",";
                string[] weekArray = s.Split(',');
                for (int i = 0; i < weekArray.Count(); i++)
                {
                    SelectedWeekNum wNum = new SelectedWeekNum()
                    {
                        WeekNum = int.Parse(weekArray[i])
                    };
                    wNum.SetWeekTime(int.Parse(weekArray[i]));
                    App.selectedWeekNumList.Add(wNum);
                }
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 7; j++)
                    {
                        App.timeSet[i, j] = new TimeSet();
                    }
                }
                foreach (var item in temp.DateItems)
                {
                    App.timeSet[int.Parse(item.Class), int.Parse(item.Day)].IsCheck = true;
                    App.SelectedTime.SelTimeString += CourseList(int.Parse(item.Class), int.Parse(item.Day)) + " ";
                }
            }

        }
        private string CourseList(int row, int column)
        {
            string weekNum = "";
            string classNum = "";
            switch (column)
            {
                case 0:
                    weekNum = "周一";
                    break;
                case 1:
                    weekNum = "周二";
                    break;
                case 2:
                    weekNum = "周三";
                    break;
                case 3:
                    weekNum = "周四";
                    break;
                case 4:
                    weekNum = "周五";
                    break;
                case 5:
                    weekNum = "周六";
                    break;
                case 6:
                    weekNum = "周末";
                    break;
            }
            switch (row)
            {
                case 0:
                    classNum = "12节";
                    break;
                case 1:
                    classNum = "34节";
                    break;
                case 2:
                    classNum = "56节";
                    break;
                case 3:
                    classNum = "78节";
                    break;
                case 4:
                    classNum = "910节";
                    break;
                case 5:
                    classNum = "1112节";
                    break;
            }
            string temp = string.Concat(weekNum, classNum);
            return temp;
        }

        //private void RemindGridButon_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    SelRemindGrid.Visibility = Visibility.Visible;
        //}

        private void SelRemindBackgroupGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }
        private int indexBefore = 0;
        private void SelRemindListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelRemindListView.SelectedIndex == -1)
            {

                beforeTime[indexBefore].IconVisibility = Visibility.Collapsed;
                indexBefore = 0;
            }
            else
            {
                beforeTime[indexBefore].IconVisibility = Visibility.Collapsed;
                int temp = indexBefore = (sender as ListView).SelectedIndex;
                beforeTime[temp].IconVisibility = Visibility.Visible;

                SelectedRemindTextBlock.Text = beforeTime[temp].BeforeString;
                SelRemindGrid.Visibility = Visibility.Collapsed;
            }

        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialization()
        {
            TitleTextBox.Text = "";
            ContentTextBox.Text = "";
            App.SelectedTime.SelTimeString = "";
            App.selectedWeek.WeekNumString = "";
            App.selectedWeekNumList.Clear();
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                    App.timeSet[i, j] = null;
            SelectedRemindTextBlock.Text = "";
            SelRemindListView.SelectedIndex = -1;

        }

        private async void SaveEditRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (TitleTextBox.Text == "")
            {
                new ErrorNotification("标题不能为空").Show();
                return;

            }
            else
            {
                if (SelectedTimeTextBlock.Text == "")
                {
                    new ErrorNotification("请选择提醒时间").Show();
                    return;
                }
                else
                {
                    if (SelectedWeekNumTextBlock.Text == "")
                    {
                        new ErrorNotification("请选择提醒周数").Show();
                        return;
                    }
                    else
                    {
                        if (SelectedRemindTextBlock.Text == "")
                        {
                            new ErrorNotification("请选择提前时间").Show();
                            return;
                        }
                        else
                        {
                            string resource = "ZSCY";
                            PasswordCredential userCredential = GetCredential.getCredential(resource);
                            string stuNum, idNum;
                            stuNum = userCredential.UserName;
                            idNum = userCredential.Password;
                            Debug.WriteLine("{0},{1}", stuNum, idNum);
                            MyRemind myRemind = new MyRemind();
                            myRemind.DateItems = new List<DateItemModel>();
                            myRemind.Id = tempID;
                            for (int i = 0; i < 7; i++)
                            {
                                for (int j = 0; j < 6; j++)
                                    if (App.timeSet[j, i].IsCheck)
                                    {
                                        //dateItem.Class += j.ToString() + ",";
                                        //dateItem.Day += i.ToString() + ",";
                                        DateItemModel dateItem = new DateItemModel();

                                        dateItem.Class = j.ToString();
                                        dateItem.Day = i.ToString();
                                        for (int k = 0; k < App.selectedWeekNumList.Count; k++)
                                        {
                                            dateItem.Week += App.selectedWeekNumList[k].WeekNum + ",";
                                        }
                                        dateItem.Week = dateItem.Week.Remove(dateItem.Week.Length - 1);
                                        myRemind.DateItems.Add(dateItem);
                                    }
                            }
                            if (SelRemindListView.SelectedIndex == 0)
                            {
                                myRemind.Time = null;
                            }
                            else
                            {

                                myRemind.Time = beforeTime[SelRemindListView.SelectedIndex].BeforeTime.TotalMinutes.ToString();
                            }
                            myRemind.Title = TitleTextBox.Text;
                            myRemind.Content = ContentTextBox.Text;
                            string databaseJson = JsonConvert.SerializeObject(myRemind);
                            myRemind.IdNum = idNum;
                            myRemind.StuNum = stuNum;

                            try
                            {
                                string content = await NetWork.httpRequest(ApiUri.editRemindApi, NetWork.editRemind(myRemind));
                            }
                            catch
                            {
                                Debug.WriteLine("网络问题请求失败");
                            }
                            string id_system = "";
                            if (beforeTime[SelRemindListView.SelectedIndex].isRemind)
                            {
                                TimeSpan time = beforeTime[SelRemindListView.SelectedIndex].BeforeTime;
                                //设置通知
                                RemindListDB temp = DatabaseMethod.ToModel(myRemind.Id);
                                string[] TagArray = temp.Id_system.Split(',');
                                var notifier = ToastNotificationManager.CreateToastNotifier();

                                for (int i = 0; i < TagArray.Count(); i++)
                                {
                                    var scheduledNotifs = notifier.GetScheduledToastNotifications()
                                  .Where(n => n.Tag.Equals(TagArray[i]));

                                    // Remove all of those from the schedule
                                    foreach (var n in scheduledNotifs)
                                    {
                                        notifier.RemoveFromSchedule(n);
                                    }
                                }
                                //重新添加
                                id_system = await RemindHelp.AddAllRemind(myRemind, time);
                            }
                            else
                            {

                            }
                            DatabaseMethod.EditDatabase(myRemind.Id, databaseJson, id_system);
                            DatabaseMethod.ReadDatabase(Visibility.Visible);
                            App.isLoad = false;
                            App.selectedWeekNumList.Clear();
                        }
                    }
                }
            }
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            this.Visibility = Visibility.Collapsed;
        }
        private void RemindGridButon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }

        private void TimeGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CourseTablePage), typeof(EditRemindPage));
        }

        private void WeekNumGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(SelWeekNumPage), typeof(EditRemindPage));
        }
    }
}