using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Util;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.LostAndFoundPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InfoPublish : Page
    {
        bool islost = true;
        string temptype;
        private static string resourceName = "ZSCY";

        List<TempItemClass> tic = new List<TempItemClass>() {
            new TempItemClass { type="一卡通",IconV=Visibility.Collapsed},
            new TempItemClass { type="钱包",IconV=Visibility.Collapsed},
            new TempItemClass { type="电子产品",IconV=Visibility.Collapsed},
            new TempItemClass { type="书包",IconV=Visibility.Collapsed},
            new TempItemClass { type="钥匙",IconV=Visibility.Collapsed},
            new TempItemClass { type="雨伞",IconV=Visibility.Collapsed},
            new TempItemClass { type="衣物",IconV=Visibility.Collapsed},
            new TempItemClass { type="其他",IconV=Visibility.Collapsed}
        };

        public InfoPublish()
        {
            this.InitializeComponent();
            SelRemindListView.ItemsSource = tic;
        }

        private void cancel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void RemindGridButon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }
        private void SelRemindBackgroupGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Collapsed;
        }
        private void e1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            islost = true;
            e2.Fill = new SolidColorBrush(Color.FromArgb(255, 217, 217, 217));
            e1.Fill = new SolidColorBrush(Color.FromArgb(255, 65, 165, 255));
        }
        private void e2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            islost = false;
            e1.Fill = new SolidColorBrush(Color.FromArgb(255, 217, 217, 217));
            e2.Fill = new SolidColorBrush(Color.FromArgb(255, 65, 165, 255));
        }

        private void SelRemindListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            temptype = ((TempItemClass)sender).type;
            Debug.WriteLine(temptype);
        }
        public class TempItemClass
        {
            public string type { get; set; }
            public Visibility IconV { get; set; }
        }

        private void SelRemindListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            temptype = ((TempItemClass)e.ClickedItem).type;
            Debug.WriteLine(temptype);
            SelRemindGrid.Visibility = Visibility.Collapsed;
            typeTb.Text = temptype;
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            SelRemindGrid.Visibility = Visibility.Visible;
        }

        private void DatePickerFlyout_DatePicked(DatePickerFlyout sender, DatePickedEventArgs args)
        {
            timebox.Text = args.NewDate.UtcDateTime.ToString().Substring(0, args.NewDate.UtcDateTime.ToString().Length-7);
        }

        private async void publishBtn_Click(object sender, RoutedEventArgs e)
        {
            if (typeTb.Text != "" && DescribeBox.Text != "" && timebox.Text != "" && addressBox.Text != "")
            {
                if (telBox.Text == "" && qqbox.Text == "")
                    Utils.Message("请至少输入一个联系方式");
                else
                {
                    var vault = new Windows.Security.Credentials.PasswordVault();
                    var credentialList = vault.FindAllByResource(resourceName);
                    //TODO:post
                    string property = "";
                    if (islost)
                        property = "寻物启事";
                    else
                        property = "失物招领";
                    List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
                    paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
                    paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
                    paramList.Add(new KeyValuePair<string, string>("property", property));
                    paramList.Add(new KeyValuePair<string, string>("category", typeTb.Text));
                    paramList.Add(new KeyValuePair<string, string>("detail", DescribeBox.Text));
                    paramList.Add(new KeyValuePair<string, string>("pickTime", timebox.Text));
                    paramList.Add(new KeyValuePair<string, string>("place", addressBox.Text));
                    if (telBox.Text == "")
                        paramList.Add(new KeyValuePair<string, string>("qq", qqbox.Text));
                    else if (qqbox.Text == "")
                        paramList.Add(new KeyValuePair<string, string>("phone", telBox.Text));
                    else
                    {
                        paramList.Add(new KeyValuePair<string, string>("phone", telBox.Text));
                        paramList.Add(new KeyValuePair<string, string>("qq", qqbox.Text));
                    }
                    string postup = await NetWork.getHttpWebRequest("laf/api/create", paramList);
                    if (postup != "")
                    {
                        JObject obj = JObject.Parse(postup);
                        if (obj["state"].ToString() == "成功添加失物招领信息")
                        {
                            Utils.Message("发表成功 青协审核后将发布在失物招领中~");
                        }
                        else
                            Utils.Message(obj["state"].ToString());
                    }
                }
            }
            else
                Utils.Message("请完善信息");
        }
    }
}
