using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SetPersonInfoPage : Page
    {
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        public SetPersonInfoPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
            {
                if (!App.showpane)
                {
                    SetPersonInfoTitleGrid.Margin = new Thickness(48, 0, 0, 0);
                }
                else
                {
                    SetPersonInfoTitleGrid.Margin = new Thickness(0);
                }
            };
        }
        private static string resourceName = "ZSCY";
        //昵称
        string nametext;
        //简介
        string abstracttext;
        //手机
        string phonetext;
        //QQ
        string qqtext;
        bool completeness1 = true;
        bool completeness2 = true;
        NavigationEventArgs ee;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ee = e;
            if (ee.Parameter.GetType() == (new PeoInfo()).GetType())
            {
                PeoInfo peoinfo = (PeoInfo)ee.Parameter;
                nameTextBox.Text = peoinfo.nickname;
                abstractTextBox.Text = peoinfo.introduction;
                qqTextBox.Text = peoinfo.qq;
                phoneTextBox.Text = peoinfo.phone;
                abstractNumTextBlock.Text = abstractTextBox.Text.Length + "/30";
                SetPersonInfoOKAppBarButton.IsEnabled = true;
            }

            if (this.Frame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += App_BackRequested;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            }
        }

        private void App_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                e.Handled = true;
                this.Frame.GoBack();
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                SystemNavigationManager.GetForCurrentView().BackRequested -= App_BackRequested;
            }
        }

        private async void SetPersonInfoOKAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            appSetting.Values["Community_nickname"] = nameTextBox.Text;
            var vault = new Windows.Security.Credentials.PasswordVault();
            var credentialList = vault.FindAllByResource(resourceName);
            credentialList[0].RetrievePassword();
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            //paramList.Add(new KeyValuePair<string, string>("stuNum", appSetting.Values["stuNum"].ToString()));
            //paramList.Add(new KeyValuePair<string, string>("idNum", appSetting.Values["idNum"].ToString()));
            //paramList.Add(new KeyValuePair<string, string>("stuuum", appSetting.Values["stuNum"].ToString()));
            paramList.Add(new KeyValuePair<string, string>("stuNum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("idNum", credentialList[0].Password));
            paramList.Add(new KeyValuePair<string, string>("stuuum", credentialList[0].UserName));
            paramList.Add(new KeyValuePair<string, string>("nickname", nameTextBox.Text));
            paramList.Add(new KeyValuePair<string, string>("introduction", abstractTextBox.Text));
            paramList.Add(new KeyValuePair<string, string>("qq", qqTextBox.Text));
            paramList.Add(new KeyValuePair<string, string>("phone", phoneTextBox.Text));
            string setinfo = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Person/setInfo", paramList);
            try
            {
                if (setinfo != "")
                {
                    JObject obj = JObject.Parse(setinfo);
                    if (Int32.Parse(obj["status"].ToString()) == 200)
                    {
                        string perInfo = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Person/search", paramList);
                        if (perInfo != "")
                        {
                            JObject jPerInfo = JObject.Parse(perInfo);
                            appSetting.Values["Community_people_id"] = jPerInfo["data"]["id"].ToString();
                            Debug.WriteLine(jPerInfo["data"]["id"].ToString());
                        }
                        var navPage = ee.Parameter;
                        if (navPage == typeof(CommunityPage))
                        {
                            Utils.Toast("新建个人信息成功~~，尽情享用吧");
                            this.Frame.Navigate(typeof(CommunityPage));
                        }
                        else if (navPage == typeof(MyPage))
                        {
                            Utils.Toast("新建个人信息成功~~，尽情享用吧");
                            this.Frame.Navigate(typeof(MyPage));
                        }
                        else
                        {
                            Frame.GoBack();
                        }
                    }
                    else if (Int32.Parse(obj["status"].ToString()) == 801)
                    {
                        Utils.Toast("更新资料失败，请检查是否包含特殊词");
                    }
                }
            }
            catch (Exception) { }
        }

        //昵称
        private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //nametext = nameTextBox.Text;
            Completeness();
        }

        //简介
        private void abstractTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            abstracttext = abstractTextBox.Text;
            abstractNumTextBlock.Text = abstracttext.Length + "/30";
            Color abstractChangeNumcolor = Color.FromArgb(255, 255, 155, 155);
            Color abstractOriginalNumcolor = Color.FromArgb(255, 153, 153, 153);
            if (abstracttext.Length >= 25)
            {
                abstractNumTextBlock.Foreground = new SolidColorBrush(abstractChangeNumcolor);
            }
            else
                abstractNumTextBlock.Foreground = new SolidColorBrush(abstractOriginalNumcolor);

            Completeness();
        }

        //手机
        private void phoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            phonetext = phoneTextBox.Text;
            if ((phonetext == null) || (Regex.IsMatch(phonetext, @"^1\d{10}$")))
            {
                completeness1 = true;
            }
            else
                completeness1 = false;

            Completeness();
        }

        //QQ
        private void qqTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            qqtext = qqTextBox.Text;
            if ((qqtext == null) || (Regex.IsMatch(qqtext, @"^\d{5,10}$")))
            {
                completeness2 = true;
            }
            else
                completeness2 = false;

            Completeness();
        }

        //判定是否可以提交
        private void Completeness()
        {
            if (nameTextBox.Text != "" && abstractTextBox.Text != "" && completeness1 == true && completeness2 == true)
            {
                SetPersonInfoOKAppBarButton.IsEnabled = true;
            }
            else
                SetPersonInfoOKAppBarButton.IsEnabled = false;
            //信息展示
            //textBlocktwo.Text = "昵称：" + nametext + "简介：" + abstracttext + "手机：" + phonetext + "QQ：" + qqtext;
        }
    }
}
