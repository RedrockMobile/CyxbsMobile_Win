using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EmptyRoomsPage : Page
    {
        private int maxFloor;
        private bool isBuildEight = false;
        private string BuildEight;

        private ApplicationDataContainer appSetting;
        private Color gridColorGray = new Color();
        private Color gridColorBlue = new Color();

        private Boolean[] gridColor = new Boolean[]{
            false,
            false,
            false,
            false,
            false,
            false,
        };

        private string[] emptyReslut = new string[6]; //保存返回值
        private string[][] emptyRoomReslut = new string[6][]; //保存返回的教室值

        private string NowWeek;
        private string buildNum = "2";
        private string NowWeekday;

        private bool isShowEmpty = true;

        private ObservableCollection<EmptyRoomList> emptyRoomList = new ObservableCollection<EmptyRoomList>();
        private ObservableCollection<EmptyRoomList> emptyRoomAndFloor = new ObservableCollection<EmptyRoomList>();

        public EmptyRoomsPage()
        {
            this.InitializeComponent();
            appSetting = ApplicationData.Current.LocalSettings;
            gridColorGray = Color.FromArgb(255, 211, 211, 211);
            gridColorBlue = Color.FromArgb(255, 6, 140, 253);
            NowWeek = appSetting.Values["NowWeek"].ToString();

            for (int i = 0; i < 6; i++)
            {
                emptyRoomReslut[i] = new string[100];
                for (int j = 0; j < 100; j++)
                    emptyRoomReslut[i][j] = "";
            }
            NowWeekday = (Int16.Parse(Utils.GetWeek()) == 0 ? 7 : Int16.Parse(Utils.GetWeek())).ToString();
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageStart("EmptyRoomsPage");
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //StatusBar statusBar = StatusBar.GetForCurrentView();
            //await statusBar.ProgressIndicator.HideAsync();
            UmengSDK.UmengAnalytics.TrackPageEnd("EmptyRoomsPage");
        }

        private void Time08Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gridColor[0])
            {
                Time08Grid.Background = new SolidColorBrush(gridColorBlue);
                gridColor[0] = true;
                EmptyPost(0);
            }
            else if (gridColor[0])
            {
                Time08Grid.Background = new SolidColorBrush(gridColorGray);
                gridColor[0] = false;
                for (int i = 0; i < emptyRoomReslut[0].Length; i++)
                    emptyRoomReslut[0][i] = "";
                ShowEmpty();
            }
        }

        private void Time10Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gridColor[1])
            {
                Time10Grid.Background = new SolidColorBrush(gridColorBlue);
                gridColor[1] = true;
                EmptyPost(1);
            }
            else if (gridColor[1])
            {
                Time10Grid.Background = new SolidColorBrush(gridColorGray);
                gridColor[1] = false;
                for (int i = 0; i < emptyRoomReslut[1].Length; i++)
                    emptyRoomReslut[1][i] = "";
                ShowEmpty();
            }
        }

        private void Time14Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gridColor[2])
            {
                Time14Grid.Background = new SolidColorBrush(gridColorBlue);
                gridColor[2] = true;
                EmptyPost(2);
            }
            else if (gridColor[2])
            {
                Time14Grid.Background = new SolidColorBrush(gridColorGray);
                gridColor[2] = false;
                for (int i = 0; i < emptyRoomReslut[2].Length; i++)
                    emptyRoomReslut[2][i] = "";
                ShowEmpty();
            }
        }

        private void Time16Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gridColor[3])
            {
                Time16Grid.Background = new SolidColorBrush(gridColorBlue);
                gridColor[3] = true;
                EmptyPost(3);
            }
            else if (gridColor[3])
            {
                Time16Grid.Background = new SolidColorBrush(gridColorGray);
                gridColor[3] = false;
                for (int i = 0; i < emptyRoomReslut[3].Length; i++)
                    emptyRoomReslut[3][i] = "";
                ShowEmpty();
            }
        }

        private void Time19Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gridColor[4])
            {
                Time19Grid.Background = new SolidColorBrush(gridColorBlue);
                gridColor[4] = true;
                EmptyPost(4);
            }
            else if (gridColor[4])
            {
                Time19Grid.Background = new SolidColorBrush(gridColorGray);
                gridColor[4] = false;
                for (int i = 0; i < emptyRoomReslut[4].Length; i++)
                    emptyRoomReslut[4][i] = "";
                ShowEmpty();
            }
        }

        private void Time21Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!gridColor[5])
            {
                Time21Grid.Background = new SolidColorBrush(gridColorBlue);
                gridColor[5] = true;
                EmptyPost(5);
            }
            else if (gridColor[5])
            {
                Time21Grid.Background = new SolidColorBrush(gridColorGray);
                gridColor[5] = false;
                for (int i = 0; i < emptyRoomReslut[5].Length; i++)
                    emptyRoomReslut[5][i] = "";
                ShowEmpty();
            }
        }

        //private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        //{
        //    JXLButton.Content = (sender as MenuFlyoutItem).Text;
        //    switch (JXLButton.Content.ToString())
        //    {
        //        case "二教":
        //            buildNum = "2";
        //            break;
        //        case "三教":
        //            buildNum = "3";
        //            break;
        //        case "四教":
        //            buildNum = "4";
        //            break;
        //        case "五教":
        //            buildNum = "5";
        //            break;
        //        case "八教":
        //            buildNum = "8";
        //            break;
        //    }
        //    emptyRoomList.Clear();

        //    Time08Grid.Background = new SolidColorBrush(gridColorGray);
        //    Time10Grid.Background = new SolidColorBrush(gridColorGray);
        //    Time14Grid.Background = new SolidColorBrush(gridColorGray);
        //    Time16Grid.Background = new SolidColorBrush(gridColorGray);
        //    Time19Grid.Background = new SolidColorBrush(gridColorGray);
        //    Time21Grid.Background = new SolidColorBrush(gridColorGray);
        //    for (int i = 0; i < 6; i++)
        //    {
        //        gridColor[i] = false;
        //    }
        //    for (int i = 0; i < 6; i++)
        //    {
        //        for (int j = 0; j < emptyRoomReslut[i].Length; j++)
        //            emptyRoomReslut[i][j] = "";
        //    }
        //}

        /// <summary>
        /// 空教室网络请求
        /// </summary>
        /// <param name="sectionNum">当前选择的时间段</param>
        /// <param name="isFail">是否为失败的重试，区别为若是重试，请求完不刷新界面</param>
        private async void EmptyPost(int sectionNum, bool isFail = false)
        {
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("buildNum", buildNum));
            paramList.Add(new KeyValuePair<string, string>("sectionNum", sectionNum.ToString()));
            paramList.Add(new KeyValuePair<string, string>("week", appSetting.Values["nowWeek"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("weekdayNum", NowWeekday));
            //await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "正在查询空教室...", isIndeterminate: true);
            string emptyRoom = await NetWork.getHttpWebRequest("api/roomEmpty", paramList);
            Debug.WriteLine("emptyRoom->" + emptyRoom);
            if (emptyRoom != "")
            {
                JObject obj = JObject.Parse(emptyRoom);
                if (Int32.Parse(obj["status"].ToString()) == 200)
                {
                    EmptyRoomList emptyRoomItem = new EmptyRoomList();
                    emptyRoomItem.GetAttribute(obj);
                    emptyRoomReslut[sectionNum] = emptyRoomItem.RoomArray;
                    if (!isFail)
                        ShowEmpty();
                }
                else
                {
                    ListFailedStackPanel.Visibility = Visibility.Visible;
                    isShowEmpty = false;
                    emptyRoomList.Clear();
                    emptyRoomAndFloor.Clear();
                }
            }
            else
            {
                ListFailedStackPanel.Visibility = Visibility.Visible;
                emptyRoomList.Clear();
                emptyRoomAndFloor.Clear();
                isShowEmpty = false;
            }
            //StatusBar statusBar = StatusBar.GetForCurrentView();
            //await statusBar.ProgressIndicator.HideAsync();
        }

        private void ShowEmpty()
        {
#if DEBUG
            for (int i = 0; i < 6; i++)
            {
                Debug.WriteLine(i + "  " + emptyRoomReslut[i][0]);
            }
#endif
            List<string[]> emptyRoomReslutuse = new List<string[]>();

            for (int i = 0; i < 6; i++)
            {
                if (emptyRoomReslut[i][0] != "")
                {
                    emptyRoomReslutuse.Add(emptyRoomReslut[i]);
                }
            }
            emptyRoomList.Clear();
            emptyRoomAndFloor.Clear();

            if (emptyRoomReslutuse.Count != 0)
            {
                for (int i = 1; i < emptyRoomReslutuse.Count; i++)
                {
                    emptyRoomReslutuse[0] = emptyRoomReslutuse[0].Intersect(emptyRoomReslutuse[i]).ToArray();
                }

                for (int i = 0; i < emptyRoomReslutuse[0].Length; i++)
                {
                    emptyRoomList.Add(new EmptyRoomList { Room = emptyRoomReslutuse[0][i] });
                }
            }

            foreach (var item in emptyRoomList)
                if (item.Room.Substring(0, 1) == "8")
                    isBuildEight = true;
            if (!isBuildEight)
            {
                //获取最大楼层
                foreach (var item in emptyRoomList)
                {
                    if (int.Parse(item.Room.Substring(1, 1)) > maxFloor)
                        maxFloor = int.Parse(item.Room.Substring(1, 1));
                }

                //创建maxFloor个List对象
                for (int i = 0; i < maxFloor; i++)
                {
                    EmptyRoomList er = new EmptyRoomList();
                    er.Floor = " #" + (i + 1).ToString();
                    er.Rooms = new List<string>();
                    foreach (var item in emptyRoomList)
                    {
                        if (item.Room.Substring(1, 1) == er.Floor.Substring(er.Floor.Length - 1, 1))
                            er.Rooms.Add(item.Room);
                    }
                    if (er.Rooms.Count != 0)
                        emptyRoomAndFloor.Add(er);
                }
            }
            else
            {
                //获取最大楼层
                foreach (var item in emptyRoomList)
                {
                    if (int.Parse(item.Room.Substring(1, 1)) > maxFloor)
                        maxFloor = int.Parse(item.Room.Substring(1, 1));
                }

                //创建maxFloor个List对象
                for (int i = 0; i < maxFloor; i++)
                {
                    EmptyRoomList er = new EmptyRoomList();
                    er.Floor = " #" + (i + 1).ToString();
                    er.Rooms = new List<string>();
                    foreach (var item in emptyRoomList)
                    {
                        if (item.Room.Substring(1, 1) == er.Floor.Substring(er.Floor.Length - 1, 1))
                            er.Rooms.Add(item.Room);
                    }
                    //八教 给Floor重命名
                    foreach (var item in er.Rooms)
                    {
                        switch (item.Substring(1, 1))
                        {
                            case "1":
                                BuildEight = " 红楼";
                                break;

                            case "2":
                                BuildEight = " 绿楼";
                                break;

                            case "3":
                                BuildEight = " 蓝楼";
                                break;
                        }
                    }
                    er.Floor = BuildEight;
                    if (er.Rooms.Count != 0)
                        emptyRoomAndFloor.Add(er);
                    BuildEight = "";
                }
                isBuildEight = false;
            }
        }

        private void ListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListFailedStackPanel.Visibility = Visibility.Collapsed;
            isShowEmpty = true;
            for (int i = 0; i < gridColor.Length; i++)
            {
                if (gridColor[i] && isShowEmpty)
                    EmptyPost(i, true);
            }
            if (isShowEmpty)
                ShowEmpty();
        }

        private MenuFlyoutItem getJXLMenuFlyoutItem(string text)
        {
            MenuFlyoutItem menuFlyoutItem = new MenuFlyoutItem();
            menuFlyoutItem.Text = text;
            menuFlyoutItem.Click += JXLMenuFlyoutItem_click;
            return menuFlyoutItem;
        }

        private void JXLMenuFlyoutItem_click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            FilterAppBarToggleButton.Label = menuFlyoutItem.Text;
            switch (menuFlyoutItem.Text)
            {
                case "二教":
                    buildNum = "2";
                    break;

                case "三教":
                    buildNum = "3";
                    break;

                case "四教":
                    buildNum = "4";
                    break;

                case "五教":
                    buildNum = "5";
                    break;

                case "八教":
                    buildNum = "8";
                    break;
            }
            emptyRoomList.Clear();
            emptyRoomAndFloor.Clear();

            Time08Grid.Background = new SolidColorBrush(gridColorGray);
            Time10Grid.Background = new SolidColorBrush(gridColorGray);
            Time14Grid.Background = new SolidColorBrush(gridColorGray);
            Time16Grid.Background = new SolidColorBrush(gridColorGray);
            Time19Grid.Background = new SolidColorBrush(gridColorGray);
            Time21Grid.Background = new SolidColorBrush(gridColorGray);
            for (int i = 0; i < 6; i++)
            {
                gridColor[i] = false;
            }
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < emptyRoomReslut[i].Length; j++)
                    emptyRoomReslut[i][j] = "";
            }
        }

        private void FilterAppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyout JXLMenuFlyout = new MenuFlyout();

            JXLMenuFlyout.Items.Add(getJXLMenuFlyoutItem("二教"));
            JXLMenuFlyout.Items.Add(getJXLMenuFlyoutItem("三教"));
            JXLMenuFlyout.Items.Add(getJXLMenuFlyoutItem("四教"));
            JXLMenuFlyout.Items.Add(getJXLMenuFlyoutItem("五教"));
            JXLMenuFlyout.Items.Add(getJXLMenuFlyoutItem("八教"));
            JXLMenuFlyout.ShowAt(FilterAppBarToggleButton);
        }
    }
}