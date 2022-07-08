using System;


namespace ZSCY_Win10.Util
{
    internal class nowWeek
    {
        static int maxDay = 147;
        /// <summary>
        /// 未登录情况下获取今日周次
        /// </summary>
        /// <returns></returns>
        public static int GetNowWeek()
        {
            DateTime today = DateTime.Now.ToLocalTime().Date;
            DateTime spring = GetSpringSemester(today.Year);
            double spi = (double)today.Subtract(spring).TotalDays;
            if (spi < 0)
            {
                DateTime fall = GetFallSemester(today.Year - 1);
                double fai = (double)today.Subtract(fall).TotalDays;
                if (fai > maxDay) return 0;
                else return (int)Math.Ceiling(fai / 7);
            }
            else if ((int)spi == 0) return 1;
            else if (spi <= maxDay) return (int)Math.Ceiling(spi / 7);
            else
            {
                DateTime fall = GetFallSemester(today.Year);
                double fai = (double)today.Subtract(fall).TotalDays;
                if (fai < 0) return 0;
                else return (int)Math.Ceiling(fai / 7);
            }
        }

        public static DateTime GetFallSemester(int year)
        {
            DateTime dt = new DateTime(year, 9, 1);
            int diff = Convert.ToInt32(dt.DayOfWeek);
            diff = (-1) * (diff == 0 ? (7 - 1) : (diff - 1));
            if (diff < 0) diff += 7;
            dt.AddDays(diff);
            return dt;
        }

        public static DateTime GetSpringSemester(int year)
        {
            System.Globalization.ChineseLunisolarCalendar cc = new System.Globalization.ChineseLunisolarCalendar();
            DateTime dt = cc.ToDateTime(year, 1, 15, 0, 0, 0, 0);
            int diff = Convert.ToInt32(dt.DayOfWeek);
            diff = (-1) * (diff == 0 ? (7 - 1) : (diff - 1));
            diff += (diff == 0 ? 7 : 0) + 7;
            dt.AddDays(diff);
            return dt;
        }

    }
}
