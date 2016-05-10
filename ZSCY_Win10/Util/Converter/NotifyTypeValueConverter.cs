using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class NotifyTypeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string r = value as string;
            if (r == "praise")
            {
                return "赞了我";
            }
            else if (int.Parse(r) >= 0)
            {
                return r;
            }
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
