using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    public sealed partial class MessagePopup : UserControl
    {
        private Popup m_Popup;
        private string MessageText = "输入的格式不正确哦";

        public MessagePopup()
        {
            this.InitializeComponent();

            m_Popup = new Popup();
            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            m_Popup.Child = this;
            this.Loaded += MessagePopup_Loaded;
            this.Unloaded += MessagePopup_Unloaded;
        }

        public MessagePopup(string showMsg) : this()
        {
            this.MessageText = showMsg;
        }

        private void MessagePopup_Loaded(object sender, RoutedEventArgs e)
        {
            this.tbContent.Text = MessageText;
            Window.Current.SizeChanged += MessagePopup_SizeChanged; ;
        }

        private void MessagePopup_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= MessagePopup_SizeChanged; ;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DismissWindow();
        }

        private void DismissWindow()
        {
            m_Popup.IsOpen = false;
        }

        public void ShowWindow()
        {
            m_Popup.IsOpen = true;
        }

        private void MessagePopup_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }
    }
}