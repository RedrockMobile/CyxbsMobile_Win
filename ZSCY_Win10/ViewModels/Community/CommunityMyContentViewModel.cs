using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;
using ZSCY_Win10.Service;

namespace ZSCY_Win10.ViewModels.Community
{
    public class CommunityMyContentViewModel : ViewModelBase
    {
        private MyFeed myVar;

        public MyFeed Item
        {
            get { return myVar; }
            set { myVar = value; OnPropertyChanged(nameof(Item)); }
        }
        public CommunityMyContentViewModel(object e)
        {
            if (e is MyFeed)
            {
                Item = e as MyFeed;
            }
            else if (e is MyNotification)
            {
                Get(e as MyNotification);
            }
        }
        private async void Get(MyNotification m)
        {
            Item = await CommunityMyContentService.GetFeed(5, m.article_id);

        }
    }
}
