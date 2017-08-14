using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZSCY.Pages;
using ZSCY_Win10.Models;
using ZSCY_Win10.ViewModels;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class JunxunPage : Page, INotifyPropertyChanged
    {
        //public AppViewBackButtonVisibility AppViewBackButtonVisibility { get; set; }

        ////     在用户调用系统提供的按钮、特定动作或后退导航的语音命令时发生。
        //public event EventHandler<BackRequestedEventArgs> BackRequested;

        ////     返回与当前窗口关联的 SystemNavigationManager 对象。

        ////     与当前窗口关联的 SystemNavigationManager 对象。
        //public static SystemNavigationManager GetForCurrentView();
        
        //public string college { get; set; }
        public int pivot_index;
        private double[] pivotitem1_ver_offest;
        private ZSCY_Win10.ViewModels.JunxunViewModel viewmodel;
        List<string> mylist = new List<string>();
        public JunxunPage():base()
        {
            this.InitializeComponent();
            viewmodel = new ZSCY_Win10.ViewModels.JunxunViewModel();
            this.DataContext = viewmodel;
            this.SizeChanged += JunXunPage_SizeChanged;

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

        //pivot选项改变
        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        private async Task First_Step()
        {
            string json = "";
            JObject json_object;

            #region 得到军训视频内容
            json = await ZSCY_Win10.Util.Request.JunxunShipin_Request();
            if (json != null)
            {
                json_object = (JObject)JsonConvert.DeserializeObject(json);
                JArray data = (JArray)json_object["Data"];
                //dynamic data = (dynamic)json_object["Data"];
                ObservableCollection<Models.junxunshipin> junxunshipin_lists = new ObservableCollection<Models.junxunshipin>();
                for (int i = 0; i < data.Count; i++)
                {
                    Models.junxunshipin item = new Models.junxunshipin();
                    item.title = data[i]["title"].ToString();
                    item.url = data[i]["url"].ToString();
                    item.cover = data[i]["cover"].ToString();
                    junxunshipin_lists.Add(item);
                }
                viewmodel.Junxunshipin = junxunshipin_lists;
            }
            #endregion

            #region 得到军训图片内容
            json = await ZSCY_Win10.Util.Request.JunxunTupian_Request();
            if (json != null)
            {
                //json_object = (JObject)JsonConvert.DeserializeObject(json);
                ////JArray data = (JArray)json_object["Data"];
                //dynamic data = (dynamic)json_object["Data"];
                ObservableCollection<Models.Junxuncontents> junxuntupian_lists = new ObservableCollection<Models.Junxuncontents>();
                //for (int i = 0; i < data.Count; i++)
                //{
                //    Models.junxuntupian item = new Models.junxuntupian();
                //    item.title = data[i]["title"].ToString();
                //    item.url = data[i]["url"].ToString();
                //    junxuntupian_lists.Add(item);
                //}
                //viewmodel.Junxuntupian = junxuntupian_lists;
                var serializer = new DataContractJsonSerializer(typeof(root));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var result = (root)serializer.ReadObject(ms);
                root contentlist = result;
                var contentli = contentlist.Data;
                for(int i=0;i<contentli.title.Count;i++)
                {
                    Models.Junxuncontents item = new Models.Junxuncontents();
                    item.title0 = contentli.title[i];
                    item.url0 = contentli.url[i];
                    junxuntupian_lists.Add(item);
                    mylist.Add(item.url0);
                }
                viewmodel.Junxuntupian = junxuntupian_lists;
                //viewmodel.Junxuntupian = contentli;
            }
            #endregion
        }
        //返回上一级的请求
        private void PC_BackRequested(object sender, Windows.UI.Core.BackRequestedEventArgs e)
        {
            FirstPage.firstpage.Second_Page_Back();
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;

        }

        //按下返回按钮
        bool isExit = false;

        public event PropertyChangedEventHandler PropertyChanged;

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

        private async Task First_step()
        {
            StorageFile file;
            string json = "";
            JObject json_object;

            #region 得到贴士介绍
            file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Json/junxun_tieshi.json", UriKind.Absolute));
            json = await FileIO.ReadTextAsync(file);
            json_object = (JObject)JsonConvert.DeserializeObject(json);
            JArray tieshi_intros = (JArray)json_object["junxun_tieshi"];
            ObservableCollection<Models.junxuntieshi> intro_lists = new ObservableCollection<Models.junxuntieshi>();
            for (int i = 0; i < tieshi_intros.Count; i++)
            {
                Models.junxuntieshi item = new Models.junxuntieshi();
                item.tieshi = new ObservableCollection<string>();
                JArray tieshi_item = (JArray)tieshi_intros[i]["tieshi"];
                for (int j = 0; j < tieshi_item.Count; j++)
                {
                    item.tieshi.Add(tieshi_item[j]["duanluo"].ToString());
                }
                intro_lists.Add(item);
            }
            #endregion
            jx_content.Children.Clear();
            for (int i = 0; i < intro_lists[0].tieshi.Count; i++)
            {
                if (intro_lists[0].tieshi[i].Contains("【"))
                {
                    jx_content.Children.Add(New_TextBlock(1, intro_lists[0].tieshi[i]));
                }
                else
                {
                    jx_content.Children.Add(New_TextBlock(2, intro_lists[0].tieshi[i]));
                }
            }
        }
        private TextBlock New_TextBlock(int p, string content)
        {
            TextBlock tb = new TextBlock();
            switch (p)
            {
                case 1: //较重标题
                    {
                        tb.Text = content.Substring(1, (content.LastIndexOf('】') - content.IndexOf('【') - 1));
                        //tb.Foreground = App.APPTheme.Content_Header_Color_Brush;
                        tb.FontSize = 16;
                        tb.Margin = new Thickness(0, 3, 0, 8);
                    }; break;
                case 2: //普通内容
                    {
                        tb.Text = content;
                        FontWeight weight = new FontWeight();
                        weight.Weight = 10;
                        tb.FontWeight = weight;
                        //tb.Foreground = App.APPTheme.Gary_Color_Brush;
                        tb.FontSize = 15;
                        tb.LineHeight = 26;
                    }; break;
            }
            tb.CharacterSpacing = 100;
            tb.TextWrapping = TextWrapping.Wrap;
            return tb;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                Dispatcher?.RunAsync(CoreDispatcherPriority.Normal, async () =>
                {
                    await First_step();
                });
                await First_Step();
            }
        }

        private void JunXunPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            viewmodel.Page_Width = e.NewSize.Width;
            viewmodel.Page_Height = e.NewSize.Height;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Models.Junxuncontents)
            {
                //Jx_Photos.ImageSource = new BitmapImage(new Uri((e.ClickedItem as Models.Junxuncontents).url0, UriKind.Absolute));
                //title.Text = (e.ClickedItem as Models.junxunshipin).title;
                myflip.ItemsSource = mylist;
            }
            back_background.Visibility = Visibility.Visible;
            back_background_sb.Begin();
            Photos_popup.IsOpen = true;
        }

        private void Photos_popup_Closed(object sender, object e)
        {
            //Jx_Photos.Source = null;
            //title.Text = "";
            back_background.Visibility = Visibility.Collapsed;
        }

        private async void bt1_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(viewmodel.Junxunshipin[0].url));
        }

        private async void bt2_Click(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(viewmodel.Junxunshipin[1].url));
        }

		Point point_new = new Point();
		Point point_old = new Point();
		private void myimage_PointerExited(object sender, PointerRoutedEventArgs e)
		{
			isPoint = false;
		}
		bool isPoint = false;
		private void myimage_Tapped(object sender, TappedRoutedEventArgs e)
		{
			Image temp = sender as Image;
			string a = (temp.Source as BitmapImage).UriSource.ToString();
			int index = -10;
			for (int i = 0; i < mylist.Count; i++)
			{
					if (a.Equals(mylist[i]))
					{
						index = i;
					}
			}
			myflip.ItemsSource = mylist;
			back_background.Visibility = Visibility.Collapsed;
			//back_background_sb.Begin();
			Photos_popup.IsOpen = false;
		}

		private void myimage_PointerMoved(object sender, PointerRoutedEventArgs e)
		{
			if(isPoint)
			{
				point_new = e.GetCurrentPoint((sender as Image).Parent as ScrollViewer).Position;
				((sender as Image).Parent as ScrollViewer).ChangeView(((sender as Image).Parent as ScrollViewer).HorizontalOffset - point_new.X + point_old.X, ((sender as Image).Parent as ScrollViewer).VerticalOffset - point_new.Y + point_old.Y, ((sender as Image).Parent as ScrollViewer).ZoomFactor, true);
				point_old = point_new;
			}
		}

		private void myimage_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			point_old = e.GetCurrentPoint((sender as Image).Parent as ScrollViewer).Position;
			isPoint = true;
		}

		private void myimage_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
			isPoint = false;
		}
	}
}
