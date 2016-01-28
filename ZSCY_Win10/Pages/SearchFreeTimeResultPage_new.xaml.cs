using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10;
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

        private string[,][] ResultName = new string[7, 6][];
        string[] kb; //课表数据的数组
        int week; //周次，0为学期
        int week_old; //周次，切换到学期后保存切换前的周次
        bool showWeekend = false;

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
            FreeDetailNameGridView.ItemsSource = peoplelist;
            FlyoutFreeDetailNameGridView.ItemsSource = peoplelist;
            initFree();
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
        }

        /// <summary>
        /// 对学号的List中的学号一个个请求课表，存入kb数组
        /// </summary>
        private async void initFree()
        {
            kb = new string[muIdList.Count];
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
                        kb[i] = kbtemp;
                        issuccess = 2;
                    }
                    else
                    {
                        kb[i] = "";
                        issuccess++;
                    }
                }
                forsearchlist.Add(muIdList[i].uName, clist);
                FreeLoddingProgressBar.Value = FreeLoddingProgressBar.Value + 100.0 / muIdList.Count;
                Debug.WriteLine(FreeLoddingProgressBar.Value);
            }
            //查无课表，参数第几周==
            EmptyClass ec = new EmptyClass(week, forsearchlist);
            ec.getfreetime(ref result,ref termresult);
            //freetime(11, forsearchlist);
            FreeLoddingStackPanel.Visibility = Visibility.Collapsed;
            FreeKBTableGrid.Visibility = Visibility.Visible;
            ShowWeekendAppBarButton.Visibility = Visibility.Visible;
            showFreeKB(result);
        }

        private void showFreeKB(ObservableCollection<ClassListLight> result, bool showWeekend = false)
        {
            kebiaoGrid.Children.Clear();
            int ClassColor = 0;
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

        private void SetClassAll(ClassListLight item, int ClassColor)
        {
            Color[] colors = new Color[]{
                   Color.FromArgb(255,255, 181, 68),//黄
                   Color.FromArgb(255,99, 202, 245),//蓝
                   Color.FromArgb(255,172, 222, 76),//绿
                   Color.FromArgb(255,249, 130, 130),//红
                };

            ResultName[item.Hash_day, item.Hash_lesson] = item.Name;

            TextBlock ClassTextBlock = new TextBlock();
            foreach (var nameitem in item.Name)
            {
                ClassTextBlock.Text = ClassTextBlock.Text + "\n" + nameitem;
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
            BackGrid.SetValue(Grid.RowProperty, System.Int32.Parse(item.Hash_lesson * 2 + ""));
            BackGrid.SetValue(Grid.ColumnProperty, System.Int32.Parse(item.Hash_day + ""));
            BackGrid.SetValue(Grid.RowSpanProperty, 2);
            BackGrid.Margin = new Thickness(0.5);

            if (item.Name.Length == muIdList.Count)
            {
                TextBlock AllPeopleTextBlock = new TextBlock();
                AllPeopleTextBlock.Foreground = new SolidColorBrush(Colors.White);
                AllPeopleTextBlock.FontSize = 11;
                AllPeopleTextBlock.Text = "全";
                AllPeopleTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                AllPeopleTextBlock.HorizontalAlignment = HorizontalAlignment.Right;
                AllPeopleTextBlock.Margin = new Thickness(1);
                BackGrid.Children.Add(AllPeopleTextBlock);
            }

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
            string[] temp = ResultName[Int32.Parse(g.GetValue(Grid.ColumnProperty).ToString()), Int32.Parse(g.GetValue(Grid.RowProperty).ToString()) / 2];

            string week = "";
            string nowclass = "";
            string time = "";
            switch (g.GetValue(Grid.ColumnProperty).ToString())
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

            switch (Int32.Parse(g.GetValue(Grid.RowProperty).ToString()) / 2)
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
            for (int i = 0; i < temp.Length; i++)
            {
                peoplelist.Add(new People { name = temp[i] });
            }
            if (FreeDetailGrid.Visibility == Visibility.Collapsed)
                KBCLassFlyout.ShowAt(commandBar);
            FreeNoClickStackPanel.Visibility = Visibility.Collapsed;
            FreeDetailStackPanel.Visibility = Visibility.Visible;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
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

        private void KBNumSearch()
        {
            if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                try
                {
                    week = int.Parse(KBNumFlyoutTextBox.Text);
                    FilterAppBarButton.Label = "第" + KBNumFlyoutTextBox.Text + "周";
                    KBNumFlyout.Hide();
                }
                catch (Exception)
                {
                    Utils.Message("请输入正确的周次");
                }
            }
            else
                Utils.Message("请输入正确的周次");
        }

        private void CalendarWeekAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (week != 0)
            {
                week_old = week;
                week = 0;
                FilterAppBarButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                week = week_old;
                FilterAppBarButton.Label = "第" + week + "周";
                FilterAppBarButton.Visibility = Visibility.Visible;
            }
        }

        private void ShowWeekendAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (showWeekend)
                ShowWeekendAppBarButton.Label = "显示周末课表";
            else
                ShowWeekendAppBarButton.Label = "隐藏周末课表";
            showWeekend = !showWeekend;
            showFreeKB(result, showWeekend);
        }
    }
}
