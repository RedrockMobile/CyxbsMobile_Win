using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ScorePage : Page
    {
        private ApplicationDataContainer appSetting;
        private static string resourceName = "ZSCY";

        public ScorePage()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.InitializeComponent();
            Debug.WriteLine("init");
            //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
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
            initScore();
            this.progress.IsActive = false;
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //StatusBar statusBar = StatusBar.GetForCurrentView();
            //await statusBar.ProgressIndicator.HideAsync();
        }

        private async void initScore()
        {
            //await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "正在紧张批改试卷...", isIndeterminate: true);
            JObject score = await Requests.Send("api/examGrade");
            Debug.WriteLine("score->" + score);
            if (score != null)
            {
                if (Int32.Parse(score["status"].ToString()) == 200)
                {
                    List<ScoreList> scoreList = new List<ScoreList>();
                    JArray ScoreListArray = (JArray)score["data"];
                    for (int i = 0; i < ScoreListArray.Count; i++)
                    {
                        ScoreList classitem = new ScoreList();
                        classitem.GetAttribute((JObject)ScoreListArray[i]);
                        scoreList.Add(classitem);
                    }
                    ScoreListView.ItemsSource = scoreList;
                }
                else if (Int32.Parse(score["status"].ToString()) == 300)
                {
                    ListFailedStackPanelTextBlock.Text = "暂无数据，过几天再来看看";

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

            //StatusBar statusBar = StatusBar.GetForCurrentView();
            //await statusBar.ProgressIndicator.HideAsync();
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