using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UmengSDK;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using Windows.System.Profile;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Data;
using ZSCY_Win10.Models.RemindModels;
using ZSCY_Win10.Pages.CommunityPages;
using ZSCY_Win10.Pages.StartPages;
using ZSCY_Win10.Util;
using ZSCY_Win10.ViewModels.Community;
using ZSCY_Win10.ViewModels.Remind;

/*
                   _ooOoo_
                  o8888888o
                  88" . "88
                  (| -_- |)
                  O\  =  /O
               ____/`---'\____
             .'  \\|     |//  `.
            /  \\|||  :  |||//  \
           /  _||||| -:- |||||-  \
           |   | \\\  -  /// |   |
           | \_|  ''\---/''  |   |
           \  .-\__  `-`  ___/-. /
         ___`. .'  /--.--\  `. . __
      ."" '<  `.___\_<|>_/___.'  >'"".
     | | :  `- \`.;`\ _ /`;.`/ - ` : | |
     \  \ `-.   \_ __\ /__ _/   .-` /  /
======`-.____`-.___\_____/___.-`____.-'======
                   `=---='
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
         佛祖保佑  永无BUG  UWP专供
*/

namespace ZSCY_Win10
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {

        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        //public static JWList[] jwlistCache;
        public static ObservableCollection<JWList> JWListCache = new ObservableCollection<JWList>();
        public static ObservableCollection<uIdList> muIdList = new ObservableCollection<uIdList>();
        public static bool showpane = true;
        public static MobileServiceClient MobileService = new MobileServiceClient("https://cqupt.azurewebsites.net");
        public static string picstart = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/";
        public static string picstartsmall = "http://hongyan.cqupt.edu.cn/cyxbsMobile/Public/photo/thumbnail/";
        public static CommunityViewModel ViewModel { get; set; }
        public static int CommunityPivotState;
        public static double CommunityScrollViewerOffset;
        public static bool isPerInfoContentImgShow = false;
        private string exampleTaskName = "MessageBackgroundTask";
        public static Resource.APPTheme APPTheme = new Resource.APPTheme();
        public static bool[] isReduced = { true, true, true, true };
        public static bool[] isLoading = { false, false, false, false, false, false, false, false };
        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();
        private static string resourceName = "ZSCY";
        #region 事件提醒
        //public static TimeSet[,] timeSet = new TimeSet[6, 7];
        //public static SelTimeStringViewModel SelectedTime = new SelTimeStringViewModel();
        //public static ObservableCollection<SelectedWeekNum> selectedWeekNumList = new ObservableCollection<SelectedWeekNum>();
        //public static SelWeekNumStringViewModel selectedWeek = new SelWeekNumStringViewModel();
        ///// <summary>
        /////提醒列表的数据源
        //l/// </summary>
        //public static ObservableCollection<MyRemind> remindList = new ObservableCollection<MyRemind>();

        public static string RemindListDBPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "RemindList.db");
        /// <summary>
        /// 防止改写事件内容是触发导航加载
        /// </summary>
        public static bool isLoad = false;
        public static List<int> SelWeekList = new List<int>();
        public static List<SelCourseModel> SelCoursList = new List<SelCourseModel>();
        public static AddRemindPageViewModel addRemindViewModel = new AddRemindPageViewModel();
        public static int indexBefore = -1;

        #endregion
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.Resuming += this.OnResuming;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
            {
                try
                {
                    var vault = new Windows.Security.Credentials.PasswordVault();
                    var credentialList = vault.FindAllByResource(resourceName);
                    credentialList[0].RetrievePassword();
                    if (JumpList.IsSupported() && credentialList.Count > 0)
                        SetSystemGroupAsync();
                    else if (JumpList.IsSupported())
                        DisableSystemJumpListAsync();
                }
                catch
                {
                    if (JumpList.IsSupported())
                        DisableSystemJumpListAsync();
                }
                //if (JumpList.IsSupported() && appSetting.Values.ContainsKey("idNum"))
            }
            //if (!appSetting.Values.ContainsKey("AllKBGray"))
            //{
            //    appSetting.Values["AllKBGray"] = false;
            //}
            if (!appSetting.Values.ContainsKey("CommunityPerInfo"))
            {
                appSetting.Values["CommunityPerInfo"] = false;
            }

            if (!appSetting.Values.ContainsKey("Community_headimg_src"))
            {
                appSetting.Values["Community_headimg_src"] = "ms-appx:///Assets/Community_nohead.png";
            }

            if (!appSetting.Values.ContainsKey("isUseingBackgroundTask"))
            {
                appSetting.Values["isUseingBackgroundTask"] = true;
                addBackgroundTask();
            }
            if (bool.Parse(appSetting.Values["isUseingBackgroundTask"].ToString()))
            {
                addBackgroundTask();
            }


            //监听异常
            CoreApplication.UnhandledErrorDetected += CoreApplication_UnhandledErrorDetected;

        }

        private async void CoreApplication_UnhandledErrorDetected(object sender, UnhandledErrorDetectedEventArgs e)
        {
            try
            {
                e.UnhandledError.Propagate();
            }
            catch (Exception ex)
            {

                StorageFile file = null;
                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync("ZSCY_Mobile_Log.txt", CreationCollisionOption.OpenIfExists);
                else
                    file = await ApplicationData.Current.LocalFolder.CreateFileAsync("ZSCY_Log.txt", CreationCollisionOption.OpenIfExists);
                string errorText = $"[{DateTime.Now.ToString()}]\r\n{ex.StackTrace}\r\n";
                await FileIO.AppendTextAsync(file, errorText, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
            finally
            {
#if DEBUG
                Application.Current.Exit();
#else
#endif
            }
        }

        private async void addBackgroundTask()
        {
            List<string> backgroundName = new List<string>();
            backgroundName.Add(exampleTaskName);
            backgroundName.Add("Toastbuilder");
            backgroundName.Add("LiveTileBackgroundTask");
            backgroundName.Add("RemindBackgroundTask");
            try
            {
    
                foreach (var item in backgroundName)
                {
                    var list = from i in BackgroundTaskRegistration.AllTasks
                               where i.Value.Name == item
                               select i;
                    foreach (var i in list)
                    {
                        i.Value.Unregister(true);
                    }
                }
                BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();
                BackgroundTaskBuilder builder1 = new BackgroundTaskBuilder();
                builder1.Name = exampleTaskName;
                builder1.TaskEntryPoint = "MyMessageBackgroundTask.MessageBackgroundTask";
                builder1.SetTrigger(new TimeTrigger(15, false)); //定时后台任务
                BackgroundTaskRegistration task = builder1.Register();
                BackgroundTaskBuilder Toastbuilder = new BackgroundTaskBuilder();
                Toastbuilder.Name = "Toastbuilder";
                Toastbuilder.TaskEntryPoint = "MyMessageBackgroundTask.ToastBackgroundTask";
                Toastbuilder.SetTrigger(new ToastNotificationActionTrigger());
                BackgroundTaskRegistration Toasttask = Toastbuilder.Register();

                BackgroundTaskBuilder builder2 = new BackgroundTaskBuilder();
                builder2.Name = "LiveTileBackgroundTask";
                builder2.TaskEntryPoint = "LiveTileBackgroundTask.LiveTileBackgroundTask";
                builder2.SetTrigger(new TimeTrigger(15, false));
                BackgroundTaskRegistration registration = builder2.Register();

                BackgroundTaskBuilder builder3 = new BackgroundTaskBuilder();
                builder3.Name = "RemindBackgroundTask";
                builder3.TaskEntryPoint = "SycnRemindBackgroundTask.RemindBackgroundTask";
                builder3.SetTrigger(new TimeTrigger(15, false));
                BackgroundTaskRegistration sycnRemind = builder3.Register();
            }
            catch (Exception) { }
        }

        private async void DisableSystemJumpListAsync()
        {
            var jumpList = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();
            jumpList.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.None;
            jumpList.Items.Clear();
            await jumpList.SaveAsync();
        }
        private Windows.UI.StartScreen.JumpListItem CreateJumpListItemTask(string u, string description, string uri)
        {
            var taskItem = JumpListItem.CreateWithArguments(
                                    u, description);
            taskItem.Description = description;
            taskItem.Logo = new Uri(uri);
            return taskItem;
        }
        private async void SetSystemGroupAsync()
        {
            var jumpList = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();
            jumpList.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.Frequent;
            jumpList.Items.Clear();
            jumpList.Items.Add(CreateJumpListItemTask("/jwzx", "资讯", "ms-appx:///Assets/iconfont-news_w.png"));
            jumpList.Items.Add(CreateJumpListItemTask("/more", "更多", "ms-appx:///Assets/iconfont-more_w.png"));
            await jumpList.SaveAsync();
        }



        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {

            //#if DEBUG
            //            if (System.Diagnostics.Debugger.IsAttached)
            //            {
            //                this.DebugSettings.EnableFrameRateCounter = true;
            //            }
            //#endif
            //UmengAnalytics.IsDebug = true;
            Frame rootFrame = Window.Current.Content as Frame;
            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;
                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }
                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }
            if (rootFrame.Content == null)
            {
                // 当导航堆栈尚未还原时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
#if WINDOWS_PHONE_APP
                Debug.WriteLine("#if WINDOWS_PHONE_APP");
#endif
                try
                {
                    //if (e.Kind == ActivationKind.Launch && (e.Arguments == "/jwzx" || e.Arguments == "/more") && appSetting.Values.ContainsKey("idNum"))
                    var vault = new Windows.Security.Credentials.PasswordVault();
                    var credentialList = vault.FindAllByResource(resourceName);
                    credentialList[0].RetrievePassword();
                    if (e.Kind == ActivationKind.Launch && (e.Arguments == "/jwzx" || e.Arguments == "/more") && credentialList.Count > 0)
                    {
                        if (!rootFrame.Navigate(typeof(StartPage), e.Arguments))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                    else
                    {
                        //if (!appSetting.Values.ContainsKey("idNum"))
                        if (!(credentialList.Count > 0))
                        {
                            //if (!rootFrame.Navigate(typeof(LoginPage), e.Arguments))
                            if (!rootFrame.Navigate(typeof(StartPage), e.Arguments))
                            {
                                throw new Exception("Failed to create initial page");
                            }
                        }
                        else
                        {
                            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar") && Utils.getPhoneWidth() < 400)
                            {
                                Debug.WriteLine("小于400的Phone" + Utils.getPhoneWidth());
                                //if (!rootFrame.Navigate(typeof(StartPage_m), e.Arguments))
                                //{
                                //    throw new Exception("Failed to create initial page");
                                //}
                                if (!rootFrame.Navigate(typeof(StartPage), "/kb"))
                                {
                                    throw new Exception("Failed to create initial page");
                                }
                            }
                            else
                            {
                                Debug.WriteLine("大于400的phone OR PC");
                                if (!rootFrame.Navigate(typeof(StartPage), "/kb"))
                                {
                                    throw new Exception("Failed to create initial page");
                                }
                            }
                        }
                    }
                }
                catch
                {
                    if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar") && Utils.getPhoneWidth() < 400)
                    {
                        Debug.WriteLine("小于400的Phone" + Utils.getPhoneWidth());
                        //if (!rootFrame.Navigate(typeof(StartPage_m), e.Arguments))
                        //{
                        //    throw new Exception("Failed to create initial page");
                        //}
                        if (!rootFrame.Navigate(typeof(StartPage), "/kb"))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("大于400的phone OR PC");
                        if (!rootFrame.Navigate(typeof(StartPage), "/kb"))
                        {
                            throw new Exception("Failed to create initial page");
                        }
                    }
                }
            }


            Window.Current.Activate();

            // 确保当前窗口处于活动状态
            //await UmengAnalytics.StartTrackAsync("55cd8c8be0f55a20ba00440d", "Marketplace_Win10"); //私有
            await UmengAnalytics.StartTrackAsync("57317d07e0f55a28fe002bec", "Marketplace_Win10"); //公共
                                                                                                   //await InitNotificationsAsync();
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 400, Height = 480 });



        }
        private async void OnResuming(object sender, object e)
        {
            //await UmengAnalytics.StartTrackAsync("55cd8c8be0f55a20ba00440d", "Marketplace_Win10"); //私有
            await UmengAnalytics.StartTrackAsync("57317d07e0f55a28fe002bec", "Marketplace_Win10"); //公共
        }

        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
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
            //TODO: 保存应用程序状态并停止任何后台活动
            await UmengAnalytics.EndTrackAsync();
            deferral.Complete();
        }
        private async Task InitNotificationsAsync()
        {
            try
            {
                // Get a channel URI from WNS.
                var channel = await PushNotificationChannelManager
                    .CreatePushNotificationChannelForApplicationAsync();

                // Register the channel URI with Notification Hubs.
                await App.MobileService.GetPush().RegisterAsync(channel.Uri);
                Debug.WriteLine(channel.Uri);

            }
            catch (Exception channel)
            {
                Debug.WriteLine(channel.Message);
            }
        }

        protected async override void OnActivated(IActivatedEventArgs args)
        {
            //判断是否为Toast所激活
            if (args.Kind == ActivationKind.ToastNotification)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    rootFrame.NavigationFailed += OnNavigationFailed;
                    Window.Current.Content = rootFrame;
                }
                rootFrame.Navigate(typeof(MainPage));
            }
            //Window.Current.Activate();
        }

    }
}
