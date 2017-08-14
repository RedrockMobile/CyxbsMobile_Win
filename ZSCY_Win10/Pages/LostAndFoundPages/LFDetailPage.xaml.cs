﻿using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Models;
using ZSCY_Win10.Resource;
using ZSCY_Win10.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.LostAndFoundPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LFDetailPage : Page
    {
        private APPTheme AppTheme = new APPTheme();
        private LFDetailPageViewModel VM;
        private LFDetailPageModel Model = new LFDetailPageModel();

        public LFDetailPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var temp = e.Parameter as string;
            VM = await Model.GetDetail(temp);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += PageBackRequested;
        }

        private void PageBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            SystemNavigationManager.GetForCurrentView().BackRequested -= PageBackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Frame.GoBack();
        }
    }
}