using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        ExcalContent[,] CourseTable = new ExcalContent[6, 7];//界面显示
        private static SolidColorBrush SelectedColor = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
        private static SolidColorBrush UnselectedColor = new SolidColorBrush(Colors.White);
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
                    CourseTable[k, j] = new ExcalContent();

                    CourseTable[k, j].Grid = new Grid();
                    Rectangle rect = new Rectangle();

                    CourseTable[k, j].Grid.HorizontalAlignment = HorizontalAlignment.Stretch;
                    CourseTable[k, j].Grid.VerticalAlignment = VerticalAlignment.Stretch;
                    CourseTable[k, j].Grid.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 246, 246, 246));


                    if (App.timeSet[k, j] == null)//判断是否第一次添加课程
                    {
                        App.timeSet[k, j] = new TimeSet();
                        App.timeSet[k, j].IsCheck = false;

                        CourseTable[k, j].IsCheck = false;
                        rect.Fill = UnselectedColor;
                    }
                    else
                    {
                        if (App.timeSet[k, j].IsCheck)
                        {
                            App.courseList.Clear();
                            rect.Fill = SelectedColor;
                            CourseTable[k, j].IsCheck = true;
                        }
                        else
                        {
                            rect.Fill = UnselectedColor;
                            CourseTable[k, j].IsCheck = false;
                        }
                    }
                    CourseTable[k, j].Grid.BorderThickness = new Thickness(1);

                    Grid.SetRowSpan(CourseTable[k, j].Grid, 2);

                    CourseTable[k, j].Grid.Children.Add(rect);

                    kebiaoGrid.Children.Add(CourseTable[k, j].Grid);
                    Grid.SetRow(CourseTable[k, j].Grid, i);
                    Grid.SetColumn(CourseTable[k, j].Grid, j);

                    rect.Tapped += SelItems_Tapped;

                }
        }

        private void SelItems_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int row = Grid.GetRow((sender as Rectangle).Parent as Grid);
            int column = Grid.GetColumn((sender as Rectangle).Parent as Grid);
            row /= 2;
            if (CourseTable[row, column].IsCheck == false)
            {

                CourseTable[row, column].IsCheck = true;
                (CourseTable[row, column].Grid.Children[0] as Rectangle).Fill = SelectedColor;

                //App.timeSet[row, column].IsCheck = true;
                //App.timeSet[row, column].Set(row);
            }
            else
            {
                CourseTable[row, column].IsCheck = false;
                (CourseTable[row, column].Grid.Children[0] as Rectangle).Fill = UnselectedColor;

                //App.timeSet[row, column].IsCheck = false;
                //App.timeSet[row, column].Set(-1);
            }

        }

        private void SaveRemind_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.SelectedTime.SelTimeString = "";
            for (int j = 0; j < 7; j++)
                for (int i = 0; i < 6; i++)
                {
                    if (CourseTable[i, j].IsCheck)
                    {
                        App.timeSet[i, j].IsCheck = true;
                        App.timeSet[i, j].Set(i);
                        App.SelectedTime.SelTimeString += CourseList(i, j)+" ";
                    }
                    else
                    {
                        App.timeSet[i, j].IsCheck = false;
                        App.timeSet[i, j].Set(-1);
                    }
                }
            Debug.WriteLine(App.SelectedTime.SelTimeString);
            Frame.Navigate(typeof(FristPage));
        }
        public string CourseList(int row, int column)
        {
            string weekNum = "";
            string classNum = "";
            switch (column)
            {
                case 0:
                    weekNum = "周一";
                    break;
                case 1:
                    weekNum = "周二";
                    break;
                case 2:
                    weekNum = "周三";
                    break;
                case 3:
                    weekNum = "周四";
                    break;
                case 4:
                    weekNum = "周五";
                    break;
                case 5:
                    weekNum = "周六";
                    break;
                case 6:
                    weekNum = "周末";
                    break;
            }
            switch (row)
            {
                case 0:
                    classNum = "12节";
                    break;        
                case 1:           
                    classNum = "34节";
                    break;        
                case 2:           
                    classNum = "56节";
                    break;        
                case 3:           
                    classNum = "78节";
                    break;
                case 4:
                    classNum = "910节";
                    break;
                case 5:
                    classNum = "1112节";
                    break;
            }
            string temp = string.Concat(weekNum, classNum);
            return temp;
        }
    }
}
