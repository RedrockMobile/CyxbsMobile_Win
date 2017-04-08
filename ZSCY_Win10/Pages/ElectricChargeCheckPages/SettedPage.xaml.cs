using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettedPage : Page
    {
        ApplicationDataContainer roomSettings = ApplicationData.Current.LocalSettings;
        string unresetableGridString;
        string resetableGridBuildingNum;
        string resetableGridRoomNum;
        public SettedPage()
        {
            this.InitializeComponent();
           
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter == null)
                SuccessGrid.Visibility = Visibility.Visible;
            else
            {
                if ((bool)e.Parameter)
                {
                    ResetableGrid.Visibility = Visibility.Visible;
                    unresetableGridString = roomSettings.Values["building"].ToString() + "栋" + roomSettings.Values["room"].ToString();
                }
                else
                {
                    UnresetableGrid.Visibility = Visibility.Visible;
                    resetableGridBuildingNum = roomSettings.Values["building"].ToString();
                    resetableGridRoomNum = roomSettings.Values["room"].ToString();
                }
            }

            
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Visibility = Visibility.Collapsed;
            while (Frame.CanGoBack)
                this.Frame.GoBack();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SetRoomPage));
        }
    }
}
