using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class FreeisTiVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            //1:显示标题 0:显示时间
            if ((int)value == 1)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FreeisCoVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((int)value == 0)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FreeWeekdayCoVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string weekday = "";
            switch ((int)value)
            {
                case 0:
                    weekday = "星期一";
                    break;
                case 1:
                    weekday = "星期二";
                    break;
                case 2:
                    weekday = "星期三";
                    break;
                case 3:
                    weekday = "星期四";
                    break;
                case 4:
                    weekday = "星期五";
                    break;
                case 5:
                    weekday = "星期六";
                    break;
                case 6:
                    weekday = "星期天";
                    break;

            }
            return weekday;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class FreeTimeCoVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string time = "";
            switch ((int)value)
            {
                case 0:
                    time = "1~2节";
                    break;
                case 1:
                    time = "3~4节";
                    break;
                case 2:
                    time = "5~6节";
                    break;
                case 3:
                    time = "7~8节";
                    break;
                case 4:
                    time = "9~10节";
                    break;
                case 5:
                    time = "11~12节";
                    break;

            }
            return time;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }






}
