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
    public class CommunityViewModel:ViewModelBase
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

           List<Feeds> feeds= await CommunityFeedsService.GetDatas();
            if(feeds!=null)
            foreach (var item in feeds)
            {
                Bbdd.Add(item);
            }
        }

    }
}
