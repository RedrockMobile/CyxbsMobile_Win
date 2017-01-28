using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.Util.Converter.RemindConverter
{
    class BeforeTimeIconColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Color color;

            if (value == null)
            {
                color = Color.FromArgb(255, 167, 167, 167);

            }
            else
            {
                color = Color.FromArgb(255, 64, 164, 255);
            }
            return new SolidColorBrush(color);

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
