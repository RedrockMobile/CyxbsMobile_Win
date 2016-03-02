﻿using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UmengSDK;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;

// “空白应用程序”模板在 http://go.microsoft.com/fwlink/?LinkId=391641 上有介绍

namespace ZSCY
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    public sealed partial class App : Application
    {
        // http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x804



        private TransitionCollection transitions;
        private ApplicationDataContainer appSetting;
        public static ObservableCollection<uIdList> muIdList = new ObservableCollection<uIdList>();

        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.Resuming += this.OnResuming;
            appSetting = ApplicationData.Current.LocalSettings;

            if (appSetting.Values.ContainsKey("donewVersion"))
            {
                if (Int32.Parse(appSetting.Values["donewVersion"].ToString()) == 1)
                {
                    newVersion();
                    appSetting.Values["donewVersion"] = 2;
                }
            }
            else
            {
                appSetting.Values["donewVersion"] = 2;
                newVersion();
            }
            //byte[] myDeviceID = (byte[])Windows.Phone.Devices.Not .DeviceExtendedProperties.GetValue("DeviceUniqueId");
            //string maAddress = BitConverter.ToString(myDeviceID);

        }

        private void newVersion()
        {

            //if (Int32.Parse(appSetting.Values["donewVersion"].ToString()) == 1)
            //{
            //    try
            //    {
            //        var a = appSetting.Values.ToArray();
            //        for (int i = 0; i < a.Length; i++)
            //        {
            //            if (a[i].Key != "idNum" && a[i].Key != "stuNum" && a[i].Key != "name" && a[i].Key != "classNum" && a[i].Key != "nowWeek" && a[i].Key != "donewVersion")
            //                appSetting.Values.Remove(a[i].Key);
            //        }
            //    }
            //    catch (Exception)
            //    {
            //        Debug.WriteLine("清除旧版本数据异常");
            //    }
            //}
        }

        private async void OnResuming(object sender, object e)
        {
            await UmengAnalytics.StartTrackAsync("55cd8c8be0f55a20ba00440d", "Marketplace");
        }


        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 当启动应用程序以打开特定的文件或显示时使用
        /// 搜索结果等
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                // TODO: 将此值更改为适合您的应用程序的缓存大小
                rootFrame.CacheSize = 1;

                //设置默认语言
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 删除用于启动的旋转门导航。
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 新页面
                if (!appSetting.Values.ContainsKey("idNum"))
                {
                    if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }
                else
                {
                    if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                    {
                        throw new Exception("Failed to create initial page");
                    }
                }

            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
            await UmengAnalytics.StartTrackAsync("55cd8c8be0f55a20ba00440d", "Marketplace");
        }


        /// <summary>
        /// 启动应用程序后还原内容转换。
        /// </summary>
        /// <param name="sender">附加了处理程序的对象。</param>
        /// <param name="e">有关导航事件的详细信息。</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await UmengAnalytics.EndTrackAsync();
            // TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }




    }
}