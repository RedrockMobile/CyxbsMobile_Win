using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY.Common;
using ZSCY.Data;
using ZSCY.Pages;
using ZSCY.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace ZSCY
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ApplicationDataContainer appSetting;
        private bool isExit = false;
        private int page = 1;
        private int wOa = 1;
        private string hubSectionChange = "KBHubSection";
        private string kb = "";
        private string stuNum = "";
        //private  ObservableCollection<Group>  morepageclass=new ObservableCollection<Group>();
        TextBlock[] DateOnKBTextBlock = new TextBlock[7] { new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock() };
        private ObservableDictionary morepageclass = new ObservableDictionary();
        //private  ObservableCollection<Morepageclass> morepageclass= new ObservableCollection<Morepageclass>();
        //private string[,,] classtime = new string[7, 6,*];
        string[,][] classtime = new string[7, 6][];
        private Dictionary<string, int> colorlist = new Dictionary<string, int>(); //课表格子颜色

        List<ClassList> classList = new List<ClassList>();
        ObservableCollection<NewsList> JWList = new ObservableCollection<NewsList>();
        ObservableCollection<NewsList> XWList = new ObservableCollection<NewsList>();
        ObservableCollection<NewsList> CYList = new ObservableCollection<NewsList>();
        ObservableCollection<NewsList> XSList = new ObservableCollection<NewsList>();

        double[] ScrollViewerVerticalOffset = new double[] { 0, 0, 0, 0 };

        int[] pagestatus = new int[] { 0, 0, 0, 0 };


        Grid backweekgrid = new Grid();

        IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;

        public ObservableDictionary Morepageclass
        {
            get
            {
                return morepageclass;
            }
        }

        public MainPage()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            //MoreGRNameTextBlock.Text = appSetting.Values["name"].ToString();
            //MoreGRClassTextBlock.Text = appSetting.Values["classNum"].ToString();
            //MoreGRNumTextBlock.Text = appSetting.Values["stuNum"].ToString();
            //this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            stuNum = appSetting.Values["stuNum"].ToString();
            //initKB();
            //initJW();
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
                backweekgrid.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 6 : Int16.Parse(Utils.GetWeek()) - 1));
                backweekgrid.SetValue(Grid.RowSpanProperty, 2);
                KebiaoWeekTitleGrid.Children.Remove(backweekgrid);
                KebiaoWeekTitleGrid.Children.Add(backweekgrid);

            }
            else
            {
                backweekgrid.Background = new SolidColorBrush(Color.FromArgb(255, 248, 248, 248));
                backweekgrid.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 6 : Int16.Parse(Utils.GetWeek()) - 1));
                backweekgrid.SetValue(Grid.RowSpanProperty, 2);
                KebiaoWeekTitleGrid.Children.Remove(backweekgrid);
                KebiaoWeekTitleGrid.Children.Add(backweekgrid);
            }
            TextBlock KebiaoWeek = new TextBlock();
            KebiaoWeek.Text = Utils.GetWeek(2);
            KebiaoWeek.FontSize = 20;
            KebiaoWeek.Foreground = new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
            KebiaoWeek.FontWeight = FontWeights.Light;
            KebiaoWeek.VerticalAlignment = VerticalAlignment.Center;
            KebiaoWeek.HorizontalAlignment = HorizontalAlignment.Center;
            KebiaoWeek.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 6 : Int16.Parse(Utils.GetWeek()) - 1));
            KebiaoWeek.SetValue(Grid.RowProperty, 1);
            KebiaoWeekTitleGrid.Children.Add(KebiaoWeek);
        }

        /// <summary>
        /// 课表网络请求
        /// </summary>
        /// <param name="isRefresh"> 是否为刷新</param>
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
                    HubSectionKBNum.Text = "第" + appSetting.Values["nowWeek"].ToString() + "周";
                    showKB(2);
                }
                catch (Exception) { Debug.WriteLine("主页->课表数据缓存异常"); }
            }
            if (stuNum == appSetting.Values["stuNum"].ToString())
            {
                HubSectionKBTitle.Text = "我的课表";
                HubSectionKBTitle.FontSize = 35;
            }

            await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "课表刷新中...", isIndeterminate: true);


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
                appSetting.Values["HttpTime"] = DateTimeOffset.Now.ToString();
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
                        Debug.WriteLine(appSetting.Values["HttpTime"].ToString());
                        DateTimeOffset d = DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString());
                        int httpweekday = (Int16)DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString()).DayOfWeek == 0 ? 7 : (Int16)DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString()).DayOfWeek;

                        Debug.WriteLine((DateTimeOffset.Now - DateTimeOffset.Parse(appSetting.Values["HttpTime"].ToString())).TotalDays);
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
            ShowWeekOnKB(weekstart);
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
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

        public void ShowWeekOnKB(DateTime datestart)
        {
            MonthTextBlock.Text = datestart.Month.ToString() + "月";
            for (int i = 0; i < 7; i++)
            {
                DateOnKBTextBlock[i].Text = datestart.AddDays(i).Day.ToString();
                DateOnKBTextBlock[i].FontSize = 18;
                DateOnKBTextBlock[i].Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                DateOnKBTextBlock[i].FontWeight = FontWeights.Light;
                DateOnKBTextBlock[i].VerticalAlignment = VerticalAlignment.Center;
                DateOnKBTextBlock[i].HorizontalAlignment = HorizontalAlignment.Center;
                DateOnKBTextBlock[i].SetValue(Grid.ColumnProperty, i);
                DateOnKBTextBlock[i].SetValue(Grid.RowProperty, 0);
                KebiaoWeekTitleGrid.Children.Remove(DateOnKBTextBlock[i]);
                KebiaoWeekTitleGrid.Children.Add(DateOnKBTextBlock[i]);
            }
        }

        /// <summary>
        /// 显示课表
        /// </summary>
        /// <param name="weekOrAll">1学期课表;2周课表</param>
        /// <param name="week">指定课表周次，默认0为本周</param>
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
                if (!colorlist.ContainsKey(classitem.Course))
                {
                    colorlist[classitem.Course] = ColorI;
                    ClassColor = ColorI;
                    ColorI++;
                    if (ColorI > 2)
                        ColorI = 0;
                }
                else
                {
                    ClassColor = System.Int32.Parse(colorlist[classitem.Course].ToString());
                }
                if (weekOrAll == 1)
                {
                    SetClass(classitem, ClassColor);
                    HubSectionKBNum.Visibility = Visibility.Collapsed;
                }
                else
                {
                    HubSectionKBNum.Visibility = Visibility.Visible;
                    if (week == 0)
                    {
                        if (Array.IndexOf(classitem.Week, Int32.Parse(appSetting.Values["nowWeek"].ToString())) != -1)
                        {
                            SetClass(classitem, ClassColor);
                            HubSectionKBNum.Text = "第" + appSetting.Values["nowWeek"].ToString() + "周";
                        }
                    }
                    else
                    {
                        if (Array.IndexOf(classitem.Week, week) != -1)
                        {
                            SetClass(classitem, ClassColor);
                            HubSectionKBNum.Text = "第" + week.ToString() + "周";
                        }
                    }
                }
            }
            colorlist.Clear();

        }


        /// <summary>
        /// 课程格子的填充
        /// </summary>
        /// <param name="item">ClassList类型的item</param>
        /// <param name="ClassColor">颜色数组，0~9</param>
        private void SetClass(ClassList item, int ClassColor)
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
            ClassTextBlock.Foreground = this.Foreground;
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
            KBCLassFlyout.ShowAt(MainHub);
        }

        /// <summary>
        /// 教务信息网络请求
        /// </summary>
        /// <param name="page">页码</param>
        private async void initJW(int page = 1)
        {
            int[] temp = pagestatus;
            JWListFailedStackPanel.Visibility = Visibility.Collapsed;
            JWListProgressStackPanel.Visibility = Visibility.Visible;

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            string jw = await NetWork.getHttpWebRequest("api/jwNewsList", paramList);
            Debug.WriteLine("jw->" + jw);
            JWListProgressStackPanel.Visibility = Visibility.Collapsed;
            if (jw != "")
            {
                JObject obj = JObject.Parse(jw);
                if (Int32.Parse(obj["status"].ToString()) == 200)
                {
                    JArray JWListArray = Utils.ReadJso(jw);
                    for (int i = 0; i < JWListArray.Count; i++)
                    {
                        int failednum = 0;
                        JWList JWitem = new JWList();
                        JWitem.GetListAttribute((JObject)JWListArray[i]);
                        List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
                        contentparamList.Add(new KeyValuePair<string, string>("id", JWitem.ID));
                        string jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
                        Debug.WriteLine("jwContent->" + jwContent);
                        if (temp[NewsPivot.SelectedIndex] != pagestatus[NewsPivot.SelectedIndex])
                        {
                            Debug.WriteLine("newsContent->在此退出");
                            return;
                        }
                        if (jwContent != "")
                        {
                            string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");

                            JObject jwContentobj = JObject.Parse(JWContentText);
                            if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                            {
                                JWitem.Content = jwContentobj["data"]["content"].ToString();
                                while (JWitem.Content.StartsWith("\r\n "))
                                    JWitem.Content = JWitem.Content.Substring(3);
                                while (JWitem.Content.StartsWith("\r\n"))
                                    JWitem.Content = JWitem.Content.Substring(2);
                                while (JWitem.Content.StartsWith("\n\t"))
                                    JWitem.Content = JWitem.Content.Substring(2);
                                while (JWitem.Content.StartsWith("\n"))
                                    JWitem.Content = JWitem.Content.Substring(1);
                            }
                            else
                            {
                                JWitem.Content = "";
                                failednum++;
                            }
                        }
                        else
                        {
                            failednum++;
                            if (failednum < 2)
                            {
                                jwContent = await NetWork.getHttpWebRequest("api/jwNewsContent", contentparamList);
                                Debug.WriteLine("jwContent->" + jwContent);
                                if (jwContent != "")
                                {
                                    string JWContentText = jwContent.Replace("(\r?\n(\\s*\r?\n)+)", "\r\n");
                                    JObject jwContentobj = JObject.Parse(JWContentText);
                                    if (Int32.Parse(jwContentobj["status"].ToString()) == 200)
                                    {
                                        JWitem.Content = jwContentobj["data"]["content"].ToString();
                                        while (JWitem.Content.StartsWith("\r\n "))
                                            JWitem.Content = JWitem.Content.Substring(3);
                                        while (JWitem.Content.StartsWith("\r\n"))
                                            JWitem.Content = JWitem.Content.Substring(2);
                                        while (JWitem.Content.StartsWith("\n\t"))
                                            JWitem.Content = JWitem.Content.Substring(2);
                                        while (JWitem.Content.StartsWith("\n"))
                                            JWitem.Content = JWitem.Content.Substring(1);
                                    }
                                    else
                                    {
                                        JWitem.Content = "";
                                        failednum++;
                                    }
                                }
                            }
                        }
                        //JWList.Add(new JWList { Title = JWitem.Title, Date = "时间：" + JWitem.Date, Read = "阅读量：" + JWitem.Read, Content = JWitem.Content, ID = JWitem.ID });
                    }
                    continueJWGrid.Visibility = Visibility.Visible;
                }
                else
                {
                    JWListFailedStackPanel.Visibility = Visibility.Visible;
                    continueJWGrid.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                JWListFailedStackPanel.Visibility = Visibility.Visible;
                continueJWGrid.Visibility = Visibility.Collapsed;
            }
        }

        private async void initNewsList(string type, int page = 0)
        {
            int[] temp = pagestatus;

            switch (type)
            {
                case "jwzx":
                    JWListFailedStackPanel.Visibility = Visibility.Collapsed;
                    JWListProgressStackPanel.Visibility = Visibility.Visible;
                    break;
                case "xwgg":
                    XWListFailedStackPanel.Visibility = Visibility.Collapsed;
                    XWListProgressStackPanel.Visibility = Visibility.Visible;
                    break;
                case "cyxw ":
                    CYListFailedStackPanel.Visibility = Visibility.Collapsed;
                    CYListProgressStackPanel.Visibility = Visibility.Visible;
                    break;
                case "xsjz ":
                    XSListFailedStackPanel.Visibility = Visibility.Collapsed;
                    XSListProgressStackPanel.Visibility = Visibility.Visible;
                    break;
            }

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("type", type));
            paramList.Add(new KeyValuePair<string, string>("page", page.ToString()));
            string news = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchtitle", paramList);
            Debug.WriteLine("news->" + news);

            switch (type)
            {
                case "jwzx":
                    JWListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case "xwgg":
                    XWListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case "cyxw ":
                    CYListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;
                case "xsjz ":
                    XSListProgressStackPanel.Visibility = Visibility.Collapsed;
                    break;
            }

            if (news != "")
            {
                JObject obj = JObject.Parse(news);
                if (Int32.Parse(obj["state"].ToString()) == 200)
                {
                    JArray NewsListArray = Utils.ReadJso(news);
                    //JWListView.ItemsSource = JWList;

                    for (int i = 0; i < NewsListArray.Count; i++)
                    {
                        int failednum = 0;
                        NewsList Newsitem = new NewsList();
                        Newsitem.GetListAttribute((JObject)NewsListArray[i]);
                        if (Newsitem.Title != "")
                        {

                            //请求正文
                            List<KeyValuePair<String, String>> contentparamList = new List<KeyValuePair<String, String>>();
                            contentparamList.Add(new KeyValuePair<string, string>("type", type));
                            contentparamList.Add(new KeyValuePair<string, string>("articleid", Newsitem.Articleid));
                            string newsContent = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/news/searchcontent", contentparamList);
                            //Debug.WriteLine("newsContent->" + newsContent);
                            if (temp[NewsPivot.SelectedIndex] != pagestatus[NewsPivot.SelectedIndex])
                            {
                                Debug.WriteLine("newsContent->在此退出");
                                return;
                            }
                            if (newsContent != "")
                            {
                                JObject newsContentobj = JObject.Parse(newsContent);
                                if (Int32.Parse(newsContentobj["state"].ToString()) == 200)
                                {
                                    string content = (JObject.Parse(newsContentobj["data"].ToString()))["content"].ToString();
                                    string content_all = content;
                                    Debug.WriteLine("content->" + content);
                                    try
                                    {
                                        while (content.IndexOf("<") != -1)
                                        {
                                            content = content.Remove(content.IndexOf("<"), content.IndexOf(">") - content.IndexOf("<") + 1);
                                        }
                                    }
                                    catch (Exception) { }

                                    //content.Replace("&nbsp;", "");

                                    //while (content.StartsWith("\r") || content.StartsWith("\n") || content.StartsWith("\t") || content.StartsWith(" ") || content.StartsWith("&nbsp;"))
                                    //    content = content.Substring(1);
                                    //while (content.StartsWith("&nbsp;"))
                                    //    content = content.Substring(6);
                                    content = content.Replace("\r", "");
                                    content = content.Replace("\t", "");
                                    content = content.Replace("\n", "");
                                    content = content.Replace("&nbsp;", "");
                                    content = content.Replace(" ", "");
                                    content = content.Replace("（见附件）", "见附件");
                                    content = content.Replace("MicrosoftInternetExplorer4", "");
                                    content = content.Replace("Normal07.8磅02falsefalsefalse", "");

                                    //while (content.StartsWith("\r\n "))
                                    //    content = content.Substring(3);
                                    //while (content.StartsWith("\r\n"))
                                    //    content = content.Substring(2);
                                    //while (content.StartsWith("\n\t"))
                                    //    content = content.Substring(2);
                                    //while (content.StartsWith("\n"))
                                    //    content = content.Substring(1);
                                    //while (content.StartsWith("\r"))
                                    //    content = content.Substring(1);
                                    //while (content.StartsWith("\t"))
                                    //    content = content.Substring(1);
                                    //while (content.StartsWith("\\"))
                                    //    content = content.Substring(1);
                                    //content.Replace('\r', '\a');
                                    //content.Replace('\n', '\a');
                                    //content.Replace(" ", "");
                                    Debug.WriteLine("content->" + content);
                                    switch (type)
                                    {
                                        case "jwzx":
                                            JWList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;
                                        case "xwgg":
                                            XWList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;
                                        case "cyxw ":
                                            CYList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;
                                        case "xsjz ":
                                            XSList.Add(new NewsList { Title = Newsitem.Title, Date = Newsitem.Date, Read = Newsitem.Read, Content = content, Content_all = newsContent, ID = Newsitem.ID });
                                            break;
                                    }
                                }
                            }

                        }
                    }
                    //JWListView.ItemsSource = JWList;
                    switch (type)
                    {
                        case "jwzx":
                            continueJWGrid.Visibility = Visibility.Visible;
                            break;
                        case "xwgg":
                            continueXWGrid.Visibility = Visibility.Visible;
                            break;
                        case "cyxw ":
                            continueCYGrid.Visibility = Visibility.Visible;
                            break;
                        case "xsjz ":
                            continueXSGrid.Visibility = Visibility.Visible;
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case "jwzx":
                            JWListFailedStackPanel.Visibility = Visibility.Visible;
                            continueJWGrid.Visibility = Visibility.Collapsed;
                            break;
                        case "xwgg":
                            XWListFailedStackPanel.Visibility = Visibility.Visible;
                            continueXWGrid.Visibility = Visibility.Collapsed;
                            break;
                        case "cyxw ":
                            CYListFailedStackPanel.Visibility = Visibility.Visible;
                            continueCYGrid.Visibility = Visibility.Collapsed;
                            break;
                        case "xsjz ":
                            XSListFailedStackPanel.Visibility = Visibility.Visible;
                            continueXSGrid.Visibility = Visibility.Collapsed;
                            break;
                    }
                }
            }
            else
            {
                switch (type)
                {
                    case "jwzx":
                        JWListFailedStackPanel.Visibility = Visibility.Visible;
                        continueJWGrid.Visibility = Visibility.Collapsed;
                        break;
                    case "xwgg":
                        XWListFailedStackPanel.Visibility = Visibility.Visible;
                        continueXWGrid.Visibility = Visibility.Collapsed;
                        break;
                    case "cyxw ":
                        CYListFailedStackPanel.Visibility = Visibility.Visible;
                        continueCYGrid.Visibility = Visibility.Collapsed;
                        break;
                    case "xsjz ":
                        XSListFailedStackPanel.Visibility = Visibility.Visible;
                        continueXSGrid.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }

        //private void JWListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    initJW();
        //}
        private void NewsListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string type = "";
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    break;
                case 1:
                    type = "xwgg";
                    break;
                case 2:
                    type = "cyxw ";
                    break;
                case 3:
                    type = "xsjz ";
                    break;
            }
            initNewsList(type);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //JWListView.ItemsSource = JWList;
            //XWListView.ItemsSource = XWList;
            //CYListView.ItemsSource = CYList;
            //XSListView.ItemsSource = XSList;
            UmengSDK.UmengAnalytics.TrackPageStart("MainPage");
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
                                                                       //this.navigationHelper.OnNavigatedTo(e);
            var group = await DataSource.Get();
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.New)
            {

                this.Morepageclass["Group"] = group;
                initKB();
            }

            await Task.Delay(1);
            JWListView.ItemsSource = JWList;
            XWListView.ItemsSource = XWList;
            CYListView.ItemsSource = CYList;
            XSListView.ItemsSource = XSList;
            JWScrollViewer.ChangeView(0, ScrollViewerVerticalOffset[0], 1);
            XWScrollViewer.ChangeView(0, ScrollViewerVerticalOffset[1], 1);
            CYScrollViewer.ChangeView(0, ScrollViewerVerticalOffset[2], 1);
            XSScrollViewer.ChangeView(0, ScrollViewerVerticalOffset[3], 1);

            //PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            // 如果本地设置中没有相关键，表明是第一次使用
            // 需要存储URL，并发送给服务器
            //if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey("url")==false)
            //{
            //    Windows.Storage.ApplicationData.Current.LocalSettings.Values["url"] = channel.Uri;
            //    SendURL(channel.Uri);
            //}
            //else
            //{
            //    string savedUrl = Windows.Storage.ApplicationData.Current.LocalSettings.Values["url"] as string;
            //    // 当URL改变了，就重新发给服务器
            //    if (savedUrl != channel.Uri)
            //    {
            //        // 再次保存本地设置
            //        Windows.Storage.ApplicationData.Current.LocalSettings.Values["url"] = channel.Uri;
            //        SendURL(channel.Uri);
            //    }
            //}

            //System.Diagnostics.Debug.WriteLine(channel.Uri);
            //SendURL(channel.Uri);

        }

        /// <summary>
        /// 通道URL发送给服务器
        /// </summary>
        /// <param name="url"></param>
        private async void SendURL(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(url);
                ByteArrayContent content = new ByteArrayContent(data);
                try
                {
                    await client.PostAsync("http://113.251.216.116/svr/", content);
                    Debug.WriteLine(content);
                }
                catch { }
            }
        }

        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var group = await DataSource.Get();
            this.Morepageclass["Group"] = group;
            //more.Margin = new Thickness(0, 0, 0, -Utils.getPhoneHeight() + 300);
            //this.morepageclass = (ObservableCollection<Group>) @group;
            //this.MoreHubSection.DataContext = Morepageclass;
            //IEnumerable<Group> g =group;
            //var a=g.ToArray();
            //for (int i = 0; i < a[0].items.Count; i++)
            //{
            //    morepageclass.Add(a[0].items[i]);
            //}
            //this.MoreHubSection.DataContext = morepageclass;
            //this.fuck.ItemsSource = morepageclass;


        }
        //离开页面时，取消事件
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            UmengSDK.UmengAnalytics.TrackPageEnd("MainPage");
            await statusBar.ProgressIndicator.HideAsync();
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
            //switch (NewsPivot.SelectedIndex)
            //{
            //    case 0:
            //        XWListView.ItemsSource = null;
            //        CYListView.ItemsSource = null;
            //        XSListView.ItemsSource = null;
            //        break;
            //    case 1:
            //        JWListView.ItemsSource = null;
            //        CYListView.ItemsSource = null;
            //        XSListView.ItemsSource = null;
            //        break;
            //    case 2:
            //        JWListView.ItemsSource = null;
            //        XWListView.ItemsSource = null;
            //        XSListView.ItemsSource = null;
            //        break;
            //    case 3:
            //        JWListView.ItemsSource = null;
            //        XWListView.ItemsSource = null;
            //        CYListView.ItemsSource = null;
            //        break;
            //}

            ScrollViewerVerticalOffset[0] = JWScrollViewer.VerticalOffset;
            ScrollViewerVerticalOffset[1] = XWScrollViewer.VerticalOffset;
            ScrollViewerVerticalOffset[2] = CYScrollViewer.VerticalOffset;
            ScrollViewerVerticalOffset[3] = XSScrollViewer.VerticalOffset;

            JWListView.ItemsSource = null;
            XWListView.ItemsSource = null;
            CYListView.ItemsSource = null;
            XSListView.ItemsSource = null;
            //this.navigationHelper.OnNavigatedFrom(e);

        }

        private async void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
            e.Handled = true;
            Color.FromArgb(255, 2, 140, 253);
            if (!isExit)
            {
                await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "再次点击返回键退出...");
                isExit = true;
                await Task.Delay(2000);
                StatusBar statusBar = StatusBar.GetForCurrentView();
                isExit = false;
                await statusBar.ProgressIndicator.HideAsync();
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private void JiaowuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            JWList JWItem = new JWList(((JWList)e.ClickedItem).ID, ((JWList)e.ClickedItem).Title, ((JWList)e.ClickedItem).Date, ((JWList)e.ClickedItem).Read, ((JWList)e.ClickedItem).Content == null ? "加载中..." : ((JWList)e.ClickedItem).Content);

            this.Frame.Navigate(typeof(JWContentPage), JWItem);
        }

        private void MainHub_SectionsInViewChanged(object sender, SectionsInViewChangedEventArgs e)
        {
            var hubSection = MainHub.SectionsInView[0];
            Debug.WriteLine(hubSection.Name);
            CommandBar commandbar = ((CommandBar)this.BottomAppBar);
            if (hubSection.Name != hubSectionChange)
            {
                switch (hubSection.Name)
                {
                    case "KBHubSection":
                        //MoreBlueGRGrid.Opacity = 0;

                        KBRefreshAppBarButton.Visibility = Visibility.Visible;
                        KBZoomAppBarButton.Visibility = Visibility.Visible;
                        KBCalendarAppBarButton.Visibility = Visibility.Visible;
                        NewsRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        MoreSwitchAppBarButton.Visibility = Visibility.Collapsed;
                        break;
                    case "JWHubSection":
                        // MoreBlueGRGrid.Opacity = 0;

                        KBRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        KBZoomAppBarButton.Visibility = Visibility.Collapsed;
                        KBCalendarAppBarButton.Visibility = Visibility.Collapsed;
                        NewsRefreshAppBarButton.Visibility = Visibility.Visible;
                        MoreSwitchAppBarButton.Visibility = Visibility.Collapsed;
                        //switch (NewsPivot.SelectedIndex)
                        //{
                        //    case 0:
                        //        JWListView.ItemsSource = JWList;
                        //        break;
                        //    case 1:
                        //        XWListView.ItemsSource = XWList;
                        //        break;
                        //    case 2:
                        //        CYListView.ItemsSource = CYList;
                        //        break;
                        //    case 3:
                        //        XSListView.ItemsSource = XSList;
                        //        break;
                        //}
                        break;
                    case "MoreHubSection":
                        //MoreGRGrid.Margin = new Thickness(-20,0,0,0);
                        //MoveMoreBlueGRGrid.Begin();
                        //MoveMoreBlueGRGrid2.Begin();

                        KBRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        KBZoomAppBarButton.Visibility = Visibility.Collapsed;
                        KBCalendarAppBarButton.Visibility = Visibility.Collapsed;
                        NewsRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        MoreSwitchAppBarButton.Visibility = Visibility.Visible;
                        break;
                }
            }
            hubSectionChange = hubSection.Name;
        }

        /// <summary>
        /// 课表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            stuNum = appSetting.Values["stuNum"].ToString();
            wOa = 1;
            initKB(true);
        }

        /// <summary>
        /// 查询他人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBZoomAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            KBZoomFlyout.ShowAt(MainHub);
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
            DateTime now = DateTime.Now;
            DateTime weekstart = GetWeekFirstDayMon(now);
            DateTime weekend = GetWeekLastDaySun(now);
            ShowWeekOnKB(weekstart);
            if (wOa == 1)
            {
                wOa = 2;
                HubSectionKBNum.Visibility = Visibility.Collapsed;
                HubSectionKBDate.Text = "学期课表";
                HubSectionKBNum.Text = "第" + appSetting.Values["nowWeek"].ToString() + "周";
            }
            else
            {
                wOa = 1;
                HubSectionKBNum.Visibility = Visibility.Visible;
                HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
            }
        }

        /// <summary>
        /// 教务刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void JWRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        //{
        //    page = 1;
        //    JWList.Clear();
        //    continueJWGrid.Visibility = Visibility.Collapsed;
        //    initJW();
        //}
        private void NewsRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {

            page = 0;
            string type = "";
            pagestatus[NewsPivot.SelectedIndex]++;

            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    JWList.Clear();
                    continueJWGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "xwgg";
                    XWList.Clear();
                    continueXWGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "cyxw ";
                    CYList.Clear();
                    continueCYGrid.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    type = "xsjz ";
                    XSList.Clear();
                    continueXSGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            initNewsList(type);
        }


        /// <summary>
        /// 切换账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreSwitchAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // appSetting.Values.Remove("idNum");
            Frame.Navigate(typeof(PersonPage));
        }

        private void KBSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (KBZoomFlyoutTextBox.Text != "" && KBZoomFlyoutTextBox.Text.Length == 10 && KBZoomFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                stuNum = KBZoomFlyoutTextBox.Text;
                HubSectionKBTitle.Text = stuNum + "的课表";
                HubSectionKBTitle.FontSize = 30;
                initKB();
                wOa = 1;
                KBZoomFlyout.Hide();
            }
            else
                Utils.Message("请输入正确的学号");
        }
        private void HubSectionKBNum_Tapped(object sender, TappedRoutedEventArgs e)
        {
            KBNumFlyout.ShowAt(MainHub);
            HubSectionKBNum.SelectAll();
        }
        private void KBNumSearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                showKB(2, Int16.Parse(KBNumFlyoutTextBox.Text));
                HubSectionKBNum.Text = "第" + KBNumFlyoutTextBox.Text + "周";
                DateTime now = DateTime.Now;
                DateTime weekstart = GetWeekFirstDayMon(KBNumFlyoutTextBox.Text == "" ? now : now.AddDays((Int16.Parse(KBNumFlyoutTextBox.Text) - Int16.Parse(appSetting.Values["nowWeek"].ToString())) * 7));
                DateTime weekend = GetWeekLastDaySun(KBNumFlyoutTextBox.Text == "" ? now : now.AddDays((Int16.Parse(KBNumFlyoutTextBox.Text) - Int16.Parse(appSetting.Values["nowWeek"].ToString())) * 7));
                this.HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
                ShowWeekOnKB(weekstart);
                KBNumFlyout.Hide();
            }
            else
                Utils.Message("请输入正确的周次");
        }


        //private void continueJWGrid_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    page++;
        //    initJW(page);
        //}

        private void continueNewsGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            page++;
            string type = "";
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    continueJWGrid.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    type = "xwgg";
                    continueXWGrid.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    type = "cyxw ";
                    continueCYGrid.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    type = "xsjz ";
                    continueXSGrid.Visibility = Visibility.Collapsed;
                    break;
            }
            initNewsList(type, page);
        }

        private void NewsPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string type = "";
            switch (NewsPivot.SelectedIndex)
            {
                case 0:
                    type = "jwzx";
                    JWListView.ItemsSource = JWList;
                    XWListView.ItemsSource = null;
                    CYListView.ItemsSource = null;
                    XSListView.ItemsSource = null;
                    JWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 11, 11));
                    XWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    CYTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    XSTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    break;
                case 1:
                    type = "xwgg";
                    XWListView.ItemsSource = XWList;
                    JWListView.ItemsSource = null;
                    CYListView.ItemsSource = null;
                    XSListView.ItemsSource = null;
                    JWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    XWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 11, 11));
                    CYTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    XSTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    break;
                case 2:
                    type = "cyxw ";
                    CYListView.ItemsSource = CYList;
                    JWListView.ItemsSource = null;
                    XWListView.ItemsSource = null;
                    XSListView.ItemsSource = null;
                    JWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    XWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    CYTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 11, 11));
                    XSTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    break;
                case 3:
                    type = "xsjz ";
                    XSListView.ItemsSource = XSList;
                    JWListView.ItemsSource = null;
                    XWListView.ItemsSource = null;
                    CYListView.ItemsSource = null;
                    JWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    XWTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    CYTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 66, 66, 66));
                    XSTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 11, 11, 11));
                    break;
            }
            if (pagestatus[NewsPivot.SelectedIndex] == 0)
            {
                pagestatus[NewsPivot.SelectedIndex]++;
                initNewsList(type);
            }
        }
        private async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as Morepageclass;
            Debug.WriteLine(item.UniqueID);
            switch (item.UniqueID)
            {
                case "Setting":
                    Frame.Navigate(typeof(SettingPage)); break;
                case "ReExam": Frame.Navigate(typeof(ExamPage), 3); break;
                case "Exam": Frame.Navigate(typeof(ExamPage), 2); break;
                case "Socre": Frame.Navigate(typeof(ScorePage)); break;
                case "ClassRoom":
                    Frame.Navigate(typeof(EmptyRoomsPage));
                    break;
                case "Calendar":
                    Frame.Navigate(typeof(CalendarPage));
                    break;
                case "FreeTime":
                    Frame.Navigate(typeof(SearchFreeTimeNumPage));
                    break;
                case "Card":
                    var a = await Launcher.LaunchUriAsync(new Uri("cquptcard:"));
                    Debug.WriteLine(a);
                    break;
                default:
                    break;
            }
        }

        private void NewsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            NewsList newsItem = new NewsList(((NewsList)e.ClickedItem).ID, ((NewsList)e.ClickedItem).Articleid, ((NewsList)e.ClickedItem).Title, ((NewsList)e.ClickedItem).Head, ((NewsList)e.ClickedItem).Date, ((NewsList)e.ClickedItem).Read, ((NewsList)e.ClickedItem).Content == null ? "加载中..." : ((NewsList)e.ClickedItem).Content, ((NewsList)e.ClickedItem).Content_all == null ? "加载中..." : ((NewsList)e.ClickedItem).Content_all);
            Frame.Navigate(typeof(NewsContentPage), newsItem);
        }
    }
}
