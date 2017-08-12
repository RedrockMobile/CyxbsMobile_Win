using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.Util.Converter
{
    public class BeforeTimeIconColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            if (value == null)
            {
                Color gray = Color.FromArgb(255, 167, 167, 167);

                return new SolidColorBrush(gray);
            }
            else
            {
                Color blue = Color.FromArgb(255, 64, 164, 255);
                return new SolidColorBrush(blue);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BeforeTimeRemind : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return value + "分钟前";
            else
            {
                return "不提醒";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class RemindListWeek : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return $"第{value}周";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class AddRemindShowString : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }
            else
            {

                string[] temp = value.ToString().Split(',');
                //莫名的多一个""
                if (temp.Length - 1 > 6)
                {
                    string s = "";
                    for (int i = 1; i < 6; i++)
                        s += temp[i] + " ";
                    s += "...";
                    return s;
                }
                else
                {

                    return value;
                }
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class AddRmindShowWeekNum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "";
            }
            else
            {

                string[] temp = value.ToString().Split('、');
                if (temp.Length - 1 > 10)
                {
                    string s = "";
                    for (int i = 1; i < 10; i++)
                        s += temp[i] + "、";
                    s = s.Remove(s.Length - 1);
                    s += "... 周";
                    return s;
                }
                else
                {
                    if (value.ToString().Length > 1)
                        value = value.ToString().Remove(value.ToString().Length - 1, 1) + " 周";
                    return value;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}