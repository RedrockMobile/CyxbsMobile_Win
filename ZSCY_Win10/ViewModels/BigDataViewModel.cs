using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.ViewModels
{
    public class BigDataViewModel:BasePageViewModel
    {
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
                _page_height = value;
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
                _page_width = value;
                RaisePropertyChanged(nameof(Page_Width));
            }
        }

        //子标题
        private ObservableCollection<ZSCY.Models.BigData> _header;
        public ObservableCollection<ZSCY.Models.BigData> Header
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

    }
}
