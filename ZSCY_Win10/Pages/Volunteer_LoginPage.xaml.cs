using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Volunteer_LoginPage : Page
    {
        public Volunteer_LoginPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (s, e) =>
              {
                  viewModel.ElementWidth1 = (int)e.NewSize.Width;
              };
        }
        ViewModels.VolunteerPageViewModel viewModel = new ViewModels.VolunteerPageViewModel();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.DataContext = viewModel;

        }

        private async void login_Button_Click(object sender, RoutedEventArgs e)
        {
            user_name.IsEnabled = false;
            user_password.IsEnabled = false;
            loading_progress.IsActive = true;
            login_Button.Visibility = Visibility.Collapsed;

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("account", user_name.Text);
            param.Add("password", user_password.Password);

            Newtonsoft.Json.Linq.JObject ret = await Util.Requests.Send("volunteer-message/binding", param: param, method: "post", token: true, check: false, json: false);

            if (int.Parse(ret["code"].ToString()) == 0)
            {
                this.Frame.Navigate((typeof(VolunteerPage)));
            }
            if (viewModel.Rootobject.code == 001)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "账号或者密码错误";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            if (viewModel.Rootobject.code == 002)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "帐号不存在";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            if (viewModel.Rootobject.code == 003)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "请输入密码";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            if (viewModel.Rootobject.code == 004)
            {
                var dialog = new ContentDialog();
                dialog.Title = "error";
                dialog.Content = "请输入帐号";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {

                };
                await dialog.ShowAsync();
            }
            user_name.IsEnabled = true;
            user_password.IsEnabled = true;
            loading_progress.IsActive = false;
            login_Button.Visibility = Visibility.Visible;

        }


    }
}
