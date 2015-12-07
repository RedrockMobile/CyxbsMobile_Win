using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchFreeTimeNumPage : Page
    {
        private ApplicationDataContainer appSetting;
        private ObservableCollection<uIdList> muIdList = new ObservableCollection<uIdList>();
        public SearchFreeTimeNumPage()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.InitializeComponent();
            //HubSectionKBNum.Text = appSetting.Values["nowWeek"].ToString();
            appSetting.Values["FreeWeek"] = appSetting.Values["nowWeek"];
            FilterAppBarToggleButton.Label = "第" + appSetting.Values["nowWeek"].ToString() + "周";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            uIdListView.ItemsSource = muIdList;
            UmengSDK.UmengAnalytics.TrackPageStart("SearchFreeTime");
        }


        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
            UmengSDK.UmengAnalytics.TrackPageEnd("SearchFreeTime");
        }



        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
                e.Handled = true;
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            mAddButton();
        }

        private async void mAddButton()
        {
            var muIDArray = muIdList.ToArray().ToList();
            if (AddTextBox.Text.Length != 10)
            {
                Utils.Message("学号不正确");
            }
            else if (muIDArray.Find(p => p.uId.Equals(AddTextBox.Text)) != null)
                Utils.Message("此学号已添加");
            else
            {
                string usename = AddTextBox.Text;
                string useid = AddTextBox.Text;

                //TextBlock t = new TextBlock();
                //t.FontSize = 18;
                //t.Foreground = new SolidColorBrush(Color.FromArgb(255, 51, 51, 51));
                //t.Text = useid;
                //uIdStackPanel.Children.Add(t);







                string name = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/searchPeople?stunum=" + AddTextBox.Text, PostORGet: 1);
                muIdList.Add(new uIdList { uId = AddTextBox.Text, uName = usename });
                Debug.WriteLine("name->" + name);
                if (name != "")
                {
                    try
                    {
                        JObject obj = JObject.Parse(name);
                        if (Int32.Parse(obj["state"].ToString()) == 200)
                        {
                            JObject dataobj = JObject.Parse(obj["data"].ToString());
                            usename = dataobj["name"].ToString();
                            //foreach (var item in uIdStackPanel.Children)
                            //{
                            //    if (((TextBlock)item).Text == useid)
                            //        uIdStackPanel.Children.Remove(item);
                            //}
                        }
                    }
                    catch (Exception) { }

                }
                if (usename != "")
                    for (int i = 0; i < muIdList.Count; i++)
                    {
                        if (muIdList[i].uId == useid)
                        {
                            ListViewItem item = new ListViewItem();
                            muIdList[i].uName = usename;
                            //uIdListView.ItemsSource = null;
                            uIdListView.ItemsSource = muIdList;
                        }
                    }
                else
                {
                    Utils.Message("学号不正确");
                    muIDArray = muIdList.ToArray().ToList();
                    uIdList u = muIDArray.Find(p => p.uId.Equals(useid));
                    muIdList.Remove(u);
                }
                AddTextBox.Text = "";
            }
        }

        private async void uIdListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ;
            var dig = new MessageDialog("确定删除" + (((uIdList)e.ClickedItem).uName) + "(" + ((uIdList)e.ClickedItem).uId + ")", "警告");
            var btnOk = new UICommand("是");
            dig.Commands.Add(btnOk);
            var btnCancel = new UICommand("否");
            dig.Commands.Add(btnCancel);
            var result = await dig.ShowAsync();
            if (null != result && result.Label == "是")
            {
                var muIDArray = muIdList.ToArray().ToList();
                uIdList u = muIDArray.Find(p => p.uId.Equals(((uIdList)e.ClickedItem).uId));
                muIdList.Remove(u);
            }
        }

        private void ForwardAppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (muIdList.Count < 2)
            {
                Utils.Message("请至少输入2个要查询学号");
            }
            else
            {
                AuIdList Au = new AuIdList { muIdList = muIdList, week = int.Parse(appSetting.Values["FreeWeek"].ToString()) };
                Frame.Navigate(typeof(SearchFreeTimeResultPage), Au);
            }
        }

        //private void HubSectionKBNum_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    KBNumFlyout.ShowAt(page);
        //    //HubSectionKBNum.SelectAll();
        //}
        private void KBNumSearchButton_Click(object sender, RoutedEventArgs e)
        {
            KBNumSearch();
        }

        private void KBNumSearch()
        {
            if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
            {
                FilterAppBarToggleButton.Label = "第" + KBNumFlyoutTextBox.Text + "周";
                appSetting.Values["FreeWeek"] = KBNumFlyoutTextBox.Text;
                KBNumFlyout.Hide();
            }
            else
                Utils.Message("请输入正确的周次");
        }

        private void FilterAppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            KBNumFlyout.ShowAt(commandBar);
        }

        private void KBNumFlyoutTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("enter");
                if (KBNumFlyoutTextBox.Text != "")
                    KBNumSearch();
                else
                {
                    Utils.Message("信息不完全");
                }
            }
        }

        private void AddTextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("enter");
                if (AddTextBox.Text != "")
                    mAddButton();
                //else
                //{
                //    Utils.Message("信息不完全");
                //}
            }
        }
    }
}
