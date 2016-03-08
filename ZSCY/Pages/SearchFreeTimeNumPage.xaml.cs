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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Data;
using ZSCY.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SearchFreeTimeNumPage : Page
    {
        private ApplicationDataContainer appSetting;
        public SearchFreeTimeNumPage()
        {
            appSetting = ApplicationData.Current.LocalSettings; //本地存储
            this.InitializeComponent();
            //HubSectionKBNum.Text = appSetting.Values["nowWeek"].ToString();
            appSetting.Values["FreeWeek"] = appSetting.Values["nowWeek"];
            if (App.muIdList.Count == 0)
                App.muIdList.Add(new uIdList { uId = appSetting.Values["stuNum"].ToString(), uName = appSetting.Values["name"].ToString() });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
            uIdListView.ItemsSource = App.muIdList;
        }


        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
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

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var muIDArray = App.muIdList.ToArray().ToList();
            AddButton.IsEnabled = false;
            AddProgressRing.IsActive = true;
            //if (AddTextBox.Text.Length != 10)
            //{
            //    Utils.Message("学号不正确");
            //}
            if (muIDArray.Find(p => p.uId.Equals(AddTextBox.Text)) != null)
                Utils.Message("此学号已添加");
            else
            {
                //for (int i = 0; i < 15; i++)
                string usename = AddTextBox.Text;
                string useid = usename;
                string peopleinfo = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/home/searchPeople/peopleList?stu=" + useid, PostORGet: 1);
                Debug.WriteLine("peopleinfo->" + peopleinfo);
                if (peopleinfo != "")
                {
                    try
                    {
                        JObject obj = JObject.Parse(peopleinfo);
                        if (Int32.Parse(obj["state"].ToString()) == 200)
                        {
                            JArray PeopleListArray = Utils.ReadJso(peopleinfo);
                            if (PeopleListArray.Count != 1)
                            {
                                MenuFlyout PeopleListMenuFlyout = new MenuFlyout();
                                for (int i = 0; i < PeopleListArray.Count; i++)
                                {
                                    PersonalIno Personalitem = new PersonalIno();
                                    Personalitem.GetAttribute((JObject)PeopleListArray[i]);
                                    PeopleListMenuFlyout.Items.Add(getPeopleListMenuFlyoutItem(Personalitem.Name + "-" + Personalitem.Major + "-" + Personalitem.Stunum));
                                }
                                PeopleListMenuFlyout.ShowAt(AddTextBox);

                            }
                            else
                            {
                                PersonalIno Personalitem = new PersonalIno();
                                Personalitem.GetAttribute((JObject)PeopleListArray[0]);
                                if (muIDArray.Find(p => p.uId.Equals(Personalitem.Stunum)) != null)
                                    Utils.Message("此学号已添加");
                                else
                                {
                                    if (Personalitem.Stunum == AddTextBox.Text || Personalitem.Name == AddTextBox.Text)
                                        App.muIdList.Add(new uIdList { uId = Personalitem.Stunum, uName = Personalitem.Name });
                                    else
                                    {
                                        MenuFlyout PeopleListMenuFlyout = new MenuFlyout();
                                        PeopleListMenuFlyout.Items.Add(getPeopleListMenuFlyoutItem(Personalitem.Name + "-" + Personalitem.Major + "-" + Personalitem.Stunum));
                                        PeopleListMenuFlyout.ShowAt(AddTextBox);
                                    }
                                }
                            }
                            //JObject dataobj = JObject.Parse(obj["data"].ToString());
                        }
                        else
                            Utils.Message("学号或姓名不正确");
                    }
                    catch (Exception) { }

                }
                AddTextBox.Text = "";

            }
            AddButton.IsEnabled = true;
            AddProgressRing.IsActive = false;
        }

        private MenuFlyoutItem getPeopleListMenuFlyoutItem(string text)
        {
            MenuFlyoutItem menuFlyoutItem = new MenuFlyoutItem();
            menuFlyoutItem.Text = text;
            menuFlyoutItem.Click += PeopleListMenuFlyoutItem_click;
            return menuFlyoutItem;
        }

        private void PeopleListMenuFlyoutItem_click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            string menuFlyoutItemText = menuFlyoutItem.Text;
            string menuFlyoutItemname = menuFlyoutItemText.Substring(0, menuFlyoutItemText.IndexOf("-"));
            string menuFlyoutItemnum = menuFlyoutItemText.Substring(menuFlyoutItem.Text.Length - 10);
            var muIDArray = App.muIdList.ToArray().ToList();
            if (muIDArray.Find(p => p.uId.Equals(menuFlyoutItemnum)) != null)
                Utils.Message("此学号已添加");
            else
                App.muIdList.Add(new uIdList { uId = menuFlyoutItemnum, uName = menuFlyoutItemname });

            //AddDateCostTextBox.Text = menuFlyoutItem.Text;
            //switch (menuFlyoutItem.Text)
            //{
            //    case "AA":
            //        cost_model = 1;
            //        break;
            //    case "你请客":
            //        cost_model = 2;
            //        break;
            //    case "我买单":
            //        cost_model = 3;
            //        break;
            //}
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
                var muIDArray = App.muIdList.ToArray().ToList();
                uIdList u = muIDArray.Find(p => p.uId.Equals(((uIdList)e.ClickedItem).uId));
                App.muIdList.Remove(u);
            }
        }

        private void ForwardAppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.muIdList.Count < 2)
            {
                Utils.Message("请至少输入2个要查询学号");
            }
            else
            {
                AuIdList Au = new AuIdList { muIdList = App.muIdList };
                Frame.Navigate(typeof(SearchFreeTimeResultPage_new), Au);
            }
        }

        //private void HubSectionKBNum_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    KBNumFlyout.ShowAt(page);
        //    HubSectionKBNum.SelectAll();
        //}
        //private void KBNumSearchButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (KBNumFlyoutTextBox.Text != "" && KBNumFlyoutTextBox.Text.IndexOf(".") == -1)
        //    {
        //        HubSectionKBNum.Text = KBNumFlyoutTextBox.Text;
        //        appSetting.Values["FreeWeek"] = KBNumFlyoutTextBox.Text;
        //        KBNumFlyout.Hide();
        //    }
        //    else
        //        Utils.Message("请输入正确的周次");
        //}

        private async void DeleteAppBarToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var dig = new MessageDialog("确定删除所有数据", "警告");
            var btnOk = new UICommand("是");
            dig.Commands.Add(btnOk);
            var btnCancel = new UICommand("否");
            dig.Commands.Add(btnCancel);
            var result = await dig.ShowAsync();
            if (null != result && result.Label == "是")
            {
                App.muIdList.Clear();
            }
        }
    }
}
