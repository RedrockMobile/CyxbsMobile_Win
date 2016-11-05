using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ZSCY_Win10.Models.RemindPage
{
    public class WeekList
    {
        private string weekName;
        private Rectangle rect;
        private Grid grid;
        private TextBlock textblock;
        private bool isCheck;
        private DateTime weekNumOfMonday;
       private static DateTime oneWeekTime = new DateTime(2016, 9, 5, 0, 0, 0);

        public void SetWeekName(int i)
        {
            weekName = string.Format("第{0}周", i);
            weekNumOfMonday = oneWeekTime.AddDays((i - 1) * 7);
            Debug.WriteLine(weekNumOfMonday);
        }
        public string WeekName
        {
            get
            {
                return weekName;
            }

            set
            {
                weekName = value;
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

        public Rectangle Rect
        {
            get
            {
                return rect;
            }

            set
            {
                rect = value;
            }
        }

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

        public TextBlock Textblock
        {
            get
            {
                return textblock;
            }

            set
            {
                textblock = value;
            }
        }

        public DateTime WeekNumOfMonday
        {
            get
            {
                return weekNumOfMonday;
            }

            set
            {
                weekNumOfMonday = value;
            }
        }
    }
}
