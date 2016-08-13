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
    public class CommunityPersonInfoViewModel : ViewModelBase
    {
        int page = 0;
        string stunum = "";
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
        public ObservableCollection<MyFeed> MyFeedlist { get; private set; } = new ObservableCollection<MyFeed>();

        public CommunityPersonInfoViewModel(string stunum)
        {
            this.stunum = stunum;
            Get();
        }
        public async void Get()
        {
            //TODO:暂时没找出毛病 白天问问部长杰哥
            Info = await CommunityPersonInfoService.GetPerson(stunum);
            List<MyFeed> list = await CommunityPersonInfoService.GetMyFeeds(stunum, page++, 15);
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].headimg = Info.photo_thumbnail_src;
                    list[i].nickname = info.nickname;
                    MyFeedlist.Add(list[i]);
                }
        }
    }
}
