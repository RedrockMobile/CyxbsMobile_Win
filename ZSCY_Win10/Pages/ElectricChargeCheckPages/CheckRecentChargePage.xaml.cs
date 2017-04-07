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
        CheckRecentChargePageViewModel VM;
        public CheckRecentChargePage()
        {
            this.InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
            this.Frame.Visibility = Visibility.Collapsed;
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            listView.ItemsSource = VM.ChargeData;
            pivot.ItemsSource = VM.ChargeData;
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

        private void Page_Loading(FrameworkElement sender, object args)
        {

            VM = new CheckRecentChargePageViewModel(new MonthCharge("十一月", 10, 20, 0, 20, new Point(1.5, 10), new PointCollection { new Point(28.5, 9), new Point(36.5, -0.5), new Point(41.5, 0), new Point(46.5, -0.5), new Point(54.5, 9), new Point(81.5, 10) }),
                                                    new MonthCharge("十二月", 15, 30, 20, 50, new Point(1.5 + 83.3333 * 1, 10), new PointCollection { new Point(28.5 + 83.3333 * 1, 9), new Point(36.5 + 83.3333 * 1, -0.5), new Point(41.5 + 83.3333 * 1, 0), new Point(46.5 + 83.3333 * 1, -0.5), new Point(54.5 + 83.3333 * 1, 9), new Point(81.5 + 83.3333 * 1, 10) }),
                                                    new MonthCharge("一月", 20, 40, 50, 90, new Point(1.5 + 83.3333 * 2, 10), new PointCollection { new Point(28.5 + 83.3333 * 2, 9), new Point(36.5 + 83.3333 * 2, -0.5), new Point(41.5 + 83.3333 * 2, 0), new Point(46.5 + 83.3333 * 2, -0.5), new Point(54.5 + 83.3333 * 2, 9), new Point(81.5 + 83.3333 * 2, 10) }),
                                                    new MonthCharge("二月", 10, 20, 90, 110, new Point(1.5 + 83.3333 * 3, 10), new PointCollection { new Point(28.5 + 83.3333 * 3, 9), new Point(36.5 + 83.3333 * 3, -0.5), new Point(41.5 + 83.3333 * 3, 0), new Point(46.5 + 83.3333 * 3, -0.5), new Point(54.5 + 83.3333 * 3, 9), new Point(81.5 + 83.3333 * 3, 10) }),
                                                    new MonthCharge("三月", 15, 30, 110, 140, new Point(1.5 + 83.3333 * 4, 10), new PointCollection { new Point(28.5 + 83.3333 * 4, 9), new Point(36.5 + 83.3333 * 4, -0.5), new Point(41.5 + 83.3333 * 4, 0), new Point(46.5 + 83.3333 * 4, -0.5), new Point(54.5 + 83.3333 * 4, 9), new Point(81.5 + 83.3333 * 4, 10) }),
                                                    new MonthCharge("四月", 10, 20, 140, 160, new Point(1.5 + 83.3333 * 5, 10), new PointCollection { new Point(28.5 + 83.3333 * 5, 9), new Point(36.5 + 83.3333 * 5, -0.5), new Point(41.5 + 83.3333 * 5, 0), new Point(46.5 + 83.3333 * 5, -0.5), new Point(54.5 + 83.3333 * 5, 9), new Point(81.5 + 83.3333 * 5, 10) })
                );

        }
    }
}
