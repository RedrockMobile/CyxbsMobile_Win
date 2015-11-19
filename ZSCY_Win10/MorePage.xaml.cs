using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MorePage : Page
    {
        ObservableDictionary morepageclass = new ObservableDictionary();
        public MorePage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width > 000 && e.NewSize.Width < 750)
                {
                    if (MoreListView.SelectedIndex != -1)
                    {
                        MoreBackAppBarButton.Visibility = Visibility.Visible;
                        HubSectionKBTitle.Text = MoreContentTitleTextBlock.Text;
                    }
                    MoreListView.Width = e.NewSize.Width;
                }
                if (e.NewSize.Width > 700)
                {
                    MoreBackAppBarButton.Visibility = Visibility.Collapsed;
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
            MoreBackAppBarButton.Visibility = Visibility.Collapsed;
            MoreFrame.Visibility = Visibility.Collapsed;
            MoreContentTitleTextBlock.Text = "";
            HubSectionKBTitle.Text = "更多";
            MoreListView.SelectedIndex = -1;

            CommandBar c = new CommandBar();
            this.BottomAppBar = c;
            c.Visibility = Visibility.Collapsed;
        }

        private async void MoreListView_ItemClick(object sender, ItemClickEventArgs e)
        {

            var item = e.ClickedItem as Morepageclass;
            if (MoreListgrid.Width != null && MoreListgrid.Width == 300)
            {
                MoreBackAppBarButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MoreBackAppBarButton.Visibility = Visibility.Visible;
                HubSectionKBTitle.Text = item.Itemname;
            }
            MoreFrame.Visibility = Visibility.Visible;
            MoreContentTitleTextBlock.Text = item.Itemname;
            Debug.WriteLine(item.UniqueID);
            {
                switch (item.UniqueID)
                {
                    case "ReExam":
                        MoreFrame.Navigate(typeof(ExamPage), 3); ;
                        MoreFrame.Visibility = Visibility.Visible; break;
                    case "Exam":
                        MoreFrame.Navigate(typeof(ExamPage), 2);
                        MoreFrame.Visibility = Visibility.Visible; break;
                    case "Socre":
                        MoreFrame.Navigate(typeof(ScorePage));
                        MoreFrame.Visibility = Visibility.Visible; break;
                    case "ClassRoom":
                        MoreFrame.Navigate(typeof(EmptyRoomsPage));
                        MoreFrame.Visibility = Visibility.Visible; break;
                    case "Calendar":
                        MoreFrame.Navigate(typeof(CalendarPage));
                        MoreFrame.Visibility = Visibility.Visible; break;
                    case "Card":
                        var a = await Launcher.LaunchUriAsync(new Uri("cquptcard:"));
                        MoreFrame.Visibility = Visibility.Collapsed;
                        break;
                    default:
                        break;
                }
            }
        }


    }
}
