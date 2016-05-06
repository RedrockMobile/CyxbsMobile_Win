using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Service;

namespace ZSCY_Win10.ViewModels.Community
{
    public class MyViewModel
    {
        public ObservableCollection<MyNotification> MyNotify { get; private set; } = new ObservableCollection<MyNotification>();
        public ObservableCollection<MyFeed> MyFeedlist { get; private set; } = new ObservableCollection<MyFeed>();
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
