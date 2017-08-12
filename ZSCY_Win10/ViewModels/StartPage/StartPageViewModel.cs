using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZSCY_Win10.Models.StartPageModels;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using ZSCY_Win10.Util.StartPage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using ZSCY_Win10.Util;
using ZSCY_Win10.Resource;
using Newtonsoft.Json;
using ZSCY_Win10.Data.StartPage;
using Windows.UI.ViewManagement;
using Windows.System.Profile;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace ZSCY_Win10.ViewModels.StartPage
{
    public class StartPageViewModel
    {
        public StartPageViewModel()
        {
            if (NetworkInterface.GetIsNetworkAvailable())
                DownLoadImage();
            SetImage();
        }
        /// <summary>
        /// 判断数据库是否有图片
        /// </summary>
        private void SetImage()
        {
            Database temp = new Database();
            string deviceType = AnalyticsInfo.VersionInfo.DeviceFamily;
            if (StartPageHelp.GetImageFromDB(ref temp))
            {
                Model.HasPictrue = false;
                Model.PictrueSource = temp.Url;
                if (deviceType == "Windows.Mobile")
                {
                    Model.StretchMode = Stretch.Uniform;
                    Model.HorMode = HorizontalAlignment.Stretch;
                    Model.VerMode = VerticalAlignment.Stretch;
                    StatusBar.GetForCurrentView().HideAsync().AsTask();
                }
                else
                {
                    Model.StretchMode = Stretch.Uniform;
                    Model.HorMode = HorizontalAlignment.Center;
                    Model.VerMode = VerticalAlignment.Stretch;
                }
            }
            else
            {

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
        }
        private async void DownLoadImage()
        {
            string content = "";
            content = await NetWork.getHttpWebRequest(Api.StartPageImagApi, PostORGet: 1, fulluri: true);
            if (string.IsNullOrWhiteSpace(content))
                return;
            ImageList imageList = JsonConvert.DeserializeObject<ImageList>(content);
            //创建加入数据库的临时变量
            Database dbTemp = new Database();
            foreach (var item in imageList.Data)
            {
                DateTime tempTime = StartPageHelp.GetTime(item.StartTime);
#if DEBUG
                string imageName = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                dbTemp.Name = item.Name;
                dbTemp.StartTime = item.StartTime;
                dbTemp.TargetUrl = item.TargetUrl;
                dbTemp.Id = item.Id;
                dbTemp.Url = StartPageHelp.ImagesPath.Path + "\\" + imageName;
                if (StartPageHelp.InserDatabase(dbTemp))
                {
                    await StartPageHelp.DownloadPictrue(item.ImageUrl, imageName);
                }
#else
                if (tempTime > DateTime.Now)
                {
                    string imageName = item.ImageUrl.Substring(item.ImageUrl.LastIndexOf('/') + 1);
                    dbTemp.Name = item.Name;
                    dbTemp.StartTime = item.StartTime;
                    dbTemp.TargetUrl = item.TargetUrl;
                    dbTemp.Id = item.Id;
                    dbTemp.Url = StartPageHelp.ImagesPath.Path + "\\" + imageName;
                    if (StartPageHelp.InserDatabase(dbTemp))
                    {
                        await StartPageHelp.DownloadPictrue(item.ImageUrl, imageName);
                    }
                }
#endif
            }
        }
        private StartPageModel model;
        public StartPageModel Model { get => model ?? (model = new StartPageModel()); set => model = value; }

    }
}
