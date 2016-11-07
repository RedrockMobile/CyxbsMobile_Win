using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Shapes;
using ZSCY_Win10.Models.RemindPage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditRemindPage : Page
    {
        ObservableCollection<BeforeTimeSel> beforeTime = new ObservableCollection<BeforeTimeSel>();
        WeekList[] weekList = new WeekList[20];

        public EditRemindPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            selCourseList.ItemsSource = App.courseList;
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前5分钟", BeforeTime = new TimeSpan(0, 5, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1小时", BeforeTime = new TimeSpan(1, 0, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1天", BeforeTime = new TimeSpan(1, 0, 0, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1星期", BeforeTime = new TimeSpan(7, 0, 0, 0, 0) });
            CreateCourseWeek();
            this.SizeChanged += (s, e) =>
            {
                SplitLine.Y2 = e.NewSize.Height - 48;
            };
        }
        private void CourseAddPressed_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.Gray);

        }

        private void CourseAddExited_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.White);
        }

        private void CreateCourseWeek()
        {

            for (int i = 0; i < weekList.Count(); i++)
            {
                weekList[i] = new WeekList();//初始化
                weekList[i].Grid = new Grid();
                weekList[i].Grid.Margin = new Thickness(3);

                weekList[i].SetWeekName(i + 1);


                weekList[i].Textblock = new TextBlock();
                weekList[i].Textblock.Text = weekList[i].WeekName;
                weekList[i].Textblock.Foreground = new SolidColorBrush(Colors.Black);
                weekList[i].Textblock.HorizontalAlignment = HorizontalAlignment.Center;
                weekList[i].Textblock.VerticalAlignment = VerticalAlignment.Center;

                weekList[i].Rect = new Rectangle();
                weekList[i].Rect.Fill = new SolidColorBrush(Colors.Azure);


                weekList[i].IsCheck = false;


                int column = i % 5;
                int row = i / 5;
                weekList[i].Grid.Children.Add(weekList[i].Rect);
                weekList[i].Grid.Children.Add(weekList[i].Textblock);
                SelWeekNumTable.Children.Add(weekList[i].Grid);
                Grid.SetColumn(weekList[i].Grid, column);
                Grid.SetRow(weekList[i].Grid, row);
                weekList[i].Rect.Tapped += Rect_Tapped;
                weekList[i].Textblock.Tapped += Textblock_Tapped;

            }
        }

        private void Rect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((sender as Rectangle).Parent as Grid);
            int column = Grid.GetColumn((sender as Rectangle).Parent as Grid);
            row *= 5;

            if (!weekList[row + column].IsCheck)
            {
                weekList[row + column].IsCheck = true;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.CadetBlue);
            }
            else
            {
                weekList[row + column].IsCheck = false;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.Azure);
            }
        }
        private void Textblock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((sender as TextBlock).Parent as Grid);
            int column = Grid.GetColumn((sender as TextBlock).Parent as Grid);
            row *= 5;

            if (!weekList[row + column].IsCheck)
            {
                weekList[row + column].IsCheck = true;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.CadetBlue);
            }
            else
            {
                weekList[row + column].IsCheck = false;
                weekList[row + column].Rect.Fill = new SolidColorBrush(Colors.Azure);
            }
        }


        private void CourseSel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CourseTablePage));
        }

        private void SaveEditRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Content = null;
        }
    }
}
