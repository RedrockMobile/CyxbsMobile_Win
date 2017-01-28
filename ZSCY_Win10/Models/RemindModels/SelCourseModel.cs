using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindModels
{
    public class SelCourseModel : RemindBaseModel
    {
        public SelCourseModel(int c, int r)
        {
            DayNum = c;
            ClassNum = r;
        }
        /// <summary>
        /// 当时时间,相对星期一0：0：0的偏移量
        /// </summary>
        /// <returns></returns>
        public TimeSpan NowTime()
        {
            TimeSpan time = new TimeSpan(DayNum, 0, 0, 0);
            time = time.Add(NowHMS(ClassNum));
            return time;
        }
        private TimeSpan NowHMS(int classNum)
        {
            TimeSpan time;
            switch (classNum)
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
                    time = new TimeSpan(20, 50, 0);
                    break;
                default:
                    time = new TimeSpan(0, 0, 0);
                    break;
            }
            return time;
        }
        private int _DayNum;
        private int _ClassNum;

        public int DayNum
        {
            get
            {
                return _DayNum;
            }

            set
            {
                _DayNum = value;
            }
        }

        public int ClassNum
        {
            get
            {
                return _ClassNum;
            }

            set
            {
                _ClassNum = value;
            }
        }
        public string CourseTime()
        {
            string weekNum = "";
            string classNum = "";
            switch (DayNum)
            {
                case 0:
                    weekNum = "周一";
                    break;
                case 1:
                    weekNum = "周二";
                    break;
                case 2:
                    weekNum = "周三";
                    break;
                case 3:
                    weekNum = "周四";
                    break;
                case 4:
                    weekNum = "周五";
                    break;
                case 5:
                    weekNum = "周六";
                    break;
                case 6:
                    weekNum = "周日";
                    break;
            }
            switch (ClassNum)
            {
                case 0:
                    classNum = "12节";
                    break;
                case 1:
                    classNum = "34节";
                    break;
                case 2:
                    classNum = "56节";
                    break;
                case 3:
                    classNum = "78节";
                    break;
                case 4:
                    classNum = "910节";
                    break;
                case 5:
                    classNum = "1112节";
                    break;
            }
            string temp = string.Concat(weekNum, classNum);
            return temp;
        }
    }
}
