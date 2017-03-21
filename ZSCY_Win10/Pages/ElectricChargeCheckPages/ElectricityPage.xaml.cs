using System;
using System.Collections.Generic;
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
        private static string byStuNumUri = "http://hongyan.cqupt.edu.cn/MagicLoop/index.php?s=/addon/ElectricityQuery/ElectricityQuery/getElectric";
        private static string byRoomNumUri = "http://hongyan.cqupt.edu.cn/MagicLoop/index.php?s=/addon/ElectricityQuery/ElectricityQuery/queryElecByRoom";
        List<KeyValuePair<string, string>> paramIniList = new List<KeyValuePair<string, string>>();
        List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
        private static string stuNum = "";
        private static string idNum = "";
        private static string resourceName = "ZSCY";
        ApplicationDataContainer roomSettings = ApplicationData.Current.LocalSettings;
        NetWork netWork = new NetWork();
        public ElectricityPage()
        {
            this.InitializeComponent();
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            stuNum = credentialList[0].UserName;
            idNum = credentialList[0].Password;
            paramIniList.Add(new KeyValuePair<string, string>("stuNum", stuNum));
            IniData();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (roomSettings.Values.ContainsKey("building") && roomSettings.Values.ContainsKey("room"))
            {
                //building.Text = roomSettings.Values["building"].ToString();
                //room.Text = roomSettings.Values["room"].ToString();
            }
        }

        private async void IniData()
        {
            try
            {
                string electricityIniJson = await netWork.GetElectricityByStuNum(byStuNumUri, paramIniList);
                ElectricityByStuNum electricityIniData = new ElectricityByStuNum();
                electricityIniData = netWork.ByStuNumStringConvertToModel(electricityIniJson);
                string elec_Cost = electricityIniData.data.result.current.elec_cost[0] + "." + electricityIniData.data.result.current.elec_cost[1];//Cost数组转为String
                electricityIniData.data.result.current.elec_perday = elec_Cost;
            }
            catch
            {
                //StuNum电费查询异常处理
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //paramList.Add(new KeyValuePair<string, string>("building", building.Text));
                //paramList.Add(new KeyValuePair<string, string>("room", room.Text));
                string electricityJson = await netWork.GetElectricityByRoomNum(byRoomNumUri, paramList);
                ElectricityByRoomNum electricityData = new ElectricityByRoomNum();
                electricityData = netWork.ByRoomNumStringConvertToModel(electricityJson);
                string elec_Cost = electricityData.elec_inf.elec_cost[0] + "." + electricityData.elec_inf.elec_cost[1];//Cost数组转为String
                electricityData.elec_inf.elec_perday = elec_Cost;
            }
            catch
            {
                //roomnum电费查询异常处理
            }
        }

        private string GetTimeStamp()
        {
            TimeSpan timeSpan = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);//获取1970/1/1到现在的时间间隔
            return Convert.ToInt64(timeSpan.TotalSeconds).ToString();//时间间隔转换为Unix时间戳
        }
    }
}
