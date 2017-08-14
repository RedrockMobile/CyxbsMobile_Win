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
using ZSCY_Win10.Util;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class BigDataPage : Page
    {
        private int pivot_index = 0;
        private ZSCY_Win10.ViewModels.BigDataViewModel viewmodel;
        public static BigDataPage bigdatacaipage;
        SexRatio.Rootobject srrb;
        List<string> collegelist;
        FailRatio.Rootobject frrb;
        List<string> collegelist1;
        List<string> majorlist;
        WorkRatio.Rootobject wrrb;
        List<string> collegelist2;
        public BigDataPage()
        {
            this.InitializeComponent();
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                string json = await NetWork.RatioRequest("RequestType", "SexRatio");
                var serializer = new DataContractJsonSerializer(typeof(SexRatio.Rootobject));
                srrb = JsonConvert.DeserializeObject<SexRatio.Rootobject>(json);
                collegelist = new List<string>();
                foreach (SexRatio.Datum item in srrb.Data)
                {
                    collegelist.Add(item.college);
                }
                comboBox0.ItemsSource = collegelist;


                string json1 = await NetWork.RatioRequest("RequestType", "FailPlus");
                var serializer1 = new DataContractJsonSerializer(typeof(FailRatio.Rootobject));
                frrb = JsonConvert.DeserializeObject<FailRatio.Rootobject>(json1);
                collegelist1 = new List<string>();
                foreach (FailRatio.Datum item in frrb.Data)
                {
                    collegelist1.Add(item.college);
                }
                comboBox1.ItemsSource = collegelist1;


                string json2 = await NetWork.RatioRequest("RequestType", "WorkRatio");
                var serializer2 = new DataContractJsonSerializer(typeof(WorkRatio.Rootobject));
                wrrb = JsonConvert.DeserializeObject<WorkRatio.Rootobject>(json2);
                collegelist2 = new List<string>();
                foreach (WorkRatio.Datum item in wrrb.Data)
                {
                    collegelist2.Add(item.college);
                }
                comboBox3.ItemsSource = collegelist2;
            }
            catch
            {
                return;
            }
        }
        //获取当前Page的高度和宽度
        private void BigDataPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewmodel.Page_Width = e.NewSize.Width;
            viewmodel.Page_Height = e.NewSize.Height;
        }
        private void comboBox0_DropDownClosed(object sender, object e)
        {
            try
            {
                if (comboBox0.SelectedIndex == -1)
                {
                    return;
                }
                int temp = comboBox0.SelectedIndex;
                a.Opacity = 1;
                b.Opacity = 1;
                a_ratio.Text = srrb.Data[temp].MenRatio.Substring(2, 2) + "." + srrb.Data[temp].MenRatio.Substring(4, 2) + "%";
                b_ratio.Text = srrb.Data[temp].WomenRatio.Substring(2, 2) + "." + srrb.Data[temp].WomenRatio.Substring(4, 2) + "%";
                a_sb_d.To = Convert.ToDouble(srrb.Data[temp].MenRatio);
                b_sb_d.To = Convert.ToDouble(srrb.Data[temp].WomenRatio);
                a_sb.Begin();
                b_sb.Begin();
            }
            catch
            {
                return;
            }
        }

        private void comboBox1_DropDownClosed(object sender, object e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    comboBox2.ItemsSource = new List<string> { "请先选择学院~" };
                    return;
                }
                majorlist = new List<string>();
                foreach (FailRatio.Major item in frrb.Data[comboBox1.SelectedIndex].major)
                {
                    majorlist.Add(item.major);
                }
                comboBox2.ItemsSource = majorlist;
            }
            catch
            {
                return;
            }
        }

        private void comboBox2_DropDownClosed(object sender, object e)
        {
            try
            {
                if (comboBox1.SelectedIndex == -1)
                {
                    return;
                }
                if (comboBox2.SelectedIndex == -1)
                {
                    return;
                }
                int temp = comboBox2.SelectedIndex;
                if (frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course.Length == 2)
                {
                    c.Opacity = 0;
                    c_ratio.Opacity = 0;
                    d.Opacity = 1;
                    f.Opacity = 1;
                    c_text_sp.Opacity = 0;
                    d_text_sp.Opacity = 1;
                    e_text_sp.Opacity = 1;
                    d_sb_d.To = Convert.ToDouble(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio);
                    f_sb_d.To = Convert.ToDouble(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio);
                    frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Insert(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Length, "000");
                    frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Insert(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Length, "000");
                    d_ratio.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Substring(2, 2) + "." + frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Substring(4, 2) + "%";
                    f_ratio.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Substring(2, 2) + "." + frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Substring(4, 2) + "%";
                    d_text.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].course;
                    e_text.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].course;
                    d_sb.Begin();
                    f_sb.Begin();
                }
                if (frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course.Length == 3)
                {
                    c.Opacity = 1;
                    c_ratio.Opacity = 1;
                    d.Opacity = 1;
                    f.Opacity = 1;
                    c_text_sp.Opacity = 1;
                    d_text_sp.Opacity = 1;
                    e_text_sp.Opacity = 1;
                    c_sb_d.To = Convert.ToDouble(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio);
                    d_sb_d.To = Convert.ToDouble(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio);
                    f_sb_d.To = Convert.ToDouble(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].ratio);
                    frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Insert(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Length, "000");
                    frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Insert(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Length, "000");
                    frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].ratio = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].ratio.Insert(frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].ratio.Length, "000");
                    c_ratio.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Substring(2, 2) + "." + frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].ratio.Substring(4, 2) + "%";
                    d_ratio.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Substring(2, 2) + "." + frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].ratio.Substring(4, 2) + "%";
                    f_ratio.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].ratio.Substring(2, 2) + "." + frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].ratio.Substring(4, 2) + "%";
                    c_text.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[0].course;
                    d_text.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[1].course;
                    e_text.Text = frrb.Data[comboBox1.SelectedIndex].major[comboBox2.SelectedIndex].course[2].course;
                    c_sb.Begin();
                    d_sb.Begin();
                    f_sb.Begin();
                }
            }
            catch
            {
                return;
            }
        }
        private void comboBox3_DropDownClosed(object sender, object e)
        {
            try
            {
                if (comboBox3.SelectedIndex == -1)
                {
                    return;
                }
                int temp = comboBox3.SelectedIndex;
                g.Opacity = 1;
                h.Opacity = 1;
                double noworkratio = 1 - Convert.ToDouble(wrrb.Data[temp].ratio);
                g_ratio.Text = wrrb.Data[temp].ratio.Substring(2, 2) + "." + wrrb.Data[temp].ratio.Substring(4, 2) + "%";
                h_ratio.Text = noworkratio.ToString().Substring(2, 2) + "." + noworkratio.ToString().Substring(4, 2) +"%";
                g_sb_d.To = Convert.ToDouble(wrrb.Data[temp].ratio);
                h_sb_d.To = Convert.ToDouble(noworkratio);
                g_sb.Begin();
                h_sb.Begin();
            }
            catch
            {
                return;
            }
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
    }
}
