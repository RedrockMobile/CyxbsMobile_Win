﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ExamPage : Page
    {
        private ApplicationDataContainer appSetting;
        int IsExamOrRe;
        public ExamPage()
        {
            this.InitializeComponent();
            appSetting = ApplicationData.Current.LocalSettings;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            IsExamOrRe = System.Int32.Parse(e.Parameter.ToString());
            initExam();
            UmengSDK.UmengAnalytics.TrackPageStart("ExamPage");
        }

        private async void initExam()
        {
            string exam = "";
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();


            await Utils.ShowSystemTrayAsync(Color.FromArgb(255, 2, 140, 253), Colors.White, text: "正在紧张安排考试...", isIndeterminate: true);
            if (IsExamOrRe == 2)
            {
                ExamTextBlock.Text = "考试安排";
                paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
                exam = await NetWork.getHttpWebRequest("api/examSchedule", paramList);
            }
            else if (IsExamOrRe == 3)
            {
                ExamTextBlock.Text = "补考安排";
#if DEBUG
                paramList.Add(new KeyValuePair<string, string>("stu", "2014214136"));
#else   
                paramList.Add(new KeyValuePair<string, string>("stu", appSetting.Values["stuNum"].ToString()));
#endif
                exam = await NetWork.getHttpWebRequest("examapi/index.php", paramList);
            }
            Debug.WriteLine("exam->" + exam);
            if (exam != "")
            {
                try
                {
                    JObject obj = JObject.Parse(exam);
                    if (Int32.Parse(obj["status"].ToString()) == 200)
                    {
                        List<ExamList> examList = new List<ExamList>();
                        JArray ExamListArray = Utils.ReadJso(exam);
                        for (int i = 0; i < ExamListArray.Count; i++)
                        {
                            ExamList examitem = new ExamList();
                            examitem.GetAttribute((JObject)ExamListArray[i]);
                            if (IsExamOrRe == 2)
                                examitem.DateTime = "第" + examitem.Week + "周周" + examitem.Weekday + "\r\n" + examitem.Begin_time + "-" + examitem.End_time;
                            else if (IsExamOrRe == 3)
                                examitem.DateTime = "日期:" + examitem.Date + "\r\n" + "时间:" + examitem.Time;
                            examList.Add(examitem);
                        }
                        examList = examList.OrderBy(x => x.DateTime).ToList();
                        var nonzeroweek = from x in examList where x.Begin_time == "待定" select x;//    examList.Select(x => !x.DateTime.Contains("周0")).ToList();
                        var zeroweek = from x in examList where x.Begin_time != "待定" select x;// examList.Select(x => x.DateTime.Contains("周0"));
                        List<ExamList> orderedlist = new List<ExamList>();
                        orderedlist.AddRange(zeroweek);
                        orderedlist.AddRange(nonzeroweek);
                        ObservableCollection<ExamList> move = new ObservableCollection<ExamList>();
                        ExamListView.ItemsSource = move;
                        for (int i = 0; i < orderedlist.Count; i++)
                        {
                            move.Add(orderedlist[i]);
                            await Task.Delay(60);
                        }
                        //ExamListView.ItemsSource = examList;
                    }
                    else if (Int32.Parse(obj["status"].ToString()) == 300)
                    {
                        ListFailedStackPanelTextBlock.Text = "暂无数据，过几天再来看看";

                        ListFailedStackPanel.Visibility = Visibility.Visible;
                        ListFailedStackPanelImage.Visibility = Visibility.Collapsed;
                        ListFailedStackPanelTextBlock.Visibility = Visibility.Visible;
                    }
                    else if (Int32.Parse(obj["status"].ToString()) == 0)
                    {
                        ListFailedStackPanelTextBlock.Text = "没补考的孩子别瞎点";

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
                catch (Exception)
                {
                    Debug.WriteLine("考试信息->解析异常");
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
            UmengSDK.UmengAnalytics.TrackPageEnd("ExamPage");
        }

        private void ListFailedStackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ListFailedStackPanel.Visibility = Visibility.Collapsed;
            ListFailedStackPanelImage.Visibility = Visibility.Collapsed;
            ListFailedStackPanelTextBlock.Visibility = Visibility.Collapsed;
            initExam();
        }
    }
}
