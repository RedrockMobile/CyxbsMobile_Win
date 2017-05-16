using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using ZSCY_Win10.Controls;
using ZSCY_Win10.Models.TopicModels;
using ZSCY_Win10.Util;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages.TopicPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchTopic : Page
    {
        private int pivot_index = 0;
        private static string resourceName = "ZSCY";
        public ObservableCollection<Topic> TopicList = new ObservableCollection<Topic>();
        public ObservableCollection<Topic> MyTopicList = new ObservableCollection<Topic>();
        public ObservableCollection<Topic> SearchTopicList = new ObservableCollection<Topic>();

        public SearchTopic()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualState000";
                if (e.NewSize.Width < 800)
                    state = "VisualState000";
                if (e.NewSize.Width > 800)
                {
                    state = "VisualState800";
                }
                VisualStateManager.GoToState(this, state, true);
                cutoffLine.Y2 = e.NewSize.Height;
            };
            GetBaseTopInfo();
            
            AllTopic.ItemsSource = TopicList;
            SearchTopicGridView.ItemsSource = SearchTopicList;
        }

        private async void GetBaseTopInfo()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            if (credentialList[0] != null)
            {
                try
                {
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                    string Topictemp = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Topic/topicList", paramList);
                    JObject Tobj = JObject.Parse(Topictemp);
                    if (Int32.Parse(Tobj["status"].ToString()) == 200)
                    {
                        JArray TopicArray = Utils.ReadJso(Topictemp);
                        for (int i = 0; i < TopicArray.Count; i++)
                        {
                            Topic item = new Topic();
                            item.GetAttribute((JObject)TopicArray[i]);
                            if (item.imgdata.Equals(""))
                            { item.imgdata = "/Assets/base.jpg"; item.color = "DarkGray"; }
                            item.keyword = $"#{item.keyword}#";
                            if (item.is_my_join)
                                MyTopicList.Add(item);
                            TopicList.Add(item);
                        }
                    }
                    if (MyTopicList.Count == 0)
                    { MyTopic.Visibility = Visibility.Collapsed; noMyTopic.Visibility = Visibility.Visible; }
                    else
                        MyTopic.ItemsSource = MyTopicList;
                }
                catch
                {
                    NotifyPopup notifyPopup = new NotifyPopup("网络异常 无法读取事项~");
                    notifyPopup.Show();
                }
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

        private void AllTopic_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(TopicContentPage),e.ClickedItem,new DrillInNavigationTransitionInfo());
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            SearchTopicList.Clear();
            if (searchBox.Text != "")
            {
                string temp = searchBox.Text;
                foreach (var item in TopicList)
                {
                    if (item.keyword.Contains(temp))
                        SearchTopicList.Add(item);
                }
                if (SearchTopicList.Count == 0)
                {
                    NotifyPopup notifyPopup = new NotifyPopup("没有找到该话题~");
                    notifyPopup.Show();
                }
                else
                {
                    pivotGrid.Visibility = Visibility.Collapsed;
                    SearchTopicGrid.Visibility = Visibility.Visible;
                }
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (pivotGrid.Visibility == Visibility.Collapsed)
            {
                searchBox.Text = "";
                pivotGrid.Visibility = Visibility.Visible;
                SearchTopicGrid.Visibility = Visibility.Collapsed;
            }
            else
                this.Frame.Navigate(typeof(CommunityPage));
        }
    }
}
