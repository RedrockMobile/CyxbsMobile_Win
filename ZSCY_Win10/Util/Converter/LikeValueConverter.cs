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
    public class LikeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool ismylike = Boolean.Parse(value as string);
            if (ismylike)
            {
                return new SolidColorBrush(Color.FromArgb(255, 235, 103, 16));
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
