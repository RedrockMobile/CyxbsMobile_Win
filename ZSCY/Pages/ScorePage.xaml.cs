using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class ScorePage : Page
    {
        private ApplicationDataContainer appSetting;
        public ScorePage()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageStart("ScorePage");
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            initScore();
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
            UmengSDK.UmengAnalytics.TrackPageEnd("ScorePage");
            await statusBar.ProgressIndicator.HideAsync();
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
        }

        private async void initScore()
        {
            await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "正在紧张批改试卷...", isIndeterminate: true);
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            string score = await NetWork.getHttpWebRequest("api/examGrade", paramList);
            Debug.WriteLine("score->" + score);
            if (score != "")
            {
                JObject obj = JObject.Parse(score);
                if (Int32.Parse(obj["status"].ToString()) == 200)
                {
                    List<ScoreList> scoreList = new List<ScoreList>();
                    JArray ScoreListArray = Utils.ReadJso(score);
                    for (int i = 0; i < ScoreListArray.Count; i++)
                    {
                        ScoreList classitem = new ScoreList();
                        classitem.GetAttribute((JObject)ScoreListArray[i]);
                        scoreList.Add(classitem);
                    }
                    ScoreListView.ItemsSource = scoreList;
                }
                else if (Int32.Parse(obj["status"].ToString()) == 300)
                {
                    ListFailedStackPanelTextBlock.Text = "暂无数据，过几再来看看";

                    ListFailedStackPanel.Visibility = Visibility.Visible;
                    ListFailedStackPanelImage.Visibility = Visibility.Collapsed;
                    ListFailedStackPanelTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    ListFailedStackPanelTextBlock.Text = "加载失败，点击重试";

                    ListFailedStackPanel.Visibility = Visibility.Visible;
                    ListFailedStackPanelImage.Visibility = Visibility.Visible;
                    ListFailedStackPanelTextBlock.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ListFailedStackPanelTextBlock.Text = "加载失败，点击重试";

                ListFailedStackPanel.Visibility = Visibility.Visible;
                ListFailedStackPanelImage.Visibility = Visibility.Visible;
                ListFailedStackPanelTextBlock.Visibility = Visibility.Visible;
            }

            StatusBar statusBar = StatusBar.GetForCurrentView();
            await statusBar.ProgressIndicator.HideAsync();
        }

        private void ListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListFailedStackPanel.Visibility = Visibility.Collapsed;
            ListFailedStackPanelImage.Visibility = Visibility.Collapsed;
            ListFailedStackPanelTextBlock.Visibility = Visibility.Collapsed;
            initScore();
        }
    }
}

