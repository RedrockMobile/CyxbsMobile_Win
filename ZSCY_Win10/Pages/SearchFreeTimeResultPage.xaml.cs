using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Phone.UI.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchFreeTimeResultPage : Page
    {
        private ObservableCollection<uIdList> muIdList = new ObservableCollection<uIdList>();
        private ObservableCollection<FreeList> mFreeList = new ObservableCollection<FreeList>();
        private string[] kb;
        private int week;
        private int[,] freeclasstime = new int[7, 6]; //7*6数组

        public SearchFreeTimeResultPage()
        {
            this.InitializeComponent();
            FreeListView.ItemsSource = mFreeList;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。
        /// 此参数通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            AuIdList auIdList = (AuIdList)e.Parameter;
            muIdList = auIdList.muIdList;

            initFree();
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
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

        private async void initFree()
        {
            kb = new string[muIdList.Count];
            for (int i = 0; i < muIdList.Count; i++)
            {
                int issuccess = 0;
                while (issuccess < 2)
                {
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("stuNum", muIdList[i].uId));
                    string kbtemp = await NetWork.getHttpWebRequest("redapi2/api/kebiao", paramList); //新
                    if (kbtemp != "")
                    {
                        kb[i] = kbtemp;
                        issuccess = 2;
                    }
                    else
                    {
                        kb[i] = "";
                        issuccess++;
                    }
                }
                FreeLoddingProgressBar.Value = FreeLoddingProgressBar.Value + 100.0 / muIdList.Count;
                Debug.WriteLine(FreeLoddingProgressBar.Value);
            }
            initFreeList();
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

        private void initFreeList()
        {
            FreeLoddingTextBlock.Text = "处理中...";
            FreeLoddingProgressBar.Value = 0;
            for (int i = 0; i < kb.Length; i++)
            {
                if (kb[i] != "")
                {
                    JObject obj = JObject.Parse(kb[i]);
                    if (Int32.Parse(obj["status"].ToString()) == 200)
                    {
                        JArray ClassListArray = Utils.ReadJso(kb[i]);
                        for (int j = 0; j < ClassListArray.Count; j++)
                        {
                            ClassList classitem = new ClassList();
                            classitem.GetAttribute((JObject)ClassListArray[j]);
                            Debug.WriteLine(Array.IndexOf(classitem.Week, week));
                            if (Array.IndexOf(classitem.Week, week) != -1)
                            {
                                freeclasstime[classitem.Hash_day, classitem.Hash_lesson] = 1;
                            }
                        }
                    }
                }
                FreeLoddingProgressBar.Value = FreeLoddingProgressBar.Value + 100.0 / muIdList.Count;
                Debug.WriteLine(FreeLoddingProgressBar.Value);
            }
            FreeLoddingStackPanel.Visibility = Visibility.Collapsed;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (freeclasstime[i, j] == 0)
                    {
                        FreeList ft = new FreeList();
                        ft.vis = 1;
                        ft.weekday = i;
                        mFreeList.Add(ft);
                        break;
                    }
                }

                for (int j = 0; j < 6; j++)
                {
                    if (freeclasstime[i, j] == 0)
                    {
                        FreeList fc = new FreeList();
                        fc.vis = 0;
                        fc.time = j;
                        mFreeList.Add(fc);
                    }
                }
            }
        }
    }
}