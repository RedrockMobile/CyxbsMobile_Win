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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetRoomPage : Page
    {
        private static string byRoomNumUri = "http://hongyan.cqupt.edu.cn/MagicLoop/index.php?s=/addon/ElectricityQuery/ElectricityQuery/queryElecByRoom";
        List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
        ApplicationDataContainer roomSettings = ApplicationData.Current.LocalSettings;
        NetWork netWork = new NetWork();
        public SetRoomPage()
        {
            this.InitializeComponent();
            if (roomSettings.Values.ContainsKey("building") && roomSettings.Values.ContainsKey("room"))
            {
                ComboBox.SelectedIndex = int.Parse(roomSettings.Values["building"].ToString());
                RoomTextBox.Text = roomSettings.Values["room"].ToString();
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
            //this.Frame.Visibility = Visibility.Collapsed;
        }

        private async void RoomButton_Click(object sender, RoutedEventArgs e)
        {
            ElectricityByRoomNum electricityData = new ElectricityByRoomNum();
            if (roomSettings.Values.ContainsKey("building") && roomSettings.Values.ContainsKey("room"))
            {
                paramList.Add(new KeyValuePair<string, string>("building", roomSettings.Values["building"].ToString()));
                paramList.Add(new KeyValuePair<string, string>("room", roomSettings.Values["room"].ToString()));
                //储存ElectricityByStuNum数据的实例
                string electricityJson = await netWork.GetElectricityByRoomNum(byRoomNumUri, paramList);
                electricityData = netWork.ByRoomNumStringConvertToModel(electricityJson);
            }
            //输入错误弹窗
            if (ComboBox.SelectedItem == null || RoomTextBox.Text.ToString().Length != 3 || electricityData.elec_inf.elec_spend == null)
            {
                var msgPopup = new MessagePopup(); //MessagePop构造方法可传string型参数作为弹窗的提示
                msgPopup.ShowWindow();              
            }
            else
            {
                roomSettings.Values["isBindingRoom"] = true;
                roomSettings.Values["BindingRoomDate"] = DateTime.Now.ToString();
                this.Frame.Navigate(typeof(SettedPage)); //设置成功之后跳转的成功画面 @Boss Qin检查下
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            roomSettings.Values["building"] = ComboBox.SelectedIndex;
        }

        private void RoomTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            roomSettings.Values["room"] = RoomTextBox.Text;
        }
    }
}
