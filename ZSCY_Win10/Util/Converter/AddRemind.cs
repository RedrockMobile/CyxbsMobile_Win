using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class RemindListDay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] temp = value.ToString().Split(',');
            if (temp.Length > 2)
            {
                string v = "";
                for (int i = 0; i < 2; i++)
                {
                    v += "周" + temp[i]+1 + "、";

                }

                v = v.Remove(v.Length - 1);
                v += "...的";
                return v;
            }
            else if(temp.Length>0)
            {
                string v = "";
                for (int i = 0; i < temp.Length; i++)
                {
                    v += "周" + temp[i]+1 + "、";
                }
                v = v.Remove(v.Length - 1);
                v += "的";
                return v;
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class RemindListClass : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] temp = value.ToString().Split(',');
            if(temp.Length>2)
            {
                string s = "";
                for(int i=0;i<2;i++)
                {
                    s +=ConverterNum(int.Parse( temp[i])) + "节、";

                }
                s = s.Remove(s.Length - 1);
                s += "...";
                return s;
            }
            else if(temp.Length>0)
            {
                string s = "";
                for(int i=0;i<temp.Length;i++)
                {
                    s += ConverterNum(int.Parse(temp[i])) + "节、";
                }
                s = s.Remove(s.Length - 1);
                return s;
            }
            else
            {
                return value;
            }
                
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
        private string ConverterNum(int s)
        {
            switch(s)
            {
                case 0:
                    return "12";
                case 1:
                    return "34";
                case 2:
                    return "56";
                case 3:
                    return "78";
                case 4:
                    return "910";
                case 5:
                    return "1112";
                default:
                    return "";
            }
        }
    }
    public class AddRemindShowString : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] temp = value.ToString().Split(' ');
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class AddRmindShowWeekNum : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
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

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


}
