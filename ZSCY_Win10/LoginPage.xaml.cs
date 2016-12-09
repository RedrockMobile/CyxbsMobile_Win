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
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.ViewManagement;
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
            List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();
            paramList.Add(new KeyValuePair<string, string>("stuNum", StuNumTextBox.Text));
            paramList.Add(new KeyValuePair<string, string>("idNum", System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(IdNumPasswordBox.Password))));
            //paramList.Add(new KeyValuePair<string, string>("idNum", IdNumPasswordBox.Password));
            string login = await NetWork.getHttpWebRequest("cyxbsMobile/index.php/Home/Verify/verifyLogin", paramList);
            Debug.WriteLine("login->" + login);
            if (login != "")
            {
                try
                {
                    JObject obj = JObject.Parse(login);
                    if (Int32.Parse(obj["status"].ToString()) == 200)
                    {
                        vault.Add(new Windows.Security.Credentials.PasswordCredential(resourceName, StuNumTextBox.Text, IdNumPasswordBox.Password));
                        //appSetting.Values["stuNum"] = StuNumTextBox.Text;
                        //appSetting.Values["idNum"] = IdNumPasswordBox.Password;
                        JObject dataobj = JObject.Parse(obj["data"].ToString());
                        appSetting.Values["name"] = dataobj["name"].ToString();
                        appSetting.Values["classNum"] = dataobj["classNum"].ToString();
                        appSetting.Values["gender"] = dataobj["gender"].ToString();
                        appSetting.Values["major"] = dataobj["major"].ToString();
                        appSetting.Values["college"] = dataobj["college"].ToString();
                        appSetting.Values["CommunityPerInfo"] = false;
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
                    else if (Int32.Parse(obj["status"].ToString()) == -100)
                    {
                        Utils.Message("学号不存在");
                        noLoginButton.Visibility = Visibility.Visible;
                    }
                    else if (Int32.Parse(obj["status"].ToString()) == 201)
                    {
                        Utils.Message("学号或密码错误");
                        noLoginButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        Utils.Message(obj["info"].ToString());
                        noLoginButton.Visibility = Visibility.Visible;
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("登陆->返回值解析异常");
                }
            }
            else
                Utils.Message("网络异常");
            LoginButton.Visibility = Visibility.Visible;
            LoginProgressBar.IsActive = false;
            StuNumTextBox.IsEnabled = true;
            IdNumPasswordBox.IsEnabled = true;
            // Debug.WriteLine(StuNumTextBox.FocusState);
            //StuNumTextBox.Focus(FocusState.Unfocused);
            // IdNumPasswordBox.Focus(FocusState.Pointer);
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
        {
            UmengSDK.UmengAnalytics.TrackPageStart("LoginPage");
        }

        //离开页面时，取消事件
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            UmengSDK.UmengAnalytics.TrackPageEnd("LoginPage");
        }

        private void noLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "/kb");
        }
    }
}
