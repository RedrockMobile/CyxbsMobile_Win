using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RemindTest : Page
    {
        public RemindTest()
        {
            this.InitializeComponent();
            ListViewToasts.ItemsSource = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications();
        }
    }
}