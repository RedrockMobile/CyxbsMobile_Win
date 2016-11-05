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
using Windows.UI.Xaml.Shapes;
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
        ExcalContent[,] excal = new ExcalContent[6, 7];//界面显示
        TimeSet[,] timeSet = new TimeSet[6, 7];
        WeekList[] weekList = new WeekList[20];

        ObservableCollection<CourseList> courseList = new ObservableCollection<CourseList>();

        private bool isFristInto = true;//为什么都未做
        private bool isEdit = false;
        private bool isEditing = false;//编写提醒
        private bool isSaving = false;//选择时间

        public AddRemind()
        {
            this.InitializeComponent();
            selCourseList.ItemsSource = courseList;
            //RemindListGrid.Visibility = Visibility.Collapsed;//测试使用
            //RemindGrid.Margin = new Thickness(0);
            //CourseTableGrid.Margin = new Thickness(0);
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前5分钟", BeforeTime = new TimeSpan(0, 5, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1小时", BeforeTime = new TimeSpan(1, 0, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1天", BeforeTime = new TimeSpan(1, 0, 0, 0) });
            beforeTime.Add(new BeforeTimeSel { BeforeString = "提前1星期", BeforeTime = new TimeSpan(7, 0, 0, 0, 0) });
            CreateCourseTable();
            CreateCourseWeek();
            this.SizeChanged += (s, e) =>
            {
                CourseTableGrid.Height = e.NewSize.Height - 50;
                RemindGrid.Height = e.NewSize.Height - 50;
                if (e.NewSize.Width < 650)
                {

                    //RemindGrid.Visibility = Visibility.Collapsed;

                    RemindGrid.Margin = new Thickness(0);
                    CourseTableGrid.Margin = new Thickness(0);
                    if (!App.showpane)
                    {
                        YourRemindTitle.Margin = new Thickness(48, 0, 0, 0);
                        AddRemindTitle.Margin = new Thickness(48, 0, 0, 0);
                    }
                    else
                    {
                        YourRemindTitle.Margin = new Thickness(0);
                        AddRemindTitle.Margin = new Thickness(0);
                    }
                    if (!isEdit)
                    {
                        CourseTableGrid.Visibility = Visibility.Collapsed;
                        RemindGrid.Visibility = Visibility.Collapsed;
                    }
                    if (isEditing)
                    {
                        SaveRemind.Visibility = Visibility.Visible;
                        EditRemind.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        EditRemind.Visibility = Visibility.Visible;
                        SaveRemind.Visibility = Visibility.Collapsed;
                    }
                    if (isSaving)
                    {
                        SaveRemind.Visibility = Visibility.Collapsed;
                        SaveCourseTime.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        SaveCourseTime.Visibility = Visibility.Collapsed;

                    }
                    AddRemindTitle.Visibility = Visibility.Collapsed;
                    SplitLine1.Visibility = Visibility.Collapsed;
                    SplitLine2.Visibility = Visibility.Collapsed;


                }
                else
                {
                    SplitLine2.Y2 = e.NewSize.Height - 48;
                    SaveRemind.Visibility = Visibility.Visible;
                    if (!isSaving)
                        RemindGrid.Visibility = Visibility.Visible;
                    else
                    {
                        SaveRemind.Visibility = Visibility.Collapsed;
                        SaveCourseTime.Visibility = Visibility.Visible;

                    }
                    if (isEditing)
                    {
                        SaveRemind.Visibility = Visibility.Visible;
                        EditRemind.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        EditRemind.Visibility = Visibility.Visible;
                        SaveRemind.Visibility = Visibility.Collapsed;
                    }

                    //TODO:页面逻辑
                    if (isFristInto)
                    {
                        isFristInto = false;
                        SaveRemind.Visibility = Visibility.Visible;
                        EditRemind.Visibility = Visibility.Collapsed;
                    }
                    RemindGrid.Margin = new Thickness(400, 0, 0, 0);
                    CourseTableGrid.Margin = new Thickness(400, 0, 0, 0);
                    AddRemindTitle.Visibility = Visibility.Visible;
                    SplitLine1.Visibility = Visibility.Visible;
                    SplitLine2.Visibility = Visibility.Visible;

                }

            };
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


                    if (timeSet[k, j] != null)//判断是否第一次添加课程
                    {
                        if (timeSet[k, j].IsCheck)
                        {
                            courseList.Clear();
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
                        timeSet[k, j] = new TimeSet();
                        timeSet[k, j].IsCheck = false;

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

                timeSet[row, column].IsCheck = true;
                timeSet[row, column].Set(row);
            }
            else
            {
                excal[row, column].IsCheck = false;
                (excal[row, column].Grid.Children[0] as Rectangle).Fill = new SolidColorBrush(Colors.SkyBlue);

                timeSet[row, column].IsCheck = false;
                timeSet[row, column].Set(-1);
            }

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
            isSaving = true;
            CourseTableGrid.Visibility = Visibility.Visible;
            SaveCourseTime.Visibility = Visibility.Visible;
            RemindGrid.Visibility = Visibility.Collapsed;
            EditRemind.Visibility = Visibility.Collapsed;
            SaveRemind.Visibility = Visibility.Collapsed;
            CreateCourseTable();
        }

        private void AddRemindAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCourseTime.Visibility = Visibility.Collapsed;
        }

        private void SaveCourseTime_Click(object sender, RoutedEventArgs e)
        {
            isEditing = true;
            isSaving = false;

            SaveCourseTime.Visibility = Visibility.Collapsed;
            CourseTableGrid.Visibility = Visibility.Collapsed;
            RemindGrid.Visibility = Visibility.Visible;
            SaveRemind.Visibility = Visibility.Visible;
            for (int i = 0; i < 6; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (timeSet[i, j].IsCheck)
                        courseList.Add(new CourseList(i, j, timeSet[i, j].IsCheck));
                }
        }

        private void Initial()//初始化
        {
            CreateCourseWeek();
            CreateCourseTable();
            courseList.Clear();
            SelBeforeTime.SelectedIndex = -1;
        }
        private void EditRemind_Click(object sender, RoutedEventArgs e)
        {
            isEdit = true;
            isEditing = true;
            SaveRemind.Visibility = Visibility.Visible;
            EditRemind.Visibility = Visibility.Collapsed;
            RemindGrid.Visibility = Visibility.Visible;
            RemindListGrid.Visibility = Visibility.Collapsed;
            SplitLine1.Visibility = Visibility.Visible;
            AddRemindTitle.Visibility = Visibility.Visible;
            Initial();
        }

        private void SaveRemind_Click(object sender, RoutedEventArgs e)
        {
            isEditing = false;
            isFristInto = true;
            RemindListGrid.Visibility = Visibility.Visible;
            RemindGrid.Visibility = Visibility.Collapsed;
            SaveRemind.Visibility = Visibility.Collapsed;
            EditRemind.Visibility = Visibility.Visible;
            SplitLine1.Visibility = Visibility.Collapsed;
            AddRemindTitle.Visibility = Visibility.Collapsed;
        }
    }
}
