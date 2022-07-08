using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

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
        private int pivot_index;
        private async void Confirm_login()
        {

            try
            {
                volunteerPage_ProgressRing.Visibility = Visibility.Visible;
                volunteerPage_Grid.Visibility = Visibility.Collapsed;
                await GetAsync();
                volunteerPage_ProgressRing.Visibility = Visibility.Collapsed;
                volunteerPage_Grid.Visibility = Visibility.Visible;
                viewModel.Line_Y1 = viewModel.Record_year1.Count * nub;
                viewModel.Line_Y2 = viewModel.Record_year2.Count * nub;
                viewModel.Line_Y3 = viewModel.Record_year3.Count * nub;
                viewModel.Line_Y4 = viewModel.Record_year4.Count * nub;
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
        private async Task GetAsync()
        {
            Newtonsoft.Json.Linq.JObject resp = await Util.Requests.Send("volunteer-message/select", method: "post", token: true, check: false);
            viewModel.Rootobject = JsonConvert.DeserializeObject<Models.VolunteerModel.Rootobject>(resp.ToString());
            if (viewModel.Rootobject.record.Length == 0)
            {
                sp1.Visibility = Visibility.Collapsed;
                none_image.Visibility = Visibility.Visible;
            }
            viewModel.Record_year1 = new ObservableCollection<Models.VolunteerModel.Record>();
            viewModel.YearTime1 = SelectyearFunction(DateTime.Now.Year, viewModel.Record_year1);
            if (viewModel.Record_year1.Count == 0)
            {
                sp2.Visibility = Visibility.Collapsed;
                none_image2.Visibility = Visibility.Visible;
            }
            viewModel.Record_year2 = new ObservableCollection<Models.VolunteerModel.Record>();
            viewModel.YearTime2 = SelectyearFunction(DateTime.Now.Year - 1, viewModel.Record_year2);
            if (viewModel.Record_year2.Count == 0)
            {
                sp3.Visibility = Visibility.Collapsed;
                none_image3.Visibility = Visibility.Visible;
            }
            viewModel.Record_year3 = new ObservableCollection<Models.VolunteerModel.Record>();
            viewModel.YearTime3 = SelectyearFunction(DateTime.Now.Year - 2, viewModel.Record_year3);
            if (viewModel.Record_year3.Count == 0)
            {
                sp4.Visibility = Visibility.Collapsed;
                none_image4.Visibility = Visibility.Visible;
            }
            viewModel.Record_year4 = new ObservableCollection<Models.VolunteerModel.Record>();
            viewModel.YearTime4 = SelectyearFunction(DateTime.Now.Year - 3, viewModel.Record_year4);
            if (viewModel.Record_year4.Count == 0)
            {
                sp5.Visibility = Visibility.Collapsed;
                none_image5.Visibility = Visibility.Visible;
            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Confirm_login();
            this.DataContext = viewModel;
            viewModel.Isopened = true;

            viewModel.Time_display = new string[5];
            viewModel.Time_display[0] = "全部";
            for (int i = 0; i < 4; i++)
            {
                viewModel.Time_display[i + 1] = (viewModel.Year1 - i).ToString();

            }



        }
        int nub = 215;
        private double SelectyearFunction(int year, ObservableCollection<Models.VolunteerModel.Record> record)
        {
            double time = 0;
            record.Clear();
            foreach (var q in viewModel.Rootobject.record)
            {

                if (q.start_time.Contains(year.ToString()))
                {
                    time += double.Parse(q.hours);
                    record.Add(q);
                }
            }
            return time;

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

                await GetAsync();
                SPivot_item.SelectedIndex = SPivot.SelectedIndex = 0;
                refresh_Icon.Visibility = Visibility.Collapsed;
                pull_text.Visibility = Visibility.Visible;
                refresh_Animation2.Begin();
            }
            pull_refresh.Height = 50;
            pull_text.Text = "↓向下拉刷新哦，=w=";

        }
    }
}
