using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ZSCY_Win10.ViewModels
{
    class LFDetailPageViewModel
    {
        public string connect_name;
        public string connect_phone;
        public string connect_wx;
        public string L_or_F_place;
        public string L_or_F_time;
        public string pro_description;
        public string pro_name;
        public string Category { get { return "来自分类 " + pro_name; } }
        public string wx_avatar;
        public BitmapImage HeadImg = new BitmapImage();
    }
}
