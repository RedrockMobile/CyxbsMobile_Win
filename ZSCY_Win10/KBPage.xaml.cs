using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class KBPage : Page
    {
        private string stuNum = "";
        private string kb = "";
        private int wOa = 1;
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        ApplicationDataContainer appSettingclass = Windows.Storage.ApplicationData.Current.RoamingSettings;
        IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
        Grid backweekgrid = new Grid();
        List<ClassList> classList = new List<ClassList>();
        string[,][] classtime = new string[7, 6][];
        public KBPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000)
                {
                    TodayTitleStackPanel.Visibility = Visibility.Collapsed;
                }
                if (e.NewSize.Width > 600)
                {
                    TodayTitleStackPanel.Margin = new Thickness(400, 0, 0, 0);
                    TodayTitleStackPanel.Visibility = Visibility.Visible;
                    state = "VisualState550";
                }
                if (e.NewSize.Width > 750)
                {
                    TodayTitleStackPanel.Margin = new Thickness(400, 0, 0, 0);
                    TodayTitleStackPanel.Visibility = Visibility.Visible;
                    KBDayFLine.X2 = e.NewSize.Width - 400;
                    state = "VisualState750";
                }
                if (e.NewSize.Width > 1000)
                {
                    TodayTitleStackPanel.Margin = new Thickness(400, 0, 0, 0);
                    TodayTitleStackPanel.Visibility = Visibility.Visible;
                    KBDayFLine.X2 = e.NewSize.Width - 400 - 250;
                    state = "VisualState1000";
                }
                VisualStateManager.GoToState(this, state, true);
                Debug.WriteLine("KBAllGrid" + KBAllGrid.Width);
                Debug.WriteLine(e.NewSize.Width);
                KebiaoAllScrollViewer.Height = e.NewSize.Height - 48 - 25;
                cutoffLine.Y2 = e.NewSize.Height - 48;
                cutoffLine2.Y2 = e.NewSize.Height - 48;
            };
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("OnNavigatedTo");
            stuNum = appSetting.Values["stuNum"].ToString();
            initKB();
            this.progress.IsActive = false;
            initToday();
            UmengSDK.UmengAnalytics.TrackPageStart("KBPage");
        }


        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Debug.WriteLine("OnNavigatedFrom");
            UmengSDK.UmengAnalytics.TrackPageEnd("KBPage");
        }

        private void SetKebiaoGridBorder(int week)
        {
            //边框
            //for (int i = 0; i < kebiaoGrid.RowDefinitions.Count; i++)
            //{
            //    for (int j = 0; j < kebiaoGrid.ColumnDefinitions.Count; j++)
            //    {
            //        var border = new Border() { BorderBrush = new SolidColorBrush(Colors.LightGray), BorderThickness = new Thickness(0.5) };
            //        Grid.SetRow(border, i);
            //        Grid.SetColumn(border, j);
            //        kebiaoGrid.Children.Add(border);
            //    }
            //}

            //星期背景色
            if (week == 0)
            {
                Grid backgrid = new Grid();
                backgrid.Background = new SolidColorBrush(Color.FromArgb(255, 254, 245, 207));
                backgrid.SetValue(Grid.RowProperty, 0);
                backgrid.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) + 6) % 7);
                backgrid.SetValue(Grid.RowSpanProperty, 12);
                kebiaoGrid.Children.Add(backgrid);

                backweekgrid.Background = new SolidColorBrush(Color.FromArgb(255, 254, 245, 207));
                backweekgrid.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek())));
                KebiaoWeekGrid.Children.Remove(backweekgrid);
                KebiaoWeekGrid.Children.Add(backweekgrid);

            }
            else
            {
                backweekgrid.Background = new SolidColorBrush(Color.FromArgb(255, 248, 248, 248));
                backweekgrid.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek())));
                KebiaoWeekGrid.Children.Remove(backweekgrid);
                KebiaoWeekGrid.Children.Add(backweekgrid);
            }
            TextBlock KebiaoWeek = new TextBlock();
            KebiaoWeek.Text = Utils.GetWeek(2);
            KebiaoWeek.FontSize = 20;
            KebiaoWeek.Foreground = new SolidColorBrush(Colors.Black);
            KebiaoWeek.FontWeight = FontWeights.Light;
            KebiaoWeek.VerticalAlignment = VerticalAlignment.Center;
            KebiaoWeek.HorizontalAlignment = HorizontalAlignment.Center;
            KebiaoWeek.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek())));
            KebiaoWeekGrid.Children.Add(KebiaoWeek);
        }

        private async void initKB(bool isRefresh = false)
        {
            if (stuNum == appSetting.Values["stuNum"].ToString() && !isRefresh)
            {
                try
                {
                    IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                    IStorageFile storageFileRE = await applicationFolder.GetFileAsync("kb");
                    IRandomAccessStream accessStream = await storageFileRE.OpenReadAsync();
                    using (StreamReader streamReader = new StreamReader(accessStream.AsStreamForRead((int)accessStream.Size)))
                    {
                        kb = streamReader.ReadToEnd();
                    }
                    HubSectionKBNum.Text = " | 第" + appSetting.Values["nowWeek"].ToString() + "周";
#if DEBUG
                    showKB(2, 5);
#else
                    showKB(2);
#endif
                }
                catch (Exception) { Debug.WriteLine("主页->课表数据缓存异常"); }
            }
            if (stuNum == appSetting.Values["stuNum"].ToString())
            {
                HubSectionKBTitle.Text = "我的课表";
            }

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", stuNum));

            string kbtemp = await NetWork.getHttpWebRequest("redapi2/api/kebiao", paramList); //新
                                                                                              //string kbtemp = await NetWork.getHttpWebRequest("api/kebiao", paramList); //旧

            if (!appSetting.Values.ContainsKey("HttpTime"))
                appSetting.Values["HttpTime"] = DateTimeOffset.Now.ToString();
            if (kbtemp != "")
            {
                kb = kbtemp;
                Debug.WriteLine("DateTimeOffset.Now.ToString()" + DateTimeOffset.Now.ToString());
                appSetting.Values["HttpTime"] = DateTimeOffset.Now.Year.ToString() + "/" + DateTimeOffset.Now.Month.ToString() + "/" + DateTimeOffset.Now.Day.ToString();
            }
            Debug.WriteLine("kb->" + kb);
            if (kb != "")
            {
                JObject obj = JObject.Parse(kb);
                if (Int32.Parse(obj["status"].ToString()) == 200)
                {
                    IStorageFile storageFileWR = await applicationFolder.CreateFileAsync("kb", CreationCollisionOption.OpenIfExists);
                    try
                    {
                        await FileIO.WriteTextAsync(storageFileWR, kb);
                    }
                    catch (Exception)
                    {
                        Debug.WriteLine("主页 -> 课表缓存，读取异常");
                    }
                    //保存当前星期

                    if (kbtemp == "")
                    {
                        Debug.WriteLine("上次时间" + appSetting.Values["HttpTime"].ToString());
                        //DateTimeOffset d = DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString());
                        Debug.WriteLine("1");
                        int httpweekday = (Int16)DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString()).DayOfWeek == 0 ? 7 : (Int16)DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString()).DayOfWeek;

                        Debug.WriteLine("差" + (DateTimeOffset.Now - DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString())).TotalDays);
                        double weekday = (DateTimeOffset.Now - DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString())).TotalDays - (7 - httpweekday);
                        Debug.WriteLine("weekday_前" + weekday);
                        //if (weekday % ((Int16)weekday) > 0 || weekday > 0 && weekday < 1)
                        //    weekday = (Int16)weekday + 1;
                        weekday = (Int16)weekday;
                        Debug.WriteLine("weekday_后" + weekday);
                        if (weekday > 0)
                            appSetting.Values["nowWeek"] = Int16.Parse(obj["nowWeek"].ToString()) + (Int16)(weekday + 6) / 7;
                        else
                            appSetting.Values["nowWeek"] = obj["nowWeek"].ToString();
                        Debug.WriteLine(" appSetting.Values[\"nowWeek\"]" + appSetting.Values["nowWeek"].ToString());
                    }
                    else
                        appSetting.Values["nowWeek"] = obj["nowWeek"].ToString();
                    HubSectionKBNum.Text = "第" + appSetting.Values["nowWeek"].ToString() + "周";
                    //showKB(2, Int32.Parse(appSetting.Values["nowWeek"].ToString()));
