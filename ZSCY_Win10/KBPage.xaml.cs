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
using Windows.UI.Xaml.Shapes;
using ZSCY.Data;
using ZSCY_Win10.Data;
using ZSCY_Win10.Util;
using Windows.UI.Popups;
using ZSCY_Win10.Controls;
using ZSCY_Win10.Pages.RemindPages;
using ZSCY_Win10.Pages.AddRemindPage;
using Windows.UI.Xaml.Media.Animation;

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
        private static string resourceName = "ZSCY";
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
        Grid backweekgrid = new Grid();
        TextBlock[] DateOnKBTextBlock = new TextBlock[7] { new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock(), new TextBlock() };
        List<ClassList> classList = new List<ClassList>();
        List<Transaction> transationList = new List<Transaction>();
        string[,][] classtime = new string[7, 6][];
        long[,][] transactiontime = new long[7, 6][];
        private Dictionary<string, int> colorlist = new Dictionary<string, int>(); //课表格子颜色
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
                if (!App.showpane)
                {
                    KBTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    KBTitleGrid.Margin = new Thickness(0);
                }
                if (e.NewSize.Width > 600)
                {
                    TodayTitleStackPanel.Margin = new Thickness(400, 0, 0, 0);
                    TodayTitleStackPanel.Visibility = Visibility.Visible;
                    state = "VisualState550";
                    cutoffLine2.Visibility = Visibility.Collapsed;
                }
                if (e.NewSize.Width > 750)
                {
                    TodayTitleStackPanel.Margin = new Thickness(400, 0, 0, 0);
                    TodayTitleStackPanel.Visibility = Visibility.Visible;
                    KBDayFLine.X2 = e.NewSize.Width - 400;
                    state = "VisualState750";
                    cutoffLine2.Visibility = Visibility.Visible;
                }
                if (e.NewSize.Width > 1000)
                {
                    TodayTitleStackPanel.Margin = new Thickness(400, 0, 0, 0);
                    TodayTitleStackPanel.Visibility = Visibility.Visible;
                    KBDayFLine.X2 = e.NewSize.Width - 400 - 250;
                    state = "VisualState1000";
                    cutoffLine2.Visibility = Visibility.Visible;
                }
                VisualStateManager.GoToState(this, state, true);
                Debug.WriteLine("KBAllGrid" + KBAllGrid.Width);
                Debug.WriteLine(e.NewSize.Width);
                //KebiaoAllScrollViewer.Height = e.NewSize.Height - 48 - 25;
                KebiaoAllpr.Height = e.NewSize.Height - 48 - 50;
                cutoffLine.Y2 = e.NewSize.Height - 48;
                cutoffLine2.Y2 = e.NewSize.Height - 48;
            };
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar") && Utils.getPhoneWidth() < 400)
            {
                KBRefreshAppBarButton.Visibility = Visibility.Collapsed;
            }
#if DEBUG
            TestButton.Visibility = Visibility.Visible;
