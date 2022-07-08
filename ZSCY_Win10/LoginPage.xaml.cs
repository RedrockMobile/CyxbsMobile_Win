using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
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
        private ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;
        private static string resourceName = "ZSCY";

        public LoginPage()
        {
            this.InitializeComponent();
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Utils.ShowSystemTrayAsync(Color.FromArgb(255, 6, 140, 253), Colors.White);
            }
            else
            {
                var view = ApplicationView.GetForCurrentView();
                view.TitleBar.BackgroundColor = Color.FromArgb(255, 4, 131, 239);
                view.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 4, 131, 239);
                view.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(255, 2, 126, 231);
                view.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(255, 2, 111, 203);
            }
            this.SizeChanged += (s, e) =>
              {
                  var state = "VisualState000";
                  if (e.NewSize.Width > 600)
                      state = "VisualState600";
                  VisualStateManager.GoToState(this, state, true);
              };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            mlogin();
        }

        private async void mlogin()
        {
            var vault = new Windows.Security.Credentials.PasswordVault();
            StuNumTextBox.IsEnabled = false;
            IdNumPasswordBox.IsEnabled = false;
            LoginProgressBar.IsActive = true;
            this.Focus(FocusState.Pointer);
            LoginButton.Visibility = Visibility.Collapsed;
            noLoginButton.Visibility = Visibility.Collapsed;
            var loginForm = new Dictionary<string, string>();
            loginForm.Add("stuNum", StuNumTextBox.Text);
            loginForm.Add("idNum", IdNumPasswordBox.Password);
            JObject loginObj = await Requests.Send("magipoke/token", param: loginForm, method: "post");
            Debug.WriteLine("login->" + loginObj);
            if (loginObj != null)
            {
                try
                {
                    if (Int32.Parse(loginObj["status"].ToString()) == 10000)
                    {
                        vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, "refreshToken", loginObj["data"]["refreshToken"].ToString()));
                        vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, "token", loginObj["data"]["token"].ToString()));
                        JObject userObj = await Requests.Send("magipoke/Person/Search", method: "post", token: true);
                        appSetting.Values["uid"] = userObj["data"]["uid"].ToString();
                        appSetting.Values["redid"] = userObj["data"]["redid"].ToString();
                        appSetting.Values["stuNum"] = userObj["data"]["stunum"].ToString();
                        appSetting.Values["gender"] = userObj["data"]["gender"].ToString();
                        appSetting.Values["grade"] = userObj["data"]["grade"].ToString();
                        appSetting.Values["college"] = userObj["data"]["college"].ToString();
                        appSetting.Values["name"] = userObj["data"]["username"].ToString();
                        appSetting.Values["isLogin"] = true;
                        if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
                        {
                            if (JumpList.IsSupported())
                                SetSystemGroupAsync();
                            else if (JumpList.IsSupported())
                                DisableSystemJumpListAsync();
                        }
                        appSetting.Values["isUseingBackgroundTask"] = true;
                        Frame.Navigate(typeof(MainPage), "/kb");
                    }
                    else if (Int32.Parse(loginObj["status"].ToString()) == -100)
                    {
                        Utils.Message("学号不存在");
                        noLoginButton.Visibility = Visibility.Visible;
                    }
                    else if (Int32.Parse(loginObj["status"].ToString()) == 201)
                    {
                        Utils.Message("学号或密码错误");
                        noLoginButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Utils.Message(loginObj["info"].ToString());
                        noLoginButton.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("登录->返回值解析异常");
                }
            }
            else
                Utils.Message("网络异常");
            LoginButton.Visibility = Visibility.Visible;
            LoginProgressBar.IsActive = false;
            StuNumTextBox.IsEnabled = true;
            IdNumPasswordBox.IsEnabled = true;
        }

        private async void DisableSystemJumpListAsync()
        {
            var jumpList = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();
            jumpList.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.None;
            jumpList.Items.Clear();
            await jumpList.SaveAsync();
        }

        private Windows.UI.StartScreen.JumpListItem CreateJumpListItemTask(string u, string description, string uri)
        {
            var taskItem = JumpListItem.CreateWithArguments(
                                    u, description);
            taskItem.Description = description;
            taskItem.Logo = new Uri(uri);
            return taskItem;
        }

        private async void SetSystemGroupAsync()
        {
            var jumpList = await Windows.UI.StartScreen.JumpList.LoadCurrentAsync();
            jumpList.SystemGroupKind = Windows.UI.StartScreen.JumpListSystemGroupKind.Frequent;
            jumpList.Items.Clear();
            jumpList.Items.Add(CreateJumpListItemTask("/jwzx", "教务信息", "ms-appx:///Assets/iconfont-news_w.png"));
            jumpList.Items.Add(CreateJumpListItemTask("/more", "更多", "ms-appx:///Assets/iconfont-more_w.png"));
            await jumpList.SaveAsync();
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
        { }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        { }

        private void noLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "/kb");
        }
    }
}