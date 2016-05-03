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
        private int hotpage = 0;
        private int bbddpage = 0;

        //public RelayCommand BBDDitemCommand { get; set; }

        public CommunityViewModel()
        {
            Get();
        }

        private async void Get()
        {
            getbbdd(1, 15, 5);
            gethot(0, 15, 5);
        }

        public async void getbbdd(int type = 1, int size = 15, int typeid = 5)
        {
            List<BBDDFeed> bbddlist;
            bbddlist = await CommunityFeedsService.GetBBDD(type, bbddpage++, size, typeid);
            for (int j = 0; j < bbddlist.Count; j++)
            {
                BBDD.Add(bbddlist[j]);
            }
        }

        public async void gethot(int type = 0, int size = 15, int typeid = 0)
        {
            List<HotFeed> hotlist;
            hotlist =await CommunityFeedsService.GetHot(type, hotpage++, size, typeid);
            for (int i = 0; i < hotlist.Count; i++)
            {
                HotFeeds.Add(hotlist[i]);
            }
        }
    }
}
