using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace ZSCY_Win10.Util.Converter
{
    public class GenderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string c = value as string;
            if (c.Contains("男"))
            {
                return new SolidColorBrush(Color.FromArgb(255, 63, 144, 227));
            }
            else
            {
                return new SolidColorBrush(Color.FromArgb(255, 239, 143, 159));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}