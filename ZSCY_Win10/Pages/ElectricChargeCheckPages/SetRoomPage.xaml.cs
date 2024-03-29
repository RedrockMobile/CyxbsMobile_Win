﻿using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.ElectricChargeCheckPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SetRoomPage : Page
    {
        private ApplicationDataContainer roomSettings = ApplicationData.Current.LocalSettings;

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

                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("building", roomSettings.Values["building"].ToString());
                param.Add("room", roomSettings.Values["room"].ToString());
                Newtonsoft.Json.Linq.JObject electricityObj = await Util.Requests.Send("magipoke-elecquery/getElectric", param: param, method: "post", json: false);
                electricityData = Newtonsoft.Json.JsonConvert.DeserializeObject<ElectricityByRoomNum>(electricityObj.ToString());
            }
            //输入错误弹窗
            if (ComboBox.SelectedItem == null || RoomTextBox.Text.ToString().Length != 3 || electricityData.elec_inf.elec_spend == null)
            {
                var msgPopup = new MessagePopup(); //MessagePop构造方法可传string型参数作为弹窗的提示
                msgPopup.ShowWindow();
            }
            else
            {
                roomSettings.Values["isSettingRoom"] = true;
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