using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSCY_Win10.Models.RemindPage
{
    /// <summary>
    /// 已选择的课列表
    /// </summary>
    public class CourseList
    {
        private string weekNum;
        private string classNum;
        private bool isCheck;
        public CourseList(int row, int column, bool isCheck)
        {
            switch (column)
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
                    weekNum = "周末";
                    break;
            }
            switch (row)
            {
                case 0:
                    classNum = "1-2";
                    break;
                case 1:
                    classNum = "3-4";
                    break;
                case 2:
                    classNum = "5-6";
                    break;
                case 3:
                    classNum = "7-8";
                    break;
                case 4:
                    classNum = "9 - 10";
                    break;
                case 5:
                    classNum = "11-12";
                    break;
            }
            this.isCheck = isCheck;
        }
       


        public string WeekNum
        {
            get
            {
                return weekNum;
            }
            set { weekNum = value; }
        }

        public string ClassNum
        {
            get
            {
                return classNum;
            }
            set { classNum = value; }


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
}
