using System;
using System.Collections.Generic;
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

//The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.AddRemindPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CourseTablePage : Page
    {
        ExcalContent[,] excal = new ExcalContent[6, 7];//界面显示

        public CourseTablePage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                SplitLine.Y2 = e.NewSize.Height - 48;
            };
            CreateCourseTable();
        }
        private void CreateCourseTable()
        {
            for (int i = 0, k = 0; i < kebiaoGrid.RowDefinitions.Count; i += 2, k++)
                for (int j = 0; j < kebiaoGrid.ColumnDefinitions.Count; j++)
                {
                    excal[k, j] = new ExcalContent();

                    excal[k, j].Grid = new Grid();
                    Rectangle rect = new Rectangle();

                    excal[k, j].Grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    excal[k, j].Grid.VerticalAlignment = VerticalAlignment.Stretch;
                    excal[k, j].Grid.BorderBrush = new SolidColorBrush(Colors.White);


                    if (App.timeSet[k, j] != null)//判断是否第一次添加课程
                    {
                        if (App.timeSet[k, j].IsCheck)
                        {
                            App.courseList.Clear();
                            rect.Fill = new SolidColorBrush(Colors.Gray);
                            excal[k, j].IsCheck = true;
                        }
                        else
                        {
                            rect.Fill = new SolidColorBrush(Colors.SkyBlue);
                            excal[k, j].IsCheck = false;
                        }
                    }
                    else
                    {
                        App.timeSet[k, j] = new TimeSet();
                        App.timeSet[k, j].IsCheck = false;

                        excal[k, j].IsCheck = false;

                        rect.Fill = new SolidColorBrush(Colors.SkyBlue);
                    }
                    excal[k, j].Grid.BorderThickness = new Thickness(1);

                    Grid.SetRowSpan(excal[k, j].Grid, 2);

                    excal[k, j].Grid.Children.Add(rect);

                    kebiaoGrid.Children.Add(excal[k, j].Grid);
                    Grid.SetRow(excal[k, j].Grid, i);
                    Grid.SetColumn(excal[k, j].Grid, j);

                    rect.Tapped += SelItems_Tapped;

                }
        }

        private void SelItems_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((sender as Rectangle).Parent as Grid);
            int column = Grid.GetColumn((sender as Rectangle).Parent as Grid);
            row /= 2;
            if (excal[row, column].IsCheck == false)
            {

                excal[row, column].IsCheck = true;
                (excal[row, column].Grid.Children[0] as Rectangle).Fill = new SolidColorBrush(Colors.Gray);

                App.timeSet[row, column].IsCheck = true;
                App.timeSet[row, column].Set(row);
            }
            else
            {
                excal[row, column].IsCheck = false;
                (excal[row, column].Grid.Children[0] as Rectangle).Fill = new SolidColorBrush(Colors.SkyBlue);

                App.timeSet[row, column].IsCheck = false;
                App.timeSet[row, column].Set(-1);
            }

        }

        private void SaveRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (App.timeSet[i, j].IsCheck)
                        App.courseList.Add(new CourseList(i, j, App.timeSet[i, j].IsCheck));
                }
            Frame.GoBack();
        }
    }
}
