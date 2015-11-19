using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ZSCY.Util;

// “空白页"项模板在 http://go.microsoft.com/fwlink/?LinkID=390556 上有介绍

namespace ZSCY
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private ApplicationDataContainer appSetting;
        public LoginPage()
        {
            this.InitializeComponent();
            appSetting = ApplicationData.Current.LocalSettings;
            if (appSetting.Values.ContainsKey("stuNum"))
                StuNumTextBox.Text = appSetting.Values["stuNum"].ToString();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageStart("LoginPage");
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;//注册重写后退按钮事件
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("LoginPage");
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;//注册重写后退按钮事件
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)//重写后退按钮，如果要对所有页面使用，可以放在App.Xaml.cs的APP初始化函数中重写。
        {
            e.Handled = true;
            Application.Current.Exit();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginProgressBar.Visibility = Visibility.Visible;
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", StuNumTextBox.Text));
            paramList.Add(new KeyValuePair<string, string>("idNum", IdNumPasswordBox.Password));
            string login = await NetWork.getHttpWebRequest("api/verify", paramList);
            Debug.WriteLine("login->" + login);
            if (login != "")
            {
                try
                {
                    JObject obj = JObject.Parse(login);
                    if (Int32.Parse(obj["status"].ToString()) == 200)
                    {
                        appSetting.Values["stuNum"] = StuNumTextBox.Text;
                        appSetting.Values["idNum"] = IdNumPasswordBox.Password;
                        JObject dataobj = JObject.Parse(obj["data"].ToString());
                        appSetting.Values["name"] = dataobj["name"].ToString();
                        appSetting.Values["classNum"] = dataobj["classNum"].ToString();
                        appSetting.Values["gender"] = dataobj["gender"].ToString();
                        appSetting.Values["major"] = dataobj["major"].ToString();
                        appSetting.Values["college"] = dataobj["college"].ToString();
                        Frame.Navigate(typeof(MainPage));
                    }
                    else if (Int32.Parse(obj["status"].ToString()) == -100)
                        Utils.Message("学号不存在");
                    else if (Int32.Parse(obj["status"].ToString()) == 201)
                        Utils.Message("学号或密码错误");
                    else
                        Utils.Message(obj["info"].ToString());
                }
                catch (Exception)
                {
                    Debug.WriteLine("登陆->返回值解析异常");
                }
            }
            else
                Utils.Message("网络异常");

            LoginProgressBar.Visibility = Visibility.Collapsed;
        }


        private void StuNumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            isLoginButtonEnable();
        }

        private void IdNumPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            isLoginButtonEnable();

        }
        private void isLoginButtonEnable()
        {
            if (StuNumTextBox.Text != "" && IdNumPasswordBox.Password != "")
                LoginButton.IsEnabled = true;
            else
                LoginButton.IsEnabled = false;
        }
    }
}
