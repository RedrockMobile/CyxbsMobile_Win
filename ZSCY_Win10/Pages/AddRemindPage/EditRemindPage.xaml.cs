using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditRemindPage : Page
    {
        private static SolidColorBrush UnselectedFontColor = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
        private static SolidColorBrush SelectedFontColor = new SolidColorBrush(Color.FromArgb(255, 233, 243, 253));
        ObservableCollection<BeforeTimeSel> beforeTime = new ObservableCollection<BeforeTimeSel>();
        WeekList[] weekList = new WeekList[20];

        public EditRemindPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //selCourseList.ItemsSource = App.courseList;
            SelRemindListView.ItemsSource = beforeTime;
            beforeTime.Add(new BeforeTimeSel { BeforeString = "不提醒", isRemind = false,IconVisibility=Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前五分钟", isRemind = true, BeforeTime = new TimeSpan(0, 5, 0),  IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前十分钟", isRemind = true, BeforeTime = new TimeSpan(0, 10, 0),IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前二十分钟", isRemind = true, BeforeTime = new TimeSpan(0, 20, 0),  IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前一个小时", isRemind = true, BeforeTime = new TimeSpan(1, 0, 0),IconVisibility = Visibility.Collapsed });
            //App.courseList.Clear();
            //CreateCourseWeek();
            this.SizeChanged += (s, e) =>
            {
                SplitLine.Y2 = e.NewSize.Height - 48;
                EditRemindScrollViewer.Height = e.NewSize.Height - 48;
            };
            //this.Initial();
        }
        private void Initial()//初始化
        {
            CreateCourseWeek();
            //SelBeforeTime.SelectedIndex = -1;
        }
        private void CourseAddPressed_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.Gray);

        }

        private void CourseAddExited_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.White);
        }

        private void CreateCourseWeek()
        {

            for (int i = 0; i < weekList.Count(); i++)
            {
                weekList[i] = new WeekList();//初始化
                weekList[i].Grid = new Grid();
                weekList[i].Grid.Margin = new Thickness(3);

                weekList[i].SetWeekName(i + 1);


                weekList[i].Textblock = new TextBlock();
                weekList[i].Textblock.Text = weekList[i].WeekName;
                weekList[i].Textblock.Foreground = new SolidColorBrush(Colors.Black);
                weekList[i].Textblock.HorizontalAlignment = HorizontalAlignment.Center;
                weekList[i].Textblock.VerticalAlignment = VerticalAlignment.Center;

                weekList[i].Rect = new Rectangle();
                weekList[i].Rect.Fill = new SolidColorBrush(Colors.Azure);


                weekList[i].IsCheck = false;


                int column = i % 5;
                int row = i / 5;
                weekList[i].Grid.Children.Add(weekList[i].Rect);
                weekList[i].Grid.Children.Add(weekList[i].Textblock);
                //SelWeekNumTable.Children.Add(weekList[i].Grid);
                Grid.SetColumn(weekList[i].Grid, column);
                Grid.SetRow(weekList[i].Grid, row);
                weekList[i].Rect.Tapped += Rect_Tapped;
                weekList[i].Textblock.Tapped += Textblock_Tapped;

            }
        }

        private void Rect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((sender as Rectangle).Parent as Grid);
            int column = Grid.GetColumn((sender as Rectangle).Parent as Grid);
            row *= 5;

            if (!weekList[row + column].IsCheck)
            {
                weekList[row + column].IsCheck = true;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.CadetBlue);
            }
            else
            {
                weekList[row + column].IsCheck = false;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.Azure);
            }
        }
        private void Textblock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((sender as TextBlock).Parent as Grid);
            int column = Grid.GetColumn((sender as TextBlock).Parent as Grid);
            row *= 5;

            if (!weekList[row + column].IsCheck)
            {
                weekList[row + column].IsCheck = true;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.CadetBlue);
            }
            else
            {
                weekList[row + column].IsCheck = false;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.Azure);
            }
        }


        private void CourseSel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CourseTablePage));
        }

        private async void SaveEditRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //string tempTitle, tempContent;
            //TimeSpan tempTime;
            //tempTitle = title.Text;
            //tempContent = content.Text;
            //if (SelBeforeTime.SelectedIndex == -1)
            //{
            //    new ErrorNotification("请选择提前时间").Show();
            //    return;
            //}
            //else
            //{
            //    tempTime = beforeTime[SelBeforeTime.SelectedIndex].BeforeTime;
            //    bool isSelCourse = false;
            //    foreach (CourseList oc in App.courseList)
            //    {
            //        if (oc.IsCheck == true)
            //        {
            //            isSelCourse = true;
            //            break;
            //        }
            //    }

            //    if (!isSelCourse)
            //    {
            //        new ErrorNotification("请选择在哪节课提醒").Show();
            //        return;
            //    }
            //    else
            //    {
            //        bool isSelWeek = false;
            //        foreach (WeekList wl in weekList)
            //        {
            //            if (wl.IsCheck == true)
            //            {
            //                isSelWeek = true;
            //                break;
            //            }
            //        }
            //        if (!isSelWeek)
            //        {
            //            new ErrorNotification("请选择在哪周提醒").Show();
            //            return;
            //        }
            //        else//添加通知
            //        {
            //            IReadOnlyList<ScheduledToastNotification> toasts;
            //            for (int wNum = 0; wNum < weekList.Count(); wNum++)
            //            {
            //                if (weekList[wNum].IsCheck)
            //                {

            //                    for (int cNum = 0; cNum < App.timeSet.Length; cNum++)
            //                    {
            //                        int row = cNum / (App.timeSet.GetLength(0) + 1);
            //                        int column = cNum % (App.timeSet.GetLength(0) + 1);
            //                        if (App.timeSet[row, column].IsCheck)
            //                        {
            //                            DateTime temp = weekList[wNum].WeekNumOfMonday.Add(App.timeSet[row, column].Time).Add(new TimeSpan(column, 0, 0, 0));
            //                            Debug.WriteLine("{0}->{1}", temp, (temp.ToUniversalTime().Ticks - 621355968000000000) / 10000000);
            //                            temp = temp.Add(-beforeTime[SelBeforeTime.SelectedIndex].BeforeTime);
            //                            //DateTime temp =DateTime.Now.AddSeconds(500);

            //                            var alarm = new MyRemind()
            //                            {
            //                                RemindTitle = tempTitle,
            //                                RemindContent = tempContent,
            //                                RemindTime = temp,

            //                            };
            //                            if (alarm.RemindTime.ToUniversalTime().Ticks < DateTime.Now.ToUniversalTime().Ticks)
            //                                continue;
            //                            await RemindHelp.AddRemind(alarm);

            //                        }

            //                    }
            //                }
            //            }
            //        }
            //        App.RemindList = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();
            //        this.Frame.GoBack();
            //    }
            //}
        }

        private void RemindGridButon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }

        private void SelRemindBackgroupGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }
        private int indexBefore = 0;
        private void SelRemindListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            beforeTime[indexBefore].IconVisibility = Visibility.Collapsed;
            int temp = indexBefore = (sender as ListView).SelectedIndex;
            beforeTime[temp].IconVisibility = Visibility.Visible;

            SelectedRemindTextBlock.Text = beforeTime[temp].BeforeString;
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }

        private void TimeGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }
    }
}
