using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ZSCY_Win10.Models.RemindPage
{
    public class ExcalContent
    {
        private Grid grid;
        private bool isCheck;

        public Grid Grid
        {
            get
            {
                return grid;
            }

            set
            {
                grid = value;
            }
        }

        public bool IsCheck
        {
            get
            {
                return isCheck;
            }

            set
            {
                isCheck = value;
            }
        }
    }
    /// <summary>
    /// 坐标转时间
    /// </summary>
    public class TimeSet
    {
        private bool isCheck;
        private TimeSpan time;
        public void Set(int row)
        {
            switch (row)
            {
                case 0:
                    time = new TimeSpan(8, 0, 0);
                    break;
                case 1:
                    time = new TimeSpan(10, 5, 0);
                    break;
                case 2:
                    time = new TimeSpan(14, 0, 0);
                    break;
                case 3:
                    time = new TimeSpan(16, 5, 0);
                    break;
                case 4:
                    time = new TimeSpan(19, 0, 0);
                    break;
                case 5:
                    time = new TimeSpan(21, 5, 0);
                    break;
                default:
                    time = new TimeSpan(0, 0, 0);
                    break;
            }
        }

        public bool IsCheck
        {
            get
            {
                return isCheck;
            }

            set
            {
                isCheck = value;
            }
        }

        public TimeSpan Time
        {
            get
            {
                return time;
            }

            //set
            //{
            //    time = value;
            //}
        }
    }
}
