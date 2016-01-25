using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
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
        string[] kb; //课表数据的数组

        public SearchFreeTimeResultPage_new()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 600)
                {
                    state = "VisualState600";
                }
                VisualStateManager.GoToState(this, state, true);
            };
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
            //auIdList里面还有一个week 是从num页面传过来的周次，现在不知道怎么用，也可以把学号页面的周次去掉，在这个页面进行周次的选择

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
                while (issuccess < 2) //失败后重试两次
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
            FreeLoddingStackPanel.Visibility = Visibility.Collapsed;
            FreeKBTableStackPanel.Visibility = Visibility.Visible;
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

    }
}
