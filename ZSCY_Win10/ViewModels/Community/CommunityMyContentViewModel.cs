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
            getItem(e);
        }

        private async void getItem(object e)
        {
            if (e is MyFeed)
            {
                Item = e as MyFeed;
            }
            else if (e is MyNotification)
            {
                Item = new MyFeed();
                Item.content = (e as MyNotification).article_content;
                Item.type_id = "5";
                Item.id = (e as MyNotification).article_id;
                Item = await CommunityMyContentService.GetFeed(5, (e as MyNotification).article_id);
            }
        }
    }
}