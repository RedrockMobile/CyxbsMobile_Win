using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ElectricityPage : Page
    {
        private ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        private ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;

        public ElectricityPage()
        {
            this.InitializeComponent();
            frame.Navigate(typeof(Page));
            if (!settings.Values.ContainsKey("isSettingRoom"))
            {
                settings.Values["isSettingRoom"] = false;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IniData();
        }

        public async void IniData()//使用学号查询电费接口初始化数据
        {
            if (bool.Parse(settings.Values["isSettingRoom"].ToString()))
            {
                try
                {
                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("building", settings.Values["building"].ToString());
                    param.Add("room", settings.Values["room"].ToString());
                    Newtonsoft.Json.Linq.JObject electricityObj = await Util.Requests.Send("magipoke-elecquery/getElectric", param: param, method: "post", json: false);
                    ElectricityByRoomNum electricityData = new ElectricityByRoomNum();
                    electricityData = Newtonsoft.Json.JsonConvert.DeserializeObject<ElectricityByRoomNum>(electricityObj.ToString());
                    //本月电费消费
                    string elec_Cost = electricityData.elec_inf.elec_cost[0] + "." + electricityData.elec_inf.elec_cost[1];//Cost数组转为String
                    electricityData.elec_inf.elec_perday = elec_Cost;
                    //百分比
                    if (!settings.Values.ContainsKey("limitCharge"))
                    {
                        settings.Values["limitCharge"] = 20;
                    }
                    electricityData.elec_inf.elec_percent = (double.Parse(elec_Cost) / double.Parse(settings.Values["limitCharge"].ToString()) * 100);
                    dialPlate.Percent = electricityData.elec_inf.elec_percent;
                    //电费消费
                    electricityData.elec_inf.elec_chargeBalance = (int.Parse(electricityData.elec_inf.elec_end) - int.Parse(electricityData.elec_inf.elec_start)).ToString();
                    this.dialPlate.BalanceProperty = elec_Cost;
                    //电量用量
                    this.dialPlate.DumpEnergyProperty = electricityData.elec_inf.elec_spend;
                    //设置数据源
                    this.DataContext = electricityData.elec_inf;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void OnNavigateToPage(object sender, RoutedEventArgs e)
        {
            var temp = sender as MenuFlyoutItem;
            if (temp.Name == "SetRemain")
                this.frame.Navigate(typeof(SetRemainPage), e);
            else if (temp.Name == "SetRoom")
            {
                this.frame.Navigate(typeof(SetRoomPage));
            }
            //this.frame.Navigate(typeof(CheckRecentChargePage), e);
            frame.Visibility = Visibility.Visible;
        }
    }
}