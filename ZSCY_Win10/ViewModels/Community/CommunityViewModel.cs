using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Service;

namespace ZSCY_Win10.ViewModels.Community
{
    public class CommunityViewModel : ViewModelBase
    {
        public ObservableCollection<Feeds> HotFeeds { get; set; } = new ObservableCollection<Feeds>();
        public ObservableCollection<Feeds> Bbdd { get; set; } = new ObservableCollection<Feeds>();
        public ObservableCollection<Feeds> OfficalFeeds { get; set; } = new ObservableCollection<Feeds>();

        public CommunityViewModel()
        {
            Get();
        }

        private async void Get()
        {
            List<Feeds> list;
            for (int i = 0; i <= 1; i++)
            {
                //获取动态
                //if (i == 0)
                //{
                //    list = await CommunityFeedsService.GetDatas(0, 1, 1, 0);
                //    if (list != null)
                //        foreach (var item in list)
                //        {
                //            HotFeeds.Add(item);
                //        }
                //}
                //if (i == 1)
                //{
                //    list = await CommunityFeedsService.GetDatas(1, 1, 1, 5);
                //    if (list != null)
                //        foreach (var item in list)
                //        {
                //            Bbdd.Add(item);
                //        }
                //}
            }
        }
    }
}
