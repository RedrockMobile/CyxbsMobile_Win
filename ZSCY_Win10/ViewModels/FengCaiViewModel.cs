using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ZSCY_Win10.ViewModels
{
    public class FengCaiViewModel : BasePageViewModel
    {
        private ObservableCollection<ZSCY.Models.fengcaiheaders> _header;
        public ObservableCollection<ZSCY.Models.fengcaiheaders> Header
        {
            get
            {
                return _header;
            }
            set
            {
                _header = value;
                RaisePropertyChanged(nameof(Header));
            }
        }

        private ObservableCollection<ZSCY.Models.zuzhi> _zuzhi;
        public ObservableCollection<ZSCY.Models.zuzhi> ZuZhi
        {
            get
            {
                return _zuzhi;
            }
            set
            {
                _zuzhi = value;
                RaisePropertyChanged(nameof(ZuZhi));
            }
        }

        private ObservableCollection<ZSCY.Models.zuzhi_intro> _zuzhi_intro;
        public ObservableCollection<ZSCY.Models.zuzhi_intro> Zuzhi_Intro
        {
            get
            {
                return _zuzhi_intro;
            }
            set
            {
                _zuzhi_intro = value;
                RaisePropertyChanged(nameof(Zuzhi_Intro));
            }
        }

        private ObservableCollection<ZSCY.Models.zuimei> _zuimei;
        public ObservableCollection<ZSCY.Models.zuimei> ZuiMei
        {
            get
            {
                return _zuimei;
            }
            set
            {
                _zuimei = value;
                RaisePropertyChanged(nameof(ZuiMei));
            }
        }

        //private ObservableCollection<string> _zuimei_photos;
        //public ObservableCollection<string> ZuiMei_Photos
        //{
        //    get
        //    {
        //        return _zuimei_photos;
        //    }
        //    set
        //    {
        //        _zuimei_photos = value;
        //        RaisePropertyChanged(nameof(ZuiMei_Photos));
        //    }
        //}

        private ObservableCollection<ZSCY.Models.yuanchuang> _yuanchuang;
        public ObservableCollection<ZSCY.Models.yuanchuang> YuanChuang
        {
            get
            {
                return _yuanchuang;
            }
            set
            {
                _yuanchuang = value;
                RaisePropertyChanged(nameof(_yuanchuang));
            }
        }

        private ObservableCollection<ZSCY.Models.xuezi> _xuezi;
        public ObservableCollection<ZSCY.Models.xuezi> XueZi
        {
            get
            {
                return _xuezi;
            }
            set
            {
                _xuezi = value;
                RaisePropertyChanged(nameof(XueZi));
            }
        }

        private ObservableCollection<ZSCY.Models.teacher> _teacher;
        public ObservableCollection<ZSCY.Models.teacher> Teacher
        {
            get
            {
                return _teacher;
            }
            set
            {
                _teacher = value;
                RaisePropertyChanged(nameof(Teacher));
            }
        }

        private double _page_height;
        private double _page_width;

        public double Page_Height
        {
            get
            {
                return _page_height;
            }
            set
            {
                Detail_Height = ZuiMei_Photo_Height = XueZi_Height = _page_height = value;
                RaisePropertyChanged(nameof(Page_Height));
            }
        }

        public double Page_Width
        {
            get
            {
                return _page_width;
            }
            set
            {
                Detail_Width = ZuiMei_Photo_Width = XueZi_Width = _page_width = value;
                RaisePropertyChanged(nameof(Page_Width));
            }
        }

        public double ZuiMei_Photo_Height
        {
            get
            {
                return (ZuiMei_Photo_Width / 2.0);
            }
            set
            {
                RaisePropertyChanged(nameof(ZuiMei_Photo_Height));
            }
        }

        public double ZuiMei_Photo_Width
        {
            get
            {
                return ((_page_width - 40.0));
            }
            set
            {
                RaisePropertyChanged(nameof(ZuiMei_Photo_Width));
            }
        }

        public double XueZi_Height
        {
            get
            {
                return ((int)(_page_height / 4.0));
            }
            set
            {
                RaisePropertyChanged(nameof(XueZi_Height));
            }
        }

        public double XueZi_Width
        {
            get
            {
                return (int)(_page_width);
            }
            set
            {
                RaisePropertyChanged(nameof(XueZi_Width));
            }
        }

        public double Detail_Height
        {
            get
            {
                return ((_page_height / 2.0) * 1.4);
            }
            set
            {
                RaisePropertyChanged(nameof(Detail_Height));
            }
        }

        public double Detail_Width
        {
            get
            {
                return _page_height;
            }
            set
            {
                RaisePropertyChanged(nameof(Detail_Width));
            }
        }
    }
}
