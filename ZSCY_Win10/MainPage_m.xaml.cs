using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY.Pages;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage_m : Page
    {
        private static string resourceName = "ZSCY";
        private ApplicationDataContainer appSetting;
        private ApplicationDataContainer appSettingclass;
        private bool isExit = false;
        private int page = 1;
        private int wOa = 1;
        private string hubSectionChange = "KBHubSection";
        private string kb = "";
        private string stuNum = "";
        //private  ObservableCollection<Group>  morepageclass=new ObservableCollection<Group>();
        private ObservableDictionary morepageclass = new ObservableDictionary();
        //private  ObservableCollection<Morepageclass> morepageclass= new ObservableCollection<Morepageclass>();
        //private string[,,] classtime = new string[7, 6,*];
        string[,][] classtime = new string[7, 6][];

        List<ClassList> classList = new List<ClassList>();
        ObservableCollection<JWList> JWList = new ObservableCollection<JWList>();

        Grid backweekgrid = new Grid();

        IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
        public ObservableDictionary Morepageclass
        {
            get
            {
                return morepageclass;
            }
        }
        public MainPage_m()
        {
            this.InitializeComponent();
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            appSettingclass = ApplicationData.Current.RoamingSettings;
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            //MoreGRNameTextBlock.Text = appSetting.Values["name"].ToString();
            //MoreGRClassTextBlock.Text = appSetting.Values["classNum"].ToString();
            //MoreGRNumTextBlock.Text = appSetting.Values["stuNum"].ToString();
            //this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += this.NavigationHelper_LoadState;

            //stuNum = appSetting.Values["stuNum"].ToString();
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            stuNum = credentialList[0].UserName;
            //initKB();
            initJW();
        }
        private void SetKebiaoGridBorder()
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

        /// <summary>
        /// 课表网络请求
        /// </summary>
        /// <param name="isRefresh"> 是否为刷新</param>
        private async void initKB(bool isRefresh = false)
        {
            //if (stuNum == appSetting.Values["stuNum"].ToString() && !isRefresh)
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                if (stuNum == credentialList[0].UserName && !isRefresh)
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
                //if (stuNum == appSetting.Values["stuNum"].ToString())
                if (stuNum == credentialList[0].UserName)
                {
                    HubSectionKBTitle.Text = "我的课表";
                    HubSectionKBTitle.FontSize = 35;
                }
            }
            catch { }

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
            SetKebiaoGridBorder();
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
            appSettingclass.Values.Clear();

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
                        JWList.Add(new JWList { Title = JWitem.Title, Date = "时间：" + JWitem.Date, Read = "阅读量：" + JWitem.Read, Content = JWitem.Content, ID = JWitem.ID });
                        JWListView.ItemsSource = JWList;
                        //setOpacity();
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

        private async void setOpacity()
        {
            try
            {
                opacityGrid.Visibility = Visibility.Visible;
                OpacityJWGrid.Begin();
                await Task.Delay(1000);
                opacityGrid.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                opacityGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void JWListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            initJW();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageStart("MainPage");
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
                                                                       //this.navigationHelper.OnNavigatedTo(e);
            var group = await DataSource.Get();
            this.Morepageclass["Group"] = group;
            if (e.NavigationMode == NavigationMode.Forward || e.NavigationMode == NavigationMode.New)
                initKB();



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
                        JWRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        MoreSwitchAppBarButton.Visibility = Visibility.Collapsed;
                        break;
                    case "JWHubSection":
                        // MoreBlueGRGrid.Opacity = 0;

                        KBRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        KBZoomAppBarButton.Visibility = Visibility.Collapsed;
                        KBCalendarAppBarButton.Visibility = Visibility.Collapsed;
                        JWRefreshAppBarButton.Visibility = Visibility.Visible;
                        MoreSwitchAppBarButton.Visibility = Visibility.Collapsed;
                        break;
                    case "MoreHubSection":
                        //MoreGRGrid.Margin = new Thickness(-20,0,0,0);
                        //MoveMoreBlueGRGrid.Begin();
                        //MoveMoreBlueGRGrid2.Begin();

                        KBRefreshAppBarButton.Visibility = Visibility.Collapsed;
                        KBZoomAppBarButton.Visibility = Visibility.Collapsed;
                        KBCalendarAppBarButton.Visibility = Visibility.Collapsed;
                        JWRefreshAppBarButton.Visibility = Visibility.Collapsed;
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

            //stuNum = appSetting.Values["stuNum"].ToString();
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            stuNum = credentialList[0].UserName;
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
                DateTime now = DateTime.Now;
                DateTime weekstart = GetWeekFirstDayMon(now);
                DateTime weekend = GetWeekLastDaySun(now);
                HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
            }
        }

        /// <summary>
        /// 教务刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void JWRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            JWList.Clear();
            continueJWGrid.Visibility = Visibility.Collapsed;
            initJW();
        }


        /// <summary>
        /// 切换账号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreSwitchAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            // appSetting.Values.Remove("idNum");
            //Frame.Navigate(typeof(PersonPage));
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
                KBNumFlyout.Hide();
            }
            else
                Utils.Message("请输入正确的周次");
        }


        private void continueJWGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            page++;
            initJW(page);
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
                    //Frame.Navigate(typeof(SearchFreeTimeNumPage));
                    break;
                case "Card":
                    var a = await Launcher.LaunchUriAsync(new Uri("cquptcard:"));
                    Debug.WriteLine(a);
                    break;
                default:
                    break;
            }
        }
    }
}
