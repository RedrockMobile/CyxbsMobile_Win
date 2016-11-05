using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Models.RemindPage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddRemind : Page
    {
        ObservableCollection<BeforeTimeSel> beforeTime = new ObservableCollection<BeforeTimeSel>();

        public AddRemind()
        {
            this.InitializeComponent();

            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前5分钟", BeforeTime = new TimeSpan(0, 5, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1小时", BeforeTime = new TimeSpan(1, 0, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1天", BeforeTime = new TimeSpan(1, 0, 0, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1星期", BeforeTime = new TimeSpan(7, 0, 0, 0, 0) });
        }

        private void CourseAddPressed_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.Gray);

        }

        private void CourseAddExited_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.White);
        }

        private void CourseSel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CourseTableGrid.Visibility = Visibility.Visible;
            RemindGrid.Visibility = Visibility.Collapsed;
        }
    }
}
