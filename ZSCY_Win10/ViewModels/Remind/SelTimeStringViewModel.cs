using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.ViewModels.Remind
{
    public  class  SelTimeStringViewModel : BaseViewModel
    {
        private  string selTimeString;
        
        public  string SelTimeString
        {
            get
            {
                return selTimeString;
            }
            set
            {
                selTimeString = value;
                RaisePropertyChanged(nameof(SelTimeString));
            }
        }



    }
    public class SelWeekNumStringViewModel : BaseViewModel
    {
        private string weekNumString;

        public string WeekNumString
        {
            get
            {
                return weekNumString;
            }

            set
            {
                weekNumString = value;
                RaisePropertyChanged(nameof(WeekNumString));
            }
        }
    }

}
