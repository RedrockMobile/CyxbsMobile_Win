using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Service;

namespace ZSCY_Win10.ViewModels.Community
{
    public class MyViewModel : ViewModelBase
    {
        private ApplicationDataContainer appSetting = Windows.Storage.ApplicationData.Current.LocalSettings;

        public ObservableCollection<MyNotification> MyNotify { get; private set; } = new ObservableCollection<MyNotification>();
        public ObservableCollection<MyFeed> MyFeedlist { get; private set; } = new ObservableCollection<MyFeed>();
        private PeoInfo info;

        public PeoInfo Info
        {
            get
            {
                return info;
            }
            set
            {
                this.info = value;

                OnPropertyChanged(nameof(Info));
            }
        }

        public int notificationspage = 0;
        public int feedspage = 0;

        public MyViewModel()
        {
            Get();
        }

        private async Task Get()
        {
            getNotifications();
            getFeeds();
            try
            {
                Info = new PeoInfo(appSetting.Values["Community_nickname"].ToString(), appSetting.Values["Community_introduction"].ToString(), appSetting.Values["gender"].ToString(), appSetting.Values["Community_phone"].ToString(), appSetting.Values["Community_qq"].ToString());
                Info.photo_src = appSetting.Values["Community_headimg_src"].ToString() == "" ? "ms-appdata:///local/headimg.png" : appSetting.Values["Community_headimg_src"].ToString();
                Info.photo_thumbnail_src = appSetting.Values["Community_headimg_src"].ToString() == "" ? "ms-appdata:///local/headimg.png" : appSetting.Values["Community_headimg_src"].ToString();
            }
            catch (Exception)
            {
            }
            PeoInfo infotemp = await MyService.GetPerson();
            if (infotemp != null)
            {
                Info = infotemp;
            }
        }

        public async void getNotifications(int size = 15)
        {
            List<MyNotification> s = await MyService.GetNotifications(notificationspage++, 15);
            for (int i = 0; i < s.Count; i++)
            {
                MyNotify.Add(s[i]);
            }
        }

        public async void getFeeds(int size = 15)
        {
            List<MyFeed> f = await MyService.GetMyFeeds(feedspage++, 15);
            for (int i = 0; i < f.Count; i++)
            {
                MyFeedlist.Add(f[i]);
            }
        }
    }
}