﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Controls;
using ZSCY_Win10.Util;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        private Point currentPoint; //最新的，当前的点
        private Point oldPoint;//上一个点
        private bool isPoint = false;

        private List<NavMenuItem> navlist = new List<NavMenuItem>(
            new[]
            {
                 new NavMenuItem()
                {
                    Image = "ms-appx:///Assets/iconfont-table.png",
                    Label = "我的课表",
                    DestPage = typeof(KBPage)
                },
                new NavMenuItem()
                {
                    Image = "ms-appx:///Assets/iconfont-news.png",
                    Label = "资讯",
                    DestPage = typeof(NewsPage)
                },
                new NavMenuItem()
                {
                    Image = "ms-appx:///Assets/iconfont-more.png",
                    Label = "更多",
                    DestPage = typeof(MorePage)
                },
                new NavMenuItem()
                {
                    Symbol = Symbol.Setting,
                    Label = "设置",
                    DestPage = typeof(SettingPage)
                },
            }
            );

        public static MainPage Current = null;

        public MainPage()
        {
            this.InitializeComponent();
            // HamburgerButton.Click += HamburgerButton_Click;
            this.Loaded += (sender, args) =>
            {
                Current = this;
                //然并卵
                this.TogglePaneButton.Focus(FocusState.Programmatic);
            };
            //this.AppFrame.Navigate(navlist[0].DestPage, navlist[0].Arguments);
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Utils.ShowSystemTrayAsync(Color.FromArgb(255, 6, 140, 253), Colors.White);
            }
            else if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += OnBackPressed;
            }
            else
            {
                var view = ApplicationView.GetForCurrentView();
                view.TitleBar.BackgroundColor = Color.FromArgb(255, 4, 131, 239);
                view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 4, 131, 239);
                view.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 2, 126, 231);
                view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 2, 111, 203);
            }
            this.SizeChanged += (s, e) =>
            {
                Debug.WriteLine(e.NewSize.Width);
                if (e.NewSize.Width >= 400)
                {
                    RootSplitView.CompactPaneLength = 48;
                    TogglePaneButton.Visibility = Visibility.Visible;
                    TogglePaneLightButton.Visibility = Visibility.Collapsed;
                    App.showpane = true;
                }
                else
                {
                    RootSplitView.CompactPaneLength = 0;
                    TogglePaneButton.Visibility = Visibility.Collapsed;
                    TogglePaneLightButton.Visibility = Visibility.Visible;
                    App.showpane = false;
                }
                if (e.NewSize.Height - 96 < navlist.Count * 48)
                {
                    //高度太小
                    NavMenuList.Margin = new Thickness(0, 48, 0, 48);
                }
                else
                {
                    NavMenuList.Margin = new Thickness(0, 48, 0, 0);
                }
            };
            //TODO:未登录时 没有名字
            try
            {
                if (appSetting.Values.ContainsKey("name"))
                    stuNameTextBlock.Text = appSetting.Values["name"].ToString();
                else
                    stuNameTextBlock.Text = "尚未登录~";
            }
            catch
            {
                stuNameTextBlock.Text = "尚未登录~";
            }

            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequseted;
            //如果是在手机上，有实体键，隐藏返回键。
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                //this.BackButton.Visibility = Visibility.Collapsed;
            }
            NavMenuList.ItemsSource = navlist;
            ActivateWindow();
            try
            {
                if (bool.Parse(appSetting.Values["isLogin"].ToString()))
                    initHeadImage();
            }
            catch { }
        }

        private bool isExit = false;

        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                return;
            }//Frame内无内容
            if (rootFrame.CurrentSourcePageType.Name != "MainPage")
            {
                if (rootFrame.CanGoBack && e.Handled == false)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            }//Frame不在MainPage页面并且可以返回则返回上一个页面并且事件未处理
            else if (e.Handled == false)
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ShowAsync();
                statusBar.ProgressIndicator.Text = "再按一次返回键即将退出程序 ~\\(≧▽≦)/~"; // 状态栏显示文本
                statusBar.ProgressIndicator.ShowAsync();

                if (isExit)
                {
                    App.Current.Exit();
                }
                else
                {
                    isExit = true;
                    Task.Run(async () =>
                    {
                        await Task.Delay(1500);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            await statusBar.ProgressIndicator.HideAsync();
                            await statusBar.ShowAsync(); //此处不隐藏状态栏
                        });
                        isExit = false;
                    });
                    e.Handled = true;
                }//Frame在其他页面并且事件未处理
            }
        }
        private async void ActivateWindow()
        {
            await Task.Delay(100);
            Window.Current.Activate();
        }

        private async void initHeadImage()
        {
            JObject headimg = await Requests.Send("cyxbsMobile/index.php/Home/Person/search");
            if (headimg != null)
            {
                try
                {
                    if (Int32.Parse(headimg["status"].ToString()) == 200)
                    {
                        if (headimg["data"].ToString() != "")
                        {
                            string a = headimg["data"].ToString();
                            JObject objdata = JObject.Parse(headimg["data"].ToString());
                            headimgImageBrush.ImageSource = new BitmapImage(new Uri(objdata["photo_src"].ToString()));
                            appSetting.Values["Community_headimg_src"] = objdata["photo_src"].ToString();

                            Size downloadSize = new Size(48, 48);
                            await Utils.DownloadAndScale("headimg.png", objdata["photo_src"].ToString(), new Size(100, 100));
                        }
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("头像下载失败");
                }
            }
            else
            {
                try
                {
                    appSetting.Values["Community_headimg_src"] = "";
                    IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                    IStorageFile storageFileRE = await applicationFolder.GetFileAsync("headimg.png");
                    headimgImageBrush.ImageSource = new BitmapImage(new Uri(storageFileRE.Path));
                }
                catch (Exception)
                {
                    Debug.WriteLine("缓存头像文件不存在");
                }
            }
        }

        public Frame AppFrame { get { return this.frame; } }

        private void SystemNavigationManager_BackRequseted(object sender, BackRequestedEventArgs e)
        {
            //e.Handled = true;
            //Application.Current.Exit();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            bool ignored = false;
            this.BackRequested(ref ignored);
        }

        private void BackRequested(ref bool handled)
        {
            // Get a hold of the current frame so that we can inspect the app back stack.

            if (this.AppFrame == null)
                return;

            // Check to see if this is the top-most page on the app back stack.
            if (this.AppFrame.CanGoBack && !handled)
            {
                // If not, set the event to handled and go back to the previous page in the app.
                handled = true;//表示已经处理，跳过默认的返回命令处理程序
                this.AppFrame.GoBack();//后退
            }
        }

        //private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        //{
        //    SplitSet.IsPaneOpen = !SplitSet.IsPaneOpen;
        //}

        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    var api = "Windows.UI.ViewManagement.StatusBar";
        //    if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(api))
        //    {
        //        //await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
        //        var statusbar = StatusBar.GetForCurrentView();
        //        statusbar.BackgroundOpacity = 1;
        //        statusbar.BackgroundColor = Colors.CornflowerBlue;

        //    }

        //}

        //private string ConvertToUpper(string txt)
        //{
        //    bool isKeyExist = appSetting.Values.ContainsKey("ResultUpper");
        //    if (!isKeyExist) return txt;
        //    else
        //    {
        //        bool isUpper = Convert.ToBoolean(appSetting.Values["ResultUpper"]);
        //        if (isUpper)
        //        {
        //            return txt.ToUpper();
        //        }
        //        else
        //        {
        //            return txt;
        //        }

        //    }
        //}

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            bool ignored = false;
            this.BackRequested(ref ignored);
        }

        #region Navigation

        /// <summary>
        /// Navigate to the Page for the selected <paramref name="listViewItem"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="listViewItem"></param>
        private async void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            if (item != null)
            {
                if (item.DestPage != null &&
                    item.DestPage != this.AppFrame.CurrentSourcePageType)
                {
                    this.AppFrame.Navigate(item.DestPage, item.Arguments);
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Ensures the nav menu reflects reality when navigation is triggered outside of
        /// the nav menu buttons.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in this.navlist where p.DestPage == e.SourcePageType select p).SingleOrDefault();
                if (item == null && this.AppFrame.BackStackDepth > 0)
                {
                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in this.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in this.navlist where p.DestPage == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                StatusBar.GetForCurrentView().ShowAsync().AsTask();
            try
            {
                NavMenuList.ItemsSource = navlist;
                var jump = e.Parameter.ToString();
                switch (jump)
                {
                    case "/kb":
                        NavMenuList.SelectedItem = 0;
                        this.AppFrame.Navigate(navlist[0].DestPage, navlist[0].Arguments);
                        break;

                    case "/jwzx":
                        NavMenuList.SelectedItem = 1;
                        this.AppFrame.Navigate(navlist[2].DestPage, navlist[2].Arguments);
                        break;

                    case "/more":
                        NavMenuList.SelectedItem = 2;
                        this.AppFrame.Navigate(navlist[5].DestPage, navlist[5].Arguments);
                        break;

                    default:
                        NavMenuList.SelectedItem = 0;
                        this.AppFrame.Navigate(navlist[0].DestPage, navlist[0].Arguments);
                        break;
                }
            }
            catch (Exception)
            {
                NavMenuList.SelectedItem = 0;
                this.AppFrame.Navigate(navlist[0].DestPage, navlist[0].Arguments);
            }
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            // After a successful navigation set keyboard focus to the loaded page
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Page_Loaded;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
        }

        #endregion Navigation

        public Rect TogglePaneButtonRect
        {
            get;
            private set;
        }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<MainPage, Rect> TogglePaneButtonRectChanged;

        /// <summary>
        /// Callback when the SplitView's Pane is toggled open or close.  When the Pane is not visible
        /// then the floating hamburger may be occluding other content in the app unless it is aware.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        /// <summary>
        /// Check for the conditions where the navigation pane does not occupy the space under the floating
        /// hamburger button and trigger the event.
        /// </summary>
        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                this.RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, this.TogglePaneButton.ActualWidth, this.TogglePaneButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            TogglePaneButton.Visibility = Visibility.Visible;
            TogglePaneLightButton.Visibility = Visibility.Collapsed;

            var handler = this.TogglePaneButtonRectChanged;
            if (handler != null)
            {
                // handler(this, this.TogglePaneButtonRect);
                handler.DynamicInvoke(this, this.TogglePaneButtonRect);
            }
        }

        /// <summary>
        /// Enable accessibility on each nav menu item by setting the AutomationProperties.Name on each container
        /// using the associated Label of each item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is NavMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((NavMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        private void RootSplitView_LayoutUpdated(object sender, object e)
        {
            if (RootSplitView.IsPaneOpen)
            {
                TogglePaneButton.Visibility = Visibility.Visible;
                TogglePaneLightButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                if (Util.Utils.getPhoneWidth() < 400)
                {
                    TogglePaneButton.Visibility = Visibility.Collapsed;
                    TogglePaneLightButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void ManipulationStackPanel_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            RootSplitView.IsPaneOpen = !RootSplitView.IsPaneOpen;
        }

        private void ManipulationStackPanel_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
        }


        //TODO:未登录时不能选择上传头像
        private async void headimgRectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                if (bool.Parse(appSetting.Values["isLogin"].ToString()))
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".png");
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".bmp");
                    openPicker.FileTypeFilter.Add(".gif");
                    openPicker.ContinuationData["Operation"] = "img";
                    StorageFile file = await openPicker.PickSingleFileAsync();
                    if (file != null)
                    {
                        ClipHeadGrid.Visibility = Visibility.Visible;
                        BackOpacityGrid.Visibility = Visibility.Visible;
                        SoftwareBitmap sb = null;
                        using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            // Create the decoder from the stream
                            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                            // Get the SoftwareBitmap representation of the file
                            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                            sb = softwareBitmap;
                            // return softwareBitmap;
                        }
                        SoftwareBitmapSource source = new SoftwareBitmapSource();
                        await source.SetBitmapAsync(sb);
                        headImage.Source = source;
                    }
                }
                else
                {
                    var msgPopup = new Data.loginControl("您还没有登录 不能上传头像哦~");
                    msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                    msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去四处逛一逛~"); };
                    msgPopup.ShowWIndow();
                }
            }
            catch
            {
                var msgPopup = new Data.loginControl("您还没有登录 不能上传头像哦~");
                msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去四处逛一逛~"); };
                msgPopup.ShowWIndow();
            }
        }
        private void BackOpacityGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClipHeadGrid.Visibility = Visibility.Collapsed;
            BackOpacityGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            oldPoint = e.GetCurrentPoint(headScrollViewer).Position;
            isPoint = true;
        }

        /// <summary>
        /// 鼠标在控件上移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headImage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isPoint)
            {
                currentPoint = e.GetCurrentPoint(headScrollViewer).Position;
                Debug.WriteLine("X:" + (currentPoint.X - oldPoint.X));
                Debug.WriteLine("Y:" + (currentPoint.Y - oldPoint.Y));
                headScrollViewer.ScrollToHorizontalOffset(headScrollViewer.HorizontalOffset - (currentPoint.X - oldPoint.X));
                headScrollViewer.ScrollToVerticalOffset(headScrollViewer.VerticalOffset - (currentPoint.Y - oldPoint.Y));
                oldPoint = currentPoint;
            }
        }

        /// <summary>
        /// 鼠标松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            isPoint = false;
        }

        /// <summary>
        /// 鼠标离开控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            isPoint = false;
        }

        private async void clipHeadOKButton_Click(object sender, RoutedEventArgs e)
        {
            upClipHeadProgressBar.Visibility = Visibility.Visible;
            try
            {
                //HttpClient _httpClient = new HttpClient();
                //CancellationTokenSource _cts = new CancellationTokenSource();
                RenderTargetBitmap mapBitmap = new RenderTargetBitmap();
                await mapBitmap.RenderAsync(headScrollViewer);
                var pixelBuffer = await mapBitmap.GetPixelsAsync();
                IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                IStorageFile saveFile = await applicationFolder.CreateFileAsync("temphead.png", CreationCollisionOption.OpenIfExists);
                using (var fileStream = await saveFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)mapBitmap.PixelWidth,
                        (uint)mapBitmap.PixelHeight,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        DisplayInformation.GetForCurrentView().LogicalDpi,
                        pixelBuffer.ToArray());
                    await encoder.FlushAsync();
                }
                JObject uphead = await Requests.Send(appSetting.Values["stuNum"].ToString()); // , "ms-appdata:///local/temphead.png"
                Debug.WriteLine(uphead);
                if (uphead != null)
                {
                    if (Int32.Parse(uphead["state"].ToString()) == 200)
                    {
                        ClipHeadGrid.Visibility = Visibility.Collapsed;
                        BackOpacityGrid.Visibility = Visibility.Collapsed;
                        initHeadImage();
                    }
                    else
                    {
                        Utils.Toast("头像上传错误");
                    }
                }
                else
                {
                    Utils.Toast("头像上传错误");
                }
                upClipHeadProgressBar.Visibility = Visibility.Collapsed;
            }
            catch (Exception)
            {
                Debug.WriteLine("设置头像，保存新头像异常");
            }
        }

        private void clipHeadDisButton_Click(object sender, RoutedEventArgs e)
        {
            ClipHeadGrid.Visibility = Visibility.Collapsed;
            BackOpacityGrid.Visibility = Visibility.Collapsed;
        }
    }
}