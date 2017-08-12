using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Models;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class ElectricityPage : Page
    {
        ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
        private static string byStuNumUri = "http://hongyan.cqupt.edu.cn/MagicLoop/index.php?s=/addon/ElectricityQuery/ElectricityQuery/getElectric";
        private static string byRoomNumUri = "http://hongyan.cqupt.edu.cn/MagicLoop/index.php?s=/addon/ElectricityQuery/ElectricityQuery/queryElecByRoom";
        List<KeyValuePair<string, string>> paramIniList = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
        private static string resourceName = "ZSCY";
        private static string stuNum = "";
        NetWork netWork = new NetWork();
        public ElectricityPage()
        {
            this.InitializeComponent();
            frame.Navigate(typeof(Page));
            //获取stuNum参数
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            stuNum = credentialList[0].UserName;
            paramIniList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
            if (!settings.Values.ContainsKey("isBindingRoom"))
            {
                settings.Values["isBindingRoom"] = false;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IniData();
        }

        public async void IniData()//使用学号查询电费接口初始化数据
        {
            if (bool.Parse(settings.Values["isBindingRoom"].ToString()))
            {
                try
                {
                    paramList.Add(new KeyValuePair<string, string>("building", settings.Values["building"].ToString()));
                    paramList.Add(new KeyValuePair<string, string>("room", settings.Values["room"].ToString()));
                    //储存ElectricityByStuNum数据的实例
                    string electricityJson = await netWork.GetElectricityByRoomNum(byRoomNumUri, paramList);
                    ElectricityByRoomNum electricityData = new ElectricityByRoomNum();
                    electricityData = netWork.ByRoomNumStringConvertToModel(electricityJson);
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
                    //电费余额
                    electricityData.elec_inf.elec_chargeBalance = (int.Parse(electricityData.elec_inf.elec_end) - int.Parse(electricityData.elec_inf.elec_start)).ToString();
                    this.dialPlate.BalanceProperty = electricityData.elec_inf.elec_chargeBalance;
                    //电量剩余度数
                    electricityData.elec_inf.elec_dumpEnergy = (30 - (int.Parse(electricityData.elec_inf.elec_end) - int.Parse(electricityData.elec_inf.elec_start))).ToString();
                    this.dialPlate.DumpEnergyProperty = electricityData.elec_inf.elec_dumpEnergy;
                    //设置数据源
                    this.DataContext = electricityData.elec_inf;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    //储存ElectricityByStuNum数据的实例
                    string electricityIniJson = await netWork.GetElectricityByStuNum(byStuNumUri, paramIniList);
                    ElectricityByStuNum electricityIniData = new ElectricityByStuNum();
                    electricityIniData = netWork.ByStuNumStringConvertToModel(electricityIniJson);
                    //本月电费消费
                    string elec_Cost = electricityIniData.data.result.current.elec_cost[0] + "." + electricityIniData.data.result.current.elec_cost[1];//Cost数组转为String
                    electricityIniData.data.result.current.elec_perday = elec_Cost;
                    //百分比
                    if (!settings.Values.ContainsKey("limitCharge"))
                    {
                        settings.Values["limitCharge"] = 20;
                    }
                    electricityIniData.data.result.current.elec_percent = (double.Parse(elec_Cost) / double.Parse(settings.Values["limitCharge"].ToString()) * 100);
                    dialPlate.Percent = electricityIniData.data.result.current.elec_percent;
                    //电费余额
                    electricityIniData.data.result.current.elec_chargeBalance = (int.Parse(electricityIniData.data.result.current.elec_end) - int.Parse(electricityIniData.data.result.current.elec_start)).ToString();
                    this.dialPlate.BalanceProperty = electricityIniData.data.result.current.elec_chargeBalance;
                    //电量剩余度数
                    electricityIniData.data.result.current.elec_dumpEnergy = (30 - (int.Parse(electricityIniData.data.result.current.elec_end) - int.Parse(electricityIniData.data.result.current.elec_start))).ToString();
                    this.dialPlate.DumpEnergyProperty = electricityIniData.data.result.current.elec_dumpEnergy;
                    //设置数据源
                    this.DataContext = electricityIniData.data.result.current;
                    Debug.WriteLine(this.dialPlate.DumpEnergyProperty);
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
                if (!settings.Values.ContainsKey("bindingRoomDate"))
                {
                    settings.Values["bindingRoomDate"] = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToString();
                }
                TimeSpan timeSpan = DateTime.Now - DateTime.Parse(settings.Values["bindingRoomDate"].ToString());
                if (bool.Parse(settings.Values["isBindingRoom"].ToString()) && timeSpan.TotalDays >= 30)
                {
                    this.frame.Navigate(typeof(SettedPage), true);          //设置过寝室且距离上次设置时间超过30天，进入可再设置页面
                }
                else if(bool.Parse(settings.Values["isBindingRoom"].ToString()) && timeSpan.TotalDays < 30)
                {
                    this.frame.Navigate(typeof(SettedPage), false);         //设置过寝室且距离上次设置时间小于30天，进入不可再设置页面
                }
                else
                {
                    this.frame.Navigate(typeof(SetRoomPage));               //从未设置过寝室，直接进入设置寝室页面
                }
            }
            else
            {
                var msgPopup = new MessagePopup("即将上线，敬请期待");
                msgPopup.ShowWindow();
            }
                //this.frame.Navigate(typeof(CheckRecentChargePage), e);
            frame.Visibility = Visibility.Visible;
        }
    }
}
