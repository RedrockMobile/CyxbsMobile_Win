using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Util.Remind;
using ZSCY_Win10.ViewModels.Remind;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.RemindPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemindListPage : Page
    {
        private RemindListViewModel viewmodel;
        public RemindListPage()
        {
            this.InitializeComponent();
            viewmodel = new RemindListViewModel();
            RemindListView.ItemsSource = viewmodel.RemindListOC;
            var state = "VisualState000";
            this.SizeChanged += (s, e) =>
              {
                  if (e.NewSize.Width > 000)
                  {
                      ListGrid1.Width = grid.Width = e.NewSize.Width;
                      state = "VisualState000";
                  }
                  if (e.NewSize.Width > 800)
                  {
                      ListGrid1.Width = grid.Width = 400;

                      state = "VisualState800";
                  }
                  VisualStateManager.GoToState(this, state, true);
              };
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += RemindListPage_BackRequested;
        }

        private void RemindListPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.Navigate(typeof(KBPage));
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().BackRequested -= RemindListPage_BackRequested;
        }

   
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if ((bool)e.Parameter)
            {
                viewmodel.RefreshList((bool)e.Parameter);
            }
            else
            {
                viewmodel.GetData();
            }
        }
        private void RewriteRemindGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int index = RemindListView.SelectedIndex;
            var remind = viewmodel.RemindListOC[index];
            Frame.Navigate(typeof(AddRemindPage), remind);
        }

        private async void DeleteRemindGridButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                int index = RemindListView.SelectedIndex;
                int num = viewmodel.RemindListOC[index].Num;
                viewmodel.DeleteRemind(num);
                viewmodel.RemindListOC.RemoveAt(index);
            }
            else
            {
                await new MessageDialog("请打开网络!!!").ShowAsync();
            }

        }

        private async void RefreshListView_RefreshInvoked(DependencyObject sender, object args)
        {
            progressRing.IsActive = true;
            viewmodel.RefreshList();
            progressRing.IsActive = false;
        }

        private void EditRemindList_Click(object sender, RoutedEventArgs e)
        {
            viewmodel.Rewrite();
        }

        private async void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            progressRing.IsActive = true;
            viewmodel.RefreshList();
            progressRing.IsActive = false;
        }
    }
}
