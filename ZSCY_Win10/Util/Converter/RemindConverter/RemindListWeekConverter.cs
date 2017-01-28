using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    class RemindListWeekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] temp = value.ToString().Split(',');
            //var array = temp.OrderBy(x => int.Parse(x)).ToList();
            string s = "";
            if (temp.Length > 5)
            {
                for (int i = 0; i < 5; i++)
                {
                    s +=temp[i] + "、";
                }
            }
            else
            {
                for(int i=0;i<temp.Length;i++)
                {
                    s += temp[i] + "、";
                }

            }
            s = $"第{s.Remove(s.Length - 1)}周";

            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
