using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Controls;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
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
                    Label = "教务信息",
                    DestPage = typeof(JWPage)
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
            this.AppFrame.Navigate(navlist[0].DestPage, navlist[0].Arguments);


            SystemNavigationManager.GetForCurrentView().BackRequested += SystemNavigationManager_BackRequseted;
            //如果是在手机上，有实体键，隐藏返回键。
            if (ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                //this.BackButton.Visibility = Visibility.Collapsed;
            }
            NavMenuList.ItemsSource = navlist;
            NavMenuList.SelectedIndex = 0;
        }

        public Frame AppFrame { get { return this.frame; } }

        private void SystemNavigationManager_BackRequseted(object sender, BackRequestedEventArgs e)
        {
            bool handled = e.Handled;
            this.BackRequested(ref handled);
            e.Handled = handled;
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
        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (NavMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);
            if (item != null)
            {
                if (item.DestPage != null &&
                    item.DestPage != this.AppFrame.CurrentSourcePageType)
                {
                    this.AppFrame.Navigate(item.DestPage, item.Arguments);
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

        #endregion
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


    }
}
