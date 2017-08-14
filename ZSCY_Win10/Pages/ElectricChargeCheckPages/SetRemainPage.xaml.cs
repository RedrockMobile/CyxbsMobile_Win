using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetRemainPage : Page
    {
        private ApplicationDataContainer limitSettings = ApplicationData.Current.LocalSettings;

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