using Newtonsoft.Json.Linq;
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
using ZSCY_Win10.Util;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ZSCY_Win10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        public LoginPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
              {
                  var state = "VisualState000";
                  if (e.NewSize.Width > 600)
                      state = "VisualState600";
                  VisualStateManager.GoToState(this, state, true);
              };
        }

        private  void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mlogin();
           
        }

        private async void mlogin()
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

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Debug.WriteLine("enter");
                if (StuNumTextBox.Text != "" && IdNumPasswordBox.Password != "")
                    mlogin();
                else
                    Utils.Message("信息不完全");
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageStart("LoginPage");
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("LoginPage");
        }
    }
}
