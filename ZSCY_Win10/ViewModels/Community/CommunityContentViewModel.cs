using System.Collections.ObjectModel;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.ViewModels.Community
{
    public class CommunityContentViewModel : ViewModelBase
    {
        private BBDDFeed bbdd;
        private HotFeed hot;
        public ObservableCollection<ViewModelBase> feeds { get; set; } = new ObservableCollection<ViewModelBase>();

        public HotFeed hotfeed
        {
            get { return hot; }
            set
            {
                hot = value;
                feeds.Add(hotfeed);
                OnPropertyChanged(nameof(hotfeed));
            }
        }

        public BBDDFeed BBDD
        {
            get
            {
                return bbdd;
            }
            set
            {
                bbdd = value;
                feeds.Add(BBDD);
                OnPropertyChanged(nameof(BBDD));
            }
        }
    }
}