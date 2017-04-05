using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Models;
using ZSCY_Win10.Resource;
using ZSCY_Win10.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.LostAndFoundPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LostAndFoundPage : Page
    {
        APPTheme AppTheme = new APPTheme();
        LostAndFoundPageModel Model = new LostAndFoundPageModel();
        LostAndFoundPageViewModel[] VM = new LostAndFoundPageViewModel[8];//
        bool[] IsItemLoaded = { false, false, false, false, false, false, false, false };
        ObservableCollection<bool> IsProgressRingActive = new ObservableCollection<bool>();
        bool isComboxLoaded = false;
        string BaseUrl;
        public LostAndFoundPage()
        {
            for (byte i = 0; i < 8; i++)
            {
                IsProgressRingActive.Add(true);
                VM[i] = new LostAndFoundPageViewModel();
            }
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;      //页面缓存,页面返回时仅调用OnNavigatedTo方法
        }

        private void LostAndFoundPageAddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(InfoPublish));        
        }

        private async void LostAndFoundPageRefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            int index = LostAndFoundPagePivot.SelectedIndex;
            if (IsItemLoaded[index])
            {
                IsProgressRingActive[index] = true;
                VM[index].data.Clear();
                LostAndFoundPageViewModel temp = await Model.LoadItems(BaseUrl, index);
                foreach (LFItem i in temp.data)
                {
                    VM[index].data.Add(i);
                }
                VM[index].next_page_url = temp.next_page_url;
                if (VM[index].next_page_url == null)
                {
                    IsProgressRingActive[index] = false;
                }
            }
        }

        private async void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;
            int index = LostAndFoundPagePivot.SelectedIndex;
            if (!e.IsIntermediate)
            {
                if (sv.VerticalOffset >= sv.ScrollableHeight - 200)
                {
                    if (VM[index].next_page_url == null)
                    {
                        IsProgressRingActive[index] = false;            //已请求所有
                    }
                    else
                    {
                        LostAndFoundPageViewModel temp = await Model.LoadItems(VM[index].next_page_url);
                        foreach (LFItem i in temp.data)
                        {
                            VM[index].data.Add(i);
                        }
                        VM[index].next_page_url = temp.next_page_url;
                    }
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var temp = e.ClickedItem as LFItem;
            Frame.Navigate(typeof(LFDetailPage), temp.pro_id, new CommonNavigationTransitionInfo());
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)            
        {
            if (!IsItemLoaded[0])
            {
                    BaseUrl = "http://hongyan.cqupt.edu.cn/laf/api/view/found/";
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private async void LostAndFoundPagePivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = LostAndFoundPagePivot.SelectedIndex;
            if (!IsItemLoaded[index])
            {
                LostAndFoundPageViewModel temp = await Model.LoadItems(BaseUrl, index);
                foreach (LFItem i in temp.data)
                {
                    VM[index].data.Add(i);
                }
                VM[index].next_page_url = temp.next_page_url;
                if (VM[index].next_page_url == null)
                {
                    IsProgressRingActive[index] = false;
                }
            }

            IsItemLoaded[index] = true;
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isComboxLoaded)
            {
                if (combox.SelectedIndex == 0)
                    BaseUrl = "http://hongyan.cqupt.edu.cn/laf/api/view/found/";
                else
                    BaseUrl = "http://hongyan.cqupt.edu.cn/laf/api/view/lost/";
                for (int i = 0; i < 8; i++)
                {
                    VM[i].data.Clear();
                    IsItemLoaded[i] = false;
                }
                int index = LostAndFoundPagePivot.SelectedIndex;
                LostAndFoundPageViewModel temp = await Model.LoadItems(BaseUrl, index);
                foreach (LFItem i in temp.data)
                {
                    VM[index].data.Add(i);
                }
                VM[index].next_page_url = temp.next_page_url;
                if (VM[index].next_page_url == null)
                {
                    IsProgressRingActive[index] = false;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            isComboxLoaded = true;
        }
    }
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ObservableCollectionBoolToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