#endif        

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //TODO:未登陆时 没有课表
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                Debug.WriteLine("OnNavigatedTo");
                //if (appSetting.Values.ContainsKey("idNum"))
                if (credentialList.Count > 0)
                {
                    progress.Visibility = Visibility.Visible;
                    //stuNum = appSetting.Values["stuNum"].ToString();
                    stuNum = credentialList[0].UserName;
                    initKB();
                    this.progress.IsActive = false;
                    initToday();
                    UmengSDK.UmengAnalytics.TrackPageStart("KBPage");
                }
                else
                {
                    progress.Visibility = Visibility.Collapsed;
                    HubSectionKBTitle.Text = "未登陆 暂无";
                    initToday();
                    baseInfoStackPanel.IsTapEnabled = false;
                    HubSectionKBNum.IsTapEnabled = false;
                    KBRefreshAppBarButton.IsEnabled = false;
                    KBCalendarAppBarButton.IsEnabled = false;
                    KBZoomAppBarButton.IsEnabled = false;
                    ShowMoreAppBarButton.IsEnabled = false;
                }
            }
            catch
            {
                progress.Visibility = Visibility.Collapsed;
                HubSectionKBTitle.Text = "未登陆 暂无";
                initToday();
                baseInfoStackPanel.IsTapEnabled = false;
                HubSectionKBNum.IsTapEnabled = false;
                KBRefreshAppBarButton.IsEnabled = false;
                KBCalendarAppBarButton.IsEnabled = false;
                KBZoomAppBarButton.IsEnabled = false;
                ShowMoreAppBarButton.IsEnabled = false;
            }

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
                //TODO:改动 当日空课表的背景色 
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
            //当日的星期
            TextBlock KebiaoWeek = new TextBlock();
            KebiaoWeek.Text = Utils.GetWeek(2);
            KebiaoWeek.FontSize = 18;
            KebiaoWeek.Foreground = new SolidColorBrush(Color.FromArgb(255, 33, 33, 33));
            KebiaoWeek.FontWeight = FontWeights.Light;
            KebiaoWeek.VerticalAlignment = VerticalAlignment.Center;
            KebiaoWeek.HorizontalAlignment = HorizontalAlignment.Center;
            KebiaoWeek.SetValue(Grid.ColumnProperty, (Int16.Parse(Utils.GetWeek()) == 0 ? 6 : Int16.Parse(Utils.GetWeek()) - 1));
            KebiaoWeek.SetValue(Grid.RowProperty, 1);
            KebiaoWeekTitleGrid.Children.Add(KebiaoWeek);


        }
        //TODO:未登陆时 没有课表
        private async void initKB(bool isRefresh = false)
        {
            string Transactiontemp = null;

            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                //if (stuNum == appSetting.Values["stuNum"].ToString() && !isRefresh)
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
                        HubSectionKBNum.Text = " | 第" + appSetting.Values["nowWeek"].ToString() + "周";
#if DEBUG
                        showKB(2, 5);
#else
                        showKB(2);
#endif
                    }
                    catch (Exception) { Debug.WriteLine("主页->课表数据缓存异常"); }
                }
                //if (stuNum == appSetting.Values["stuNum"].ToString())
                if (stuNum == credentialList[0].UserName)
                {
                    HubSectionKBTitle.Text = "我的课表";
                    HubSectionKBTitle.FontSize = 18;

                }

            }
            catch { }

            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
            //if (isRefresh)
            //    paramList.Add(new KeyValuePair<string, string>("forceFetch", "true"));


            string kbtemp = await NetWork.getHttpWebRequest("redapi2/api/kebiao", paramList); //新
                                                                                              //string kbtemp = await NetWork.getHttpWebRequest("api/kebiao", paramList); //旧
            try
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                List<KeyValuePair<String, String>> TransactionparamList = new List<KeyValuePair<String, String>>();
                TransactionparamList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                TransactionparamList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                Transactiontemp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Person/getTransaction", TransactionparamList);
            }
            catch
            {
                NotifyPopup notifyPopup = new NotifyPopup("网络异常 无法读取事项~");
                notifyPopup.Show();
            }
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
                    HubSectionKBNum.Text = " | 第" + appSetting.Values["nowWeek"].ToString() + "周";
                    todayNumofstuTextBlock.Text = "开学第" + ((Int16.Parse(appSetting.Values["nowWeek"].ToString()) - 1) * 7 + (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek()))).ToString() + "天";
                    //showKB(2, Int32.Parse(appSetting.Values["nowWeek"].ToString()));
