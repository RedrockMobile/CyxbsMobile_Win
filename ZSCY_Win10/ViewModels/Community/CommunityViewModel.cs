using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        public ObservableCollection<HotFeed> HotFeeds { get; set; } = new ObservableCollection<HotFeed>();
        public ObservableCollection<BBDDFeed> BBDD { get; set; } = new ObservableCollection<BBDDFeed>();
        public ObservableCollection<BBDDFeed> OfficalFeeds { get; set; } = new ObservableCollection<BBDDFeed>();

        public CommunityViewModel()
        {
            Get();
        }

        private async void Get()
        {
            List<BBDDFeed> bbddlist;

            bbddlist = await CommunityFeedsService.GetBBDD(1, 1, 1, 5);
            for (int j = 0; j < bbddlist.Count; j++)
            {
                BBDD.Add(bbddlist[j]);
            }
            List<HotFeed> hotlist;
            hotlist = await CommunityFeedsService.GetHot(0, 1, 1, 5);
            for (int i = 0; i < hotlist.Count; i++)
            {
                HotFeeds.Add(hotlist[i]);
            }
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
