using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using XamlAnimatedGif;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ZSCY_Win10.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class VolunteerPage : Page
    {
        public VolunteerPage()
        {
            this.InitializeComponent();
            this.SizeChanged += (_s, _e) =>
            {

                //Debug.WriteLine(_e.NewSize.Width);
                viewModel.ElementHeight = (int)_e.NewSize.Height;
                viewModel.ElementWidth = (int)_e.NewSize.Width;
            };

        }

        ViewModels.VolunteerPageViewModel viewModel = new ViewModels.VolunteerPageViewModel();
        private static string resourceName = "ZSCY_Volunteer";
        private int pivot_index;
        private async void Confirm_login()
        {

            try
            {
                volunteerPage_ProgressRing.Visibility = Visibility.Visible;
                volunteerPage_Grid.Visibility = Visibility.Collapsed;
                GetAsync();
                volunteerPage_ProgressRing.Visibility = Visibility.Collapsed;
                volunteerPage_Grid.Visibility = Visibility.Visible;
                viewModel.Line_Y1 = viewModel.Record_year1.Count * nub;
                viewModel.Line_Y2 = viewModel.Record_year2.Count * nub;
                viewModel.Line_Y3 = viewModel.Record_year3.Count * nub;
                viewModel.Line_Y4 = viewModel.Record_year4.Count * nub;
                viewModel.Line_Y5 = viewModel.Record_year5.Count * nub;
                viewModel.Line_Y6 = viewModel.Record_year6.Count * nub;
                viewModel.Line_Y7 = viewModel.Record_year7.Count * nub;
            }
            catch (Exception)
            {

                var dialog = new ContentDialog();
                dialog.Title = " ";
                dialog.Content = "同学，请先绑定帐号哦";
                dialog.PrimaryButtonText = "确定";
                dialog.PrimaryButtonClick += (_s, _e) =>
                {
                    this.Frame.Navigate(typeof(Volunteer_LoginPage));
                };
                await dialog.ShowAsync();
            }
        }
        private async void GetAsync()
        {
            Newtonsoft.Json.Linq.JObject resp = await Util.Requests.Send("volunteer-message/select", method: "post", token: true, check: false);
            viewModel.Rootobject = JsonConvert.DeserializeObject<Models.VolunteerModel.Rootobject>(resp.ToString());
            if (viewModel.Rootobject.record.Length == 0)
            {
                sp1.Visibility = Visibility.Collapsed;
                none_image.Visibility = Visibility.Visible;
            }
            viewModel.Record_year1 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year, viewModel.Record_year1);
            if (viewModel.Record_year1.Count == 0)
            {
                sp2.Visibility = Visibility.Collapsed;
                none_image2.Visibility = Visibility.Visible;
            }
            viewModel.Record_year2 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year - 1, viewModel.Record_year2);
            if (viewModel.Record_year2.Count == 0)
            {
                sp3.Visibility = Visibility.Collapsed;
                none_image3.Visibility = Visibility.Visible;
            }
            viewModel.Record_year3 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year - 2, viewModel.Record_year3);
            if (viewModel.Record_year3.Count == 0)
            {
                sp4.Visibility = Visibility.Collapsed;
                none_image4.Visibility = Visibility.Visible;
            }
            viewModel.Record_year4 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year - 3, viewModel.Record_year4);
            if (viewModel.Record_year4.Count == 0)
            {
                sp5.Visibility = Visibility.Collapsed;
                none_image5.Visibility = Visibility.Visible;
            }
            viewModel.Record_year5 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year - 4, viewModel.Record_year5);
            if (viewModel.Record_year5.Count == 0)
            {
                sp6.Visibility = Visibility.Collapsed;
                none_image6.Visibility = Visibility.Visible;
            }
            viewModel.Record_year6 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year - 5, viewModel.Record_year6);
            if (viewModel.Record_year6.Count == 0)
            {
                sp7.Visibility = Visibility.Collapsed;
                none_image7.Visibility = Visibility.Visible;
            }
            viewModel.Record_year7 = new ObservableCollection<Models.VolunteerModel.Record>();
            SelectyearFunction(DateTime.Now.Year - 6, viewModel.Record_year7);
            if (viewModel.Record_year7.Count == 0)
            {
                sp8.Visibility = Visibility.Collapsed;
                none_image8.Visibility = Visibility.Visible;
            }

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Confirm_login();
            this.DataContext = viewModel;
            viewModel.Isopened = true;

            viewModel.Time_display = new string[8];
            viewModel.Time_display[0] = "全部";
            for (int i = 0; i < 7; i++)
            {
                viewModel.Time_display[i + 1] = (viewModel.Year1 - i).ToString();

            }



        }
        int nub = 215;
        private void SelectyearFunction(int year, ObservableCollection<Models.VolunteerModel.Record> record)
        {

            record.Clear();
            foreach (var q in viewModel.Rootobject.record)
            {

                if (q.start_time.Contains(year.ToString()))
                {
                    record.Add(q);
                }
            }

        }

        private void display_Button_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Isopened)
            {
                time_GridAnimation_Close.Begin();
                viewModel.Isopened = false;
                user_cancel.Visibility = Windows.UI.Xaml.Visibility.Visible;

            }
            else
            {
                user_cancel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                time_GridAnimation_Open.Begin();
                viewModel.Isopened = true;

            }


        }

        private void SPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (SPivot.SelectedIndex < 0)
                {
                    SPivot.SelectedIndex = pivot_index = 0;
                }
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.Content_Header_Color_Brush;
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Collapsed;
                pivot_index = SPivot.SelectedIndex;
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[0] as TextBlock).Foreground = App.APPTheme.APP_Color_Brush;
                (((SPivot.Items[pivot_index] as PivotItem).Header as Grid).Children[1] as Line).Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                return;
            }
            SPivot_item.SelectedIndex = SPivot.SelectedIndex;

        }
        private void Refresh()
        {
            GetAsync();
            SPivot_item.SelectedIndex = SPivot.SelectedIndex = 0;

        }

        private void scrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            //scrollViewer.ChangeView(null, 30, null);

        }

        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            var sv = sender as ScrollViewer;

            if (!e.IsIntermediate)
            {
                //if (sv.VerticalOffset == 0.0)
                //{
                //    viewModel.IsPullRefresh = true;
                //    await Task.Delay(2000);
                //    Refresh();
                //    sv.ChangeView(null, 30, null);
                //}
                viewModel.IsPullRefresh = false;

            }
        }

        private async void user_cancel_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            dialog.Title = "0.0";
            dialog.Content = "亲，真的要取消已绑定帐号吗？";
            dialog.PrimaryButtonText = "确定";
            dialog.PrimaryButtonClick += (_s, _e) =>
            {
                var vault = new Windows.Security.Credentials.PasswordVault();
                var credentialList = vault.FindAllByResource(resourceName);
                foreach (var item in credentialList)
                {
                    vault.Remove(item);
                }
                this.Frame.Navigate(typeof(Volunteer_LoginPage));
            };
            dialog.SecondaryButtonText = "取消";
            dialog.SecondaryButtonClick += (_s, _e) => { };
            await dialog.ShowAsync();
        }

        private void pull_refresh_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            pull_refresh.Height += e.Delta.Translation.Y;
            if (pull_refresh.Height >= 150)
            {
                pull_refresh.Height = 150;
                pull_text.Text = "停！停！放手！痛死了！die~~~~OAO";
            }
        }

        private async void pull_refresh_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (pull_refresh.Height == 150)
            {
                pull_text.Visibility = Visibility.Collapsed;
                refresh_Icon.Visibility = Visibility.Visible;
                refresh_Animation1.Begin();

                await Task.Delay(3500);
                Refresh();
                refresh_Icon.Visibility = Visibility.Collapsed;
                pull_text.Visibility = Visibility.Visible;
                refresh_Animation2.Begin();
            }
            pull_refresh.Height = 50;
            pull_text.Text = "↓向下拉刷新哦，=w=";

        }
    }
}
