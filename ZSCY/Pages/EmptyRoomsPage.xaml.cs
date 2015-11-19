using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EmptyRoomsPage : Page
    {
        private ApplicationDataContainer appSetting;
        Color gridColorGray = new Color();
        Color gridColorBlue = new Color();
        Boolean[] gridColor = new Boolean[]{
            false,
            false,
            false,
            false,
            false,
            false,
        };

        string[] emptyReslut = new string[6]; //保存返回值
        string[][] emptyRoomReslut = new string[6][]; //保存返回的教室值

        string NowWeek;
        string buildNum = "2";
        string NowWeekday;

        bool isShowEmpty = true;

        ObservableCollection<EmptyRoomList> emptyRoomList = new ObservableCollection<EmptyRoomList>();

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
            EmptyGridView.ItemsSource = emptyRoomList;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            UmengSDK.UmengAnalytics.TrackPageStart("EmptyRoomsPage");
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }

        }

        //离开页面时，取消事件
        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
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


        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            JXLButton.Content = (sender as MenuFlyoutItem).Text;
            switch (JXLButton.Content.ToString())
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
            await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "正在查询空教室...", isIndeterminate: true);
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
                }
            }
            else
            {
                ListFailedStackPanel.Visibility = Visibility.Visible;
                emptyRoomList.Clear();
                isShowEmpty = false;
            }
            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
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

            //for (int i = 0, j = 0; i < 6; i++)
            //{
            //    Debug.WriteLine("item" + i + "   " + item[i]);
            //}

            //if (item[0] == -1)
            //{
            //    emptyRoomList.Clear();
            //}
            //else if (item[1] == -1)
            //{
            //    emptyRoomList.Clear();
            //    for (int i = 0; i < emptyRoomReslut[item[0]].Length; i++)
            //    {
            //        if (emptyRoomReslut[item[0]][i] == null || emptyRoomReslut[item[0]][i] == "")
            //            break;
            //        emptyRoomList.Add(new EmptyRoomList { Room = emptyRoomReslut[item[0]][i] });
            //    }
            //}
            //else
            //{
            //    emptyRoomList.Clear();
            //    for (int i = 0; i < emptyRoomReslut[item[0]].Length; i++)
            //        emptyClassUnrepeat[i] = emptyRoomReslut[item[0]][i];
            //    for (int i = 0; item[i + 1] != -1; i++)
            //    {
            //        if (item[i + 1] == -1)
            //            break;
            //        IEnumerable<string> skip = emptyClassUnrepeat.Skip(0);
            //        IEnumerable<string> take = emptyRoomReslut[item[i + 1]].Skip(0);
            //        IEnumerable<string> intersect = skip.Intersect(take);

            //        int j = 0;
            //        foreach (var s in intersect)
            //        {
            //            Debug.WriteLine(s);
            //            emptyClassUnrepeat[j] = s;
            //            j++;
            //        }

            //        if (i == 5)
            //            break;

            //    }
            //    for (int i = 0; i < 100; i++)
            //    {
            //        if (i > 1)
            //        {
            //            if (emptyClassUnrepeat[i] != "" && Int16.Parse(emptyClassUnrepeat[i]) < Int16.Parse(emptyClassUnrepeat[i - 1]))
            //                break;
            //            else if (emptyClassUnrepeat[i] == "")
            //                break;
            //        }

            //        if (emptyClassUnrepeat[i] != "")
            //            emptyRoomList.Add(new EmptyRoomList { Room = emptyClassUnrepeat[i] });
            //    }
            //}
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


    }
}
