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
        }

        private void InitializeWeekList()
        {
            for (int i = 0; i < 20; i++)
            {
              
                SelWeekList.Add(new SelWeekNum()
                {
                    ItemContent = (i + 1).ToString(),
                    ItemContentColor = UnselectedFontColor,
                    ItemFillColor = UnselectedFillColor,
                    isSelected = false
                });
            }
        }


        private void SelWeek_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int temp = (sender as GridView).SelectedIndex;
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



        private void SaveSelected_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.selectedWeek.WeekNumString = "";
            for (int i = 0; i < SelWeekList.Count; i++)
            {
                if(SelWeekList[i].isSelected)
                {
                    SelectedWeekNum temp = new SelectedWeekNum() { WeekNum = i + 1 };
                    temp.SetWeekTime(i);
                    App.selectedWeekNumList.Add(temp);
                    App.selectedWeek.WeekNumString += (i + 1).ToString() + "、";
                }
            }
            Frame.Navigate(typeof(FristPage));
            Debug.WriteLine(App.selectedWeek.WeekNumString);
        }
    }
}
