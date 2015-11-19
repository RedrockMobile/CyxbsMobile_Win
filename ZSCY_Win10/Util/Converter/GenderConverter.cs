using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class GenderConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string gender = value as string;
            if (gender.Contains("男"))
            {
                return "ms-appx:///Assets/Boy-100.png";
            }
            else if (gender.Contains("女"))
            {
                return "ms-appx:///Assets/Girl-100.png";
            }
            else
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
