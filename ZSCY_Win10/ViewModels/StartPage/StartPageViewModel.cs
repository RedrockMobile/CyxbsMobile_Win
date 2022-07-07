using System;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using ZSCY_Win10.Models.StartPageModels;

namespace ZSCY_Win10.ViewModels.StartPage
{
    public class StartPageViewModel
    {
        public StartPageViewModel()
        {
            string deviceType = AnalyticsInfo.VersionInfo.DeviceFamily;

            Model.HasPictrue = true;
            Model.PictrueSource = @"Assets/SplashScreen.png";

            if (deviceType == "Windows.Mobile")
            {
                Model.StretchMode = Stretch.Uniform;
                Model.HorMode = HorizontalAlignment.Center;
                Model.VerMode = VerticalAlignment.Center;
                StatusBar.GetForCurrentView().HideAsync().AsTask();
            }
            else
            {
                ApplicationView.GetForCurrentView().TitleBar.BackgroundColor =
                ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 5, 139, 254);
                Model.StretchMode = Stretch.None;
                Model.HorMode = HorizontalAlignment.Center;
                Model.VerMode = VerticalAlignment.Stretch;
            }
        }

        private StartPageModel model;
        public StartPageModel Model { get => model ?? (model = new StartPageModel()); set => model = value; }
    }
}