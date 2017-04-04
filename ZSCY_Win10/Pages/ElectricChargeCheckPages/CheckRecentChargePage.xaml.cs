using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.ViewModels.ElectricChargeCheckPages;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CheckRecentChargePage : Page
    {
        bool isPageLoaded = false;
        public CheckRecentChargePage()
        {
            this.InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Visibility = Visibility.Collapsed;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = new CheckRecentChargePageViewModel("一月", "二月", "三月", "四月", "五月", "六月").ChargeData;
            isPageLoaded = true;
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isPageLoaded)
                pivot.SelectedIndex = listView.SelectedIndex;
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isPageLoaded)
                listView.SelectedIndex = pivot.SelectedIndex;
        }
    }
}
