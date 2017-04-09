using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            if (e.Parameter == null)                            //当传入的参数为空时，作为设置成功页面
                SuccessGrid.Visibility = Visibility.Visible;
            else
            {
                if ((bool)e.Parameter)                          //传入参数为true时，作为可再设置页面
                {
                    resetableGridBuildingNum = roomSettings.Values["building"].ToString();
                    resetableGridRoomNum = roomSettings.Values["room"].ToString();
                    ResetableGrid.Visibility = Visibility.Visible;
                }
                else                                            //传入参数为false时，作为不可再设置页面
                {
                    unresetableGridString = roomSettings.Values["building"].ToString() + "栋" + roomSettings.Values["room"].ToString();
                    UnresetableGrid.Visibility = Visibility.Visible;
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
