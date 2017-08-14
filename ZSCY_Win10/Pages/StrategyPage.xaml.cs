using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ZSCY.Models;
using ZSCY_Win10.Resource;
using ZSCY_Win10.ViewModels;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZSCY_Win10;
using Windows.Phone.UI.Input;
using Windows.UI.ViewManagement;
using ZSCY_Win10.Models;
using Windows.UI.Xaml.Media.Imaging;
using ZSCY_Win10.Util;
using ZSCY_Win10.Data;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class StrategyPage : Page
    {
        private int pivot_index;
        StrategyViewModel viewmodel = new StrategyViewModel();
        SQLiteHelper1 sqliteHelper1 = new SQLiteHelper1();
        SQLiteHelper2 sqliteHelper2 = new SQLiteHelper2();
        ObservableCollection<XSQqqData> xsqqqlist = new ObservableCollection<XSQqqData>();
        ObservableCollection<LXQqqData> lxqqqlist = new ObservableCollection<LXQqqData>();
        ObservableCollection<XSQqqData> xsqqqlist_result = new ObservableCollection<XSQqqData>();
        ObservableCollection<LXQqqData> lxqqqlist_result = new ObservableCollection<LXQqqData>();
        public StrategyPage()
        {
            this.InitializeComponent();
            this.DataContext = viewmodel;
            this.SizeChanged += StrategyPage_SizeChanged;
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

        private void StrategyPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewmodel.Page_Width = e.NewSize.Width;
            viewmodel.Page_Height = e.NewSize.Height;
        }

        private void PC_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                for (int i = 0; i < 8; i++)
                {
                    App.isLoading[i] = false;
                }
                SchoolBudings_Get();
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
        public async Task<string> HttpClient(string type)
        {
            string result = null;
            try
            {
                const string api = "http://www.yangruixin.com/test/apiForGuide.php?RequestType={0}";
                HttpClient httpclient = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();
                string Api = api.Replace("{0}", type);
                response = await httpclient.GetAsync(Api);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch
            {
                var messageDialog = new MessageDialog("好像没网络耶！！！");

                await messageDialog.ShowAsync();
            }
            return result;
        }
        private async void SchoolBudings_Get()
        {
            try
            {
                string type = "SchoolBuildings";
                string result = await HttpClient(type);
                if (result != null)
                {
                    SchoolBuildings.Rootobject data = JsonConvert.DeserializeObject<SchoolBuildings.Rootobject>(result);
                    viewmodel.SchoolBuildings = data.Data;
                }
            }
            catch
            {
                return;
            }
        }
        private async void Dormitory_Get()
        {
            try
            {
                string type = "Dormitory";
                string result = await HttpClient(type);
                if (result != null)
                {
                    Dormitory.Rootobject data = JsonConvert.DeserializeObject<Dormitory.Rootobject>(result);
                    viewmodel.Dormitory = data.Data;
                }
            }
            catch
            {
                return;
            }
        }
        private async void Canteen_Get()
        {
            try
            {
                string type = "Canteen";
                string result = await HttpClient(type);
                if (result != null)
                {
                    Canteen.Rootobject data = JsonConvert.DeserializeObject<Canteen.Rootobject>(result);
                    viewmodel.Canteen = data.Data;
                }
            }
            catch
            {
                return;
            }
        }
        private async void DailyLife_Get()
        {
            try
            {
                string type = "LifeInNear";
                string result = await HttpClient(type);
                if (result != null)
                {
                    DailyLife.Rootobject data = JsonConvert.DeserializeObject<DailyLife.Rootobject>(result);
                    viewmodel.DailyLife = data.Data;
                }
            }
            catch
            {
                return;
            }
        }
        private async void Eat_Get()
        {
            try
            {
                string type = "Cate";
                string result = await HttpClient(type);
                if (result != null)
                {
                    Eat.Rootobject data = JsonConvert.DeserializeObject<Eat.Rootobject>(result);
                    viewmodel.Eat = data.Data;
                }
            }
            catch
            {
                return;
            }
        }
        private async void BeautyInNear_Get()
        {
            try
            {
                string type = "BeautyInNear";
                string result = await HttpClient(type);
                if (result != null)
                {
                    BeautyInNear.Rootobject data = JsonConvert.DeserializeObject<BeautyInNear.Rootobject>(result);
                    viewmodel.BeautyInNear = data.Data;
                }
            }
            catch
            {
                return;
            }
        }

        private void SPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (SPivot.SelectedIndex < 0)
                {
                    SPivot.SelectedIndex = pivot_index = 0;
                }
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.Content_Header_Color_Brush;
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Collapsed;
                pivot_index = SPivot.SelectedIndex;
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.APP_Color_Brush;
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                return;
            }
            if (SPivot.SelectedIndex == 1 && !App.isLoading[1])
            {
                Dormitory_Get();
                App.isLoading[1] = true;
            }
            if (SPivot.SelectedIndex == 2 && !App.isLoading[2])
            {
                Canteen_Get();
                App.isLoading[2] = true;
            }
            if (SPivot.SelectedIndex ==4 && !App.isLoading[4])
            {
                sqliteHelper1.CreateDB();
                sqliteHelper2.CreateDB();
                List<XSQqqData> temp = sqliteHelper1.CheckData("重庆邮电大学总群：");
                List<LXQqqData> temp1 = sqliteHelper2.CheckData("海南：");//随便搜索一下，看一下有没有表在
                if (temp.Count==0)
                {
                    List<XSQqqData> a = new List<XSQqqData>();
                    a.Add(new XSQqqData() { college = "重庆邮电大学总群：", qq = "636208141" });
                    a.Add(new XSQqqData() { college = "通信与信息工程学院：", qq = "498167991" });
                    a.Add(new XSQqqData() { college = "计算机与科学技术学院：", qq = "638612170" });
                    a.Add(new XSQqqData() { college = "自动化学院：", qq = "574872113" });
                    a.Add(new XSQqqData() { college = "光电工程学院/国际半导体学院：", qq = "636449199" });
                    a.Add(new XSQqqData() { college = "外国语学院：", qq = "333094013" });
                    a.Add(new XSQqqData() { college = "传媒艺术学院：", qq = "527468298" });
                    a.Add(new XSQqqData() { college = "生物信息学院：", qq = "637402699" });
                    a.Add(new XSQqqData() { college = "经济管理学院信息管理与信息系统专业：", qq = "362192309" });
                    a.Add(new XSQqqData() { college = "经济管理学院：", qq = "545772871" });
                    a.Add(new XSQqqData() { college = "经济管理学院工程管理专业：", qq = "552540368" });
                    a.Add(new XSQqqData() { college = "软件工程学院：", qq = "482656306" });
                    a.Add(new XSQqqData() { college = "网络空间安全与信息法学院：", qq = "162240404" });
                    a.Add(new XSQqqData() { college = "理学院：", qq = "575159267" });
                    a.Add(new XSQqqData() { college = "体育学院：", qq = "649510732" });
                    a.Add(new XSQqqData() { college = "国际学院：", qq = "17443276" });
                    a.Add(new XSQqqData() { college = "先进制造工程学院：", qq = "563565394" });
                    foreach (XSQqqData item in a)
                    {
                        sqliteHelper1.AddData(item);
                    }
                }
                if (temp.Count==0)
                {

                    List<LXQqqData> b = new List<LXQqqData>();
                    b.Add(new LXQqqData() { area = "海南：", qq = "9334029" });
                    b.Add(new LXQqqData() { area = "贵州：", qq = "601631814" });
                    b.Add(new LXQqqData() { area = "河北：", qq = "548535234" });
                    b.Add(new LXQqqData() { area = "安徽：", qq = "562487104" });
                    b.Add(new LXQqqData() { area = "辽宁：", qq = "134489031" });
                    b.Add(new LXQqqData() { area = "河南老乡群1：", qq = "310222276" });
                    b.Add(new LXQqqData() { area = "河南老乡群2：", qq = "251311309" });
                    b.Add(new LXQqqData() { area = "河南安阳：", qq = "116198098" });
                    b.Add(new LXQqqData() { area = "山东：", qq = "384043802" });
                    b.Add(new LXQqqData() { area = "江苏：", qq = "123736116" });
                    b.Add(new LXQqqData() { area = "黑龙江：", qq = "316348915" });
                    b.Add(new LXQqqData() { area = "潮汕：", qq = "4958681" });
                    b.Add(new LXQqqData() { area = "江西：", qq = "3889855" });
                    b.Add(new LXQqqData() { area = "江西上饶：", qq = "476426072" });
                    b.Add(new LXQqqData() { area = "浙江：", qq = "247010642" });
                    b.Add(new LXQqqData() { area = "广西贵港：", qq = "5819894" });
                    b.Add(new LXQqqData() { area = "广西南宁：", qq = "16026851" });
                    b.Add(new LXQqqData() { area = "广西：", qq = "9651531" });
                    b.Add(new LXQqqData() { area = "广西柳州：", qq = "7045893" });
                    b.Add(new LXQqqData() { area = "广东：", qq = "113179139" });
                    b.Add(new LXQqqData() { area = "广东韶关：", qq = "66484867" });
                    b.Add(new LXQqqData() { area = "广东惠州：", qq = "213337022" });
                    b.Add(new LXQqqData() { area = "山西：", qq = "119738941" });
                    b.Add(new LXQqqData() { area = "福建：", qq = "173210510" });
                    b.Add(new LXQqqData() { area = "吉林：", qq = "118060379" });
                    b.Add(new LXQqqData() { area = "云南宣威：", qq = "211910023" });
                    b.Add(new LXQqqData() { area = "云南玉溪：", qq = "256581906" });
                    b.Add(new LXQqqData() { area = "云南曲靖：", qq = "117499346" });
                    b.Add(new LXQqqData() { area = "云南：", qq = "548640416" });
                    b.Add(new LXQqqData() { area = "云南官方群：", qq = "42052111" });
                    b.Add(new LXQqqData() { area = "天津：", qq = "8690505" });
                    b.Add(new LXQqqData() { area = "湖北恩施：", qq = "179765240" });
                    b.Add(new LXQqqData() { area = "湖北：", qq = "33861584" });
                    b.Add(new LXQqqData() { area = "湖北黄冈：", qq = "181704337" });
                    b.Add(new LXQqqData() { area = "湖南：", qq = "204491110" });
                    b.Add(new LXQqqData() { area = "重庆梁平：", qq = "85423833" });
                    b.Add(new LXQqqData() { area = "重庆忠县：", qq = "115637967" });
                    b.Add(new LXQqqData() { area = "重庆铜梁：", qq = "198472776" });
                    b.Add(new LXQqqData() { area = "重庆大足：", qq = "462534986" });
                    b.Add(new LXQqqData() { area = "重庆开县：", qq = "5657168" });
                    b.Add(new LXQqqData() { area = "重庆荣昌：", qq = "149452192" });
                    b.Add(new LXQqqData() { area = "重庆永川：", qq = "467050041" });
                    b.Add(new LXQqqData() { area = "重庆丰都：", qq = "343292119" });
                    b.Add(new LXQqqData() { area = "重庆涪陵：", qq = "199748999" });
                    b.Add(new LXQqqData() { area = "重庆云阳：", qq = "118971621" });
                    b.Add(new LXQqqData() { area = "重庆璧山：", qq = "112571803" });
                    b.Add(new LXQqqData() { area = "重庆石柱：", qq = "289615375" });
                    b.Add(new LXQqqData() { area = "重庆彭水：", qq = "283978475" });
                    b.Add(new LXQqqData() { area = "重庆南川：", qq = "423494314" });
                    b.Add(new LXQqqData() { area = "重庆垫江：", qq = "307233230" });
                    b.Add(new LXQqqData() { area = "重庆合川：", qq = "226325326" });
                    b.Add(new LXQqqData() { area = "重庆荣昌：", qq = "149452192" });
                    b.Add(new LXQqqData() { area = "重庆綦江：", qq = "109665788" });
                    b.Add(new LXQqqData() { area = "重庆奉节：", qq = "50078959" });
                    b.Add(new LXQqqData() { area = "重庆铜梁：", qq = "198472776" });
                    b.Add(new LXQqqData() { area = "重庆黔江：", qq = "102897346" });
                    b.Add(new LXQqqData() { area = "重庆万州：", qq = "469527984" });
                    b.Add(new LXQqqData() { area = "重庆巫溪：", qq = "143884210" });
                    b.Add(new LXQqqData() { area = "重庆巫山：", qq = "129440237" });
                    b.Add(new LXQqqData() { area = "四川大群：", qq = "142604890" });
                    b.Add(new LXQqqData() { area = "四川成都：", qq = "298299346" });
                    b.Add(new LXQqqData() { area = "四川自贡：", qq = "444020511" });
                    b.Add(new LXQqqData() { area = "四川绵阳：", qq = "191653502" });
                    b.Add(new LXQqqData() { area = "陕西：", qq = "193388613" });
                    b.Add(new LXQqqData() { area = "新疆：", qq = "248052400" });
                    b.Add(new LXQqqData() { area = "青海：", qq = "282597612" });
                    b.Add(new LXQqqData() { area = "北京：", qq = "143833720" });
                    b.Add(new LXQqqData() { area = "甘肃美术：", qq = "578076400" });
                    b.Add(new LXQqqData() { area = "甘肃：", qq = "155724412" });//我这时候这辈子都不想碰ctrlcv了
                    foreach (LXQqqData item in b)
                    {
                        sqliteHelper2.AddData(item);
                    }
                }
                xsqqqlist = sqliteHelper1.ReadData(xsqqqlist);
                xsqlistview.ItemsSource = xsqqqlist;
                lxqqqlist = sqliteHelper2.ReadData(lxqqqlist);
                lxqlistview.ItemsSource = lxqqqlist;
                App.isLoading[4] = true;
            }
            if (SPivot.SelectedIndex == 5 && !App.isLoading[5])
            {
                DailyLife_Get();
                App.isLoading[5] = true;
            }
            if (SPivot.SelectedIndex == 6 && !App.isLoading[6])
            {
                Eat_Get();
                App.isLoading[6] = true;
            }
            if (SPivot.SelectedIndex == 7 && !App.isLoading[7])
            {
                BeautyInNear_Get();
                App.isLoading[7] = true;
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image temp = sender as Image;
            bigImageBrush.ImageSource = temp.Source;
            back_background.Visibility = Visibility.Visible;
            back_background_sb.Begin();
            image_popup.IsOpen = true;
        }
        private void image_popup_Closed(object sender, object e)
        {
            bigImageBrush.ImageSource = null;
            back_background.Visibility = Visibility.Collapsed;
            bigimage_sc.ChangeView(0, 0, 1);
        }

        private void bigImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            image_popup.IsOpen = false;
        }
        private void bigImageGroup_Tapped(object sender, TappedRoutedEventArgs e)
        {
            imagegroup_popup.IsOpen = false;
        }
        Point Point_new = new Point();
        Point Point_old = new Point();
        private void bigImage_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            isPoint = false;
        }
        bool isPoint = false;
        private void bigImage_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isPoint)
            {
                Point_new = e.GetCurrentPoint(bigimage_sc).Position;
                bigimage_sc.ChangeView(bigimage_sc.HorizontalOffset - Point_new.X + Point_old.X, bigimage_sc.VerticalOffset - Point_new.Y + Point_old.Y, bigimage_sc.ZoomFactor, true);
                Point_old = Point_new;
            }
        }
        private void bigImage_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point_old = e.GetCurrentPoint(bigimage_sc).Position;
            isPoint = true;
        }
        private void bigimagegroup_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (isPoint)
            {
                Point_new = e.GetCurrentPoint((sender as Image).Parent as ScrollViewer).Position;
                ((sender as Image).Parent as ScrollViewer).ChangeView(((sender as Image).Parent as ScrollViewer).HorizontalOffset - Point_new.X + Point_old.X, ((sender as Image).Parent as ScrollViewer).VerticalOffset - Point_new.Y + Point_old.Y, ((sender as Image).Parent as ScrollViewer).ZoomFactor, true);
                Point_old = Point_new;
            }
        }

        private void bigimagegroup_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Point_old = e.GetCurrentPoint((sender as Image).Parent as ScrollViewer).Position;
            isPoint = true;
        }

        private void bigImage_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            isPoint = false;
        }
        private void ImageGroup_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image temp = sender as Image;
            string a= (temp.Source as BitmapImage).UriSource.ToString();
            int index=-10;
            for(int i=0;i<viewmodel.Dormitory.Count;i++)
            {
                for(int j=0;j<viewmodel.Dormitory[i].url.Count;j++)
                {
                    if(a.Equals(viewmodel.Dormitory[i].url[j]))
                    {
                        index =i;
                    }
                }
            }
            imagegroupflipview.ItemsSource = viewmodel.Dormitory[index].url;
            back_background.Visibility = Visibility.Visible;
            back_background_sb.Begin();
            imagegroup_popup.IsOpen = true;
        }
        private void ImageGroup1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image temp = sender as Image;
            string a = (temp.Source as BitmapImage).UriSource.ToString();
            int index = -10;
            for (int i = 0; i < viewmodel.Canteen.Count; i++)
            {
                for (int j = 0; j < viewmodel.Canteen[i].url.Count; j++)
                {
                    if (a.Equals(viewmodel.Canteen[i].url[j]))
                    {
                        index = i;
                    }
                }
            }
            imagegroupflipview.ItemsSource = viewmodel.Canteen[index].url;
            back_background.Visibility = Visibility.Visible;
            back_background_sb.Begin();
            imagegroup_popup.IsOpen = true;
        }

        private void dot_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            imagegroupflipview.SelectedIndex = dot_listbox.SelectedIndex;
        }

        private void imagegroupflipview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dot_listbox.ItemsSource = imagegroupflipview.ItemsSource;
            dot_listbox.SelectedIndex = imagegroupflipview.SelectedIndex;
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(SearchBox.Text=="")
            {
                qqstackpanel.Visibility = Visibility.Visible;
                searchresult.Visibility = Visibility.Collapsed;
                return;
            }
            xsqqqlist_result.Clear();
            lxqqqlist_result.Clear();
            qqstackpanel.Visibility = Visibility.Collapsed;
            searchresult.Visibility = Visibility.Visible;
            List<XSQqqData> temp = sqliteHelper1.CheckData(SearchBox.Text);
            foreach(XSQqqData item in temp)
            {
                xsqqqlist_result.Add(item);
            }
            xsqlistview_result.ItemsSource = xsqqqlist_result;
            List<LXQqqData> temp1 = sqliteHelper2.CheckData(SearchBox.Text);
            foreach (LXQqqData item in temp1)
            {
                lxqqqlist_result.Add(item);
            }
            lxqlistview_result.ItemsSource = lxqqqlist_result;
        }

        private void imagegroup_popup_Closed(object sender, object e)
        {
            imagegroupflipview.ItemsSource = null;
            back_background.Visibility = Visibility.Collapsed;
        }
    }
}
