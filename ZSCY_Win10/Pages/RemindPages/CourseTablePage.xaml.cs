using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZSCY_Win10.Models.RemindModels;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10.Pages.RemindPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class CourseTablePage : Page
    {
        private static SolidColorBrush MainGridBorderBrush = new SolidColorBrush(Color.FromArgb(255, 246, 246, 246));
        private static SolidColorBrush MainRectUnselectColor = new SolidColorBrush(Colors.White);
        private static SolidColorBrush MainRectSelectColor = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
        /// <summary>
        /// 关于行的需要除以2
        /// </summary>
        public CourseTablePage()
        {
            this.InitializeComponent();
            InitializeCourseTable();
            var state = "VisualState000";
            this.SizeChanged += (s, e) =>
             {
                 if (e.NewSize.Width > 000)
                 {
                     state = "VisualState000";
                 }
                 if (e.NewSize.Width > 550)
                     state = "VisualState550";
                 VisualStateManager.GoToState(this, state, true);
             };
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }


        private void InitializeCourseTable()
        {
            if (App.SelCoursList.Count > 0)
            {
                CreateCourseTable(true);
            }
            else
            {
                CreateCourseTable();
            }
        }

        private void CreateCourseTable(bool isReload = false)
        {
            for (int i = 0; i < kebiaoGrid.RowDefinitions.Count; i += 2)
                for (int j = 0; j < kebiaoGrid.ColumnDefinitions.Count; j++)
                {
                    Grid MainGrid = new Grid();
                    MainGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    MainGrid.VerticalAlignment = VerticalAlignment.Stretch;
                    MainGrid.BorderBrush = MainGridBorderBrush;
                    MainGrid.BorderThickness = new Thickness(1, 0, 0, 1);
                    Rectangle MainRect = new Rectangle();

                    MainRect.Fill = MainRectUnselectColor;
                    if (isReload)
                    {
                        if (App.SelCoursList.Where(x => x.ClassNum == i / 2 && x.DayNum == j).Count() > 0)
                        {
                            SelectCourse(MainRect, i, j, isReload);
                        }
                    }
                    MainGrid.Children.Add(MainRect);
                    kebiaoGrid.Children.Add(MainGrid);
                    Grid.SetRow(MainGrid, i);
                    Grid.SetRowSpan(MainGrid, 2);
                    Grid.SetColumn(MainGrid, j);
                    MainRect.Tapped += MainRect_Tapped;

                }

        }
        private void SelectCourse(Rectangle mainRect, int r, int c, bool isReload = false)
        {
            if (mainRect.Fill == MainRectUnselectColor)
            {
                mainRect.Fill = MainRectSelectColor;
                if (!isReload)
                    App.SelCoursList.Add(new SelCourseModel(c, r / 2));
            }
            else
            {
                mainRect.Fill = MainRectUnselectColor;
                int index = App.SelCoursList.FindIndex(x => x.ClassNum == r / 2 && x.DayNum == c);
                App.SelCoursList.RemoveAt(index);
            }
        }
        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainRect_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int r = Grid.GetRow((sender as Rectangle).Parent as Grid);
            int c = Grid.GetColumn((sender as Rectangle).Parent as Grid);
            Grid MainGrid = (sender as Rectangle).Parent as Grid;
            Rectangle MainRect = sender as Rectangle;
            SelectCourse(MainRect, r, c);
        }

        private void SaveRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.addRemindViewModel.RemindModel.DayAndClass = "";
            if (App.SelCoursList.Count > 0)
            {

                var list = App.SelCoursList.OrderBy(x => x.DayNum).ThenBy(y => y.ClassNum).ToList<SelCourseModel>();
                foreach (var item in list)
                {
                    App.addRemindViewModel.RemindModel.DayAndClass += $"{item.CourseTime()}、";
                }
                App.addRemindViewModel.RemindModel.DayAndClass =
                     App.addRemindViewModel.RemindModel.DayAndClass.Remove(App.addRemindViewModel.RemindModel.DayAndClass.Length - 1);
            }
            this.Visibility = Visibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

        }
    }
}
