using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Models.RemindPage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SelWeekNumPage : Page
    {
        private static SolidColorBrush UnselectedFillColor = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        private static SolidColorBrush SelectedFillColor = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
        private static SolidColorBrush UnselectedFontColor = new SolidColorBrush(Color.FromArgb(255, 89, 89, 89));
        private static SolidColorBrush SelectedFontColor = new SolidColorBrush(Color.FromArgb(255, 255, 254, 254));
        ObservableCollection<SelWeekNum> SelWeekList = new ObservableCollection<SelWeekNum>();
        public SelWeekNumPage()
        {
            this.InitializeComponent();
            WeekListGridView.ItemsSource = SelWeekList;
            InitializeWeekList();
            //SystemNavigationManager.GetForCurrentView().BackRequested += SelWeekNumPage_BackRequested; ; ;
            //SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        //private void SelWeekNumPage_BackRequested(object sender, BackRequestedEventArgs e)
        //{
        //    if (page == typeof(EditRemindPage))
        //    {
        //        Frame.GoBack();
        //        SystemNavigationManager.GetForCurrentView().BackRequested -= SelWeekNumPage_BackRequested;
        //        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //    }
        //    else
        //    {
        //        this.Visibility = Visibility.Collapsed;
        //        //Frame.GoBack();
        //        SystemNavigationManager.GetForCurrentView().BackRequested -= SelWeekNumPage_BackRequested;
        //        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        //    }
        //}

        private void InitializeWeekList()
        {
            for (int i = 0; i < 20; i++)
            {

                SelWeekNum temp = new SelWeekNum()
                {
                    ItemContent = (i + 1).ToString(),
                    ItemContentColor = UnselectedFontColor,
                    ItemFillColor = UnselectedFillColor,
                    isSelected = false
                };
                var item = App.selectedWeekNumList.Where(x => x.WeekNum == i + 1);
                if (item.Count() > 0)
                {
                    temp.isSelected = true;
                    temp.ItemContentColor = SelectedFontColor;
                    temp.ItemFillColor = SelectedFillColor;
                }
                SelWeekList.Add(temp);
            }
        }
        Type page;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            page = e.Parameter as Type;
        }
        private void SelWeek_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int temp = (sender as GridView).SelectedIndex;
            if (temp>-1)
            {
                if (!SelWeekList[temp].isSelected)
                {
                    SelWeekList[temp].ItemContentColor = SelectedFontColor;
                    SelWeekList[temp].ItemFillColor = SelectedFillColor;
                    SelWeekList[temp].isSelected = true;
                }
                else
                {
                    SelWeekList[temp].ItemContentColor = UnselectedFontColor;
                    SelWeekList[temp].ItemFillColor = UnselectedFillColor;
                    SelWeekList[temp].isSelected = false;
                }
            }

        }



        private void SaveSelected_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.selectedWeek.WeekNumString = "";
            App.selectedWeekNumList.Clear();
            for (int i = 0; i < SelWeekList.Count; i++)
            {
                if (SelWeekList[i].isSelected)
                {
                    SelectedWeekNum temp = new SelectedWeekNum() { WeekNum = i + 1 };
                    temp.SetWeekTime(i);
                    App.selectedWeekNumList.Add(temp);
                    App.selectedWeek.WeekNumString += (i + 1).ToString() + "、";
                }
            }
            if (page == typeof(EditRemindPage))
                Frame.GoBack();
            else
            {
                Frame rootFrame = Window.Current.Content as Frame;
                //rootFrame.Visibility = Visibility.Collapsed;
                this.Visibility = Visibility.Collapsed;
                //Frame.GoBack();
            }
            Debug.WriteLine(App.selectedWeek.WeekNumString);
        }
    }
}
