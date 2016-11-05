using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindPage
{
    public class BeforeTimeSel
    {
        private string beforeString;
        private TimeSpan beforeTime;

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
    }
}
