using System;
using System.Collections.Generic;
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
    }
}
