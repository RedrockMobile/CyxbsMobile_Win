using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Data;
//using ZSCY_Win10.Models.RemindPage;
using ZSCY_Win10.Util.Converter;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ZSCY_Win10.Controls
{
    //课程名字/教师名字/教室/类型不变 时间变为 day+lesson 周数变成 weekBegin+weekEnd
    //考试名字不变 日期变为week+weekday 时间变为 begin_time+end_time+classroom
    public sealed partial class ClassInfoControl : UserControl
    {
        private Popup m_Popup;
        //判断课程个数
        bool istype1 = true;
        public ClassInfoControl(List<ClassList> cl, List<Transaction> mmr, ExamList el)
        {
            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                outGrid.Width = 350;
                outGrid.Margin = new Thickness(0, 0, 0, 0);
                contentAll.Padding = new Thickness(0);
            }
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width <= 450)
                {
                    outGrid.Margin = new Thickness(0, 0, 0, 0);
                    contentAll.Padding = new Thickness(0);
                }
                if (e.NewSize.Width > 450)
                {
                    state = "VisualState800";

                }
                VisualStateManager.GoToState(this, state, true);
            };

            this.Width = Window.Current.Bounds.Width;
            this.Height = Window.Current.Bounds.Height;
            this.Loaded += MessagePopupWindow_Loaded;
            this.Unloaded += MessagePopupWindow_Unloaded;
            ExamList ei = Resources["ei"] as ExamList;
            if (el != null)
            {
                ei.Course = el.Course;
                ei.Week = $"第{el.Week}周";
                switch (el.Weekday[0])
                {
                    case '1':
                        ei.Week += " 星期一";
                        break;
                    case '2':
                        ei.Week += " 星期二";
                        break;
                    case '3':
                        ei.Week += " 星期三";
                        break;
                    case '4':
                        ei.Week += " 星期四";
                        break;
                    case '5':
                        ei.Week += " 星期五";
                        break;
                    case '6':
                        ei.Week += " 星期六";
                        break;
                    case '7':
                        ei.Week += " 星期日";
                        break;
                }
                ei.Begin_time = $"{el.Begin_time}-{el.End_time} {el.Classroom}教室";
            }
            else
            {
                ei.Course = "这节暂时没有考试哦~";
            }
            if (cl.Count != 0)
            {
                ClassList pl = Resources["pl"] as ClassList;
                ClassList pl2 = Resources["pl2"] as ClassList;
                if (cl[0].Course.Length > 15)
                    cl[0].Course = cl[0].Course.Substring(0, 12) + "..";
                pl.Course = cl[0].Course;
                pl.Teacher = cl[0].Teacher;
                pl.Classroom = cl[0].Classroom;
                pl.Day = $"{cl[0].Day} {cl[0].Lesson}";
                pl.Type = cl[0].Type;
                pl.RawWeek = cl[0].RawWeek;
                if (cl.Count != 1)
                {
                    istype1 = false;
                    type1.Visibility = Visibility.Collapsed;
                    type2.Visibility = Visibility.Visible;
                    if (cl[1].Course.Length > 15)
                        cl[1].Course = cl[1].Course.Substring(0, 12) + "..";
                    pl2.Course = cl[1].Course;
                    pl2.Teacher = cl[1].Teacher;
                    pl2.Classroom = cl[1].Classroom;
                    pl2.Day = $"{cl[1].Day} {cl[1].Lesson}";
                    pl2.Type = cl[1].Type;
                    pl2.RawWeek = cl[1].RawWeek;
                }
            }
            else
            {
                //ClassList pl = Resources["pl"] as ClassList;
                //pl.Course = "这里有个图没切 先这样吧老大";
                classGrid.Visibility = Visibility.Collapsed;
                noclassGrid.Visibility = Visibility.Visible;
            }
            List<Transaction> mrl = new List<Transaction>();
            if (mmr.Count != 0)
            {
                foreach (var item in mmr)
                {
                    Transaction temp = new Transaction();
                    if (item.date.Count > 1)
                    {
                        for (int i = 0; i < item.date.Count; i++)
                        {
                            for (int j = 0; j < item.date[i].week.Length; j++)
                            {
                                if (temp.week == null)
                                    if (item.date[i].week.Length == 1)
                                        temp.week += item.date[i].week[j].ToString();
                                    else
                                        temp.week += item.date[i].week[j].ToString() + "、";
                                else if (j == item.date[i].week.Length - 1 && !temp.week.Contains(item.date[i].week[j].ToString()))
                                    temp.week += item.date[i].week[j].ToString();
                                else if (temp.week != null)
                                {
                                    if (!temp.week.Contains(item.date[i].week[j].ToString()))
                                        temp.week += item.date[i].week[j].ToString() + "、";
                                }
                            }
                        }
                    }
                    else if (item.date.Count == 1)
                    {
                        temp.title = mmr[0].title;
                        temp.content = mmr[0].content;
                        temp.week = $"{mmr[0].date[0].week[0].ToString()}";
                    }
                    temp.week += "周";
                    temp.title = item.title;
                    temp.content = item.content;
                    mrl.Add(temp);
                }
            }
            else 
            {
                //mrl.Add(new Transaction { title = "没做完 别看" });
                transactionGridson.Visibility = Visibility.Collapsed;
                notransactionGrid.Visibility = Visibility.Visible;
            }
            //else if (mmr.Count == 1)
            //{
            //    Transaction temp = new Transaction();
            //    temp.title = mmr[0].title;
            //    temp.content = mmr[0].content;
            //    temp.week = $"{mmr[0].date[0].week[0].ToString()}周";
            //    mrl.Add(temp);
            //}
            RemindListView.ItemsSource = mrl;
            m_Popup = new Popup();
            m_Popup.Child = this;
        }

        private void MessagePopupWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += MessagePopupWindow_SizeChanged; ;
        }

        private void MessagePopupWindow_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
            this.Height = e.Size.Height;
        }
        private void MessagePopupWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= MessagePopupWindow_SizeChanged; ;
        }
        public void ShowWIndow()
        {
            m_Popup.IsOpen = true;
        }

        private void DismissWindow()
        {
            m_Popup.IsOpen = false;
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (type2pivot.SelectedIndex)
            {
                case 0:
                    classinfoheader0.Text = "\uEA3B";
                    classinfoheader1.Text = "\uEA3A";
                    break;
                case 1:
                    classinfoheader0.Text = "\uEA3A";
                    classinfoheader1.Text = "\uEA3B";
                    break;
            }
        }
        private void classBtn_Click(object sender, RoutedEventArgs e)
        {
            classBtn.Background = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            transactonBtn.Background = new SolidColorBrush(Colors.White);
            classtb.Foreground = new SolidColorBrush(Colors.White);
            transactiontb.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            examBtn.Background = new SolidColorBrush(Colors.White);
            examtb.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            if (transactionGrid.Visibility == Visibility.Visible || examGrid.Visibility == Visibility.Visible)
            {
                transactionGrid.Visibility = Visibility.Collapsed; examGrid.Visibility = Visibility.Collapsed;
                if (istype1)
                    type1.Visibility = Visibility.Visible;
                else type2.Visibility = Visibility.Visible;
            }
        }
        private void transactonBtn_Click(object sender, RoutedEventArgs e)
        {
            transactonBtn.Background = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            classBtn.Background = new SolidColorBrush(Colors.White);
            transactiontb.Foreground = new SolidColorBrush(Colors.White);
            classtb.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            examtb.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            examBtn.Background = new SolidColorBrush(Colors.White);
            if (type1.Visibility == Visibility.Visible || type2.Visibility == Visibility.Visible || examGrid.Visibility == Visibility.Visible)
            {
                if (type1.Visibility == Visibility.Visible)
                    type1.Visibility = Visibility.Collapsed;
                else if (type2.Visibility == Visibility.Visible)
                    type2.Visibility = Visibility.Collapsed;
                examGrid.Visibility = Visibility.Collapsed;
                transactionGrid.Visibility = Visibility.Visible;
            }

        }

        private void examBtn_Click(object sender, RoutedEventArgs e)
        {
            classBtn.Background = new SolidColorBrush(Colors.White);
            transactonBtn.Background = new SolidColorBrush(Colors.White);
            examBtn.Background = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            classtb.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255)); ;
            transactiontb.Foreground = new SolidColorBrush(Color.FromArgb(255, 65, 162, 255));
            examtb.Foreground = new SolidColorBrush(Colors.White);
            if (type1.Visibility == Visibility.Visible || type2.Visibility == Visibility.Visible || transactionGrid.Visibility == Visibility.Visible)
            {
                type1.Visibility = Visibility.Collapsed;
                type2.Visibility = Visibility.Collapsed;
                transactionGrid.Visibility = Visibility.Collapsed;
                examGrid.Visibility = Visibility.Visible;
            }
        }

        private void cancelBtn_Click(object sender, TappedRoutedEventArgs e)
        {
            DismissWindow();
        }

        private void classinfoheader0_Tapped(object sender, TappedRoutedEventArgs e)
        {
            type2pivot.SelectedIndex = 0;
            type2pivot.SelectedItem = type2pivot.Items[0];
            classinfoheader0.Text = "\uEA3B";
            classinfoheader1.Text = "\uEA3A";
        }

        private void classinfoheader1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            type2pivot.SelectedIndex = 1;
            type2pivot.SelectedItem = type2pivot.Items[1];
            classinfoheader1.Text = "\uEA3B";
            classinfoheader0.Text = "\uEA3A";
        }
    }
}
