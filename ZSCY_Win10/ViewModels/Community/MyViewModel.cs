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
        public  MyViewModel()
        {
            Get();
        }
        private async Task Get()
        {
            List<MyNotification> s = await MyService.GetNotifications(0, 15);
            for (int i = 0; i < s.Count; i++)
            {
                MyNotify.Add(s[i]);
            }
            List<MyFeed> f = await MyService.GetMyFeeds(0, 15);
            for (int i = 0; i < f.Count; i++)
            {
                MyFeedlist.Add(f[i]);
            }
        }
    }
}
