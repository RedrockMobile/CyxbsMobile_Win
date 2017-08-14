using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchFreeTimeResultPage_new : Page
    {
        private ObservableCollection<uIdList> muIdList = new ObservableCollection<uIdList>(); //存放学号的List

        //private List<List<ClassListLight>> forsearchlist = new List<List<ClassListLight>>();//存放查到学号的课
        private Dictionary<string, List<ClassListLight>> forsearchlist = new Dictionary<string, List<ClassListLight>>();

        private ObservableCollection<ClassListLight> result = new ObservableCollection<ClassListLight>();
        private ObservableCollection<EmptyTable> termresult = new ObservableCollection<EmptyTable>();
        private ObservableCollection<People> peoplelist = new ObservableCollection<People>();
        private Dictionary<string, int> colorlist = new Dictionary<string, int>();
        private bool weekorterm = false;//查询的是周还是学期，周false,学期true;
        public bool IsBusy = false;
        private string[,][] ResultName = new string[7, 6][];
        private int week; //周次，-100为学期
        private int week_old; //周次，切换到学期后保存切换前的周次
        private bool showWeekend = false;

        #region 一些方法太多了。。

        private ApplicationDataContainer appSetting;

        public SearchFreeTimeResultPage_new()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 600)
                {
                    KBCLassFlyout.Hide();
                    state = "VisualState600";
                }
                //else
                //{
                //    if(FreeDetailStackPanel.Visibility ==Visibility.Visible)
                //        KBCLassFlyout.ShowAt(FreeDetailGrid);
                //}
                FreeDetailNameListView.Height = e.NewSize.Height - 50 - 40 - 10 - 20 - 48;
                KebiaoAllScrollViewer.Height = e.NewSize.Height - 48 - 25;
                cutoffLine.Y2 = e.NewSize.Height;
                VisualStateManager.GoToState(this, state, true);
            };
            week_old = week = int.Parse(appSetting.Values["nowWeek"].ToString());
            FilterAppBarButton.Label = "第" + appSetting.Values["nowWeek"].ToString() + "周";
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AuIdList auIdList = (AuIdList)e.Parameter;
            muIdList = auIdList.muIdList;
            FreeDetailNameListView.ItemsSource = peoplelist;
            FlyoutFreeDetailNameGridView.ItemsSource = peoplelist;
            initFree();
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
        }

        /// <summary>
        /// 获得各个学号的课表
        /// </summary>
        private async void initFree()
        {
            for (int i = 0; i < muIdList.Count; i++)
            {
                int issuccess = 0;
                List<ClassListLight> clist = new List<ClassListLight>();
                while (issuccess < 2) //失败后重试两次
                {
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("stuNum", muIdList[i].uId));
                    string kbtemp = await NetWork.getHttpWebRequest("redapi2/api/kebiao", paramList); //新
                    if (kbtemp != "")
                    {
                        JObject job = JObject.Parse(kbtemp);
                        if (Int32.Parse(job["status"].ToString()) == 200)
                        {
                            JArray jarry = Utils.ReadJso(kbtemp);
                            for (int j = 0; j < jarry.Count; j++)
                            {
                                ClassListLight cll = new ClassListLight();
                                var istriple = cll.getattribute((JObject)jarry[j]);
                                cll.Name = new string[] { muIdList[i].uName }; // muIdList[i].uName;
                                if (istriple != null)
                                {
                                    clist.Add(istriple);
                                    istriple.Name = cll.Name;
                                }
                                clist.Add(cll);
                            }
                        }
                        issuccess = 2;
                    }
                    else
                    {
                        issuccess++;
                    }
                }
                forsearchlist.Add(muIdList[i].uName, clist);
                FreeLoddingProgressBar.Value = FreeLoddingProgressBar.Value + 100.0 / muIdList.Count;
                Debug.WriteLine(FreeLoddingProgressBar.Value);
            }
            //查无课表，参数第几周==
            EmptyClass ec = new EmptyClass(week, forsearchlist);
            //ec.getfreetime(result, termresult);
            // await ec.getfreetimeasync(result, termresult);
            await GetData(ec);
            //freetime(11, forsearchlist);
            FreeLoddingStackPanel.Visibility = Visibility.Collapsed;
            FreeKBTableGrid.Visibility = Visibility.Visible;
            ShowWeekendAppBarButton.Visibility = Visibility.Visible;
            if (result.Count != 0)
                showFreeKB(result);
            else
                showFreeKB(termresult);
        }

        private void showFreeKB(ObservableCollection<ClassListLight> result, bool showWeekend = false)
        {
            kebiaoGrid.Children.Clear();
            int ClassColor = 0;
            colorlist.Clear();

            for (int i = 0; i < result.Count; i++)
            {
                peoplelist.Clear();
                string nametemp = "";
                for (int j = 0; j < result[i].Name.Length; j++)
                {
                    nametemp = nametemp + result[i].Name[j];
                    //peoplelist.Add(new People { name = result[i].Name[j] });
                }
                if (!colorlist.ContainsKey(nametemp))
                {
                    if (result[i].Hash_day <= 4 || showWeekend)
                        SetClassAll(result[i], ClassColor);
                    colorlist.Add(nametemp, ClassColor);
                    ClassColor = (ClassColor + 1) % 4;
                }
                else
                {
                    if (result[i].Hash_day <= 4 || showWeekend)
                        SetClassAll(result[i], colorlist[nametemp]);
                }
            }
        }

        /// <summary>
        /// 给每个时间指定颜色，根据名字，尽量不同。。。
        /// </summary>
        /// <param name="termresult"></param>
        /// <param name="showWeekend"></param>
        private void showFreeKB(ObservableCollection<EmptyTable> termresult, bool showWeekend = false)
        {
            kebiaoGrid.Children.Clear();
            int ClassColor = 0;
            colorlist.Clear();
            colorlist.Clear();
            for (int i = 0; i < termresult.Count; i++)
            {
                peoplelist.Clear();
                string nametemp = "";
                string[] names = termresult[i].nameweek.Keys.ToArray();
                for (int j = 0; j < names.Length; j++)
                {
                    nametemp = nametemp + names[j];
                    //peoplelist.Add(new People { name = result[i].Name[j] });
                }
                if (!colorlist.ContainsKey(nametemp))
                {
                    if (termresult[i].Hash_day <= 4 || showWeekend)
                        SetClassAll(termresult[i], ClassColor);
                    colorlist.Add(nametemp, ClassColor);
                    ClassColor = (ClassColor + 1) % 4;
                }
                else
                {
                    if (termresult[i].Hash_day <= 4 || showWeekend)
                        SetClassAll(termresult[i], colorlist[nametemp]);
                }
            }
        }

        /// <summary>
        /// 给一个课程格子设置颜色，并添加到视图中
        /// </summary>
        /// <param name="setcoloritem"></param>
        /// <param name="ClassColor"></param>
        private void SetClassAll(EmptyClassDayLesson setcoloritem, int ClassColor)
        {
            Color[] colors = new Color[]{
                   Color.FromArgb(255,88, 179, 255),//蓝
                   Color.FromArgb(255,255, 181, 68),//黄
                   Color.FromArgb(255,172, 222, 76),//绿
                   Color.FromArgb(255,249, 130, 130),//红
                };
            int day = setcoloritem.Hash_day;
            int lesson = setcoloritem.Hash_lesson;
            TextBlock ClassTextBlock = new TextBlock();
            if (setcoloritem is ClassListLight)
            {
                ClassListLight item = setcoloritem as ClassListLight;
                ResultName[day, lesson] = item.Name;
                foreach (var nameitem in ResultName[day, lesson])
                {
                    ClassTextBlock.Text = ClassTextBlock.Text + "\n" + nameitem;
                }
            }
            if (setcoloritem is EmptyTable)
            {
                EmptyTable item = setcoloritem as EmptyTable;
                ResultName[day, lesson] = item.nameweek.Keys.ToArray();
                foreach (var nameitem in ResultName[day, lesson])
                {
                    if (item.nameweek[nameitem].Length != 0)
                    {
                        ClassTextBlock.Text = ClassTextBlock.Text + "\n" + nameitem + "\r";
                        ClassTextBlock.Text = ClassTextBlock.Text + WeeknumConverter(item.nameweek[nameitem]);
                    }
                }
            }
            ClassTextBlock.Foreground = new SolidColorBrush(Colors.White);
            ClassTextBlock.FontSize = 12;
            ClassTextBlock.TextWrapping = TextWrapping.WrapWholeWords;
            ClassTextBlock.VerticalAlignment = VerticalAlignment.Center;
            ClassTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            if (Utils.getPhoneWidth() < 333)
                ClassTextBlock.Margin = new Thickness(1);
            else
                ClassTextBlock.Margin = new Thickness(3);
            ClassTextBlock.MaxLines = 6;

            Grid BackGrid = new Grid();
            BackGrid.Background = new SolidColorBrush(colors[ClassColor]);
            BackGrid.SetValue(Grid.RowProperty, System.Int32.Parse(lesson * 2 + ""));
            BackGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(day + ""));
            BackGrid.SetValue(Grid.RowSpanProperty, 2);
            BackGrid.Margin = new Thickness(0.5);

            if ((setcoloritem is ClassListLight) && ResultName[day, lesson].Length == muIdList.Count)
            {
                TextBlock AllPeopleTextBlock = new TextBlock();
                AllPeopleTextBlock.Foreground = new SolidColorBrush(Colors.White);
                AllPeopleTextBlock.FontSize = 11;
                AllPeopleTextBlock.Text = "全";
                AllPeopleTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                AllPeopleTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                AllPeopleTextBlock.Margin = new Thickness(1);
                //BackGrid.Background = new SolidColorBrush(Color.FromArgb(255, 88, 179, 255));
                BackGrid.Children.Add(AllPeopleTextBlock);
            }
            //else
            //    BackGrid.Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
            BackGrid.Children.Add(ClassTextBlock);
            BackGrid.Tapped += BackGrid_Tapped;
            kebiaoGrid.Children.Add(BackGrid);
        }

        private void BackGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            peoplelist.Clear();
            Grid g = sender as Grid;
            Debug.WriteLine(g.GetValue(Grid.ColumnProperty));
            Debug.WriteLine(g.GetValue(Grid.RowProperty));
            int day = Int32.Parse(g.GetValue(Grid.ColumnProperty).ToString());
            int lesson = Int32.Parse(g.GetValue(Grid.RowProperty).ToString()) / 2;
            string[] temp = ResultName[day, lesson];
            string week = "";
            string nowclass = "";
            string time = "";
            switch (day.ToString())
            {
                case "0":
                    week = "一";
                    break;

                case "1":
                    week = "二";
                    break;

                case "2":
                    week = "三";
                    break;

                case "3":
                    week = "四";
                    break;

                case "4":
                    week = "五";
                    break;

                case "5":
                    week = "六";
                    break;

                case "6":
                    week = "日";
                    break;
            }
            switch (lesson)
            {
                case 0:
                    nowclass = "1-2节";
                    time = "8:00 - 9:40 AM";
                    break;

                case 1:
                    nowclass = "3-4节";
                    time = "10:05 - 11:45 AM";
                    break;

                case 2:
                    nowclass = "5-6节";
                    time = "14:00 - 15:40 PM";
                    break;

                case 3:
                    nowclass = "7-8节";
                    time = "16:05 - 17:45 PM";
                    break;

                case 4:
                    nowclass = "9-10节";
                    time = "19:00 - 20:40 PM";
                    break;

                case 5:
                    nowclass = "11-12节";
                    time = "20:50 - 22:30 PM";
                    break;
            }
            FreeDetailWeekTextBlock.Text = "星期" + week + " " + nowclass;
            FreeDetailTimeTextBlock.Text = time;
            FreeDetailPeopleTextBlock.Text = "共计" + temp.Length + "人";

            FlyoutFreeDetailWeekTextBlock.Text = "星期" + week + " " + nowclass;
            FlyoutFreeDetailTimeTextBlock.Text = time;
            FlyoutFreeDetailPeopleTextBlock.Text = "共计" + temp.Length + "人";
            if (!weekorterm)
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    peoplelist.Add(new People { name = temp[i] });
                }
            }
            else
            {
                for (int i = 0; i < temp.Length; i++)
                {
                    int[] weeks = (from n in termresult where n.Hash_day == day && n.Hash_lesson == lesson select n.nameweek[temp[i]]).ToArray()[0];
                    string weekstr = "";
                    if (weeks.Length != 0)
                        weekstr = WeeknumConverter(weeks);
                    peoplelist.Add(new People { name = temp[i], weekstostr = weekstr });
                }
            }
            if (FreeDetailGrid.Visibility == Visibility.Collapsed)
                KBCLassFlyout.ShowAt(commandBar);
            FreeNoClickStackPanel.Visibility = Visibility.Collapsed;
            FreeDetailStackPanel.Visibility = Visibility.Visible;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
                return;
            if (rootFrame.CanGoBack && e.Handled == false)
            {
                e.Handled = true;
                rootFrame.GoBack();
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void FilterAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            KBNumFlyout.ShowAt(commandBar);
        }

        /// <summary>
        /// 周次选择的搜索键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBNumSearchButton_Click(object sender, RoutedEventArgs e)
        {
            weekorterm = false;
            KBNumSearch();
        }

        /// <summary>
        /// 课表选择页面对回车键的监听
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBNumFlyoutTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("enter");
                if (KBNumFlyoutTextBox.Text != "")
                    KBNumSearch();
                else
                {
                    Utils.Message("信息不完全");
                }
            }
        }

        /// <summary>
        /// 输入周查课表
        /// </summary>
        private async void KBNumSearch()
        {
            if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                try
                {
                    week = int.Parse(KBNumFlyoutTextBox.Text);
                    FilterAppBarButton.Label = "第" + KBNumFlyoutTextBox.Text + "周";
                    KBNumFlyout.Hide();
                    ResultName = new string[7, 6][];
                    EmptyClass ec = new EmptyClass(week, forsearchlist);
                    result = new ObservableCollection<ClassListLight>();
                    //ec.getfreetime(result, termresult);
                    //await ec.getfreetimeasync(result, termresult);
                    await GetData(ec);
                    showFreeKB(result, showWeekend);
                }
                catch (Exception)
                {
                    Utils.Message("请输入正确的周次");
                }
            }
            else
                Utils.Message("请输入正确的周次");
        }

        /// <summary>
        /// 学期和周的切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CalendarWeekAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (week != -100)
            {
                week_old = week;
                week = -100;
                weekorterm = true;
                FilterAppBarButton.Visibility = Visibility.Collapsed;
                ResultName.Initialize();
                EmptyClass ec = new EmptyClass(week, forsearchlist);
                //ec.getfreetime(result, termresult);
                //await ec.getfreetimeasync(result, termresult);
                Debug.WriteLine(DateTime.Now.Second + "---开始学期---" + DateTime.Now.Millisecond);
                await GetData(ec);
                Debug.WriteLine(DateTime.Now.Second + "---结束学期---" + DateTime.Now.Millisecond);
                showFreeKB(termresult, showWeekend);
            }
            else
            {
                weekorterm = false;
                week = week_old;
                FilterAppBarButton.Label = "第" + week + "周";
                FilterAppBarButton.Visibility = Visibility.Visible;
                ResultName.Initialize();
                EmptyClass ec = new EmptyClass(week, forsearchlist);
                //ec.getfreetime(result, termresult);
                //await ec.getfreetimeasync(result, termresult);
                Debug.WriteLine(DateTime.Now.Second + "---开始学期---" + DateTime.Now.Millisecond);
                await GetData(ec);
                Debug.WriteLine(DateTime.Now.Second + "--完成学期---" + DateTime.Now.Millisecond);
                showFreeKB(result, showWeekend);
            }
        }

        /// <summary>
        /// 显示隐藏周末
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ShowWeekendAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (showWeekend)
                ShowWeekendAppBarButton.Label = "显示周末课表";
            else
                ShowWeekendAppBarButton.Label = "隐藏周末课表";
            showWeekend = !showWeekend;
            EmptyClass ec = new EmptyClass(week, forsearchlist);
            //ec.getfreetime(result, termresult); if (week != -100)
            //await ec.getfreetimeasync(result, termresult);
            await GetData(ec);
            if (week != -100)
            {
                showFreeKB(result, showWeekend);
            }
            else
            {
                showFreeKB(termresult, showWeekend);
            }
        }

        /// <summary>
        /// 周数友好显示
        /// </summary>
        /// <param name="weeks">空闲周数组</param>
        /// <returns>友好的字符串</returns>
        private string WeeknumConverter(int[] weeks)
        {
            int len = weeks.Length;
            Array.Sort(weeks);
            if (len == 18)
            {
                return "";
            }
            else if (weeks.All(x => x % 2 == 0) && weeks[0] == 2 && weeks[weeks.Length - 1] == 18)
            {
                return "双周";
            }
            else if (weeks.All(x => x % 2 == 2) && weeks[0] == 1 && weeks[weeks.Length - 1] == 17)
            {
                return "单周";
            }
            else if (weeks[len - 1] - weeks[0] == len - 1 && len != 1)
            {
                return $"{weeks[0]}-{weeks[len - 1]}周";
            }
            else if (weeks.Length < 9)
            {
                string r = string.Empty;
                for (int i = 0; i < weeks.Length; i++)
                {
                    r += $"{weeks[i]} ";
                }
                return r + "周";
            }
            else
            {
                int[] allweeks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 };
                int[] exceptweeks = allweeks.Except(weeks).ToArray();
                Array.Sort(exceptweeks);
                if (exceptweeks.Length == 1)
                {
                    return $"除{exceptweeks[0]}周";
                }
                else if (exceptweeks[exceptweeks.Length - 1] - exceptweeks[0] == exceptweeks.Length - 1)
                {
                    return $"除{exceptweeks[0]}-{exceptweeks[exceptweeks.Length - 1]}周";
                }
                else
                {
                    string r = string.Empty;
                    for (int i = 0; i < exceptweeks.Length; i++)
                    {
                        r += $"{exceptweeks[i]} ";
                    }
                    return "除" + r + "周";
                }
            }
        }

        #endregion 一些方法太多了。。

        private async Task GetData(EmptyClass ec)
        {
            IsBusy = true;
            Func<ObservableCollection<ClassListLight>> calcw = () =>
            {
                ObservableCollection<ClassListLight> w = new ObservableCollection<ClassListLight>();
                w = ec.getweekresult();
                return w;
            };
            Func<ObservableCollection<EmptyTable>> calct = () =>
            {
                ObservableCollection<EmptyTable> t = new ObservableCollection<EmptyTable>();
                t = ec.gettermresult();
                return t;
            };
            if (ec.Weeknum != -100)
            {
                Debug.WriteLine(DateTime.Now.Second + "开始0异步" + DateTime.Now.Millisecond);
                this.result = await Task.Run(calcw);
                Debug.WriteLine(DateTime.Now.Second + "结束0异步" + DateTime.Now.Millisecond);
            }
            else
            {
                Debug.WriteLine(DateTime.Now.Second + "开始1异步" + DateTime.Now.Millisecond);
                this.termresult = await Task.Run(calct);
                Debug.WriteLine(DateTime.Now.Second + "结束1异步" + DateTime.Now.Millisecond);
            }
            IsBusy = false;
        }
    }
}