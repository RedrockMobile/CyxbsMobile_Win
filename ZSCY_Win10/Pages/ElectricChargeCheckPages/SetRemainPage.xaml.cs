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
    public sealed partial class SetRemainPage : Page
    {
        ApplicationDataContainer limitSettings = ApplicationData.Current.LocalSettings;
        public SetRemainPage()
        {
            this.InitializeComponent();
            if (limitSettings.Values.ContainsKey("limitCharge"))
            {
                RemainTextBox.Text = (limitSettings.Values["limitCharge"]).ToString();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
            this.Frame.Visibility = Visibility.Collapsed;
        }

        private void RemainButton_Click(object sender, RoutedEventArgs e)
        {
            limitSettings.Values["limitCharge"] = RemainTextBox.Text;
            this.Frame.Navigate(typeof(SettedPage));
        }
    }
}
