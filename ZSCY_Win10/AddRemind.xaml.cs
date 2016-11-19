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
using Windows.UI.Popups;
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
using ZSCY_Win10.Pages.AddRemindPage;
using ZSCY_Win10.Util;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddRemind : Page
    {
        ObservableCollection<BeforeTimeSel> beforeTime = new ObservableCollection<BeforeTimeSel>();

        private static SolidColorBrush UnselectedFontColor = new SolidColorBrush(Color.FromArgb(255, 70, 70, 70));
        private static SolidColorBrush SelectedFontColor = new SolidColorBrush(Color.FromArgb(255, 233, 243, 253));
        private bool isEdit = false;//为什么都未做或都做完了
        private bool isSmall = false;
        private bool TwoStatus = true;//编写提醒中
        private bool ThreeStatus = true;//选择课程中
        private Frame frameTemp;
        public AddRemind()
        {
            this.InitializeComponent();
            SelRemindListView.ItemsSource = beforeTime;
            SelectedTimeTextBlock.DataContext = App.SelectedTime;
            SelectedWeekNumTextBlock.DataContext = App.selectedWeek;
            App.selectedWeek.WeekNumString = "";
            App.SelectedTime.SelTimeString = "";
            beforeTime.Add(new BeforeTimeSel { BeforeString = "不提醒", isRemind = false, IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前五分钟", isRemind = true, BeforeTime = new TimeSpan(0, 5, 0), IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前十分钟", isRemind = true, BeforeTime = new TimeSpan(0, 10, 0), IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前二十分钟", isRemind = true, BeforeTime = new TimeSpan(0, 20, 0), IconVisibility = Visibility.Collapsed });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前一个小时", isRemind = true, BeforeTime = new TimeSpan(1, 0, 0), IconVisibility = Visibility.Collapsed });
            //Frame2.Navigate(typeof(FristPage));
            SystemNavigationManager.GetForCurrentView().BackRequested += AddRemind_BackRequested; ;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            this.SizeChanged += (s, e) =>
            {

                Frame2.Height = e.NewSize.Height;
                RemindGrid1.Width = 400;                 
                var state = "VisualState000";
                if (e.NewSize.Width > 000)
                {
                    RemindGrid1.Width = e.NewSize.Width;
                    Frame2.Width = e.NewSize.Width;
                    SplitLine1.Visibility = Visibility.Collapsed;
                    //isSmall = true;
                    //if(isEdit)
                    //{
                    //    RemindGrid1.Visibility = Visibility.Collapsed;
                    //}
                    //else
                    //{
                    //    RemindGrid1.Visibility = Visibility.Visible;
                    //}
                }
                if (e.NewSize.Width > 800)
                {
                    RemindGrid1.Width = 400;
                    Frame2.Width = e.NewSize.Width - 400;
                    isSmall = false;
                    //RemindGrid1.Visibility = Visibility.Visible;
                    SplitLine1.Visibility = Visibility.Visible;
                    state = "VisualState800";

                }
                VisualStateManager.GoToState(this, state, true);

            };
            Initialize();
        }

        private void AddRemind_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (RemindGrid2.Visibility == Visibility.Visible)
            {
                RemindGrid2.Visibility = Visibility.Collapsed;

            }
            else if(SelRemindGrid.Visibility==Visibility.Visible)
            {
                SelRemindGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                Frame.GoBack();
                SystemNavigationManager.GetForCurrentView().BackRequested -= AddRemind_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Initialize()
        {
            App.selectedWeek.WeekNumString = "";
            App.SelectedTime.SelTimeString = "";
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    App.timeSet[i, j] = null;
                }
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            isEdit = false;
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

        private void SaveEdit_Tapped(object sender, TappedRoutedEventArgs e)
        {


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
                            myRemind.Time = beforeTime[SelRemindListView.SelectedIndex].BeforeTime.TotalMinutes.ToString();
                            myRemind.Title = TitleTextBox.Text;
                            myRemind.Content = ContentTextBox.Text;

                            myRemind.IdNum = idNum;
                            myRemind.StuNum = stuNum;
                            try
                            {
                                AddRemindReturn returnStatus = new AddRemindReturn();
                                //TODO 由于不提醒上传的time是一个null无法返回404错误
                                string content = await NetWork.httpRequest(ApiUri.addRemindApi, NetWork.addRemind(myRemind));
                                returnStatus = JsonConvert.DeserializeObject<AddRemindReturn>(content);
                                myRemind.Id = returnStatus.Id;
                            }
                            catch
                            {

                            }
                            string id_system = "";
                            if (beforeTime[SelRemindListView.SelectedIndex].isRemind)
                            {
                                TimeSpan time = beforeTime[SelRemindListView.SelectedIndex].BeforeTime;
                                //设置通知
                                id_system = await RemindHelp.AddAllRemind(myRemind, time);
                            }
                            else
                            {

                            }
                            myRemind.IdNum = "";
                            myRemind.StuNum = "";
                            string databaseJson = JsonConvert.SerializeObject(myRemind);
                            DatabaseMethod.ToDatabase(myRemind.Id, databaseJson, id_system);
                        }
                    }
                }
            }

            Initialization();
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
        private void TimeGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RemindGrid2.Visibility = Visibility.Visible;
            Frame2.Navigate(typeof(CourseTablePage));
        }

        private void WeekNumGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RemindGrid2.Visibility = Visibility.Visible;
            Frame2.Navigate(typeof(SelWeekNumPage));
        }

        //private void CreateCourseWeek()
        //{

        //    for (int i = 0; i < weekList.Count(); i++)
        //    {
        //        weekList[i] = new WeekList();//初始化
        //        weekList[i].Grid = new Grid();
        //        weekList[i].Grid.Margin = new Thickness(3);

        //        weekList[i].SetWeekName(i + 1);


        //        weekList[i].Textblock = new TextBlock();
        //        weekList[i].Textblock.Text = weekList[i].WeekName;
        //        weekList[i].Textblock.Foreground = new SolidColorBrush(Colors.Black);
        //        weekList[i].Textblock.HorizontalAlignment = HorizontalAlignment.Center;
        //        weekList[i].Textblock.VerticalAlignment = VerticalAlignment.Center;

        //        weekList[i].Rect = new Rectangle();
        //        weekList[i].Rect.Fill = new SolidColorBrush(Colors.Azure);


        //        weekList[i].IsCheck = false;


        //        int column = i % 5;
        //        int row = i / 5;
        //        weekList[i].Grid.Children.Add(weekList[i].Rect);
        //        weekList[i].Grid.Children.Add(weekList[i].Textblock);
        //        SelWeekNumTable.Children.Add(weekList[i].Grid);
        //        Grid.SetColumn(weekList[i].Grid, column);
        //        Grid.SetRow(weekList[i].Grid, row);
        //        weekList[i].Rect.Tapped += Rect_Tapped;
        //        weekList[i].Textblock.Tapped += Textblock_Tapped;

        //    }
        //}
        //private void Rect_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    int row = Grid.GetRow((sender as Rectangle).Parent as Grid);
        //    int column = Grid.GetColumn((sender as Rectangle).Parent as Grid);
        //    row *= 5;

        //    if (!weekList[row + column].IsCheck)
        //    {
        //        weekList[row + column].IsCheck = true;
        //        weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.CadetBlue);
        //    }
        //    else
        //    {
        //        weekList[row + column].IsCheck = false;
        //        weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.Azure);
        //    }
        //}
        //private void Textblock_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    int row = Grid.GetRow((sender as TextBlock).Parent as Grid);
        //    int column = Grid.GetColumn((sender as TextBlock).Parent as Grid);
        //    row *= 5;

        //    if (!weekList[row + column].IsCheck)
        //    {
        //        weekList[row + column].IsCheck = true;
        //        weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.CadetBlue);
        //    }
        //    else
        //    {
        //        weekList[row + column].IsCheck = false;
        //        weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.Azure); 
        //    }
        //}

        //private void CreateCourseTable()
        //{
        //    for (int i = 0, k = 0; i < kebiaoGrid.RowDefinitions.Count; i += 2, k++)
        //        for (int j = 0; j < kebiaoGrid.ColumnDefinitions.Count; j++)
        //        {
        //            excal[k, j] = new ExcalContent();

        //            excal[k, j].Grid = new Grid();
        //            Rectangle rect = new Rectangle();

        //            excal[k, j].Grid.HorizontalAlignment = HorizontalAlignment.Stretch;
        //            excal[k, j].Grid.VerticalAlignment = VerticalAlignment.Stretch;
        //            excal[k, j].Grid.BorderBrush = new SolidColorBrush(Colors.White);


        //            if (timeSet[k, j] != null)//判断是否第一次添加课程
        //            {
        //                if (timeSet[k, j].IsCheck)
        //                {
        //                    courseList.Clear();
        //                    rect.Fill = new SolidColorBrush(Colors.Gray);
        //                    excal[k, j].IsCheck = true;
        //                }
        //                else
        //                {
        //                    rect.Fill = new SolidColorBrush(Colors.SkyBlue);
        //                    excal[k, j].IsCheck = false;
        //                }
        //            }
        //            else
        //            {
        //                timeSet[k, j] = new TimeSet();
        //                timeSet[k, j].IsCheck = false;

        //                excal[k, j].IsCheck = false;

        //                rect.Fill = new SolidColorBrush(Colors.SkyBlue);
        //            }
        //            excal[k, j].Grid.BorderThickness = new Thickness(1);

        //            Grid.SetRowSpan(excal[k, j].Grid, 2);

        //            excal[k, j].Grid.Children.Add(rect);

        //            kebiaoGrid.Children.Add(excal[k, j].Grid);
        //            Grid.SetRow(excal[k, j].Grid, i);
        //            Grid.SetColumn(excal[k, j].Grid, j);

        //            rect.Tapped += SelItems_Tapped;

        //        }
        //}
        //private void SelItems_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    int row = Grid.GetRow((sender as Rectangle).Parent as Grid);
        //    int column = Grid.GetColumn((sender as Rectangle).Parent as Grid);
        //    row /= 2;
        //    if (excal[row, column].IsCheck == false)
        //    {

        //        excal[row, column].IsCheck = true;
        //        (excal[row, column].Grid.Children[0] as Rectangle).Fill = new SolidColorBrush(Colors.Gray);

        //        timeSet[row, column].IsCheck = true;
        //        timeSet[row, column].Set(row);
        //    }
        //    else
        //    {
        //        excal[row, column].IsCheck = false;
        //        (excal[row, column].Grid.Children[0] as Rectangle).Fill = new SolidColorBrush(Colors.SkyBlue);

        //        timeSet[row, column].IsCheck = false;
        //        timeSet[row, column].Set(-1);
        //    }

        //}
        //private void CourseAddPressed_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    (sender as Grid).Background = new SolidColorBrush(Colors.Gray);

        //}

        //private void CourseAddExited_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    (sender as Grid).Background = new SolidColorBrush(Colors.White);
        //}

        //private void CourseSel_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    CourseTableGrid.Visibility = Visibility.Visible;
        //    SaveCourseTime.Visibility = Visibility.Visible;
        //    RemindGrid.Visibility = Visibility.Collapsed;
        //    EditRemind.Visibility = Visibility.Collapsed;
        //    SaveRemind.Visibility = Visibility.Collapsed;
        //    CreateCourseTable();
        //}

        //private void AddRemindAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveCourseTime.Visibility = Visibility.Collapsed;
        //}

        //private void SaveCourseTime_Click(object sender, RoutedEventArgs e)
        //{

        //    SaveCourseTime.Visibility = Visibility.Collapsed;
        //    CourseTableGrid.Visibility = Visibility.Collapsed;
        //    RemindGrid.Visibility = Visibility.Visible;
        //    SaveRemind.Visibility = Visibility.Visible;
        //    for (int i = 0; i < 6; i++)
        //        for (int j = 0; j < 7; j++)
        //        {
        //            if (timeSet[i, j].IsCheck)
        //                courseList.Add(new CourseList(i, j, timeSet[i, j].IsCheck));
        //        }
        //}

        //private void Initial()//初始化
        //{
        //    CreateCourseWeek();
        //    CreateCourseTable();
        //    courseList.Clear();
        //    SelBeforeTime.SelectedIndex = -1;
        //}
        //private void EditRemind_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveRemind.Visibility = Visibility.Visible;
        //    EditRemind.Visibility = Visibility.Collapsed;
        //    RemindGrid.Visibility = Visibility.Visible;
        //    RemindListGrid.Visibility = Visibility.Collapsed;
        //    SplitLine1.Visibility = Visibility.Visible;
        //    AddRemindTitle.Visibility = Visibility.Visible;
        //    Initial();
        //}

        //private void SaveRemind_Click(object sender, RoutedEventArgs e)
        //{
        //    RemindListGrid.Visibility = Visibility.Visible;
        //    RemindGrid.Visibility = Visibility.Collapsed;
        //    SaveRemind.Visibility = Visibility.Collapsed;
        //    EditRemind.Visibility = Visibility.Visible;
        //    SplitLine1.Visibility = Visibility.Collapsed;
        //    AddRemindTitle.Visibility = Visibility.Collapsed;
        //}

        //private void AddRemindGrid_Tapped(object sender, TappedRoutedEventArgs e)
        //{

        //}
    }
}
