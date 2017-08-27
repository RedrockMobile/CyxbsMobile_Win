using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZSCY.Models;
using ZSCY.Pages;
using ZSCY_Win10.Resouces;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    ///

    public sealed partial class BigDataPage : Page
    {
        private ZSCY_Win10.ViewModels.BigDataViewModel viewmodel;
        public static BigDataPage bigDatePage;
        private int pivot_index;

        //男女比例
        private List<ZSCY.Models.SexDatum> sexList = new List<SexDatum>();

        //就业率
        private List<ZSCY.Models.WorkDatum> workList = new List<WorkDatum>();

        //最难科目
        private List<ZSCY.Models.CourseDatum> collegeList = new List<ZSCY.Models.CourseDatum>();

        //最难科目 的combobox的index
        private int _thisCollegeIndex;

        private int _thisMajorIndex;

        public BigDataPage()
        {
            this.InitializeComponent();
            pivot_index = 0;
            viewmodel = new ZSCY_Win10.ViewModels.BigDataViewModel();
            this.DataContext = viewmodel;
            bigDatePage = this;

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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                await First_Step();
                await Task.Delay(100);
                myPivot.SelectedIndex = pivot_index = 0;
                //载入界面显示的第一个 pivot index是0，手动隐藏 index 1/2的 横线
                Line2.Visibility = Line3.Visibility = Visibility.Collapsed;
                //载入界面显示的第一个 pivot index是0，手动改变index 1/2的颜色
                CourseTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
                WorkTextBlock.Foreground = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
            }
        }

        private async Task First_Step()
        {
            StorageFile file;
            string json = "";
            JObject json_object;
            string uri = "http://hongyan.cqupt.edu.cn/welcome/2017/api/apiRatio.php";

            #region 得到次级标题Header列表

            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/BigDate_Header_Lists.json", UriKind.Absolute));
            json = await FileIO.ReadTextAsync(file);
            json_object = (JObject)JsonConvert.DeserializeObject(json);
            JArray headers = (JArray)json_object["header_lists"];
            ObservableCollection<ZSCY.Models.BigData> header_lists = new ObservableCollection<ZSCY.Models.BigData>();
            for (int i = 0; i < headers.Count; i++)
            {
                ZSCY.Models.BigData item = new ZSCY.Models.BigData();
                item.header = headers[i]["header"].ToString();
                header_lists.Add(item);
            }
            viewmodel.Header = header_lists;

            #endregion 得到次级标题Header列表

            #region 得到男女比例选择栏 -> 学院

            //uri = "http://www.yangruixin.com/test/apiRatio.php";
            List<KeyValuePair<String, String>> SexParamList = new List<KeyValuePair<String, String>>();
            SexParamList.Add(new KeyValuePair<string, string>("RequestType", "SexRatio"));

            json = await ZSCY.Models.NewStudentsPageNetworkRequest.NetworkRequest(uri, SexParamList, 0);

            if (json != null)
            {
                ZSCY.Models.SexRootobject dataModel = new SexRootobject();
                dataModel = JsonConvert.DeserializeObject<ZSCY.Models.SexRootobject>(json);
                //循环绑定
                foreach (var item in dataModel.Data)
                {
                    sexList.Add(item);
                }
                //绑定数据
                sexRatio_ComboBox.ItemsSource = sexList;
            }

            #endregion 得到男女比例选择栏 -> 学院

            #region 得到就业率选择栏 -> 学院

            //uri = "http://www.yangruixin.com/test/apiRatio.php";
            List<KeyValuePair<String, String>> WorkParamList = new List<KeyValuePair<String, String>>();
            WorkParamList.Add(new KeyValuePair<string, string>("RequestType", "DataOfJob"));

            json = await ZSCY.Models.NewStudentsPageNetworkRequest.NetworkRequest(uri, WorkParamList, 0);

            if (json != null)
            {
                ZSCY.Models.WorkRootobject dataModel = new WorkRootobject();
                dataModel = JsonConvert.DeserializeObject<ZSCY.Models.WorkRootobject>(json);
                //循环绑定
                foreach (var item in dataModel.Data)
                {
                    workList.Add(item);
                }
                //绑定数据
                workRatioListView.ItemsSource = workList;
                //Listview 中 datatemple 导致 无法拿到x:name 
                //放弃listview 改为手动的 stackpanel排列  = =! 
            }

            #endregion 得到就业率选择栏 -> 学院

            #region 得到最难科目选择栏 -> 学院+专业

            //uri = "http://www.yangruixin.com/test/apiRatio.php";
            List<KeyValuePair<String, String>> FailParamList = new List<KeyValuePair<String, String>>();
            FailParamList.Add(new KeyValuePair<string, string>("RequestType", "FailPlus"));

            json = await ZSCY.Models.NewStudentsPageNetworkRequest.NetworkRequest(uri, FailParamList, 0);

            if (json != null)
            {
                ZSCY.Models.CourseRootobject dataModel = new CourseRootobject();
                dataModel = JsonConvert.DeserializeObject<ZSCY.Models.CourseRootobject>(json);
                //新姿势
                //循环绑定
                foreach (var item in dataModel.Data)
                {
                    collegeList.Add(item);
                }
                //绑定数据
                collegeCourse_ComboBox.ItemsSource = collegeList;
            }

            #endregion 得到最难科目选择栏 -> 学院+专业
        }

       

        //myPivot.Header 切换效果实现  (有bug)
        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region 标题切换
            try
            {
                if (myPivot.SelectedIndex < 0)
                {
                    myPivot.SelectedIndex = pivot_index = 0;
                }
                (((myPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.Content_Header_Color_Brush;
                (((myPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Collapsed;
                pivot_index = myPivot.SelectedIndex;
                (((myPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.APP_Color_Brush;
                (((myPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                return;
            }
            #endregion


            //就业数据动画
            #region 就业数据动画
            Debug.WriteLine("PivotHeader Index is " + myPivot.SelectedIndex );

            if(myPivot.SelectedIndex == 2)
            {
                try
                {
                    //Sun, 27 Aug 2017 16:34:35 GMT
                    //改用VisualTreeHelper 
                    //动画不完美 第一次切入无动画 随后切换才会显示
                    //应该是 手动设置pivot header index的原因
                    List<LineChart> myLineChartList = new List<LineChart>();
                    FindChildren<LineChart>(myLineChartList, workRatioListView);
                    Debug.WriteLine("LineChart has been found! Total: " + myLineChartList.Count);
                    foreach (var _lineChart in myLineChartList)
                    {
                        if (_lineChart != null)
                        {
                            
                            LineChart lineChart = _lineChart as LineChart;

                            CircleEase circleEase = new CircleEase();
                            circleEase.EasingMode = EasingMode.EaseInOut;

                            var storyBoard = new Storyboard();

                            var extendAnimation = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = lineChart.ActualValue, EnableDependentAnimation = true };

                            extendAnimation.EasingFunction = circleEase;

                            Storyboard.SetTarget(extendAnimation, lineChart);
                            Storyboard.SetTargetProperty(extendAnimation, "ActualValue");

                            extendAnimation.EasingFunction = circleEase;
                            storyBoard.Children.Add(extendAnimation);


                            storyBoard.AutoReverse = false;
                            storyBoard.Begin();
                        }

                    }
                    // LineChart _lineChart = FindFirstVisualChild<LineChart>(workRatioListView, "workLineChart");
                    
                    
                }
                catch (Exception)
                {
                    return;
                }
            }
            #endregion
        }



        //遍历实例化树
        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
  where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }
        
        private childItem FindVisualChild<childItem>(DependencyObject obj)
    where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }
        public T FindFirstVisualChild<T>(DependencyObject obj, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T && child.GetValue(NameProperty).ToString() == childName)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindFirstVisualChild<T>(child, childName);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }

       

        private void PC_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
        }

        private bool isExit = false;

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

        //脑残版数据绑定  男女比例
        private void sexRatio_ComboBox_DropDownClosed(object sender, object e)
        {
            if (sexRatio_ComboBox.SelectedItem == null)
                return;

            int index = sexRatio_ComboBox.SelectedIndex;
            double __menRatio = Convert.ToDouble(sexList[index].MenRatio);
            double __womenRatio = Convert.ToDouble(sexList[index].WomenRatio);

            //保留两位小数
            double _menRatio = Math.Round(__menRatio, 2);
            double _womenRatio = Math.Round(__womenRatio, 2);
            Debug.WriteLine("sexRatio_ComboBox.SelectedIndex " + index);
            Debug.WriteLine(_menRatio + "    " + _womenRatio);

            //storyboard 定义的动画最后的 to 不需要赋值
            //MenRatioCPB.ProgressNum = _menRatio;
            //WomenRatioCPB.ProgressNum = _womenRatio;

            //动画
            CircleEase circleEase = new CircleEase();
            circleEase.EasingMode = EasingMode.EaseInOut;

            var storyBoard = new Storyboard();
            var extendAnimation1 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = _menRatio, EnableDependentAnimation = true };

            extendAnimation1.EasingFunction = circleEase;

            Storyboard.SetTarget(extendAnimation1, MenRatioCPB);
            Storyboard.SetTargetProperty(extendAnimation1, "ProgressNum");

            var extendAnimation2 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = _womenRatio, EnableDependentAnimation = true };

            extendAnimation2.EasingFunction = circleEase;

            Storyboard.SetTarget(extendAnimation2, WomenRatioCPB);
            Storyboard.SetTargetProperty(extendAnimation2, "ProgressNum");

            storyBoard.Children.Add(extendAnimation1);
            storyBoard.Children.Add(extendAnimation2);

            storyBoard.AutoReverse = false;
            storyBoard.Begin();
        }

        //最难科目 第一个下拉框 -> 学院
        private void collegeCourse_ComboBox_DropDownClosed(object sender, object e)
        {
            if (collegeCourse_ComboBox.SelectedItem == null)
                return;

            _thisCollegeIndex = collegeCourse_ComboBox.SelectedIndex;
            Debug.WriteLine("collegeCourse_ComboBox.SelectedIndex" + _thisCollegeIndex);

            //最难科目 第二个下拉框 -> 专业
            marjorCourse_ComboBox.ItemsSource = collegeList[_thisCollegeIndex].major;
        }

        //最难科目 第二个下拉框 -> 专业
        private void marjorCourse_ComboBox_DropDownClosed(object sender, object e)
        {
            if (marjorCourse_ComboBox.SelectedItem == null)
                return;

            _thisMajorIndex = marjorCourse_ComboBox.SelectedIndex;
            Debug.WriteLine("marjorCourse_ComboBox.SelectedIndex " + _thisMajorIndex);

            //科目
            string _courseName_1 = collegeList[_thisCollegeIndex].major[_thisMajorIndex].course[0].course;
            string _courseName_2 = collegeList[_thisCollegeIndex].major[_thisMajorIndex].course[1].course;
            string _courseName_3 = collegeList[_thisCollegeIndex].major[_thisMajorIndex].course[2].course;

            //比率
            double __courseRatio_1 = Convert.ToDouble(collegeList[_thisCollegeIndex].major[_thisMajorIndex].course[0].ratio);
            double __courseRatio_2 = Convert.ToDouble(collegeList[_thisCollegeIndex].major[_thisMajorIndex].course[1].ratio);
            double __courseRatio_3 = Convert.ToDouble(collegeList[_thisCollegeIndex].major[_thisMajorIndex].course[2].ratio);

            //保留两位小数
            double _courseRatio_1 = Math.Round(__courseRatio_1, 2);
            double _courseRatio_2 = Math.Round(__courseRatio_2, 2);
            double _courseRatio_3 = Math.Round(__courseRatio_3, 2);

            courseTextBlock_1.Text = _courseName_1;
            ratioTextBlock_1.Text = _courseRatio_1.ToString();
            //
            courseTextBlock_2.Text = _courseName_2;
            ratioTextBlock_2.Text = _courseRatio_2.ToString();
            //
            courseTextBlock_3.Text = _courseName_3;
            ratioTextBlock_3.Text = _courseRatio_3.ToString();

            //动画
            CircleEase circleEase = new CircleEase();
            circleEase.EasingMode = EasingMode.EaseInOut;
            var storyBoard = new Storyboard();
            var extendAnimation1 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = _courseRatio_1, EnableDependentAnimation = true };

            extendAnimation1.EasingFunction = circleEase;
            Storyboard.SetTarget(extendAnimation1, OutsideCourseRatioCPB);
            Storyboard.SetTargetProperty(extendAnimation1, "ProgressNum");

            var extendAnimation2 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = _courseRatio_2, EnableDependentAnimation = true };

            extendAnimation2.EasingFunction = circleEase;
            Storyboard.SetTarget(extendAnimation2, CentralCourseRatioCPB);
            Storyboard.SetTargetProperty(extendAnimation2, "ProgressNum");

            var extendAnimation3 = new DoubleAnimation { Duration = new Duration(TimeSpan.FromSeconds(0.5)), From = 0, To = _courseRatio_3, EnableDependentAnimation = true };

            extendAnimation3.EasingFunction = circleEase;
            Storyboard.SetTarget(extendAnimation3, InnerCourseRatioCPB);
            Storyboard.SetTargetProperty(extendAnimation3, "ProgressNum");

            storyBoard.Children.Add(extendAnimation1);
            storyBoard.Children.Add(extendAnimation2);
            storyBoard.Children.Add(extendAnimation3);

            storyBoard.AutoReverse = false;
            storyBoard.Begin();
        }
    }
}