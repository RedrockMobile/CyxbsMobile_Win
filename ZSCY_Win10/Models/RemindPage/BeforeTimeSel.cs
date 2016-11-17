using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.Models.RemindPage
{
    public class BeforeTimeSel:BaseModel
    {
        private string beforeString;
        private TimeSpan beforeTime;
        public int getBeforeTime()
        {
           return (int)BeforeTime.TotalMinutes;
        }
        private Visibility iconVisibility;
        public bool isRemind { get; set; }
        public string BeforeString
        {
            get
            {
                return beforeString;
            }

            set
            {
                beforeString = value;
            }
        }

        public TimeSpan BeforeTime
        {
            get
            {
                return beforeTime;
            }

            set
            {
                beforeTime = value;
            }
        }

      
        public Visibility IconVisibility
        {
            get
            {
                return iconVisibility;
            }

            set
            {
                iconVisibility = value;
                RaisePropertyChanged(nameof(IconVisibility));
            }
        }
    }
}
