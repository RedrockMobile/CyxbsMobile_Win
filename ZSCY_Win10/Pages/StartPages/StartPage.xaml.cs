using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using ZSCY_Win10.Util;
using ZSCY_Win10.Util.StartPage;
using ZSCY_Win10.ViewModels.StartPage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ZSCY_Win10.Pages.StartPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {
        private StartPageViewModel _ViewModel;
        public StartPageViewModel ViewModel { get => _ViewModel; set => _ViewModel = value; }
        private object eParameter = new object();
        Task task;
        public StartPage()
        {
            this.InitializeComponent();
            ViewModel = new StartPageViewModel();
            GaussianBlurHelp.InitializeBlur(Glass);
            //Navigate();
        }

       DispatcherTimer timer = new DispatcherTimer();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            eParameter = e.Parameter;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void loadThinds()
        {
            while(true)
            {
                //判断需要加载的是否完成
                //如果完成break
            }
            //timer.Interval = new TimeSpan(0,0,0);
            //timer.Start();
        }
        private void Timer_Tick(object sender, object e)
        {
            Frame.Navigate(typeof(MainPage),eParameter);
            timer.Stop();
        }

    }
}
