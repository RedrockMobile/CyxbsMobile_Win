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
        private List<ClassListLight> result = new List<ClassListLight>();
        string[] kb; //课表数据的数组
        int week; //周次，0为学期
        int week_old; //周次，切换到学期后保存切换前的周次

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
                    state = "VisualState600";
                }
                KebiaoAllScrollViewer.Height = e.NewSize.Height - 48 - 25;

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
            ec.getfreetime(ref result);
            //freetime(11, forsearchlist);
            FreeLoddingStackPanel.Visibility = Visibility.Collapsed;
            FreeKBTableGrid.Visibility = Visibility.Visible;
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
        ///// <summary>
        ///// 传入周数，返回该周多人空闲时段
        ///// </summary>
        ///// <param name="weeknum"></param>
        //private void freetime(int weeknum, Dictionary<string, List<ClassListLight>> searchlist)
        //{
        //    //所有人的名字
        //    string[] names = (from n in muIdList select n.uName).ToArray();
        //    //星期，时间段，人名数组
        //    List<ClassListLight> clist = new List<ClassListLight>();
        //    if (weeknum != 0)
        //    {

        //        foreach (var key in searchlist.Keys)
        //        {
        //            //找到该周的所有课程
        //            clist.AddRange((from n in searchlist[key] where n.Week.Contains(weeknum) select n).ToList());
        //        }
        //        //添加都没课的时间
        //        //day和lesson都没有在clist里出现过就添加一个classlistlight对象
        //        for (int i = 0; i < 7; i++)//一周
        //        {
        //            for (int j = 0; j < 6; j++)//一天
        //            {
        //                //查时间有没有在集合里出现过
        //                ClassListLight ourfreetime = new ClassListLight() { Hash_day = i, Hash_lesson = j };
        //                if (!clist.Contains(ourfreetime, new ClassListLigthCompare()))
        //                {
        //                    ourfreetime.Name = names;
        //                    clist.Add(ourfreetime);
        //                }
        //            }
        //        }
        //        clist = clist.OrderBy(x => x.Hash_day).ToList();
        //        //筛选出该周内所有不在同一时间上课的课    
        //        var diisclist = from n in clist group n by new { n.Hash_day, n.Hash_lesson } into g where g.Count() < forsearchlist.Count select g;
        //        var ll = diisclist.ToList();
        //        for (int i = 0; i < ll.Count; i++)
        //        {
        //            ClassListLight tobeadded = ll[i].ToList()[0];
        //            if (tobeadded.Name != names)
        //            {
        //                tobeadded.Name = names.Except(tobeadded.Name).ToArray();
        //            }
        //            if (tobeadded.Course != null)
        //                Debug.WriteLine(tobeadded.Course);
        //            result.Add(tobeadded);
        //        }
        //        //大家都没课的时间
        //    }
        //    else
        //    {
        //        //todo学期空课表





        //    }
        //}
        ///// <summary>
        ///// 比较器
        ///// </summary>
        //private class ClassListLigthCompare : IEqualityComparer<ClassListLight>
        //{
        //    public bool Equals(ClassListLight x, ClassListLight y)
        //    {
        //        if (x.Hash_day == y.Hash_day && x.Hash_lesson == y.Hash_lesson)
        //        {
        //            return true;
        //        }
        //        else
        //            return false;
        //    }

        //    public int GetHashCode(ClassListLight obj)
        //    {
        //        return 0;
        //    }
        //}
    }
}
