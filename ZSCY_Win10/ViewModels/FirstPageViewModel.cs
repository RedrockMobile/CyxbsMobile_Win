using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.ViewModels
{
    public class FirstPageViewModel : BasePageViewModel
    {
        private double _page_height;
        public double Page_Height
        {
            get
            {
                return ((_page_height - 60) / 4.0) - 15;
            }
            set
            {
                _page_height = value;
                RaisePropertyChanged(nameof(Page_Height));
            }
        }

        private double _page_width;
        public double Page_Width
        {
            get
            {
                if (_page_width - 30 > 500.0)
                {
                    return 350;
                }
                return _page_width - 30;
            }
            set
            {
                _page_width = value;
                RaisePropertyChanged(nameof(Page_Width));
            }
        }
    }
}
