using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.LostAndFoundPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPublish : Page
    {
        bool islost = true;
        string temptype;
        
        List<TempItemClass> tic = new List<TempItemClass>() {
            new TempItemClass { type="一卡通",IconV=Visibility.Collapsed},
            new TempItemClass { type="钱包",IconV=Visibility.Collapsed},
            new TempItemClass { type="电子产品",IconV=Visibility.Collapsed},
            new TempItemClass { type="书包",IconV=Visibility.Collapsed},
            new TempItemClass { type="钥匙",IconV=Visibility.Collapsed},
            new TempItemClass { type="雨伞",IconV=Visibility.Collapsed},
            new TempItemClass { type="衣物",IconV=Visibility.Collapsed},
            new TempItemClass { type="其他",IconV=Visibility.Collapsed}
        };

        public InfoPublish()
        {
            this.InitializeComponent();
            SelRemindListView.ItemsSource = tic;
        }

        private void cancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void RemindGridButon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }
        private void SelRemindBackgroupGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }
        private void e1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            islost = true;
            e2.Fill = new SolidColorBrush(Color.FromArgb(255, 217, 217, 217));
            e1.Fill = new SolidColorBrush(Color.FromArgb(255, 65, 165, 255));
        }
        private void e2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            islost = false;
            e1.Fill = new SolidColorBrush(Color.FromArgb(255, 217, 217, 217));
            e2.Fill = new SolidColorBrush(Color.FromArgb(255, 65, 165, 255));
        }

        private void SelRemindListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            temptype = ((TempItemClass)sender).type;
            Debug.WriteLine(temptype);
        }
        public class TempItemClass
        {
            public string type { get; set; }
            public Visibility IconV { get; set; }
        }

        private void SelRemindListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            temptype = ((TempItemClass)e.ClickedItem).type;
            Debug.WriteLine(temptype);
            SelRemindGrid.Visibility = Visibility.Collapsed;
            typeTb.Text = temptype;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }

        private void DatePickerFlyout_DatePicked(DatePickerFlyout sender, DatePickedEventArgs args)
        {
            timebox.Text = args.NewDate.UtcDateTime.ToString();
        }

        private void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            //TODO:post
        }
    }
}
