using ZSCY.Models;
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
using ZSCY_Win10.Resource;
using ZSCY_Win10;
using Windows.UI.Popups;
using Windows.Phone.UI.Input;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BigDataPage : Page
    {
        private int pivot_index = 0;
        private int comboBox1_index = 0;
        private ZSCY_Win10.ViewModels.BigDataViewModel viewmodel;
        public static BigDataPage bigdatacaipage;

        public BigDataPage()
        {
            this.InitializeComponent();
            comboBox0.ItemsSource = colleage;
            comboBox1.ItemsSource = attention;
            comboBox2.ItemsSource = colleage;
            comboBox3.ItemsSource = colleage;

            viewmodel = new ZSCY_Win10.ViewModels.BigDataViewModel();
            this.DataContext = viewmodel;
            bigdatacaipage = this;
            this.SizeChanged += BigDataPage_SizeChanged;

            //手机物理返回键订阅事件
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                Windows.Phone.UI.Input.HardwareButtons.BackPressed += OnBackPressed;
            }
            else
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += PC_BackRequested;
            }
        }
        private void PC_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;

        }
        //获取当前Page的高度和宽度
        private void BigDataPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewmodel.Page_Width = e.NewSize.Width;
            viewmodel.Page_Height = e.NewSize.Height;
        }

        public string[] colleage = {"光电工程学院",
                                    "经济管理学院",
                                    "国际学院",
                                    "法学院",
                                    "传媒艺术学院",
                                    "通信与信息工程学院",
                                    "计算机科学与技术学院",
                                    "软件工程学院",
                                    "体育学院",
                                    "生物信息学院",
                                    "理学院",
                                    "先进制造工程学院",
                                    "外国语学院",
                                    "自动化学院" };
        public string[] attention = { "学院都没选就想看专业，哼~" };

        private void comboBox0_DropDownClosed(object sender, object e)
        {
            if (comboBox0.SelectedItem == null)
            {
                comboBox1.ItemsSource = attention;
                return;
            }
            comboBox1.ItemsSource = Switch.MajorSwitch(comboBox0.SelectedItem.ToString());
        }

        private void comboBox1_DropDownClosed(object sender, object e)
        {
            if (comboBox1.SelectedItem == null)
                return;
            try
            {
                TextBlock1.Text = comboBox0.SelectedItem.ToString() + "-" + comboBox1.SelectedItem.ToString() + "专业的男女比例饼图如下";
                Male.Text = Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).MaleRatio.ToString() + "  " + "(" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Male.ToString() + "人)";
                Female.Text = Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).FemaleRatio.ToString() + "  " + "(" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Female.ToString() + "人)"; ;
            }
            catch
            {
                return;
            }
            List<PieDataItem> datas = new List<PieDataItem>();
            datas.Add(new PieDataItem { Value = Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Male, Brush = new SolidColorBrush(Color.FromArgb(255, 185, 230, 252)) });
            datas.Add(new PieDataItem { Value = Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Female, Brush = new SolidColorBrush(Color.FromArgb(255, 207, 205, 252)) });
            piePlotter0.DataContext = datas;
            piePlotter0.ShowPie();
        }

        private void comboBox2_DropDownClosed(object sender, object e)
        {
            if (comboBox2.SelectedItem == null)
                return;
            try
            {
                TextBlock2.Text = comboBox2.SelectedItem.ToString() + "的最难科目饼图如下";
                Sub1.Text = "1." + Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).Hardest.ToString() + "   " + "(" + Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).HardestRatio.ToString() + "%)";
                Sub2.Text = "2." + Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).Harder.ToString() + "   " + "(" + Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).HarderRatio.ToString() + "%)";
                Sub3.Text = "3." + Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).Hard.ToString() + "   " + "(" + Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).HardRatio.ToString() + "%)";
            }
            catch
            {
                return;
            }
            List<PieDataItem> datas = new List<PieDataItem>();
            datas.Add(new PieDataItem { Value = Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).HardestRatio, Brush = new SolidColorBrush(Color.FromArgb(255, 207, 205, 252)) });
            datas.Add(new PieDataItem { Value = Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).HarderRatio, Brush = new SolidColorBrush(Color.FromArgb(255, 185, 230, 254)) });
            datas.Add(new PieDataItem { Value = Switch.SubjectRatioSwitch(comboBox2.SelectedItem.ToString()).HardRatio, Brush = new SolidColorBrush(Color.FromArgb(255, 254, 199, 227)) });
            piePlotter1.DataContext = datas;
            piePlotter1.ShowPie();
        }

        private void comboBox3_DropDownClosed(object sender, object e)
        {
            if (comboBox3.SelectedItem == null)
                return;
            try
            {
                TextBlock3.Text = comboBox3.SelectedItem.ToString() + "的毕业去向饼图如下";
                Road1.Text = "就业: " + Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).Employed.ToString() + "%";
                Road2.Text = "出国留学: " + Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).Abroad.ToString() + "%";
                Road3.Text = "待业: " + Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).Unemployed.ToString() + "%";
                Road4.Text = "自由职业: " + Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).FreeWork.ToString() + "%";

            }
            catch
            {
                return;
            }
            List<PieDataItem> datas = new List<PieDataItem>();

            datas.Add(new PieDataItem { Value = Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).Employed, Brush = new SolidColorBrush(Color.FromArgb(255, 207, 205, 252)) });
            datas.Add(new PieDataItem { Value = Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).Abroad, Brush = new SolidColorBrush(Color.FromArgb(255, 254, 199, 227)) });
            datas.Add(new PieDataItem { Value = Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).Unemployed, Brush = new SolidColorBrush(Color.FromArgb(255, 158, 252, 238)) });
            datas.Add(new PieDataItem { Value = Switch.CareerRatioSwitch(comboBox3.SelectedItem.ToString()).FreeWork, Brush = new SolidColorBrush(Color.FromArgb(255, 185, 230, 254)) });
            piePlotter2.DataContext = datas;
            piePlotter2.ShowPie();
        }

        private void back_but_Click(object sender, RoutedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
        }

        bool isExit = false;
        private void OnBackPressed(object sender, BackPressedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                return;
            }//Frame内无内容
            if (rootFrame.CurrentSourcePageType.Name != "MainPage")
            {
                if (rootFrame.CanGoBack && e.Handled == false)
                {
                    e.Handled = true;
                    rootFrame.GoBack();
                }
            }//Frame不在MainPage页面并且可以返回则返回上一个页面并且事件未处理
            else if (e.Handled == false)
            {
                StatusBar statusBar = StatusBar.GetForCurrentView();
                statusBar.ShowAsync();
                statusBar.ProgressIndicator.Text = "再按一次返回键即将退出程序 ~\\(≧▽≦)/~"; // 状态栏显示文本
                statusBar.ProgressIndicator.ShowAsync();

                if (isExit)
                {
                    App.Current.Exit();
                }
                else
                {
                    isExit = true;
                    Task.Run(async () =>
                    {
                        await Task.Delay(1500);
                        await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        {
                            await statusBar.ProgressIndicator.HideAsync();
                            await statusBar.ShowAsync(); //此处不隐藏状态栏
                        });
                        isExit = false;
                    });
                    e.Handled = true;
                }//Frame在其他页面并且事件未处理
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (pivot.SelectedIndex < 0)
                {
                    pivot.SelectedIndex = pivot_index = 0;
                }
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.Content_Header_Color_Brush;
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Collapsed;
                pivot_index = pivot.SelectedIndex;
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.APP_Color_Brush;
                (((pivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                return;
            }
        }

        private async void MaleAndFemale_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (comboBox0.SelectedItem != null)
            {
                //MessageDialog msg = new MessageDialog(
                //      "男生人数:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Male.ToString() + "   " + "男生比例:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).MaleRatio.ToString() + "\n"
                //    + "女生人数:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Female.ToString() + "   " + "女生比例:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).FemaleRatio.ToString());
                //msg.Title = "男女比例:";
                //await msg.ShowAsync();

                detail_title.Text = "男女比例:";
                detail_content.Text = "男生人数:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Male.ToString() + "   " + "男生比例:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).MaleRatio.ToString() + "\n"
                    + "女生人数:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).Female.ToString() + "   " + "女生比例:" + Switch.SexRatioSwitch(comboBox1.SelectedItem.ToString()).FemaleRatio.ToString();
                black_background.Visibility = Visibility.Visible;
                black_background_sb.Begin();
                detail_popup.IsOpen = true;
            }
            else
            {
                //await new MessageDialog("你都还没有选点我干嘛~").ShowAsync();

                detail_title.Text = "男女比例:";
                detail_content.Text = "记得先点左边的框框再来看我 \\(≧▽≦)/";
                black_background.Visibility = Visibility.Visible;
                black_background_sb.Begin();
                detail_popup.IsOpen = true;
            }
        }

        private async void Subjects_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                //MessageDialog msg = new MessageDialog(
                //      Sub1.Text.ToString() + "\n"
                //    + Sub2.Text.ToString() + "\n"
                //    + Sub3.Text.ToString());
                //msg.Title = "科目难度排行:";
                //await msg.ShowAsync();

                detail_title.Text = "科目难度排行:";
                detail_content.Text = Sub1.Text.ToString() + "\n"
                    + Sub2.Text.ToString() + "\n"
                    + Sub3.Text.ToString();
                black_background.Visibility = Visibility.Visible;
                black_background_sb.Begin();
                detail_popup.IsOpen = true;
            }
            else
            {
                //await new MessageDialog("你都还没有选点我干嘛~").ShowAsync();

                detail_title.Text = "科目难度排行:";
                detail_content.Text = "你都还没选，点我我怎么知道 →_→";
                black_background.Visibility = Visibility.Visible;
                black_background_sb.Begin();
                detail_popup.IsOpen = true;
            }
        }

        private async void Road_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (comboBox3.SelectedItem != null)
            {
                //MessageDialog msg = new MessageDialog(
                //      Road1.Text.ToString() + "\n"
                //    + Road2.Text.ToString() + "\n"
                //    + Road3.Text.ToString() + "\n"
                //    + Road4.Text.ToString());
                //msg.Title = "毕业去向:";
                //await msg.ShowAsync();

                detail_title.Text = "毕业去向:";
                detail_content.Text = Road1.Text.ToString() + "\n"
                    + Road2.Text.ToString() + "\n"
                    + Road3.Text.ToString() + "\n"
                    + Road4.Text.ToString();
                black_background.Visibility = Visibility.Visible;
                black_background_sb.Begin();
                detail_popup.IsOpen = true;
            }
            else
            {
                //await new MessageDialog("你都还没有选点我干嘛~").ShowAsync();

                detail_title.Text = "毕业去向:";
                detail_content.Text = "你要先点左边那个萌萌哒的框框 \\(^o^)/";
                black_background.Visibility = Visibility.Visible;
                black_background_sb.Begin();
                detail_popup.IsOpen = true;
            }
        }

        //清除Popup内容
        private void detail_popup_Closed(object sender, object e)
        {
            detail_title.Text = "";
            detail_content.Text = "";
            black_background.Visibility = Visibility.Collapsed;
        }
    }
}
