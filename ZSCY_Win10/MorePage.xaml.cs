using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY.Pages;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data;
using ZSCY_Win10.Pages.ElectricChargeCheckPages;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    /// 
    public sealed partial class MorePage : Page
    {
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        ObservableDictionary morepageclass = new ObservableDictionary();
        public static int isFreeRe = 0;
        private static string resourceName = "ZSCY";
        public MorePage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000 && e.NewSize.Width < 700)
                {
                    if (MoreListView.SelectedIndex != -1)
                    {
                        //MoreBackAppBarButton.Visibility = Visibility.Visible;
                        SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                        HubSectionKBTitle.Text = MoreContentTitleTextBlock.Text;
                    }
                    MoreListView.Width = e.NewSize.Width;
                }
                if (!App.showpane)
                {
                    MoreTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    MoreTitleGrid.Margin = new Thickness(0);
                }
                if (e.NewSize.Width > 700)
                {
                    //MoreBackAppBarButton.Visibility = Visibility.Collapsed;
                    SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    HubSectionKBTitle.Text = "更多";
                    MoreListView.Width = 300;
                    state = "VisualState700";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
        }



        public ObservableDictionary Morepageclass
        {
            get
            {
                return morepageclass;
            }
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var group = await DataSource.Get();
            this.Morepageclass["Group"] = group;
            InitMore();
            UmengSDK.UmengAnalytics.TrackPageStart("MorePage");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("MorePage");
        }

        private void InitMore()
        {
        }


        public Frame MoreFrame { get { return this.frame; } }



        private void MoreBackAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //if (JWFrame == null)
            //    return;
            //if (JWFrame.CanGoBack)
            //{
            //    JWFrame.GoBack();
            //}
            //MoreBackAppBarButton.Visibility = Visibility.Collapsed;
            MoreFrame.Visibility = Visibility.Collapsed;
            MoreContentTitleTextBlock.Text = "";
            HubSectionKBTitle.Text = "更多";
            MoreListView.SelectedIndex = -1;

            CommandBar c = new CommandBar();
            this.BottomAppBar = c;
            c.Visibility = Visibility.Collapsed;
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            //if (JWFrame == null)
            //    return;
            //if (JWFrame.CanGoBack)
            //{
            //    JWFrame.GoBack();
            //}
            //MoreBackAppBarButton.Visibility = Visibility.Collapsed;
            e.Handled = true;
            if (isFreeRe == 0)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                MoreFrame.Visibility = Visibility.Collapsed;
                MoreContentTitleTextBlock.Text = "";
                HubSectionKBTitle.Text = "更多";
                MoreListView.SelectedIndex = -1;

                CommandBar c = new CommandBar();
                this.BottomAppBar = c;
                c.Visibility = Visibility.Collapsed;
            }
            else if (isFreeRe == 1)
            {
                if (MoreFrame != null && MoreFrame.CanGoBack)
                {
                    MoreFrame.GoBack();
                    isFreeRe = 0;
                }
            }
        }

        private async void MoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {

            var item = e.ClickedItem as Morepageclass;
            if ((MoreListgrid.Width == 300) || item.UniqueID == "Card")
            {
                //MoreBackAppBarButton.Visibility = Visibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            }
            else
            {
                //MoreBackAppBarButton.Visibility = Visibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                HubSectionKBTitle.Text = item.Itemname;
            }
            //MoreFrame.Visibility = Visibility.Visible;
            MoreContentTitleTextBlock.Text = item.Itemname;
            //TODO:未登陆时 没有 补考/考试/分数信息 可查询空闲 无法自动添加自己的信息
            Debug.WriteLine(item.UniqueID);
            {
                int count;
                var vault = new Windows.Security.Credentials.PasswordVault();
                try
                {
                    var credentialList = vault.FindAllByResource(resourceName);
                    count = credentialList.Count;
                }
                catch
                {
                    count = 0;
                }
                switch (item.UniqueID)
                {
                    case "ReExam":
                        //if (appSetting.Values.ContainsKey("idNum"))
                        if (count > 0)
                        {
                            MoreFrame.Navigate(typeof(ExamPage), 3); ;
                            MoreFrame.Visibility = Visibility.Visible;
                            isFreeRe = 0;
                            break;
                        }
                        else
                        {
                            var msgPopup = new Data.loginControl("您还没有登录 无法查看补考信息~");
                            msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                            msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去社区逛一逛~"); };
                            msgPopup.ShowWIndow();
                            break;
                        }
                    case "Exam":
                        //if (appSetting.Values.ContainsKey("idNum"))
                        if (count > 0)
                        {
                            MoreFrame.Navigate(typeof(ExamPage), 2);
                            MoreFrame.Visibility = Visibility.Visible;
                            isFreeRe = 0;
                            break;
                        }
                        else
                        {
                            var msgPopup = new Data.loginControl("您还没有登录 无法查看考试信息~");
                            msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                            msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去社区逛一逛~"); };
                            msgPopup.ShowWIndow();
                            break;
                        }
                    case "Socre":
                        //if (appSetting.Values.ContainsKey("idNum"))
                        if (count > 0)
                        {
                            MoreFrame.Navigate(typeof(ScorePage));
                            MoreFrame.Visibility = Visibility.Visible;
                            isFreeRe = 0;
                            break;
                        }
                        else
                        {
                            var msgPopup = new Data.loginControl("您还没有登录 无法查看成绩~");
                            msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                            msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去社区逛一逛~"); };
                            msgPopup.ShowWIndow();
                            break;
                        }
                    case "ClassRoom":
                        //if (appSetting.Values.ContainsKey("idNum"))
                        if (count > 0)
                        {
                            MoreFrame.Navigate(typeof(EmptyRoomsPage));
                            MoreFrame.Visibility = Visibility.Visible;
                            isFreeRe = 0;
                            break;
                        }
                        else
                        {
                            var msgPopup = new Data.loginControl("您还没有登录 无法查询空教室~");
                            msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                            msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去社区逛一逛~"); };
                            msgPopup.ShowWIndow();
                            break;
                        }
                    case "Calendar":
                        MoreFrame.Navigate(typeof(CalendarPage));
                        MoreFrame.Visibility = Visibility.Visible;
                        isFreeRe = 0;
                        break;
                    case "FreeTime":
                        if (count > 0)
                        {
                            MoreFrame.Navigate(typeof(SearchFreeTimeNumPage));
                            MoreFrame.Visibility = Visibility.Visible;
                            isFreeRe = 0;
                            break;
                        }
                        else
                        {
                            var msgPopup = new Data.loginControl("您还没有登录 无法查询空闲~");
                            msgPopup.LeftClick += (s, c) => { Frame rootFrame = Window.Current.Content as Frame; rootFrame.Navigate(typeof(LoginPage)); };
                            msgPopup.RightClick += (s, c) => { new MessageDialog("您可以先去社区逛一逛~"); };
                            msgPopup.ShowWIndow();
                            break;
                        }
                    case "Card":
                        var a = await Launcher.LaunchUriAsync(new Uri("cquptcard:"));
                        MoreFrame.Visibility = Visibility.Collapsed;
                        break;
                    case "freshMan":
                        Frame.Navigate(typeof(FirstPage));
                        MoreFrame.Visibility = Visibility.Collapsed;
                        isFreeRe = 0;
                        break;
                    case "Electricity":
                        Frame.Navigate(typeof(ElectricityPage));
                        MoreFrame.Visibility = Visibility.Collapsed;
                        isFreeRe = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    }


}
