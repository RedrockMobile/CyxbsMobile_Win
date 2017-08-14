using System;
using Windows.UI.Xaml.Data;

namespace ZSCY_Win10.Util.Converter
{
    public class KBGridWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return double.Parse(value.ToString()) - 10;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class KBGridHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((double.Parse(value.ToString()) - 20) / 23 * 3) * 1.2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class KBScrollViewerHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Utils.getPhoneHeight() - double.Parse(value.ToString()) - 50 - 50 - 45;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class JWScrollViewerHeighConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Utils.getPhoneHeight() - double.Parse(value.ToString()) - 50 - 25 - 15;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}