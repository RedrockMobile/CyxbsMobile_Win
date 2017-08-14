using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace ZSCY_Win10.ViewModels
{
    internal class LostAndFoundPageViewModel
    {
        public string next_page_url { get; set; }
        public ObservableCollection<LFItem> data = new ObservableCollection<LFItem>();
    }

    internal class LFItem
    {
        public string pro_id;
        public string connect_name;
        public string created_at;
        public string pro_description;
        public string pro_name;
        public string Category { get { return "来自分类 " + pro_name; } }
        public string wx_avatar;
        public BitmapImage HeadImg;
    }
}