#if DEBUG
                    showKB(2);
#else
                    showKB(2);
#endif
                }
            }
            DateTime now = DateTime.Now;
            DateTime weekstart = GetWeekFirstDayMon(now);
            DateTime weekend = GetWeekLastDaySun(now);
            this.HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
        }
        public DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            //星期一为第一天   
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。   
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天   
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        public DateTime GetWeekLastDaySun(DateTime datetime)
        {
            //星期天为最后一天   
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天   
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);
        }

        private void showKB(int weekOrAll = 1, int week = 0)
        {
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 6; j++)
                    classtime[i, j] = null;


            kebiaoGrid.Children.Clear();
            SetKebiaoGridBorder(week);
            classList.Clear();
            JArray ClassListArray = Utils.ReadJso(kb);
            int ColorI = 0;
            for (int i = 0; i < ClassListArray.Count; i++)
            {
                ClassList classitem = new ClassList();
                classitem.GetAttribute((JObject)ClassListArray[i]);
                classList.Add(classitem);
                int ClassColor = 0;
                if (!appSettingclass.Values.ContainsKey(classitem.Course))
                {
                    appSettingclass.Values[classitem.Course] = ColorI;
                    ClassColor = ColorI;
                    ColorI++;
                    if (ColorI > 2)
                        ColorI = 0;
                }
                else
                {
                    ClassColor = System.Int32.Parse(appSettingclass.Values[classitem.Course].ToString());
                }
                if (weekOrAll == 1)
                {
                    SetClassAll(classitem, ClassColor);
                    HubSectionKBNum.Visibility = Visibility.Collapsed;
                }
                else
                {
                    HubSectionKBNum.Visibility = Visibility.Visible;
                    if (week == 0)
                    {
                        if (Array.IndexOf(classitem.Week, Int32.Parse(appSetting.Values["nowWeek"].ToString())) != -1)
                        {
                            SetClassAll(classitem, ClassColor);
                            HubSectionKBNum.Text = " | 第" + appSetting.Values["nowWeek"].ToString() + "周";
                        }
                    }
                    else
                    {
                        if (Array.IndexOf(classitem.Week, week) != -1)
                        {
                            SetClassAll(classitem, ClassColor);
                            HubSectionKBNum.Text = " | 第" + week.ToString() + "周";
                        }
                    }
                }
            }
            appSettingclass.Values.Clear();

            //当日课表显示
            KebiaoDayGrid.Children.Clear();
            for (int i = 0; i < ClassListArray.Count; i++)
            {
                ClassList classitem = new ClassList();
                classitem.GetAttribute((JObject)ClassListArray[i]);
#if DEBUG
                if (Array.IndexOf(classitem.Week, 5) != -1 && classitem.Hash_day == 2)
                {
                    SetClassDay(classitem);
                }
#else
                if (Array.IndexOf(classitem.Week, Int32.Parse(appSetting.Values["nowWeek"].ToString())) != -1 && classitem.Hash_day == (Int16.Parse(Utils.GetWeek()) + 6) % 7)
                {
                    SetClassDay(classitem);
                }
#endif
            }


        }

        /// <summary>
        /// 日视图课程格子的填充
        /// </summary>
        /// <param name="classitem"></param>
        private void SetClassDay(ClassList classitem)
        {
            Grid BackGrid = new Grid();
            BackGrid.Background = new SolidColorBrush(Color.FromArgb(255, 88, 179, 255));
            //BackGrid.Background = new SolidColorBrush(Colors.Wheat);
            BackGrid.SetValue(Grid.RowProperty, System.Int32.Parse(classitem.Hash_lesson * 2 + ""));
            BackGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(classitem.Hash_day + ""));
            BackGrid.SetValue(Grid.RowSpanProperty, System.Int32.Parse(classitem.Period + ""));

            StackPanel BackStackPanel = new StackPanel();
            BackStackPanel.Margin = new Thickness(15);
            BackStackPanel.VerticalAlignment = VerticalAlignment.Center;

            TextBlock classNameTextBlock = new TextBlock();
            classNameTextBlock.Text = classitem.Course;
            classNameTextBlock.FontSize = 20;
            classNameTextBlock.Margin = new Thickness(0, 3, 0, 3);
            classNameTextBlock.Foreground = new SolidColorBrush(Colors.White);


            StackPanel classTeaStackPanel = new StackPanel();
            Image classTeaImage = new Image();
            TextBlock classTeaTextBlock = new TextBlock();
            classTeaImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/dialog_course_peo_white.png", UriKind.Absolute));
            classTeaImage.Width = 15;
            classTeaImage.Height = 15;
            classTeaTextBlock.Text = classitem.Teacher;
            classTeaTextBlock.FontSize = 15;
            classTeaTextBlock.Margin = new Thickness(10, 0, 0, 0);
            classTeaTextBlock.Foreground = new SolidColorBrush(Colors.White);
            classTeaStackPanel.Orientation = Orientation.Horizontal;
            classTeaStackPanel.Children.Add(classTeaImage);
            classTeaStackPanel.Children.Add(classTeaTextBlock);
            classTeaStackPanel.Margin = new Thickness(0, 3, 0, 3);


            StackPanel classAddStackPanel = new StackPanel();
            Image classAddImage = new Image();
            TextBlock classAddTextBlock = new TextBlock();
            classAddImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/dialog_course_add_white.png", UriKind.Absolute));
            classAddImage.Width = 15;
            classAddImage.Height = 15;
            classAddTextBlock.Text = classitem.Classroom;
            classAddTextBlock.FontSize = 15;
            classAddTextBlock.Margin = new Thickness(10, 0, 0, 0);
            classAddTextBlock.Foreground = new SolidColorBrush(Colors.White);
            classAddStackPanel.Orientation = Orientation.Horizontal;
            classAddStackPanel.Children.Add(classAddImage);
            classAddStackPanel.Children.Add(classAddTextBlock);
            classAddStackPanel.Margin = new Thickness(0, 3, 0, 3);

            StackPanel classTypeStackPanel = new StackPanel();
            Image classTypeImage = new Image();
            TextBlock classTypeTextBlock = new TextBlock();
            classTypeImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/dialog_course_type_white.png", UriKind.Absolute));
            classTypeImage.Width = 15;
            classTypeImage.Height = 15;
            classTypeTextBlock.Text = classitem.Type;
            classTypeTextBlock.FontSize = 15;
            classTypeTextBlock.Margin = new Thickness(10, 0, 0, 0);
            classTypeTextBlock.Foreground = new SolidColorBrush(Colors.White);
            classTypeStackPanel.Orientation = Orientation.Horizontal;
            classTypeStackPanel.Children.Add(classTypeImage);
            classTypeStackPanel.Children.Add(classTypeTextBlock);
            classTypeStackPanel.Margin = new Thickness(0, 3, 0, 3);

            BackStackPanel.Children.Add(classNameTextBlock);
            BackStackPanel.Children.Add(classTeaStackPanel);
            BackStackPanel.Children.Add(classAddStackPanel);
            BackStackPanel.Children.Add(classTypeStackPanel);

            BackGrid.Children.Add(BackStackPanel);

            KebiaoDayGrid.Children.Add(BackGrid);
        }


        /// <summary>
        /// 周视图课程格子的填充
        /// </summary>
        /// <param name="item">ClassList类型的item</param>
        /// <param name="ClassColor">颜色数组，0~9</param>
        private void SetClassAll(ClassList item, int ClassColor)
        {

            Color[] colors = new Color[]{
                   //Color.FromArgb(255,132, 191, 19),
                   //Color.FromArgb(255,67, 182, 229),
                   //Color.FromArgb(255,253, 137, 1),
                   //Color.FromArgb(255,128, 79, 242),
                   //Color.FromArgb(255,240, 68, 189),
                   //Color.FromArgb(255,229, 28, 35),
                   //Color.FromArgb(255,156, 39, 176),
                   //Color.FromArgb(255,3, 169, 244),
                   //Color.FromArgb(255,255, 193, 7),
                   //Color.FromArgb(255,255, 152, 0),
                   //Color.FromArgb(255,96, 125, 139),
                   Color.FromArgb(255,255, 161, 16),
                   Color.FromArgb(255,56, 188, 242),
                   Color.FromArgb(255,159, 213, 27),
                };

            TextBlock ClassTextBlock = new TextBlock();

            ClassTextBlock.Text = item.Course + "\n" + item.Classroom + "\n" + item.Teacher;
            ClassTextBlock.Foreground = new SolidColorBrush(Colors.White);
            ClassTextBlock.FontSize = 12;
            ClassTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
            ClassTextBlock.VerticalAlignment = VerticalAlignment.Center;
            ClassTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            ClassTextBlock.Margin = new Thickness(3);
            ClassTextBlock.MaxLines = 6;

            Grid BackGrid = new Grid();
            BackGrid.Background = new SolidColorBrush(colors[ClassColor]);
            BackGrid.SetValue(Grid.RowProperty, System.Int32.Parse(item.Hash_lesson * 2 + ""));
            BackGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(item.Hash_day + ""));
            BackGrid.SetValue(Grid.RowSpanProperty, System.Int32.Parse(item.Period + ""));

            BackGrid.Children.Add(ClassTextBlock);

            if (classtime[item.Hash_day, item.Hash_lesson] != null)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("ms-appx:///Assets/shape.png", UriKind.Absolute));
                img.VerticalAlignment = VerticalAlignment.Bottom;
                img.HorizontalAlignment = HorizontalAlignment.Right;
                img.Width = 10;
                BackGrid.Children.Add(img);

                string[] temp = classtime[item.Hash_day, item.Hash_lesson];
                string[] tempnew = new string[temp.Length + 1];
                for (int i = 0; i < temp.Length; i++)
                    tempnew[i] = temp[i];
                tempnew[temp.Length] = item._Id;
                classtime[item.Hash_day, item.Hash_lesson] = tempnew;
            }
            else
            {
                string[] tempnew = new string[1];
                tempnew[0] = item._Id;
                classtime[item.Hash_day, item.Hash_lesson] = tempnew;
            }

            BackGrid.Tapped += BackGrid_Tapped;
            kebiaoGrid.Children.Add(BackGrid);
        }

        private void BackGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Debug.WriteLine("前" + KBCLassFlyoutPivot.Items.Count.ToString());
            do
            {
                KBCLassFlyoutPivot.Items.RemoveAt(0);
            }
            while (KBCLassFlyoutPivot.Items.Count.ToString() != "0");
            Debug.WriteLine("删除" + KBCLassFlyoutPivot.Items.Count.ToString());
            Grid g = sender as Grid;
            Debug.WriteLine(g.GetValue(Grid.ColumnProperty));
            Debug.WriteLine(g.GetValue(Grid.RowProperty));
            string[] temp = classtime[Int32.Parse(g.GetValue(Grid.ColumnProperty).ToString()), Int32.Parse(g.GetValue(Grid.RowProperty).ToString()) / 2];
            for (int i = 0; i < temp.Length; i++)
            {
                ClassList c = classList.Find(p => p._Id.Equals(temp[i]));

                PivotItem pi = new PivotItem();
                TextBlock HeaderTextBlock = new TextBlock();
                HeaderTextBlock.Text = c.Course;
                HeaderTextBlock.FontSize = 25;
                pi.Header = HeaderTextBlock;
                ListView lv = new ListView();
                lv.ItemTemplate = KBCLassFlyoutListView.ItemTemplate;
                List<ClassList> cc = new List<ClassList>();
                cc.Add(c);
                lv.ItemsSource = cc;
                pi.Content = lv;
                KBCLassFlyoutPivot.Items.Add(pi);
                Debug.WriteLine("后" + KBCLassFlyoutPivot.Items.Count.ToString());
            }
            KBCLassFlyout.ShowAt(page);
        }

        /// <summary>
        /// 课表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.progress.IsActive = true;
            stuNum = appSetting.Values["stuNum"].ToString();
            wOa = 1;
            initKB(true);
            this.progress.IsActive = false;
        }

        /// <summary>
        /// 查询他人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBZoomAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            KBZoomFlyout.ShowAt(page);
            KBZoomFlyoutTextBox.SelectAll();
        }

        /// <summary>
        /// 切换课表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBCalendarAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            showKB(wOa);
            if (wOa == 1)
            {
                wOa = 2;
                HubSectionKBNum.Visibility = Visibility.Collapsed;
                HubSectionKBNum.Text = " | 第" + appSetting.Values["nowWeek"].ToString() + "周";
            }
            else
            {
                wOa = 1;
                HubSectionKBNum.Visibility = Visibility.Visible;
                DateTime now = DateTime.Now;
                DateTime weekstart = GetWeekFirstDayMon(now);
                DateTime weekend = GetWeekLastDaySun(now);
                HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
            }
        }



        private void KBSearchButton_Click(object sender, RoutedEventArgs e)
        {
            KBSearchButton.IsChecked = false;
            if (KBZoomFlyoutTextBox.Text != "" && KBZoomFlyoutTextBox.Text.Length == 10 && KBZoomFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                stuNum = KBZoomFlyoutTextBox.Text;
                HubSectionKBTitle.Text = stuNum + "的课表";
                initKB();
                wOa = 1;
                KBZoomFlyout.Hide();
            }
            else
                Utils.Message("请输入正确的学号");
        }

        private void HubSectionKBNum_Tapped(object sender, TappedRoutedEventArgs e)
        {
            KBNumFlyout.ShowAt(page);
            HubSectionKBNum.SelectAll();
        }

        private void KBNumSearchButton_Click(object sender, RoutedEventArgs e)
        {
            KBNumSearchButton.IsChecked = false;
            if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                showKB(2, Int16.Parse(KBNumFlyoutTextBox.Text));
                HubSectionKBNum.Text = " | 第" + KBNumFlyoutTextBox.Text + "周";
                DateTime now = DateTime.Now;
                DateTime weekstart = GetWeekFirstDayMon(KBNumFlyoutTextBox.Text == "" ? now : now.AddDays((Int16.Parse(KBNumFlyoutTextBox.Text) - Int16.Parse(appSetting.Values["nowWeek"].ToString())) * 7));
                DateTime weekend = GetWeekLastDaySun(KBNumFlyoutTextBox.Text == "" ? now : now.AddDays((Int16.Parse(KBNumFlyoutTextBox.Text) - Int16.Parse(appSetting.Values["nowWeek"].ToString())) * 7));
                this.HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
                KBNumFlyout.Hide();
            }
            else
                Utils.Message("请输入正确的周次");
        }
        private void initToday()
        {
            todaydateTextBlock.Text = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
            try
            {
                todayNumofstuTextBlock.Text = "开学第" + ((Int16.Parse(appSetting.Values["nowWeek"].ToString()) - 1) * 7 + (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek()))).ToString() + "天";
            }
            catch (Exception)
            {
                todayNumofstuTextBlock.Text = "开学第 天";

            }
        }

    }
}
