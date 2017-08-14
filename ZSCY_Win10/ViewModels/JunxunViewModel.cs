using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.ViewModels
{
    class JunxunViewModel:BasePageViewModel
    {
        private ObservableCollection<ZSCY_Win10.Models.junxunshipin> _junxunshipin;
        public ObservableCollection<ZSCY_Win10.Models.junxunshipin> Junxunshipin
        {
            get
            {
                return _junxunshipin;
            }
            set
            {
                _junxunshipin = value;
                RaisePropertyChanged(nameof(Junxunshipin));
            }
        }


        private ObservableCollection<ZSCY_Win10.Models.Junxuncontents> _junxuntupian;
        public ObservableCollection<ZSCY_Win10.Models.Junxuncontents> Junxuntupian
        {
            get
            {
                return _junxuntupian;
            }
            set
            {
                _junxuntupian = value;
                RaisePropertyChanged(nameof(Junxuntupian));
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
                Photo_Height =_page_height = value;
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
                Photo_Width =_page_width = value;
                RaisePropertyChanged(nameof(Page_Width));
            }
        }


        public double Photo_Height
        {
            get
            {
                return (Photo_Width/1.4);
            }
            set
            {
                RaisePropertyChanged(nameof(Photo_Height));
            }
        }

        public double Photo_Width
        {
            get
            {
                return ((_page_width - 40.0));
            }
            set
            {
                RaisePropertyChanged(nameof(Photo_Width));
            }
        }
    }
}