#if DEBUG
                    showKB(2, 0, Transactiontemp);
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

        private void showKB(int weekOrAll = 1, int week = 0, string transactioncontent = null)
        {
            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 6; j++)
                    classtime[i, j] = null;

            for (int i = 0; i < 7; i++)
                for (int j = 0; j < 6; j++)
                    transactiontime[i, j] = null;

            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            if (stuNum == credentialList[0].UserName)
                GetTransaction(transactioncontent);

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
                    //if (bool.Parse(appSetting.Values["AllKBGray"].ToString()))
                    //    if (Array.IndexOf(classitem.Week, Int32.Parse(appSetting.Values["nowWeek"].ToString())) != -1)
                    //        SetClassAll(classitem, ClassColor);
                    //    else
                    //        SetClassAll(classitem, 3);
                    //else
                    SetClassAll(classitem, ClassColor);
                    SetTransactionAll(transationList, classList);
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
                            if (transationList.Count != 0)
                                SetTransactionDay(transationList, classList);

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
                        SetTransactionDay(transationList, classList, week);
                    }
                }
            }

            KebiaoDayGrid.Children.Clear();
            //这特么在逗我
            //if (transationList.Count != 0)
            //   SetTransactionDay(transationList, classList);

            //当日课表显示
            for (int i = 0; i < ClassListArray.Count; i++)
            {
                ClassList classitem = new ClassList();
                classitem.GetAttribute((JObject)ClassListArray[i]);
                //#if DEBUG
                //                if (Array.IndexOf(classitem.Week, 5) != -1 && classitem.Hash_day == 2)
                //                {
                //                    SetClassDay(classitem);
                //                }
                //#else
                if (Array.IndexOf(classitem.Week, Int32.Parse(appSetting.Values["nowWeek"].ToString())) != -1 && classitem.Hash_day == (Int16.Parse(Utils.GetWeek()) + 6) % 7)
                {
                    SetClassDay(classitem);
                }
                //#endif
            }

            colorlist.Clear();
        }

        /// <summary>
        /// 当日事项填充
        /// </summary>
        private void SetTransactionDay(List<Transaction> transationList, List<ClassList> classlist, int week = 0, int weekOrAll = 0)
        {
            int nowWEEK;
            if (week == 0)
                nowWEEK = Int32.Parse(appSetting.Values["nowWeek"].ToString());
            else
                nowWEEK = week;
            //事项的背景颜色 略淡
            Color[] Tcolor = new Color[]{
                   Color.FromArgb(255,232,245,254),
                   Color.FromArgb(255,255,245,233),
                   Color.FromArgb(255,230,255,251)
                };
            //通过在某某节确定背景颜色
            int RightC = 0;
            bool isInClassGrid = false;
            foreach (var transactionitem in transationList)
            {
                for (int i = 0; i < transactionitem.date.Count; i++)
                {
                    if (Array.IndexOf(transactionitem.date[i].week, nowWEEK) != -1)
                    {
                        foreach (var classitem in classlist)
                        {
                            if (Array.IndexOf(classitem.Week, nowWEEK) != -1)
                            {
                                //当前课与当前时段的事件在同一周
                                //如果本周任意一节课与事件事件冲突 
                                if (transactionitem.date[i].day == classitem.Hash_day && transactionitem.date[i]._class == classitem.Hash_lesson)
                                { isInClassGrid = true; break; }
                            }
                        }
                        switch (transactionitem.date[i]._class)
                        {
                            case 0:
                            case 1:
                                RightC = 0;
                                break;
                            case 2:
                            case 3:
                                RightC = 1;
                                break;
                            case 4:
                            case 5:
                                RightC = 2;
                                break;
                        }
                        if (transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class] == null)
                        {
                            long[] tempstr = new long[1];
                            tempstr[0] = transactionitem.id;
                            transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class] = tempstr;
                        }
                        else if (Array.IndexOf(transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class], transactionitem.id) == -1)
                        {
                            long[] temp = transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class];
                            long[] templ = new long[temp.Length + 1];
                            for (int a = 0; a < temp.Length; a++)
                                templ[a] = temp[a];
                            //if (Array.IndexOf(templ, transactionitem.id) != -1)
                            templ[temp.Length] = transactionitem.id;
                            transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class] = templ;
                        }
                        if (isInClassGrid)
                        {
                            //事件与课程冲突 就不订阅tapped事件了
                            Grid transactionGrid = new Grid();
                            transactionGrid.SetValue(Grid.RowProperty, System.Int32.Parse(transactionitem.date[i]._class * 2 + ""));
                            transactionGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(transactionitem.date[i].day + ""));
                            transactionGrid.SetValue(Grid.RowSpanProperty, System.Int32.Parse(2 + ""));

                            transactionGrid.Margin = new Thickness(2);

                            transactionGrid.HorizontalAlignment = HorizontalAlignment.Right;
                            transactionGrid.VerticalAlignment = VerticalAlignment.Top;
                            transactionGrid.Width = 8;
                            transactionGrid.Height = 8;
                            transactionGrid.BorderThickness = new Thickness(0);
                            transactionGrid.Background = new SolidColorBrush(Colors.Transparent);
                            Polygon pl = new Polygon();
                            PointCollection collection = new PointCollection();
                            collection.Add(new Point(0, 0));
                            collection.Add(new Point(10, 10));
                            collection.Add(new Point(10, 0));
                            pl.Points = collection;
                            pl.StrokeThickness = 0;
                            pl.Fill = new SolidColorBrush(Colors.White);
                            transactionGrid.Children.Add(pl);
                            isInClassGrid = false;
                            kebiaoGrid.Children.Add(transactionGrid);
                        }
                        else
                        {
                            Grid transactionGrid = new Grid();
                            transactionGrid.SetValue(Grid.RowProperty, System.Int32.Parse(transactionitem.date[i]._class * 2 + ""));
                            transactionGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(transactionitem.date[i].day + ""));
                            transactionGrid.SetValue(Grid.RowSpanProperty, System.Int32.Parse(2 + ""));

                            transactionGrid.Background = new SolidColorBrush(Tcolor[RightC]);

                            Grid polygonGrid = new Grid();
                            polygonGrid.HorizontalAlignment = HorizontalAlignment.Right;
                            polygonGrid.VerticalAlignment = VerticalAlignment.Top;
                            polygonGrid.Height = 8;
                            polygonGrid.Width = 8;
                            polygonGrid.Background = new SolidColorBrush(Colors.Transparent);
                            polygonGrid.Margin = new Thickness(2);
                            Polygon pl = new Polygon();
                            PointCollection collection = new PointCollection();
                            collection.Add(new Point(0, 0));
                            collection.Add(new Point(10, 10));
                            collection.Add(new Point(10, 0));
                            pl.Points = collection;
                            pl.Fill = new SolidColorBrush(Colors.White);
                            pl.StrokeThickness = 0;
                            polygonGrid.Children.Add(pl);
                            transactionGrid.Children.Add(polygonGrid);

                            isInClassGrid = false;
                            transactionGrid.Margin = new Thickness(0.5);

                            transactionGrid.Tapped += BackGrid_Tapped;
                            kebiaoGrid.Children.Add(transactionGrid);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 学期事项填充
        /// </summary>
        /// 跟当日事件填充除了判断条件基本一致 但是不太好合/头有点晕 有空再做优化
        private void SetTransactionAll(List<Transaction> transationList, List<ClassList> classlist)
        {
            Color[] Tcolor = new Color[]{
                   Color.FromArgb(255,232,245,254),
                   Color.FromArgb(255,255,245,233),
                   Color.FromArgb(255,230,255,251)
                };
            int RightC = 0;
            bool IsInClass = false;
            foreach (var transactionitem in transationList)
            {
                for (int i = 0; i < transactionitem.date.Count; i++)
                {
                    foreach (var classitem in classlist)
                    {
                        if (transactionitem.date[i]._class == classitem.Hash_lesson)
                        { IsInClass = true; break; }
                    }
                    switch (transactionitem.date[i]._class)
                    {
                        case 0:
                        case 1:
                            RightC = 0;
                            break;
                        case 2:
                        case 3:
                            RightC = 1;
                            break;
                        case 4:
                        case 5:
                            RightC = 2;
                            break;
                    }
                    if (transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class] == null)
                    {
                        long[] tempstr = new long[1];
                        tempstr[0] = transactionitem.id;
                        transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class] = tempstr;
                    }
                    else if (Array.IndexOf(transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class], transactionitem.id) == -1)
                    {
                        long[] temp = transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class];
                        long[] templ = new long[temp.Length + 1];
                        for (int a = 0; a < temp.Length; a++)
                            templ[a] = temp[a];
                        //if (Array.IndexOf(templ, transactionitem.id) != -1)
                        templ[temp.Length] = transactionitem.id;
                        transactiontime[transactionitem.date[i].day, transactionitem.date[i]._class] = templ;
                    }
                    if (IsInClass)
                    {
                        Grid transactionGrid = new Grid();
                        transactionGrid.SetValue(Grid.RowProperty, System.Int32.Parse(transactionitem.date[i]._class * 2 + ""));
                        transactionGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(transactionitem.date[i].day + ""));
                        transactionGrid.SetValue(Grid.RowSpanProperty, System.Int32.Parse(2 + ""));

                        transactionGrid.Margin = new Thickness(2);

                        transactionGrid.HorizontalAlignment = HorizontalAlignment.Right;
                        transactionGrid.VerticalAlignment = VerticalAlignment.Top;
                        transactionGrid.Width = 8;
                        transactionGrid.Height = 8;
                        transactionGrid.BorderThickness = new Thickness(0);
                        transactionGrid.Background = new SolidColorBrush(Colors.Transparent);
                        Polygon pl = new Polygon();
                        PointCollection collection = new PointCollection();
                        collection.Add(new Point(0, 0));
                        collection.Add(new Point(10, 10));
                        collection.Add(new Point(10, 0));
                        pl.Points = collection;
                        pl.StrokeThickness = 0;
                        pl.Fill = new SolidColorBrush(Colors.White);
                        transactionGrid.Children.Add(pl);
                        IsInClass = false;
                        kebiaoGrid.Children.Add(transactionGrid);
                    }
                    else
                    {
                        Grid transactionGrid = new Grid();
                        transactionGrid.SetValue(Grid.RowProperty, System.Int32.Parse(transactionitem.date[i]._class * 2 + ""));
                        transactionGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(transactionitem.date[i].day + ""));
                        transactionGrid.SetValue(Grid.RowSpanProperty, System.Int32.Parse(2 + ""));

                        transactionGrid.Background = new SolidColorBrush(Tcolor[RightC]);

                        Grid polygonGrid = new Grid();
                        polygonGrid.HorizontalAlignment = HorizontalAlignment.Right;
                        polygonGrid.VerticalAlignment = VerticalAlignment.Top;
                        polygonGrid.Height = 8;
                        polygonGrid.Width = 8;
                        polygonGrid.Background = new SolidColorBrush(Colors.Transparent);
                        polygonGrid.Margin = new Thickness(2);
                        Polygon pl = new Polygon();
                        PointCollection collection = new PointCollection();
                        collection.Add(new Point(0, 0));
                        collection.Add(new Point(10, 10));
                        collection.Add(new Point(10, 0));
                        pl.Points = collection;
                        pl.Fill = new SolidColorBrush(Colors.White);
                        pl.StrokeThickness = 0;
                        polygonGrid.Children.Add(pl);
                        transactionGrid.Children.Add(polygonGrid);

                        IsInClass = false;
                        transactionGrid.Margin = new Thickness(0.5);

                        transactionGrid.Tapped += BackGrid_Tapped;
                        kebiaoGrid.Children.Add(transactionGrid);
                    }
                }
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

        //bool isGetTransactionSuccess = false;
        //新增:获取事项信息
        private async void GetTransaction(string contentstring = null)
        {
            bool secondTimeAdd = false;
            if (contentstring == null)
            {
                //clear出了无法理解的问题..暂时用一个判断吧 聊胜于无           
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                credentialList[0].RetrievePassword();
                if (credentialList[0] != null)
                {
                    try
                    {
                        List<KeyValuePair<String, String>> TransactionparamList = new List<KeyValuePair<String, String>>();
                        TransactionparamList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                        TransactionparamList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                        string Transactiontemp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Person/getTransaction", TransactionparamList);
                        //isGetTransactionSuccess = true;
                        JObject Tobj = JObject.Parse(Transactiontemp);
                        if (Int32.Parse(Tobj["status"].ToString()) == 200)
                        {
                            JArray TransactionArray = Utils.ReadJso(Transactiontemp);
                            for (int i = 0; i < TransactionArray.Count; i++)
                            {
                                Transaction transactionItem = new Transaction();
                                transactionItem.GetAttribute((JObject)TransactionArray[i]);
                                foreach (var existItem in transationList)
                                {
                                    if (transactionItem.id == existItem.id)
                                    { secondTimeAdd = true; break; }
                                }
                                if (!secondTimeAdd)
                                    transationList.Add(transactionItem);
                                Debug.WriteLine(i);
                            }
                        }
                    }
                    catch
                    {
                        NotifyPopup notifyPopup = new NotifyPopup("网络异常 无法读取事项~");
                        notifyPopup.Show();
                    }
                }
            }
            else
            {
                if (contentstring != null && contentstring != "")
                {
                    JObject Tobj = JObject.Parse(contentstring);
                    if (Int32.Parse(Tobj["status"].ToString()) == 200)
                    {
                        JArray TransactionArray = Utils.ReadJso(contentstring);
                        for (int i = 0; i < TransactionArray.Count; i++)
                        {
                            Transaction transactionItem = new Transaction();
                            transactionItem.GetAttribute((JObject)TransactionArray[i]);
                            foreach (var existItem in transationList)
                            {
                                if (transactionItem.id == existItem.id)
                                { secondTimeAdd = true; break; }
                            }
                            if (!secondTimeAdd)
                                transationList.Add(transactionItem);
                            Debug.WriteLine(i);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 周视图课程格子的填充
        /// </summary>
        /// <param name="item">ClassList类型的item</param>
        /// <param name="ClassColor">颜色数组，0~9</param>
        private void SetClassAll(ClassList item, int ClassColor)
        {
            //有事项的画个角- -
            //foreach (var transactionItem in transationList) {
            //    if (item.Week == transactionItem.week && item.Lesson == transactionItem.classToLesson)
            //    {

            //    }
            //}

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
                   Color.FromArgb(255,200, 200, 200), //灰色
                };

            //折叠角的颜色数组
            Color[] _color = new Color[] {
                Color.FromArgb(255,255,219,178),
                Color.FromArgb(255,162,229,255),
                Color.FromArgb(255,155,244,244),
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
            BackGrid.Margin = new Thickness(0.5);
            BackGrid.Children.Add(ClassTextBlock);

            //TODO:新增 折叠三角
            if (classtime[item.Hash_day, item.Hash_lesson] != null)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("ms-appx:///Assets/shape.png", UriKind.Absolute));
                img.VerticalAlignment = VerticalAlignment.Bottom;
                img.HorizontalAlignment = HorizontalAlignment.Right;
                img.Width = 10;

                //他要折叠..我画一个三角好了..
                Grid _grid = new Grid();
                Polygon pl = new Polygon();
                PointCollection collection = new PointCollection();
                collection.Add(new Point(0, 0));
                collection.Add(new Point(10, 0));
                collection.Add(new Point(0, 10));
                pl.Points = collection;
                pl.Stroke = new SolidColorBrush(Colors.Black);
                pl.StrokeThickness = 0;
                _grid.Children.Add(pl);
                _grid.Background = new SolidColorBrush(_color[ClassColor]);
                _grid.Width = 10;
                _grid.Height = 10;
                _grid.VerticalAlignment = VerticalAlignment.Bottom;
                _grid.HorizontalAlignment = HorizontalAlignment.Right;
                BackGrid.Children.Add(_grid);

                BackGrid.Children.Add(img);

                string[] temp = classtime[item.Hash_day, item.Hash_lesson];
                string[] tempnew = new string[temp.Length + 1];
                for (int i = 0; i < temp.Length; i++)
                    tempnew[i] = temp[i];
                tempnew[temp.Length] = item._Id;
                Debug.WriteLine("if~id->" + item._Id);
                classtime[item.Hash_day, item.Hash_lesson] = tempnew;
            }
            else
            {
                string[] tempnew = new string[1];
                tempnew[0] = item._Id;
                Debug.WriteLine("else~id->" + item._Id);
                classtime[item.Hash_day, item.Hash_lesson] = tempnew;
            }

            BackGrid.Tapped += BackGrid_Tapped;
            kebiaoGrid.Children.Add(BackGrid);
        }

        private void BackGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Debug.WriteLine("前" + KBCLassFlyoutPivot.Items.Count.ToString());
            //do
            //{
            //    KBCLassFlyoutPivot.Items.RemoveAt(0);
            //}
            //while (KBCLassFlyoutPivot.Items.Count.ToString() != "0");
            //Debug.WriteLine("删除" + KBCLassFlyoutPivot.Items.Count.ToString());
            //改用控件了 暂时停了以前的flyout系列方法
            List<ClassList> cl = new List<ClassList>();
            List<Transaction> tl = new List<Transaction>();
            Grid g = sender as Grid;
            Debug.WriteLine(g.GetValue(Grid.ColumnProperty));
            Debug.WriteLine(g.GetValue(Grid.RowProperty));
            string[] tempClass = classtime[Int32.Parse(g.GetValue(Grid.ColumnProperty).ToString()), Int32.Parse(g.GetValue(Grid.RowProperty).ToString()) / 2];
            if (tempClass != null)
            {
                for (int i = 0; i < tempClass.Length; i++)
                {
                    ClassList tempC = classList.Find(p => p._Id.Equals(tempClass[i]));
                    cl.Add(tempC);
                }
            }

            //传入事件 ┑(￣Д ￣)┍
            long[] tempTra = transactiontime[Int32.Parse(g.GetValue(Grid.ColumnProperty).ToString()), Int32.Parse(g.GetValue(Grid.RowProperty).ToString()) / 2];
            if (tempTra != null)
            {
                for (int i = 0; i < tempTra.Length; i++)
                {
                    Transaction tempt = transationList.Find(p => p.id.Equals(tempTra[i]));
                    tl.Add(tempt);
                }
            }
            var control2 = new ClassInfoControl(cl, tl, null);
            control2.ShowWIndow();


            //for (int i = 0; i < temp.Length; i++)
            //{
            //    ClassList c = classList.Find(p => p._Id.Equals(temp[i]));

            //    PivotItem pi = new PivotItem();
            //    TextBlock HeaderTextBlock = new TextBlock();
            //    HeaderTextBlock.Text = c.Course;
            //    HeaderTextBlock.FontSize = 25;
            //    pi.Header = HeaderTextBlock;
            //    ListView lv = new ListView();
            //    lv.ItemTemplate = KBCLassFlyoutListView.ItemTemplate;
            //    List<ClassList> cc = new List<ClassList>();
            //    cc.Add(c);
            //    lv.ItemsSource = cc;
            //    pi.Content = lv;
            //    KBCLassFlyoutPivot.Items.Add(pi);
            //    Debug.WriteLine("后" + KBCLassFlyoutPivot.Items.Count.ToString());
            //}
            //KBCLassFlyout.ShowAt(page);
        }

        /// <summary>
        /// 课表刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KBRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO:未登陆时 无法刷新
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            this.progress.IsActive = true;
            //stuNum = appSetting.Values["stuNum"].ToString();
            stuNum = credentialList[0].UserName;
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
            //transationList.Clear();

            showKB(wOa);
            DateTime now = DateTime.Now;
            DateTime weekstart = GetWeekFirstDayMon(now);
            DateTime weekend = GetWeekLastDaySun(now);
            ShowWeekOnKB(weekstart);
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
                HubSectionKBDate.Text = weekstart.Month + "." + weekstart.Day + "--" + weekend.Month + "." + weekend.Day;
            }
        }



        private void KBSearchButton_Click(object sender, RoutedEventArgs e)
        {
            transationList.Clear();
            KBSearch();
        }

        private void KBSearch()
        {
            //KBSearchButton.IsChecked = false;
            if (KBZoomFlyoutTextBox.Text != "" && KBZoomFlyoutTextBox.Text.Length == 10 && KBZoomFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                stuNum = KBZoomFlyoutTextBox.Text;
                HubSectionKBTitle.Text = stuNum + "的课表";
                HubSectionKBTitle.FontSize = 15;
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
            KBNumSearch();
        }

        private void KBNumSearch()
        {
            //TODO:未登陆不能选择周次
            //KBNumSearchButton.IsChecked = false;
            if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                showKB(2, Int16.Parse(KBNumFlyoutTextBox.Text));
                HubSectionKBNum.Text = " | 第" + KBNumFlyoutTextBox.Text + "周";
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

        private void initToday()
        {
            todaydateTextBlock.Text = DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日";
            try
            {
                todayNumofstuTextBlock.Text = "开学第" + ((Int16.Parse(appSetting.Values["nowWeek"].ToString()) - 1) * 7 + (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek()))).ToString() + "天";
            }
            catch (Exception)
            {
                todayNumofstuTextBlock.Text = "开学第    天";
            }
        }

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
                    KBNumFlyout.ShowAt(page);
                    HubSectionKBNum.SelectAll();
                }
            }
        }


        private void KBZoomFlyoutTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("enter");
                if (KBZoomFlyoutTextBox.Text != "")
                    KBSearch();
                else
                {
                    Utils.Message("信息不完全");
                    KBZoomFlyout.ShowAt(page);
                    KBZoomFlyoutTextBox.SelectAll();
                }
            }
        }

        private void KebiaoAllpr_RefreshInvoked(DependencyObject sender, object args)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            this.progress.IsActive = true;
            //stuNum = appSetting.Values["stuNum"].ToString();
            stuNum = credentialList[0].UserName;
            wOa = 1;
            initKB(true);
            this.progress.IsActive = false;
        }

        private void AddRemind_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddRemindPage),"add");
        }

        private void RemindList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RemindListPage),false);
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RemindTest));
        }
    }
}
