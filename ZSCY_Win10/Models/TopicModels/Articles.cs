using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSCY_Win10.Common;
using ZSCY_Win10.Data.Community;

namespace ZSCY_Win10.Models.TopicModels
{
   public class Articles:ViewModelBase
    {
        private string is_my_like;
        private string like_nums;
        private string remark_nums;
        public string article_photo_src { get; set; }
        public string article_thumbnail_src { get; set; }
        public string num_id { get; set; } = "0";
        public string remark_num {
            get {
                return remark_nums;
            }
            set {
                remark_nums = value;
                OnPropertyChanged(nameof(remark_num));

            }
        }
        public string like_num {
            get
            {
                return like_nums;
            }
            set
            {
                like_nums = value;
                OnPropertyChanged(nameof(like_num));
            }
        }
        public string created_time { get; set; }
        public int article_id { get; set; }
        public string nickname { get; set; }
        public string user_photo_src { get; set; }
        public string is_my_Like {
            get
            {
                return is_my_like;
            }
            set
            {
                is_my_like = value;
                OnPropertyChanged(nameof(is_my_Like));
            }
        }
        public string content { get; set; }
        public ObservableCollection<pic> articlesPic { get; set; } = new ObservableCollection<pic>();
    }
}
