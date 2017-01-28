using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    public class WeekNumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                if (value.ToString().Length > 0)
                {
                    var temp = value.ToString().Split('、');
                    if (temp.Length > 5)
                    {
                        value = "";
                        for (int i = 0; i < 5; i++)
                        {
                            value += $"{temp[i]}、";
                        }
                        value = value.ToString().Remove(value.ToString().Length - 1);
                        value += "...";
                    }
                    else
                    {
                        value = value.ToString().Remove(value.ToString().Length - 1);
                    }
                    value = $"第{value}周";
                }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
